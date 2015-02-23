﻿using MediaBrowser.Model.Plugins;
using System;

namespace MediaBrowser.Channels.HitboxTV.Configuration
{
    /// <summary>
    /// Class PluginConfiguration
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String authToken { get; set; }
    }
}