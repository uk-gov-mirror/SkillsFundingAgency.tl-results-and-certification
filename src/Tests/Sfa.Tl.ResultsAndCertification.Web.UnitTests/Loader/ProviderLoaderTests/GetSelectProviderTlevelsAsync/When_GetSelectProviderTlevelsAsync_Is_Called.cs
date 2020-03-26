﻿using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetSelectProviderTlevelsAsync
{
    public abstract class When_GetSelectProviderTlevelsAsync_Is_Called : BaseTest<ProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ProviderLoader Loader;
        protected readonly long Ukprn = 12345678;
        protected readonly int ProviderId = 1;

        protected ProviderTlevels ApiClientResponse;
        protected ProviderTlevelsViewModel ActualResult;

        public override void Setup()
        {
            ApiClientResponse = new ProviderTlevels
            {
                ProviderId = 1,
                DisplayName = "Test1",
                Ukprn = 12345,
                Tlevels = new List<ProviderTlevelDetails>
                    {
                        new ProviderTlevelDetails { TqAwardingOrganisationId = 1, ProviderId = 1, PathwayId = 1, RouteName = "Route1", PathwayName = "Pathway1"},
                        new ProviderTlevelDetails { TqAwardingOrganisationId = 1, ProviderId = 1, PathwayId = 2, RouteName = "Route2", PathwayName = "Pathway2"}
                    }
            };

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetSelectProviderTlevelsAsync(Ukprn, ProviderId)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.GetSelectProviderTlevelsAsync(Ukprn, ProviderId).Result;
        }
    }
}