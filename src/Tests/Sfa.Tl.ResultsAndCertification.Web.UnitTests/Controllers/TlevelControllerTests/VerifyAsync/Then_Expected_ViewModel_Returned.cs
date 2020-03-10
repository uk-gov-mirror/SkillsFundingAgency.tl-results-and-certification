﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public class Then_Expected_ViewModel_Returned : When_VerifyAsync_Get_Action_Is_Called
    {
        private VerifyTlevelViewModel expectedModel;

        public override void Given()
        {
            pathwayId = 10;
            expectedModel = new VerifyTlevelViewModel 
            {
                TqAwardingOrganisationId = 1,
                RouteId = 2,
                PathwayId = pathwayId,
                PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation,
                IsEverythingCorrect = true,
                PathwayName = "Pathway 1",
                Specialisms = new List<string> { "sp1", "sp2" }
            };

            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId)
                .Returns(expectedModel);
        }

        [Fact]
        public void Then_Expected_ViewModel_Results_Are_Returned()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as VerifyTlevelViewModel;

            model.Should().NotBeNull();
            model.TqAwardingOrganisationId.Should().Be(expectedModel.TqAwardingOrganisationId);
            model.RouteId.Should().Be(expectedModel.RouteId);
            model.PathwayId.Should().Be(expectedModel.PathwayId);
            model.PathwayStatusId.Should().Be(expectedModel.PathwayStatusId);
            model.PathwayName.Should().Be(expectedModel.PathwayName);
            model.IsEverythingCorrect.Should().Be(expectedModel.IsEverythingCorrect);

            model.Specialisms.Should().NotBeNull();
            model.Specialisms.Count().Should().Be(2);
        }
    }
}