using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Channels;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Channels;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaBrowser.Channels.HitboxTV
{
    class HitboxChannel : IChannel, IRequiresMediaInfoCallback
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IJsonSerializer _jsonSerializer;

        public HitboxChannel(IHttpClient httpClient, IJsonSerializer jsonSerializer, ILogManager logManager)
        {
            _httpClient = httpClient;
            _logger = logManager.GetLogger(GetType().Name);
            _jsonSerializer = jsonSerializer;
        }

        public string DataVersion
        {
            get { return "1"; }
        }

        public string Description
        {
            get { return "Stream live feeds from Hitbox.tv."; }
        }

        public InternalChannelFeatures GetChannelFeatures()
        {
            return new InternalChannelFeatures
            {
                ContentTypes = new List<ChannelMediaContentType>
                {
                    ChannelMediaContentType.Clip
                },

                MediaTypes = new List<ChannelMediaType>
                {
                    ChannelMediaType.Video
                },

                // https://github.com/justintv/Twitch-API/blob/master/v3_resources/streams.md
                
                MaxPageSize = 100,

                DefaultSortFields = new List<ChannelItemSortField>
                {
                    ChannelItemSortField.CommunityRating,
                },
            };
        }

        public Task<DynamicImageResponse> GetChannelImage(ImageType type, CancellationToken cancellationToken)
        {
            switch (type)
            {
                case ImageType.Thumb:
                case ImageType.Backdrop:
                case ImageType.Primary:
                    {
                        var path = GetType().Namespace + ".Resources.logo.png";

                        return Task.FromResult(new DynamicImageResponse
                        {
                            Format = ImageFormat.Png,
                            HasImage = true,
                            Stream = GetType().Assembly.GetManifestResourceStream(path)
                        });
                    }
                default:
                    throw new ArgumentException("Unsupported image type: " + type);
            }
        }

        public async Task<ChannelItemResult> GetChannelItems(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            ChannelItemResult result;

            _logger.Debug("cat ID : " + query.FolderId);

            if (query.FolderId == null)
            {
                result = await GetChannelsInternal(query, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                result = await GetChannelItemsInternal(query, cancellationToken).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<ChannelItemResult> GetChannelsInternal(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            var offset = query.StartIndex.GetValueOrDefault();
            var downloader = new HitboxChannelDownloader(_logger, _jsonSerializer, _httpClient);
            var channels = await downloader.GetHitboxChannelList(offset, cancellationToken);

            string baseurl = "http://hitbox.tv/{0}";

            var items = channels.livestream.OrderByDescending(x => x.channel.followers)
                .Select(i => new ChannelItemInfo
                {
                    Type = ChannelItemType.Folder,
                    ImageUrl = String.Format(baseurl, i.media_thumbnail_large),
                    Name = i.media_name,
                    Id = i.media_id,
                    CommunityRating = Convert.ToSingle(i.channel.followers),
                });

            return new ChannelItemResult
            {
                Items = items.ToList(),
                TotalRecordCount = channels.livestream.Count,
            };
        }

        private async Task<ChannelItemResult> GetChannelItemsInternal(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            var offset = query.StartIndex.GetValueOrDefault();
            var downloader = new HitboxUserDownloader(_logger, _jsonSerializer, _httpClient);
            _logger.Info("Folder ID: " + query.FolderId);
            var streams = await downloader.GetHitboxUserData(query.FolderId, offset, cancellationToken)
                .ConfigureAwait(false);
            var livestream = streams.livestream[0];

            var items = new List<ChannelItemInfo>();

            items.Add(new ChannelItemInfo {
                    ImageUrl = livestream.media_thumbnail_large,
                    IsInfiniteStream = true,
                    MediaType = ChannelMediaType.Video,
                    Name = livestream.media_user_name,
                    Id = livestream.media_user_name,
                    Type = ChannelItemType.Media,
                    CommunityRating = Convert.ToSingle(livestream.channel.followers),
                    DateCreated = !String.IsNullOrEmpty(livestream.channel.media_live_since) ? 
                        Convert.ToDateTime(livestream.media_live_since) : (DateTime?)null,
                });
            _logger.Debug("GetInternalChannelItems items: " + items.OrderByDescending(x => x.Name));

            return new ChannelItemResult
            {
                Items = items,
                TotalRecordCount = items.Count,
            };
        }

        public IEnumerable<ImageType> GetSupportedChannelImages()
        {
            return new List<ImageType>
            {
                ImageType.Thumb,
                ImageType.Primary,
                ImageType.Backdrop
            };
        }

        public string HomePageUrl
        {
            get { return "http://hitbox.tv"; }
        }

        public bool IsEnabledFor(string userId)
        {
            return true;
        }

        public string Name
        {
            get { return "Hitbox"; }
        }

        public ChannelParentalRating ParentalRating
        {
            get { return ChannelParentalRating.GeneralAudience; }
        }

        public async Task<IEnumerable<ChannelMediaInfo>> GetChannelItemMediaInfo(string id, CancellationToken cancellationToken)
        {
            return new List<ChannelMediaInfo>
            {
                new ChannelMediaInfo
                {
                    Path = "http://api.hitbox.tv/player/hls/" + id + ".m3u8",
                }
            };
        }
    }
}
