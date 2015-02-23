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
    class HitboxUserDownloader
    {
        private ILogger _logger;
		private readonly IHttpClient _httpClient;
		private readonly IJsonSerializer _jsonSerializer;

		public HitboxUserDownloader(ILogger logManager, IJsonSerializer jsonSerializer, IHttpClient httpClient)
		{
			_logger = logManager;
			_jsonSerializer = jsonSerializer;
			_httpClient = httpClient;
		}

        public async Task<RootObject> GetHitboxUserData(string query, int offset, CancellationToken cancellationToken)
        {
            RootObject reg;

            string authToken = Plugin.Instance.Configuration.authToken;

            _logger.Debug("Getting user data for user with ID: " + query);

            using (var json = await _httpClient.Get(String.Format("http://api.hitbox.tv/media/livestream/{0}.json?offset={1}&authtoken={2}", query, offset, authToken), CancellationToken.None).ConfigureAwait(false))
            {
                reg = _jsonSerializer.DeserializeFromStream<RootObject>(json);
            }

            return reg;
        }
    }
}
