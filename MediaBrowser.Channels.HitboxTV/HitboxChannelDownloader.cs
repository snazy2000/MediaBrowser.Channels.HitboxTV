using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Channels;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaBrowser.Channels.HitboxTV
{
    class HitboxChannelDownloader
    {
        private ILogger _logger;
		private readonly IHttpClient _httpClient;
		private readonly IJsonSerializer _jsonSerializer;

		public HitboxChannelDownloader(ILogger logManager, IJsonSerializer jsonSerializer, IHttpClient httpClient)
		{
			_logger = logManager;
			_jsonSerializer = jsonSerializer;
			_httpClient = httpClient;
		}

        public async Task<RootObject> GetHitboxChannelList(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            int limit = 100;
            if (query.Limit.HasValue)
                limit = query.Limit.Value;
            
            var offset = query.StartIndex.GetValueOrDefault();

            string authtoken = Plugin.Instance.Configuration.authToken;

            _logger.Debug("Limit = " + limit);

            using (var json = await _httpClient.Get(String.Format("http://api.hitbox.tv/media.json?offset={0}&limit={1}&liveonly=true&authtoken={2}", offset, limit, authtoken), CancellationToken.None).ConfigureAwait(false))
            {
                return _jsonSerializer.DeserializeFromStream<RootObject>(json);
            }
        }
    }
}
