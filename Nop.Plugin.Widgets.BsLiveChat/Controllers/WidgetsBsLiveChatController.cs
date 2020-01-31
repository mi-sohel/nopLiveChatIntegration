
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Widgets.BsLiveChat.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.BsLiveChat.Controllers
{
    [Area(AreaNames.Admin)]
    public class WidgetsBsLiveChatController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;

        public WidgetsBsLiveChatController(IWorkContext workContext,
            IStoreContext storeContext, 
            IStoreService storeService,
            ISettingService settingService, 
            IOrderService orderService, 
            ILogger logger, 
            ICategoryService categoryService,
            IProductAttributeParser productAttributeParser,
            ILocalizationService localizationService,INotificationService notificationService)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._orderService = orderService;
            this._logger = logger;
            this._categoryService = categoryService;
            this._productAttributeParser = productAttributeParser;
            this._localizationService = localizationService;
            _notificationService = notificationService;
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