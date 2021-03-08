﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferencePost
{
    public class When_Called_With_Send_Qualification : TestSetup
    {       
        private FindLearnerRecord _learnerRecord;
        private readonly long _uln = 9874561236;

        public override void Given()
        {
            _learnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", IsLearnerRegistered = true, IsSendQualification = true };
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = _uln.ToString() };

            var cacheModel = new AddLearnerRecordViewModel { LearnerRecord = _learnerRecord, Uln = EnterUlnViewModel };

            TrainingProviderLoader.FindLearnerRecordAsync(providerUkprn, _uln).Returns(_learnerRecord);
            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(cacheModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).FindLearnerRecordAsync(providerUkprn, _uln);
        }

        [Fact]
        public void Then_Redirected_To_AddIndustryPlacementQuestion()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.AddIndustryPlacementQuestion);
        }
    }
}
