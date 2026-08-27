using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminDeleteNotificationGet
{
    public class When_NotificationDetails_Found : AdminNotificationControllerBaseTest
    {
        private const int NotificationId = 1;

        private readonly AdminDeleteNotificationViewModel _viewModel = new()
        {
            NotificationId = NotificationId,
            Title = "title",
            Content = "content",
            StartDay = "10",
            StartMonth = "12",
            StartYear = "2024",
            EndDay = "31",
            EndMonth = "12",
            EndYear = "2024"
        };

        public override void Given()
        {
            AdminNotificationLoader.GetDeleteNotificationViewModel(NotificationId).Returns(_viewModel);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminDeleteNotificationAsync(NotificationId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminNotificationLoader.GetDeleteNotificationViewModel(NotificationId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var viewModel = _result.ShouldBeViewResult<AdminDeleteNotificationViewModel>();
            viewModel.Should().Be(_viewModel);

            viewModel.BackLink.RouteName.Should().Be(RouteConstants.AdminNotificationDetails);
            viewModel.BackLink.RouteAttributes.Should().Contain(Constants.NotificationId, NotificationId.ToString());
        }
    }
}