using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Seo;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

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
        private readonly IStoreContext _storeContext;
        private readonly SeoSettings _seoSettings;

        public GoogleAnalyticPlugin(ISettingService settingService,
            IWebHelper webHelper, 
            ILocalizationService localizationService,
            IStoreContext storeContext,
            SeoSettings seoSettings
            )
        {
            this._settingService = settingService;
            _webHelper = webHelper;
            _localizationService = localizationService;
            _storeContext = storeContext;
            _seoSettings = seoSettings;
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
               PublicWidgetZones.BodyEndHtmlTagBefore
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
            // Adding Meta Tags.
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var seoSettings = _settingService.LoadSetting<SeoSettings>(storeScope);
            var customHeadTags = seoSettings.CustomHeadTags;
            var finalCustomHeadTags = customHeadTags + "<meta name=\"referrer\"content=\"no-referrer-when-downgrade\">";
            seoSettings.CustomHeadTags = finalCustomHeadTags;
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.CustomHeadTags, true, storeScope, false);
            _settingService.ClearCache();

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript", " Live Chat Script code from chat provider: ");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript.Hint", "Paste the tracking code generated from chat provider");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            // Deleting Meta Tags

            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var seoSettings = _settingService.LoadSetting<SeoSettings>(storeScope);
            var customHeadTags = seoSettings.CustomHeadTags;
            var deleteMetaTag = "<meta name=\"referrer\"content=\"no-referrer-when-downgrade\">";
            int start = customHeadTags.IndexOf(deleteMetaTag);
            int lastIndex = start + deleteMetaTag.Length;
            var firstSubstring = "";
            var lastSubstring = "";
            int length = customHeadTags.Length;
            if(start != -1)
            {
                if (start != 0)
                    firstSubstring = customHeadTags.Substring(0, start);
                if (lastIndex != customHeadTags.Length)
                    lastSubstring = customHeadTags.Substring(lastIndex, (length - lastIndex));

                var finalCustomHeadTags = firstSubstring + lastSubstring;
                seoSettings.CustomHeadTags = finalCustomHeadTags;

                _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.CustomHeadTags, true, storeScope, false);
                _settingService.ClearCache();
            }
            

            //settings
            _settingService.DeleteSetting<BsLiveChatSettings>();

            //locales
            
           _localizationService.DeletePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript");
           _localizationService.DeletePluginLocaleResource("Plugins.Widgets.BsLiveChat.TrackingScript.Hint");
            

            base.Uninstall();
        }

       
    }
}
