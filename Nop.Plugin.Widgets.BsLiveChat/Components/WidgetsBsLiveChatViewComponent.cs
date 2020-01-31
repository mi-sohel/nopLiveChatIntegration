using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.BsLiveChat.Components
{
    [ViewComponent(Name = "WidgetsBsLiveChat")]
    public class WidgetsBsLiveChatViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly ILogger _logger;

        public WidgetsBsLiveChatViewComponent(IStoreContext storeContext, 
            IStaticCacheManager cacheManager, 
            ISettingService settingService, 
            IPictureService pictureService, ILogger logger)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
            this._pictureService = pictureService;
            _logger = logger;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            string globalScript = "";

            try
            {




                globalScript += GetTrackingScript();

            }
            catch (Exception ex)
            {
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, "Error creating scripts for  Live Chat", ex.ToString());
            }
            return View("~/Plugins/Widgets.BsLiveChat/Views/WidgetsBsLiveChat/PublicInfo.cshtml", globalScript);
        }

        

        private string GetTrackingScript()
        {
            var liveChatSettings = _settingService.LoadSetting<BsLiveChatSettings>(_storeContext.CurrentStore.Id);
            var analyticsTrackingScript = liveChatSettings.TrackingScript + "\n";

            return analyticsTrackingScript;
        }
    }
}
