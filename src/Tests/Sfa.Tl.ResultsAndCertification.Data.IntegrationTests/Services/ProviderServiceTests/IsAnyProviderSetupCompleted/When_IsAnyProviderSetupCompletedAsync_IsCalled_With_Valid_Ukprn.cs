﻿using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.IsAnyProviderSetupCompleted
{
    public class When_IsAnyProviderSetupCompletedAsync_IsCalled_With_Valid_Ukprn : ProviderServiceBaseTest
    {
        private bool _result;

        public override void Given()
        {            
            SeedTestData();

            ProviderRepositoryLogger = Substitute.For<ILogger<ProviderRepository>>();
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = Substitute.For<ILogger<GenericRepository<TlProvider>>>();
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        public override void When()
        {
            _result = ProviderService.IsAnyProviderSetupCompletedAsync(TlAwardingOrganisation.UkPrn).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            _result.Should().BeTrue();
        }
    }
}
