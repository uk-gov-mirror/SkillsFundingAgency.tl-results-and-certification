﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using Xunit;
using RegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadSuccessful
{
    public class Then_If_TempData_Exists_Expected_Results_Returned : When_UploadSuccessful_Is_Called
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            UploadSuccessfulViewModel = new UploadSuccessfulViewModel { Stats = new ViewModel.BulkUploadStatsViewModel { TotalRecordsCount = 10, NewRecordsCount = 5, AmendedRecordsCount = 3, UnchangedRecordsCount = 2 } };
            TempData[Constants.UploadSuccessfulViewModel] = JsonConvert.SerializeObject(UploadSuccessfulViewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UploadSuccessfulViewModel;

            model.Should().NotBeNull();

            model.Stats.Should().NotBeNull();
            model.Stats.TotalRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.TotalRecordsCount);
            model.Stats.NewRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.NewRecordsCount);
            model.Stats.AmendedRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.AmendedRecordsCount);
            model.Stats.UnchangedRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.UnchangedRecordsCount);
            model.HasMoreThanOneStatsToShow.Should().BeTrue();
            model.SuccessfulRegistrationText.Should().Be(string.Format(RegistrationContent.UploadSuccessful.Successfully_Sent_Total_Registrations_Text, UploadSuccessfulViewModel.Stats.TotalRecordsCount));
        }
    }
}