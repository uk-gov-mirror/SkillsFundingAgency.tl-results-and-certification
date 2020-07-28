﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using RegistrationDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.RegistrationDetails;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationDetails
{
    public class Then_On_Valid_ProfileId_Expected_Results_Returned : When_RegistrationDetails_Action_Is_Called
    {
        private RegistrationDetailsViewModel mockresult = null;
        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                DateofBirth = DateTime.UtcNow,
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
                AcademicYear = 2020
            };

            RegistrationLoader.GetRegistrationDetailsByProfileIdAsync(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_RegistrationDetailsViewModel_Is_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RegistrationDetailsViewModel));

            var model = viewResult.Model as RegistrationDetailsViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.DateofBirth.Should().Be(mockresult.DateofBirth);
            model.ProviderDisplayName.Should().Be(mockresult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.SpecialismsDisplayName.Should().BeEquivalentTo(mockresult.SpecialismsDisplayName);
            model.AcademicYear.Should().Be(mockresult.AcademicYear);


            // Summary LearnerName
            model.SummaryLearnerName.Should().NotBeNull();
            model.SummaryLearnerName.Title.Should().Be(RegistrationDetailsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(mockresult.Name);
            model.SummaryLearnerName.RouteName.Should().Be(RouteConstants.AddRegistrationLearnersName);

            // Summary DateofBirth
            model.SummaryDateofBirth.Should().NotBeNull();
            model.SummaryDateofBirth.Title.Should().Be(RegistrationDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(mockresult.DateofBirth.ToShortDateString());
            model.SummaryDateofBirth.RouteName.Should().Be(RouteConstants.AddRegistrationDateofBirth);

            // Summary Provider
            model.SummaryProvider.Should().NotBeNull();
            model.SummaryProvider.Title.Should().Be(RegistrationDetailsContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(mockresult.ProviderDisplayName);
            model.SummaryProvider.RouteName.Should().Be(RouteConstants.AddRegistrationProvider);

            // Summary Core
            model.SummaryCore.Should().NotBeNull();
            model.SummaryCore.Title.Should().Be(RegistrationDetailsContent.Title_Core_Text);
            model.SummaryCore.Value.Should().Be(mockresult.PathwayDisplayName);
            model.SummaryCore.RouteName.Should().Be(RouteConstants.AddRegistrationCore);

            // Summary Specialisms
            model.SummarySpecialisms.Should().NotBeNull();
            model.SummarySpecialisms.Title.Should().Be(RegistrationDetailsContent.Title_Specialism_Text);
            model.SummarySpecialisms.Value.Should().BeEquivalentTo(mockresult.SpecialismsDisplayName);
            model.SummarySpecialisms.RouteName.Should().Be(RouteConstants.AddRegistrationSpecialism);

            // Summary Academic Year
            model.SummaryAcademicYear.Should().NotBeNull();
            model.SummaryAcademicYear.Title.Should().Be(RegistrationDetailsContent.Title_AcademicYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(mockresult.AcademicYear.ToString());

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RegistrationDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Registration_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchRegistration);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Registration);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Registration_Details);
        }
    }
}