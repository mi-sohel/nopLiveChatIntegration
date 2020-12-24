using Nop.Plugin.Widgets.BsLiveChat.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
 

namespace Nop.Plugin.Widgets.BsLiveChat.Controllers
{
    [Area(AreaNames.Admin)]
    public class WidgetsBsLiveChatController : BasePluginController
    {
         
        private readonly IStoreContext _storeContext;
 
        private readonly ISettingService _settingService;
      
      
     
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;

        public WidgetsBsLiveChatController( 
            IStoreContext storeContext,         
            ISettingService settingService,         
           
          
            ILocalizationService localizationService,
            INotificationService notificationService)
        {
            
            this._storeContext = storeContext;             
                        
            this._localizationService = localizationService;
            this._notificationService = notificationService;
            this._settingService = settingService;
        }

        [AuthorizeAdmin]
        public IActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var liveChatSettings = _settingService.LoadSetting<BsLiveChatSettings>(storeScope);
            var model = new ConfigurationModel
            {
                TrackingScript = liveChatSettings.TrackingScript,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
              
                model.TrackingScript_OverrideForStore = _settingService.SettingExists(liveChatSettings,
                    x => x.TrackingScript, storeScope);
            }
            return View("~/Plugins/Widgets.BsLiveChat/Views/WidgetsBsLiveChat/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public IActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var liveChatSettings = _settingService.LoadSetting<BsLiveChatSettings>(storeScope);
            liveChatSettings.TrackingScript = model.TrackingScript;
            
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
           
            
            if (model.TrackingScript_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(liveChatSettings, x => x.TrackingScript, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(liveChatSettings, x => x.TrackingScript, storeScope);
            
                
            //now clear settings cache
            _settingService.ClearCache();

           _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }


     
        
        
        
    }
}