﻿using FluentAssertions;
using Lrs.PersonalLearningRecordService.Api.Client;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.PersonalLearningRecordService
{
    public class When_Called_With_Empty_Data : TestSetup
    {
        private List<RegistrationLearnerDetails> _registrationLearnerDetails;
        private LearnerVerificationAndLearningEventsResponse _expectedResult;
        private GetLearnerLearningEventsResponse _apiResponse;

        public override void Given()
        {
            _registrationLearnerDetails = new List<RegistrationLearnerDetails>();
            LearnerRecordService.GetPendingVerificationAndLearningEventsLearners().Returns(_registrationLearnerDetails);

            PersonalLearningRecordApiClient.GetLearnerEventsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(_apiResponse);

            _expectedResult = new LearnerVerificationAndLearningEventsResponse { IsSuccess = true, RegistrationsRecordsCount = 0, LrsRecordsCount = 0, ModifiedRecordsCount = 0, SavedRecordsCount = 0 };
            LearnerRecordService.ProcessLearnerRecords(Arg.Any<List<LearnerRecordDetails>>()).Returns(_expectedResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            LearnerRecordService.Received(1).GetPendingVerificationAndLearningEventsLearners();
            PersonalLearningRecordApiClient.DidNotReceive().GetLearnerEventsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>());
            LearnerRecordService.Received(1).ProcessLearnerRecords(Arg.Any<List<LearnerRecordDetails>>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(_expectedResult.IsSuccess);
            ActualResult.RegistrationsRecordsCount.Should().Be(_expectedResult.RegistrationsRecordsCount);
            ActualResult.LrsRecordsCount.Should().Be(_expectedResult.LrsRecordsCount);
            ActualResult.ModifiedRecordsCount.Should().Be(_expectedResult.ModifiedRecordsCount);
            ActualResult.SavedRecordsCount.Should().Be(_expectedResult.SavedRecordsCount);
        }
    }
}
