using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.BsLiveChat.Components
{
    [ViewComponent(Name = "WidgetsBsLiveChat")]
    public class WidgetsBsLiveChatViewComponent : NopViewComponent
    {
        private readonly BsLiveChatSettings _bsLivesetting;

        public WidgetsBsLiveChatViewComponent(BsLiveChatSettings bsLivesetting)
        {
            _bsLivesetting = bsLivesetting;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            return View("~/Plugins/Widgets.BsLiveChat/Views/WidgetsBsLiveChat/PublicInfo.cshtml", _bsLivesetting.TrackingScript);
        }
    }
}