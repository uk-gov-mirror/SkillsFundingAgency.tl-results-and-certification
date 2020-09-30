﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireRegistrationsEditorAccess)]
    public class ManageRegistrationController : Controller
    {
        private readonly IRegistrationLoader _registrationLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.RegistrationCacheKey); }
        }

        private string ReregisterCacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.ReregisterCacheKey); }
        }

        public ManageRegistrationController(
            IRegistrationLoader registrationLoader,
            ICacheService cacheService,
            ILogger<ManageRegistrationController> logger)
        {
            _registrationLoader = registrationLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("change-learners-name/{profileId}", Name = RouteConstants.ChangeRegistrationLearnersName)]
        public async Task<IActionResult> ChangeLearnersNameAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeLearnersNameViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeLearnersNameAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("change-learners-name", Name = RouteConstants.SubmitChangeRegistrationLearnersName)]
        public async Task<IActionResult> ChangeLearnersNameAsync(ChangeLearnersNameViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var response = await _registrationLoader.ProcessProfileNameChangeAsync(User.GetUkPrn(), viewModel);

            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { viewModel.ProfileId });

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("change-learners-date-of-birth/{profileId}", Name = RouteConstants.ChangeRegistrationDateofBirth)]
        public async Task<IActionResult> ChangeDateofBirthAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeDateofBirthViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeDateofBirthAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("change-learners-date-of-birth", Name = RouteConstants.SubmitChangeRegistrationDateofBirth)]
        public async Task<IActionResult> ChangeDateofBirthAsync(ChangeDateofBirthViewModel viewModel)
        {
            if (!IsValidDateofBirth(viewModel))
                return View(viewModel);

            var response = await _registrationLoader.ProcessDateofBirthChangeAsync(User.GetUkPrn(), viewModel);

            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { viewModel.ProfileId });

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("registration-details-change-confirmation", Name = RouteConstants.ChangeRegistrationConfirmation)]
        public async Task<IActionResult> ChangeConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ManageRegistrationResponse>(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read ChangeRegistrationConfirmationViewModel from redis cache in change registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("change-provider/{profileId}/{isback:bool?}", Name = RouteConstants.ChangeRegistrationProvider)]
        public async Task<IActionResult> ChangeProviderAsync(int profileId, bool isback = false)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeProviderViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeProviderAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var registeredProviders = await GetAoRegisteredProviders();
            viewModel.ProvidersSelectList = registeredProviders.ProvidersSelectList;
            
            if (isback)
                viewModel.SelectedProviderUkprn = TempData.Get<string>(Constants.ChangeRegistrationCoreNotSupportedProviderUkprn) ?? viewModel.SelectedProviderUkprn;
            
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-provider", Name = RouteConstants.SubmitChangeRegistrationProvider)]
        public async Task<IActionResult> ChangeProviderAsync(ChangeProviderViewModel model)
        {
            if (model == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registeredProviderViewModel = await GetAoRegisteredProviders();

            if (!ModelState.IsValid)
            {
                model.ProvidersSelectList = registeredProviderViewModel.ProvidersSelectList;
                return View(model);
            }
            
            var response = await _registrationLoader.ProcessProviderChangesAsync(User.GetUkPrn(), model);

            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { profileId = model.ProfileId });

            if (response.IsCoreNotSupported)
            {
                TempData.Set(Constants.ChangeRegistrationCoreNotSupportedProviderUkprn, model.SelectedProviderUkprn);
                var providerDetailsModel = new ChangeProviderCoreNotSupportedViewModel { ProviderDisplayName = registeredProviderViewModel?.ProvidersSelectList?.FirstOrDefault(p => p.Value == model.SelectedProviderUkprn)?.Text };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel), providerDetailsModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.ChangeRegistrationCoreQuestion, new { profileId = model.ProfileId });
            }

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("change-core-and-provider/{profileId}", Name = RouteConstants.ChangeRegistrationCoreQuestion)]
        public async Task<IActionResult> ChangeCoreQuestionAsync(int profileId)
        {
            var cacheViewModel = await _cacheService.GetAndRemoveAsync<ChangeProviderCoreNotSupportedViewModel>(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel));
            if (cacheViewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read ChangeProviderCoreNotSupportedViewModel from redis cache in ChangeCoreQuestion page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = await GetChangeCoreQuestionDetailsAsync(profileId, cacheViewModel);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration change core question details found. Method: ChangeCoreQuestionAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }        

        [HttpPost]
        [Route("change-core-and-provider", Name = RouteConstants.SubmitChangeCoreQuestion)]
        public async Task<IActionResult> ChangeCoreQuestionAsync(ChangeCoreQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheViewModel = new ChangeProviderCoreNotSupportedViewModel { ProfileId = model.ProfileId, ProviderDisplayName = model.ProviderDisplayName, CoreDisplayName = model.CoreDisplayName, CanChangeCore = model.CanChangeCore };
            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel), cacheViewModel);
            return RedirectToRoute(model.CanChangeCore == true ? RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw : RouteConstants.ChangeRegistrationProviderNotOfferingSameCore); 
        }

        [HttpGet]
        [Route("change-registration-provider-and-core-need-to-withdraw", Name = RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw)]
        public async Task<IActionResult> ChangeProviderAndCoreNeedToWithdrawAsync()
        {
            var cacheViewModel = await _cacheService.GetAsync<ChangeProviderCoreNotSupportedViewModel>(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel));
            if (cacheViewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read ChangeCoreProviderDetailsViewModel from redis cache in ChangeProviderAndCoreNeedToWithdraw page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new ChangeProviderAndCoreNeedToWithdrawViewModel { ProfileId = cacheViewModel.ProfileId };
            return View(viewModel);
        }

        [HttpGet]
        [Route("provider-not-offering-same-core", Name = RouteConstants.ChangeRegistrationProviderNotOfferingSameCore)]
        public async Task<IActionResult> ChangeProviderNotOfferingSameCoreAsync()
        {
            var cacheViewModel = await _cacheService.GetAsync<ChangeProviderCoreNotSupportedViewModel>(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel));
            if (cacheViewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read ChangeCoreProviderDetailsViewModel from redis cache in ChangeProviderNotOfferingSameCore page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new ChangeProviderNotOfferingSameCoreViewModel { ProfileId = cacheViewModel.ProfileId, ProviderDisplayName = cacheViewModel.ProviderDisplayName, CoreDisplayName = cacheViewModel.CoreDisplayName };
            return View(viewModel);
        }
        
        [HttpGet]
        [Route("change-core/{profileId}", Name = RouteConstants.ChangeRegistrationCore)]
        public async Task<IActionResult> ChangeCoreAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeCoreViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeCoreAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("change-registration-learner-decided-specialism-question/{profileId}", Name = RouteConstants.ChangeRegistrationSpecialismQuestion)]
        public async Task<IActionResult> ChangeRegistrationSpecialismQuestionAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeSpecialismQuestionViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeRegistrationSpecialismQuestionAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-registration-learner-decided-specialism-question", Name = RouteConstants.SubmitChangeRegistrationSpecialismQuestion)]
        public async Task<IActionResult> ChangeRegistrationSpecialismQuestionAsync(ChangeSpecialismQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.HasLearnerDecidedSpecialism.HasValue && model.HasLearnerDecidedSpecialism.Value)
            {
                return RedirectToRoute(RouteConstants.ChangeRegistrationSpecialisms, new { profileId = model.ProfileId });
            }
            else
            {
                var response = await _registrationLoader.ProcessSpecialismQuestionChangeAsync(User.GetUkPrn(), model);

                if (response == null || !response.IsSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
            }
        }

        [HttpGet]
        [Route("change-registration-select-specialism/{profileId}", Name = RouteConstants.ChangeRegistrationSpecialisms)]
        public async Task<IActionResult> ChangeSpecialismsAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeRegistrationSpecialismsAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.PathwaySpecialisms = await GetPathwaySpecialismsAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-registration-select-specialism", Name = RouteConstants.SubmitChangeRegistrationSpecialisms)]
        public async Task<IActionResult> ChangeSpecialismsAsync(ChangeSpecialismViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = await _registrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(User.GetUkPrn(), viewModel.ProfileId);
                if (model == null)
                    return RedirectToRoute(RouteConstants.PageNotFound);

                viewModel.SpecialismCodes = model.SpecialismCodes;
                return View(viewModel);
            }

            var response = await _registrationLoader.ProcessSpecialismChangeAsync(User.GetUkPrn(), viewModel);
            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { viewModel.ProfileId });

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("academic-year-cannot-change/{profileId}", Name = RouteConstants.ChangeAcademicYear)]
        public async Task<IActionResult> ChangeAcademicYearAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeAcademicYearViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeAcademicYearAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("amend-active-registration/{profileId}/{changeStatusId:int?}", Name = RouteConstants.AmendActiveRegistration)]
        public async Task<IActionResult> AmendActiveRegistrationAsync(int profileId, int? changeStatusId)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Active)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: AmendActiveRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var viewModel = new AmendActiveRegistrationViewModel { ProfileId = registrationDetails.ProfileId, ChangeStatusId = changeStatusId };
            viewModel.SetChangeStatus();
            return View(viewModel);
        }

        [HttpPost]
        [Route("amend-active-registration", Name = RouteConstants.SubmitAmendActiveRegistration)]
        public IActionResult AmendActiveRegistrationAsync(AmendActiveRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ChangeStatus == RegistrationChangeStatus.Withdrawn)
            {
                return RedirectToRoute(RouteConstants.WithdrawRegistration, new { profileId = model.ProfileId, withdrawBackLinkOptionId = (int)WithdrawBackLinkOptions.AmendActiveRegistrationPage });
            }

            if (model.ChangeStatus == RegistrationChangeStatus.Delete)
            {
                return RedirectToRoute(RouteConstants.DeleteRegistration, new { profileId = model.ProfileId });
            }
            return View(model);
        }

        [HttpGet]
        [Route("withdraw-registration/{profileId}/{withdrawBackLinkOptionId:int?}", Name = RouteConstants.WithdrawRegistration)]
        public async Task<IActionResult> WithdrawRegistrationAsync(int profileId, int? withdrawBackLinkOptionId)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Active)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: WithdrawRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new WithdrawRegistrationViewModel { ProfileId = registrationDetails.ProfileId, Uln = registrationDetails.Uln, WithdrawBackLinkOptionId = withdrawBackLinkOptionId };
            return View(viewModel);
        }

        [HttpPost]
        [Route("withdraw-registration", Name = RouteConstants.SubmitWithdrawRegistration)]
        public async Task<IActionResult> WithdrawRegistrationAsync(WithdrawRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(!model.CanWithdraw.Value)
            {
                return RedirectToRoute(model.BackLink.RouteName, model.BackLink.RouteAttributes);
            }
            else
            {
                var response = await _registrationLoader.WithdrawRegistrationAsync(User.GetUkPrn(), model);

                if (!response.IsSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.WithdrawRegistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.WithdrawRegistrationConfirmation);
            }
        }

        [HttpGet]
        [Route("registration-withdrawn-confirmation", Name = RouteConstants.WithdrawRegistrationConfirmation)]
        public async Task<IActionResult> WithdrawConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<WithdrawRegistrationResponse>(string.Concat(CacheKey, Constants.WithdrawRegistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read WithdrawRegistrationConfirmationViewModel from redis cache in withdraw registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("amend-withdrawn-registration/{profileId}/{changeStatusId:int?}", Name = RouteConstants.AmendWithdrawRegistration)]
        public async Task<IActionResult> AmendWithdrawRegistrationAsync(int profileId, int? changeStatusId)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: AmendWithdrawRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var viewModel = new AmendWithdrawRegistrationViewModel { ProfileId = registrationDetails.ProfileId, ChangeStatusId = changeStatusId };
            viewModel.SetChangeStatus();
            return View(viewModel);
        }

        [HttpPost]
        [Route("amend-withdrawn-registration", Name = RouteConstants.SubmitAmendWithdrawRegistration)]
        public IActionResult AmendWithdrawRegistrationAsync(AmendWithdrawRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ChangeStatus == RegistrationChangeStatus.Rejoin)
            {
                return RedirectToRoute(RouteConstants.RejoinRegistration, new { profileId = model.ProfileId });
            }
            else if (model.ChangeStatus == RegistrationChangeStatus.Reregister)
            {
                return RedirectToRoute(RouteConstants.ReregisterProvider, new { profileId = model.ProfileId });
            }

            return View(model);
        }

        [HttpGet]
        [Route("reactivate-registration-same-course/{profileId}", Name = RouteConstants.RejoinRegistration)]
        public async Task<IActionResult> RejoinRegistrationAsync(int profileId)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: RejoinRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new RejoinRegistrationViewModel { ProfileId = registrationDetails.ProfileId, Uln = registrationDetails.Uln };
            return View(viewModel);
        }

        [HttpPost]
        [Route("reactivate-registration-same-course", Name = RouteConstants.SubmitRejoinRegistration)]
        public async Task<IActionResult> RejoinRegistrationAsync(RejoinRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.CanRejoin.Value)
            {
                return RedirectToRoute(model.BackLink.RouteName, model.BackLink.RouteAttributes);
            }
            else
            {
                var response = await _registrationLoader.RejoinRegistrationAsync(User.GetUkPrn(), model);

                if (!response.IsSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.RejoinRegistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RejoinRegistrationConfirmation);
            }
        }

        [HttpGet]
        [Route("registration-reactivated-confirmation", Name = RouteConstants.RejoinRegistrationConfirmation)]
        public async Task<IActionResult> RejoinConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<RejoinRegistrationResponse>(string.Concat(CacheKey, Constants.RejoinRegistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read RejoinRegistrationConfirmationViewModel from redis cache in Rejoin registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }


        [HttpGet]
        [Route("register-learner-new-course-select-provider/{profileId}", Name = RouteConstants.ReregisterProvider)]
        public async Task<IActionResult> ReregisterProviderAsync(int profileId)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: ReregisterProviderAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            var registeredProviders = await GetAoRegisteredProviders();
            var viewModel = cacheModel?.ReregisterProvider == null ? new ReregisterProviderViewModel() : cacheModel.ReregisterProvider;
            viewModel.ProfileId = profileId;
            viewModel.ProvidersSelectList = registeredProviders.ProvidersSelectList;
            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-select-provider", Name = RouteConstants.SubmitReregisterProvider)]
        public async Task<IActionResult> ReregisterProviderAsync(ReregisterProviderViewModel model)
        {           
            var registeredProviderViewModel = await GetAoRegisteredProviders();

            if (!ModelState.IsValid)
            {
                model.ProvidersSelectList = registeredProviderViewModel.ProvidersSelectList;
                return View(model);
            }            

            model.SelectedProviderDisplayName = registeredProviderViewModel?.ProvidersSelectList?.FirstOrDefault(p => p.Value == model.SelectedProviderUkprn)?.Text;

            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            if (cacheModel?.ReregisterProvider != null)
                cacheModel.ReregisterProvider = model;
            else
                cacheModel = new ReregisterViewModel { ReregisterProvider = model };

            await _cacheService.SetAsync(ReregisterCacheKey, cacheModel);
            //return RedirectToRoute(RouteConstants.ReregisterProvider, new { profileId = model.ProfileId });

            return RedirectToRoute(RouteConstants.ReregisterSpecialismQuestion);
        }

        [HttpGet]
        [Route("register-learner-new-course-has-learner-decided-specialism", Name = RouteConstants.ReregisterSpecialismQuestion)]
        public async Task<IActionResult> ReregisterSpecialismQuestionAsync()
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            if (cacheModel == null) // TODO: SelectCore 
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SpecialismQuestion == null ? new ReregisterSpecialismQuestionViewModel() : cacheModel.SpecialismQuestion;
            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-has-learner-decided-specialism", Name = RouteConstants.SubmitReregisterSpecialismQuestion)]
        public async Task<IActionResult> ReregisterSpecialismQuestionAsync(ReregisterSpecialismQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);
            if (model == null || cacheModel == null)  // TODO: SelectCore 
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!model.HasLearnerDecidedSpecialism.Value)
            {
                cacheModel.SelectSpecialisms = null;
            }

            cacheModel.SpecialismQuestion = model;
            await _cacheService.SetAsync(ReregisterCacheKey, cacheModel);

            // TODO: route setup
            return RedirectToRoute(model.HasLearnerDecidedSpecialism.Value ? RouteConstants.AddRegistrationSpecialisms : RouteConstants.AddRegistrationAcademicYear);   
        }

        private async Task<SelectProviderViewModel> GetAoRegisteredProviders()
        {
            return await _registrationLoader.GetRegisteredTqAoProviderDetailsAsync(User.GetUkPrn());
        }

        private bool IsValidDateofBirth(ChangeDateofBirthViewModel model)
        {
            var validationerrors = model.DateofBirth.ValidateDate("Date of birth");
            
            if (validationerrors?.Count == 0)
                return true;

            foreach (var error in validationerrors)
                ModelState.AddModelError(error.Key, error.Value);

            return false;
        }

        private async Task<PathwaySpecialismsViewModel> GetPathwaySpecialismsAsync(ChangeSpecialismViewModel viewModel)
        {
            var coreSpecialisms = await _registrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(User.GetUkPrn(), viewModel.CoreCode);
            
            // Update IsSelected flag.
            coreSpecialisms.Specialisms.ToList().ForEach(x => { x.IsSelected = viewModel.SpecialismCodes.Contains(x.Code); });
            
            return coreSpecialisms;
        }

        private async Task<ChangeCoreQuestionViewModel> GetChangeCoreQuestionDetailsAsync(int profileId, ChangeProviderCoreNotSupportedViewModel providerViewModel)
        {
            var coreQuestionDetails = await _registrationLoader.GetRegistrationChangeCoreQuestionDetailsAsync(User.GetUkPrn(), profileId);

            if (coreQuestionDetails != null)
            {
                coreQuestionDetails.ProviderDisplayName = providerViewModel?.ProviderDisplayName;
                coreQuestionDetails.CanChangeCore = providerViewModel.CanChangeCore;
            }
            return coreQuestionDetails;
        }
    }
}