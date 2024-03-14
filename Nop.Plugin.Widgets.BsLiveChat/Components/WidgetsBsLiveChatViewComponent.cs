using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
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
        private readonly BsLiveChatSettings _bsLivesetting;
        private readonly IPictureService _pictureService;
        private readonly ILogger _logger;

        public WidgetsBsLiveChatViewComponent(IStoreContext storeContext, 
            IStaticCacheManager cacheManager,
            BsLiveChatSettings bsLivesetting, 
            IPictureService pictureService, ILogger logger)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._bsLivesetting = bsLivesetting;
            this._pictureService = pictureService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
             
           // return   new HtmlContentViewComponentResult(new HtmlString(_bsLivesetting.TrackingScript ?? string.Empty));
            return View("~/Plugins/Widgets.BsLiveChat/Views/WidgetsBsLiveChat/PublicInfo.cshtml", _bsLivesetting.TrackingScript);
        }

        

        
    }
}
