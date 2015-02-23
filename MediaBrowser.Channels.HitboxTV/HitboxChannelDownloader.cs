using MediaBrowser.Common.Net;
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

        public async Task<RootObject> GetHitboxChannelList(int offset, CancellationToken cancellationToken)
        {
            RootObject reg;

            using (var json = await _httpClient.Get(String.Format("http://api.hitbox.tv/media.json?offset={0}&limit=100&liveonly=true", offset), CancellationToken.None).ConfigureAwait(false))
            {
                reg = _jsonSerializer.DeserializeFromStream<RootObject>(json);
            }

            return reg;
        }
    }
}
