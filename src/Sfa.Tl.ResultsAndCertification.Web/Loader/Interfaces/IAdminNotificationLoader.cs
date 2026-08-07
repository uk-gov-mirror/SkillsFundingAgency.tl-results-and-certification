using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminNotificationLoader
    {
        AdminFindNotificationCriteriaViewModel LoadFilters();

        Task<AdminFindNotificationViewModel> SearchNotificationAsync(AdminFindNotificationCriteriaViewModel criteria);

        Task<AdminNotificationDetailsViewModel> GetNotificationDetailsViewModel(int notificationId);

        Task<AdminEditNotificationViewModel> GetEditNotificationViewModel(int notificationId);
        
        Task<bool> SubmitUpdateNotificationRequest(AdminEditNotificationViewModel viewModel);

        Task<AdminDeleteNotificationViewModel> GetDeleteNotificationViewModel(int notificationId);
        
        Task<DeleteNotificationResponse> SubmitDeleteNotificationRequest(AdminDeleteNotificationViewModel viewModel);

        Task<AddNotificationResponse> SubmitAddNotificationRequest(AdminAddNotificationViewModel viewModel);
    }
}