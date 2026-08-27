using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminDeleteNotificationGet
{
    public class When_NotificationDetails_Not_Found : AdminNotificationControllerBaseTest
    {
        private const int NotificationId = 1; 

        public override void Given()
        {
            AdminNotificationLoader.GetDeleteNotificationViewModel(NotificationId).Returns(null as AdminDeleteNotificationViewModel);
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
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}