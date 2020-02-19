﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PathwayService : IPathwayService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly IMapper _mapper;

        public PathwayService(IRepository<TlPathway> pathwayRepository, IMapper mapper)
        {
            _pathwayRepository = pathwayRepository;
            _mapper = mapper;
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var tlevel = await _pathwayRepository.GetFirstOrDefaultAsync(p => p.Id == id && 
                                                                         p.TqAwardingOrganisations.Any(x => x.TlPathwayId == p.Id && x.TlAwardingOrganisaton.UkPrn == ukprn),
                                                                         navigationPropertyPath: new Expression<Func<TlPathway, object>>[] { r => r.TlRoute, s => s.TlSpecialisms });
            return _mapper.Map<TlevelPathwayDetails>(tlevel);
        }
    }
}
