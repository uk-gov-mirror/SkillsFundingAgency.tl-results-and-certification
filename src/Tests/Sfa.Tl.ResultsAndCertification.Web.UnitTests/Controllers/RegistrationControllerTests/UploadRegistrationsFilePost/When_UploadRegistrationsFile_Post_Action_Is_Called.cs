﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadRegistrationsFilePost
{
    public abstract class When_UploadRegistrationsFile_Post_Action_Is_Called : BaseTest<RegistrationController>
    {
        protected IRegistrationLoader RegistrationLoader;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        protected UploadRegistrationsRequestViewModel ViewModel;
        protected IFormFile FormFile;
        public Task<IActionResult> Result { get; private set; }

        public override void Setup()
        {
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            Controller = new RegistrationController(RegistrationLoader, Logger);
            ViewModel = new UploadRegistrationsRequestViewModel();
        }

        public override void When()
        {
            Result = Controller.UploadRegistrationsFile(ViewModel);
        }
    }
}