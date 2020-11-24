﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AssessmentDetailsViewModel mockresult = null;

        public override void Given()
        {
            mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = "Summer 2021",
                SpecialismDisplayName = "Specialism1 (2345678)",
                SpecialismAssessmentSeries = "Autumn 2022",
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            AssessmentLoader.GetAssessmentDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentDetailsViewModel));

            var model = viewResult.Model as AssessmentDetailsViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.ProviderDisplayName.Should().Be(mockresult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SpecialismDisplayName.Should().Be(mockresult.SpecialismDisplayName);
            model.SpecialismAssessmentSeries.Should().Be(mockresult.SpecialismAssessmentSeries);
            model.PathwayStatus.Should().Be(mockresult.PathwayStatus);

            // Summary CoreAssessment Entry            
            model.SummaryCoreAssessmentEntry.Should().NotBeNull();
            model.SummaryCoreAssessmentEntry.Title.Should().Be(AssessmentDetailsContent.Title_Assessment_Entry_Text);
            model.SummaryCoreAssessmentEntry.Value.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SummaryCoreAssessmentEntry.ActionText.Should().Be(AssessmentDetailsContent.Change_Action_Link_Text);
            model.SummaryCoreAssessmentEntry.RenderHiddenActionText.Should().Be(true);

            if (!string.IsNullOrWhiteSpace(mockresult.PathwayDisplayName))
            {
                model.SummaryCoreAssessmentEntry.HiddenActionText.Should().Be(AssessmentDetailsContent.Change_Core_Assessment_Entry_Hidden_Text);
            }

            // Summary SpecialismAssessment Entry
            var expectedSpecialismActionText = !string.IsNullOrWhiteSpace(mockresult.SpecialismDisplayName) ? AssessmentDetailsContent.Change_Action_Link_Text : null;
            model.SummarySpecialismAssessmentEntry.Should().NotBeNull();
            model.SummarySpecialismAssessmentEntry.Title.Should().Be(AssessmentDetailsContent.Title_Assessment_Entry_Text);
            model.SummarySpecialismAssessmentEntry.Value.Should().Be(mockresult.SpecialismAssessmentSeries);
            model.SummarySpecialismAssessmentEntry.ActionText.Should().Be(expectedSpecialismActionText);
            model.SummarySpecialismAssessmentEntry.RenderHiddenActionText.Should().Be(true);

            if (!string.IsNullOrWhiteSpace(mockresult.SpecialismDisplayName))
            {
                model.SummarySpecialismAssessmentEntry.HiddenActionText.Should().Be(AssessmentDetailsContent.Change_Specialism_Assessment_Entry_Hidden_Text);
            }

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.AssessmentDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Assessment_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchAssessments);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Assessments);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Learners_Assessment_entries);
        }
    }
}