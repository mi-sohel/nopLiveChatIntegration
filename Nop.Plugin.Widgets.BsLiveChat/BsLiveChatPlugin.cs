using System.Collections.Generic;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Widgets.BsLiveChat
{
    /// <summary>
    /// Live person provider
    /// </summary>
    public class GoogleAnalyticPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;

        

        public GoogleAnalyticPlugin(ISettingService settingService, IWebHelper webHelper, ILocalizationService localizationService)
        {
            this._settingService = settingService;
            _webHelper = webHelper;
            _localizationService = localizationService;
        }
        public bool HideInWidgetList => false;
        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string>
            { 
                "body_end_html_tag_before"
            };
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsBsLiveChat/Configure";
        }

        /// <summary>
        /// Gets a view component for displaying plugin in public store
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <param name="viewComponentName">View component name</param>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsBsLiveChat";
        }
     
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {


           _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript", " Live Chat Script code from chat provider: ");
           _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript.Hint", "Paste the tracking code generated from chat provider");
            

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<BsLiveChatSettings>();

            //locales
            
           _localizationService.DeletePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript");
            _localizationService.DeletePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript.Hint");
            

            base.Uninstall();
        }

       
    }
}
