using System.Collections.Generic;
using System.Threading.Tasks;
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
       
        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        /// 
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.BodyEndHtmlTagBefore });
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
        public override async Task InstallAsync()
        {
            // Adding Meta Tags.
         
            var customHeadTags = _seoSettings.CustomHeadTags;
            var finalCustomHeadTags = customHeadTags + "<meta name=\"referrer\"content=\"no-referrer-when-downgrade\">";
            _seoSettings.CustomHeadTags = finalCustomHeadTags;
            await _settingService.SaveSettingAsync(_seoSettings, x => x.CustomHeadTags);
            await _settingService.ClearCacheAsync();

           
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.BsLiveChat.TrackingScript"] = "Live Chat Script code from chat provider:",
                ["Plugins.Widgets.BsLiveChat.TrackingScript.Hint"] = "Paste the tracking code generated from chat provider"
            });


            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override async Task UninstallAsync()
        {
            // Deleting Meta Tags

            
            //settings
           await _settingService.DeleteSettingAsync<BsLiveChatSettings>();

            //locales

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.BsLiveChat");
            await base.UninstallAsync();
        }

        public bool HideInWidgetList => false;
    }
}
