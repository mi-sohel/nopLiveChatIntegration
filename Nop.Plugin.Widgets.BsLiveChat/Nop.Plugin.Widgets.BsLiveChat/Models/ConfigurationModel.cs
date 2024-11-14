using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.BsLiveChat.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }
       
        //tracking code
        [NopResourceDisplayName("Plugins.Widgets.BsLiveChat.TrackingScript")]
        public string TrackingScript { get; set; }
        public bool TrackingScript_OverrideForStore { get; set; }
    }
}