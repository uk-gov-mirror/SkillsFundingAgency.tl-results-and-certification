﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UlnCannotBeRegisteredGet
{
    public abstract class When_UlnCannotBeRegistered_Action_Is_Called : BaseTest<RegistrationController>
    {
        protected Guid UserId;
        protected string CacheKey;
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        protected RegistrationViewModel ViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected TempDataDictionary TempData;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            Controller = new RegistrationController(RegistrationLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<RegistrationController>(Controller)
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.RegistrationCacheKey);
            TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            Controller.TempData = TempData;
        }

        public override void When()
        {
            Result = Controller.UlnCannotBeRegistered();
        }
    }
}