﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AwardingOrganisationService : IAwardingOrganisationService
    {
        private readonly IRepository<TqAwardingOrganisation> _awardingOrganisationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<TqAwardingOrganisation>> _logger;

        public AwardingOrganisationService(
            IRepository<TqAwardingOrganisation> _repository,
            IMapper mapper,
            ILogger<IRepository<TqAwardingOrganisation>> logger)
        {
            _awardingOrganisationRepository = _repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            var tlevels = await _awardingOrganisationRepository
                .GetManyAsync(x => x.TlAwardingOrganisaton.UkPrn == ukprn,
                        n => n.TlRoute,
                        n => n.TlPathway,
                        n => n.TlAwardingOrganisaton)
                .ToListAsync();

            var awardOrgPathwayStatus = _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(tlevels);
            return awardOrgPathwayStatus;
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            var tlevels = await _awardingOrganisationRepository
               .GetManyAsync(x => x.TlAwardingOrganisaton.UkPrn == ukprn && statusId == x.ReviewStatus,
                       n => n.TlRoute,
                       n => n.TlPathway,
                       n => n.TlAwardingOrganisaton)
               .ToListAsync();

            var awardOrgPathwayStatus = _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(tlevels);
            return awardOrgPathwayStatus;
        }

        public async Task<bool> ConfirmTlevelAsync(ConfirmTlevelDetails model)
        {
            var tqAwardingOrganisation = await _awardingOrganisationRepository.GetSingleOrDefaultAsync(p => p.Id == model.TqAwardingOrganisationId);
            
            if (tqAwardingOrganisation == null) return false;
            
            tqAwardingOrganisation = _mapper.Map(model, tqAwardingOrganisation);
            return await _awardingOrganisationRepository.UpdateAsync(tqAwardingOrganisation) > 0;
        }
    }
}
