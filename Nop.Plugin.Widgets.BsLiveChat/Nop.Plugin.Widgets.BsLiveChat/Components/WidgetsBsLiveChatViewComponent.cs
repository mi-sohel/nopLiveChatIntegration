using System.Threading.Tasks;
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

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
             
           // return   new HtmlContentViewComponentResult(new HtmlString(_bsLivesetting.TrackingScript ?? string.Empty));
            return View("~/Plugins/Widgets.BsLiveChat/Views/WidgetsBsLiveChat/PublicInfo.cshtml", _bsLivesetting.TrackingScript);
        }

        

        
    }
}
