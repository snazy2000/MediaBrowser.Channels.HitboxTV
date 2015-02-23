using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBrowser.Channels.HitboxTV
{       
    public class Channel
    {
        public string followers { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_status { get; set; }
        public string user_logo { get; set; }
        public string user_cover { get; set; }
        public string user_logo_small { get; set; }
        public string user_partner { get; set; }
        public string media_is_live { get; set; }
        public string media_live_since { get; set; }
        public string twitter_account { get; set; }
        public string twitter_enabled { get; set; }
        public string livestream_count { get; set; }
        public string channel_link { get; set; }
    }

    public class Request
    {
        public string This { get; set; }
    }

    public class Livestream
    {
        public string media_user_name { get; set; }
        public string media_id { get; set; }
        public string media_file { get; set; }
        public string media_user_id { get; set; }
        public List<string> media_profiles { get; set; }
        public string media_type_id { get; set; }
        public string media_is_live { get; set; }
        public string media_live_delay { get; set; }
        public string media_date_added { get; set; }
        public string media_live_since { get; set; }
        public string media_transcoding { get; set; }
        public string media_chat_enable { get; set; }
        public string[] media_countries { get; set; }
        public string media_hosted_id { get; set; }
        public string user_banned { get; set; }
        public string media_name { get; set; }
        public string media_display_name { get; set; }
        public string media_status { get; set; }
        public string media_title { get; set; }
        public string media_tags { get; set; }
        public string media_duration { get; set; }
        public string media_bg_image { get; set; }
        public string media_views { get; set; }
        public string media_views_daily { get; set; }
        public string media_views_weekly { get; set; }
        public string media_views_monthly { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
        public string category_name_short { get; set; }
        public string category_seo_key { get; set; }
        public string category_viewers { get; set; }
        public string category_media_count { get; set; }
        public string category_channels { get; set; }
        public string category_logo_small { get; set; }
        public string category_logo_large { get; set; }
        public string category_updated { get; set; }
        public string team_name { get; set; }
        public string media_start_in_sec { get; set; }
        public string media_duration_format { get; set; }
        public string media_thumbnail { get; set; }
        public string media_thumbnail_large { get; set; }
        public Channel channel { get; set; }
    }

    public class RootObject
    {
        public Request request { get; set; }
        public string media_type { get; set; }
        public List<Livestream> livestream { get; set; }
    }
}
