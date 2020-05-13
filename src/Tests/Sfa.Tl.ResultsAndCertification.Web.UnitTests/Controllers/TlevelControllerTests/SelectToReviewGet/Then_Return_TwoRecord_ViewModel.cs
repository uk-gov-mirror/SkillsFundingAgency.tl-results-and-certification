﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewGet
{
    public class Then_Return_TwoRecord_ViewModel : When_SelecctToReview_Get_Action_Is_Called
    {
        private SelectToReviewPageViewModel mockresult;

        public override void Given()
        {
            mockresult = new SelectToReviewPageViewModel 
            { 
                TlevelsToReview  = new List<TlevelToReviewViewModel> 
                {
                    new TlevelToReviewViewModel { PathwayId = 1, TlevelTitle = "Route1: Pathway1"},
                    new TlevelToReviewViewModel { PathwayId = 2, TlevelTitle = "Route2: Pathway2"}
                } 
            };

            TlevelLoader.GetTlevelsToReviewByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTlevelsToReviewByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetTlevelsToReviewByUkprnAsync(ukprn);
        }

        [Fact]
        public void Then_GetTlevelsToReviewByUkprnAsync_ViewModel_Return_Two_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as SelectToReviewPageViewModel;

            model.Should().NotBeNull();
            model.SelectedPathwayId.Should().Be(mockresult.SelectedPathwayId);
            model.TlevelsToReview.Should().NotBeNull();

            model.TlevelsToReview.Count().Should().Be(2);
        }

        [Fact]
        public void Then_GetTlevelsToReviewByUkprnAsync_Index_Returns_Expected_ViewModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as SelectToReviewPageViewModel;

            model.TlevelsToReview.Should().NotBeNull();
            model.SelectedPathwayId.Should().Be(0);

            var expectedFirstItemModel = mockresult.TlevelsToReview.FirstOrDefault();
            var actualFirstItemModel = model.TlevelsToReview.FirstOrDefault();
            
            expectedFirstItemModel.PathwayId.Should().Be(actualFirstItemModel.PathwayId);
            expectedFirstItemModel.TlevelTitle.Should().Be(actualFirstItemModel.TlevelTitle);

            // Breadcrumb
            model.BreadCrumb.Should().NotBeNull();
            model.BreadCrumb.BreadcrumbItems.Count().Should().Be(2);
            model.BreadCrumb.BreadcrumbItems.First().DisplayName.Should().Be(BreadcrumbContent.Home);
            model.BreadCrumb.BreadcrumbItems.First().RouteName.Should().Be(RouteConstants.Home);
            model.BreadCrumb.BreadcrumbItems.Last().DisplayName.Should().Be(BreadcrumbContent.Tlevel_Review_Select);
            model.BreadCrumb.BreadcrumbItems.Last().RouteName.Should().BeNull();
        }
    }
}
