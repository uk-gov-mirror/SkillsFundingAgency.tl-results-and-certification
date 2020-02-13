﻿using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using AutoMapper;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AwardingOrganisationLoader : IAwardingOrganisationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public AwardingOrganisationLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<YourTLevelDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(int id)
        {
            var tLevelPathwayInfo = await _internalApiClient.GetTlevelDetailsByPathwayIdAsync(id);
            var viewModel = _mapper.Map<YourTLevelDetailsViewModel>(tLevelPathwayInfo);
            
            return await Task.Run(() => viewModel);
        }

        public async Task<IEnumerable<YourTlevelsViewModel>> GetTlevelsByAwardingOrganisationAsync()
        {
            var tLevels = await _internalApiClient.GetAllTlevelsByAwardingOrganisationAsync();
            var viewModel = _mapper.Map<IEnumerable<YourTlevelsViewModel>>(tLevels);

            return await Task.Run(() => viewModel);
        }
    }
}
