﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class Then_Null_ViewModel_Reredirected_To_PageNotFound : When_ConfirmTlevel_Action_Is_Called
    {
        [Fact]
        public void Then_Null_ViewModel_Redirected_To_Route_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}