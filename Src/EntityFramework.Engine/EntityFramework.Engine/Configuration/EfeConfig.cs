/*
  <section name="EfeConfig" type="Lc.Core.Caching.CacheConfig,Lc.Core" requirePermission="false" /> 
 
  <EfeConfig Description="缓存配置">
    <ConnectionString Enabled="false" ConnectionString="169.254.60.119:6379,allowAdmin=true,defaultDatabase=0" Description="Redis缓存配置" />
    <Setting Enabled="false" File="~/App_Data/Setting.txt" Description="设置方式" />
  </EfeConfig>

 */
using System;
using System.Configuration;
using System.Xml;

namespace EntityFramework.Engine.Configuration
{
    public partial class EfeConfig : IConfigurationSectionHandler
    {
        #region Fields
        private const string CONFIG_SECTION_NAME = "EfeConfig";
        #endregion

        #region Properties
        //// Settings
        //public bool EnabledSettingFile { get; private set; }
        //public string SettingPath { get; private set; }


        // ConnectionString
        //public bool EnabledConnectionString { get; private set; }
        public string ConnectionString { get; private set; }

        #endregion

        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new EfeConfig();
            var connectionString = section.SelectSingleNode("ConnectionString");
            if (connectionString != null && connectionString.Attributes != null)
            {
                //var enabled = connectionString.Attributes["Enabled"];
                //if (enabled != null)
                //    config.EnabledConnectionString = (Boolean.TryParse(enabled.Value, out bool enabledValue) ? enabledValue : false);

                var value = connectionString.Attributes["ConnectionString"];
                if (value != null)
                    config.ConnectionString = value.Value;
            }


            //var settings = section.SelectSingleNode("SettingFile");
            //if (settings != null && settings.Attributes != null)
            //{
            //    var enabled = settings.Attributes["Enabled"];
            //    if (enabled != null)
            //        config.EnabledSettingFile = (Boolean.TryParse(enabled.Value, out bool enabledValue) ? enabledValue : false);

            //    var file = settings.Attributes["File"];
            //    if (file != null)
            //        config.SettingPath = file.Value;
            //}
            return config;
        }

    }
}
