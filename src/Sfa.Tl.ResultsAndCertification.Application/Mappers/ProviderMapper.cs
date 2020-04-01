﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<ProviderTlevel, TqProvider>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
                .ForMember(d => d.TlPathwayId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy));

            CreateMap<TlProvider, ProviderDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.UkPrn));

            CreateMap<TlProvider, ProviderMetadata>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName));
        }
    }
}
