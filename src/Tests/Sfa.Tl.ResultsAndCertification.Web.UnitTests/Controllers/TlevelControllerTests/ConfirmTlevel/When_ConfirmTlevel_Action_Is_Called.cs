﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public abstract class When_ConfirmTlevel_Action_Is_Called : BaseTest<TlevelController>
    {
        protected ITlevelLoader TlevelLoader;
        protected TlevelController Controller;
        protected Task<IActionResult> Result;
        protected long ukprn;
        protected VerifyTlevelViewModel InputModel;
        protected TempDataDictionary TempData;

        public override void Setup()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(CustomClaimTypes.Ukprn, ukprn.ToString())
                }))
            });

            TempData = new TempDataDictionary(httpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());

            TlevelLoader = Substitute.For<ITlevelLoader>();
            Controller = new TlevelController(TlevelLoader)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextAccessor.HttpContext
                },

                TempData = TempData
            };
        }

        public override void Given() { }

        public override void When()
        {
            Result = Controller.ConfirmTlevel(InputModel);
        }
    }
}