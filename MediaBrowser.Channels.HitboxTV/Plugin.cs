using MediaBrowser.Channels.HitboxTV.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBrowser.Channels.HitboxTV
{
    public class Plugin : BasePlugin<PluginConfiguration>
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }
        public override string Name
        {
            get { return "Hitbox TV"; }
        }

        public override string Description
        {
            get
            {
                return "Stream live feeds from Hitbox TV.";
            }
        }

        public static Plugin Instance { get; private set; }
    }
}
