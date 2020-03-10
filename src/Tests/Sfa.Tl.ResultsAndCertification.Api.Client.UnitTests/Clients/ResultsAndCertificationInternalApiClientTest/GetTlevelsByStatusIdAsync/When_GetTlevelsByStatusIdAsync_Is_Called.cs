﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetTlevelsByStatusIdAsync
{
    public class When_GetTlevelsByStatusIdAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected readonly long ukprn = 1024;
        protected Task<IEnumerable<AwardingOrganisationPathwayStatus>> Result;

        protected readonly string RouteName = "Construction";
        protected readonly string PathwayName = "Design, Surveying and Planning";
        protected readonly int StatusId = 1;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected List<AwardingOrganisationPathwayStatus> _mockHttpResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationApiSettings = new ResultsAndCertificationApiSettings { InternalApiUri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { PathwayName = PathwayName, RouteName = RouteName, StatusId = StatusId }
            };            
            
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IEnumerable<AwardingOrganisationPathwayStatus>>(_mockHttpResult, string.Format(ApiConstants.GetTlevelsByStatus, ukprn, StatusId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            Result = _apiClient.GetTlevelsByStatusIdAsync(ukprn, StatusId);
        }
    }
}