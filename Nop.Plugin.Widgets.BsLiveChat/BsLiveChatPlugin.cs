using Nop.Core;
using Nop.Core.Domain.Seo;
using Nop.Plugin.Widgets.BsLiveChat.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.BsLiveChat
{
    public class BsLiveChatPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly SeoSettings _seoSettings;

        public BsLiveChatPlugin(ISettingService settingService,
            IWebHelper webHelper,
            ILocalizationService localizationService,
            SeoSettings seoSettings)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _localizationService = localizationService;
            _seoSettings = seoSettings;
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.BodyEndHtmlTagBefore });
        }

        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsBsLiveChat/Configure";
        }

        public override async Task InstallAsync()
        {
            // Adding Meta Tags.

            var customHeadTags = _seoSettings.CustomHeadTags;
            var finalCustomHeadTags = customHeadTags + "<meta name=\"referrer\"content=\"no-referrer-when-downgrade\">";
            _seoSettings.CustomHeadTags = finalCustomHeadTags;
            await _settingService.SaveSettingAsync(_seoSettings, x => x.CustomHeadTags);
            await _settingService.ClearCacheAsync();

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.BsLiveChat.TrackingScript"] = "Live Chat Script code from chat provider:",
                ["Plugins.Widgets.BsLiveChat.TrackingScript.Hint"] = "Paste the tracking code generated from chat provider"
            });

            await base.InstallAsync();
        }

        public override async Task UpdateAsync(string currentVersion, string targetVersion)
        {
            // Adding Meta Tags.

            var customHeadTags = _seoSettings.CustomHeadTags;
            if (string.IsNullOrEmpty(customHeadTags) || !customHeadTags.Contains($"<meta name=\"referrer\"content=\"no-referrer-when-downgrade\">"))
            {
                var finalCustomHeadTags = customHeadTags + $"<meta name=\"referrer\"content=\"no-referrer-when-downgrade\">";
                _seoSettings.CustomHeadTags = finalCustomHeadTags;
                await _settingService.SaveSettingAsync(_seoSettings, x => x.CustomHeadTags);
                await _settingService.ClearCacheAsync();
            }
        }

        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<BsLiveChatSettings>();

            //locales

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.BsLiveChat");
            await base.UninstallAsync();
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            if (widgetZone == null)
                throw new ArgumentNullException(nameof(widgetZone));

            return typeof(WidgetsBsLiveChatViewComponent);
        }

        public bool HideInWidgetList => false;
    }
}