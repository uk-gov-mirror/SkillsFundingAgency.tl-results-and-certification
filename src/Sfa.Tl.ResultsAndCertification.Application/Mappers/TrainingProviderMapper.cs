﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class TrainingProviderMapper : Profile
    {
        public TrainingProviderMapper()
        {
            CreateMap<TqRegistrationPathway, FindLearnerRecord>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => $"{s.TqRegistrationProfile.Firstname} {s.TqRegistrationProfile.Lastname}"))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.IsLearnerRegistered, opts => opts.MapFrom(s => s.Status == RegistrationPathwayStatus.Active || s.Status == RegistrationPathwayStatus.Withdrawn))
                .ForMember(d => d.HasLrsEnglishAndMaths, opts => opts.MapFrom(s => s.TqRegistrationProfile.IsRcFeed == false && s.TqRegistrationProfile.QualificationAchieved.Any()))
                .ForMember(d => d.HasSendQualification, opts => opts.MapFrom(s => s.TqRegistrationProfile.QualificationAchieved.Any(q => q.Qualification != null && q.Qualification.IsSendQualification)));
        }
    }
}