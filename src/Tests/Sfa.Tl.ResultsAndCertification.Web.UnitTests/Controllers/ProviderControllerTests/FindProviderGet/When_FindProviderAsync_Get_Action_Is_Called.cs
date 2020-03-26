﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderGet
{
    public abstract class When_FindProviderAsync_Post_Action_Is_Called : BaseTest<ProviderController>
    {
        // DI Mocks
        protected IProviderLoader ProviderLoader;
        protected ILogger<ProviderController> Logger;

        // input, output and other mock variables
        protected ProviderController Controller;
        protected IActionResult Result;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            Logger = Substitute.For<ILogger<ProviderController>>();
            Controller = new ProviderController(ProviderLoader, Logger);
        }

        public override void When()
        {
            Result = Controller.FindProviderAsync();
        }
    }
}