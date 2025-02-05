﻿using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class TlevelLoader : ITlevelLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public TlevelLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<TLevelDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var tLevelPathwayInfo = await _internalApiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, id);
            return _mapper.Map<TLevelDetailsViewModel>(tLevelPathwayInfo);
        }

        public async Task<YourTlevelsViewModel> GetYourTlevelsViewModel(long ukprn)
        {
            var tLevels = await _internalApiClient.GetAllTlevelsByUkprnAsync(ukprn);
            return _mapper.Map<YourTlevelsViewModel>(tLevels);
        }

        public async Task<IEnumerable<YourTlevelViewModel>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            var tLevels = await _internalApiClient.GetTlevelsByStatusIdAsync(ukprn, statusId);
            return _mapper.Map<IEnumerable<YourTlevelViewModel>>(tLevels);
        }

        public async Task<SelectToReviewPageViewModel> GetTlevelsToReviewByUkprnAsync(long ukprn)
        {
            var tLevels = await _internalApiClient.GetAllTlevelsByUkprnAsync(ukprn);
            return _mapper.Map<SelectToReviewPageViewModel>(tLevels);
        }

        public async Task<ConfirmTlevelViewModel> GetVerifyTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var tLevelPathwayInfo = await _internalApiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, id);
            return _mapper.Map<ConfirmTlevelViewModel>(tLevelPathwayInfo);
        }

        public async Task<bool> ConfirmTlevelAsync(ConfirmTlevelViewModel viewModel)
        {
            var confirmModel = _mapper.Map<VerifyTlevelDetails>(viewModel);
            return await _internalApiClient.VerifyTlevelAsync(confirmModel);
        }
        
        public async Task<bool> ReportIssueAsync(TlevelQueryViewModel viewModel)
        {
            var queriedModel = _mapper.Map<VerifyTlevelDetails>(viewModel);
            return await _internalApiClient.VerifyTlevelAsync(queriedModel);
        }

        public async Task<TlevelConfirmationViewModel> GetTlevelConfirmationDetailsAsync(long ukprn, int pathwayId)
        {
            var tLevels = await _internalApiClient.GetAllTlevelsByUkprnAsync(ukprn);
            return _mapper.Map<TlevelConfirmationViewModel>(tLevels, opt => opt.Items["pathwayId"] = pathwayId);
        }

        public async Task<TlevelQueryViewModel> GetQueryTlevelViewModelAsync(long ukprn, int id)
        {
            // id is mapped to TqAwardingOrganisation.id in DB
            var tlevelDetails = await _internalApiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, id);
            return _mapper.Map<TlevelQueryViewModel>(tlevelDetails);
        }

        private static IEnumerable<YourTlevelViewModel> FilterTlevelsByStatus(IEnumerable<AwardingOrganisationPathwayStatus> tLevels, TlevelReviewStatus status)
        {
            return tLevels?.Where(x => x.StatusId == (int)status)
                           .Select(x => new YourTlevelViewModel
                           {
                                PathwayId = x.PathwayId,
                                TlevelTitle = x.TlevelTitle
                           });
        }
    }
}
