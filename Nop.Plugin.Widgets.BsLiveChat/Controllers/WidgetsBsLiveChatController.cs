using Nop.Plugin.Widgets.BsLiveChat.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System.Threading.Tasks;
using Nop.Services.Security;

namespace Nop.Plugin.Widgets.BsLiveChat.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class WidgetsBsLiveChatController : BasePluginController
    {
         
        private readonly IStoreContext _storeContext;
 
        private readonly ISettingService _settingService;
      
      
     
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;

        public WidgetsBsLiveChatController( 
            IStoreContext storeContext,         
            ISettingService settingService,         
           
          
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService)
        {
            
            this._storeContext = storeContext;             
                        
            this._localizationService = localizationService;
            this._notificationService = notificationService;
            this._settingService = settingService;
            _permissionService = permissionService;
        }

       
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var liveChatSettings = await _settingService.LoadSettingAsync<BsLiveChatSettings>(storeScope);
            var model = new ConfigurationModel
            {
                TrackingScript = liveChatSettings.TrackingScript,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
              
                model.TrackingScript_OverrideForStore = await _settingService.SettingExistsAsync(liveChatSettings,
                    x => x.TrackingScript, storeScope);
            }
            return View("~/Plugins/Widgets.BsLiveChat/Views/WidgetsBsLiveChat/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var liveChatSettings = await _settingService.LoadSettingAsync<BsLiveChatSettings>(storeScope);

            liveChatSettings.TrackingScript = model.TrackingScript;
            
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
           
            
 
            await _settingService.SaveSettingOverridablePerStoreAsync(liveChatSettings, x => x.TrackingScript,model.TrackingScript_OverrideForStore, storeScope, false);
                 
            //now clear settings cache
          await  _settingService.ClearCacheAsync();

           _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }


     
        
        
        
    }
}