using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetQueryTlevelViewModel
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
    {
        // Dependencies
        protected IResultsAndCertificationInternalApiClient InternalApiClient;

        protected IMapper Mapper;
        protected ILoggerFactory LoggerFactory;

        // Data Objects
        protected TlevelLoader Loader;

        protected TlevelPathwayDetails ApiClientResponse;
        protected TlevelQueryViewModel ActualResult;

        // Params
        protected readonly long Ukprn = 9;

        protected int PathwayId = 11;

        public override void Setup()
        {
            ApiClientResponse = new TlevelPathwayDetails
            {
                TqAwardingOrganisationId = 1,
                RouteId = 2,
                PathwayId = 3,
                RouteName = "Test Route",
                PathwayName = "Test Pathway",
                PathwayCode = "12345678",
                TlevelTitle = "T Level in Education",
                PathwayStatusId = 1,
                Specialisms = new List<SpecialismDetails> {
                    new SpecialismDetails { Name = "Civil Engineering", Code = "97865897" },
                    new SpecialismDetails { Name = "Assisting teaching", Code = "7654321" }
                }
            };

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            InternalApiClient.GetTlevelDetailsByPathwayIdAsync(Ukprn, PathwayId)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TlevelMapper).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public override async Task When()
        {
            ActualResult = await Loader.GetQueryTlevelViewModelAsync(Ukprn, PathwayId);
        }
    }
}