﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsPost
{
    public class Then_Null_ViewModel_Reredirected_To_PageNotFound : When_SelectProviderTlevelsAsync_Post_Action_Is_Called
    {
        public override void Given() { }

        [Fact]
        public void Then_Null_ViewModel_Redirected_To_Route_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}