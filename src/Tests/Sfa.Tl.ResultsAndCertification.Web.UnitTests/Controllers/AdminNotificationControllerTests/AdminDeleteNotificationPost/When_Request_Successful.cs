using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminDeleteNotificationPost
{
    public class When_Request_Successful : AdminNotificationControllerBaseTest
    {
        private const int NotificationId = 1;

        private readonly AdminDeleteNotificationViewModel _viewModel = new()
        {
            NotificationId = NotificationId,
            Title = "title",
            Content = "content",
            Target = NotificationTarget.Both,
            StartDay = "10",
            StartMonth = "12",
            StartYear = "2024",
            EndDay = "31",
            EndMonth = "12",
            EndYear = "2024"
        };

        private IActionResult _result;

        public override void Given()
        {
            AdminNotificationLoader.SubmitDeleteNotificationRequest(_viewModel).Returns(new DeleteNotificationResponse { Success = true });
        }

        public override async Task When()
        {
            _result = await Controller.AdminDeleteNotificationAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminNotificationLoader.SubmitDeleteNotificationRequest(_viewModel);
            CacheService.Received().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminFindNotification);
        }
    }
}