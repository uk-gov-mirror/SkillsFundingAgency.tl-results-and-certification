﻿using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsByStatusIdAsync
{
    public abstract class When_Called_Method_GetTlevelsByStatusIdAsync : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected readonly int statusId = 9;
        protected readonly long Ukprn = 1024;

        protected int PathwayId = 1;
        protected int StatusId = 1;
        protected string ExpectedTLevelTitle = "Route: Pathway";
        protected string ExpectedPageTitle = "Your T Levels";

        protected IEnumerable<AwardingOrganisationPathwayStatus> ApiClientResponse;
        protected IEnumerable<YourTlevelsViewModel> ActualResult;
        protected AwardingOrganisationPathwayStatus ExpectedResult;

        public override void Setup()
        {
            ExpectedResult = new AwardingOrganisationPathwayStatus { PathwayId = PathwayId, PathwayName = "Pathway", RouteName = "Route", StatusId = 1 };
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus> { ExpectedResult };
            
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetTlevelsByStatusIdAsync(Ukprn, statusId)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TlevelMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

        }

        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.GetTlevelsByStatusIdAsync(Ukprn, statusId).Result;
        }
    }
}