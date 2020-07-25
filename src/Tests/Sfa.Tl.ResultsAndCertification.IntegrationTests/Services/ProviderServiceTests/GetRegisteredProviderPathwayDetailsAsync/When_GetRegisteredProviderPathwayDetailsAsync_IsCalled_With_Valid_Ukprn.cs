﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetRegisteredProviderPathwayDetailsAsync
{
    public class When_GetRegisteredProviderPathwayDetailsAsync_IsCalled_With_Valid_Ukprn : ProviderServiceBaseTest
    {
        private IList<TlRoute> _routes;
        private IList<TlPathway> _pathways;
        private IList<TqAwardingOrganisation> _tqAwardingOrganisations;
        private IList<PathwayDetails> _result;

        public override void Given()
        {
            SeedTestData();
            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = new Logger<GenericRepository<TlProvider>>(new NullLoggerFactory());
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        public override void When()
        {
            _result = ProviderService.GetRegisteredProviderPathwayDetailsAsync(TlAwardingOrganisation.UkPrn, TlProvider.UkPrn).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            _result.Should().NotBeNull();

            var actualResult = _result.FirstOrDefault();
            actualResult.Id.Should().Be(Pathway.Id);
            actualResult.Code.Should().Be(Pathway.LarId);
            actualResult.Name.Should().Be(Pathway.Name);
        }

        [Fact]
        public void Then_Pathway_Details_Count_Is_As_Expected()
        {
            _result.Count().Should().Be(1);
        }

        protected override void SeedTestData()
        {
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Pearson);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Pearson);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Pearson, Route);
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
            DbContext.SaveChangesAsync();
        }
    }
}