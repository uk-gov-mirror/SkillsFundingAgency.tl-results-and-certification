using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests
{
    public abstract class AwardingOrganisationServiceBaseTest : BaseTest<TlAwardingOrganisation>
    {
        protected ILoggerFactory LoggerFactory;

        protected AwardingOrganisationService CreateService()
            => new(Repository, CreateMapper(LoggerFactory));

        private static Mapper CreateMapper(ILoggerFactory loggerFactory)
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AwardingOrganisationMapper).Assembly), loggerFactory);
            return new Mapper(mapperConfig);
        }

        protected AwardingOrganisationService Service;

        protected TlAwardingOrganisation Ncfe = new()
        {
            Id = 1,
            UkPrn = 10009696,
            DisplayName = "Ncfe",
            Name = "Ncfe"
        };

        protected TlAwardingOrganisation Pearson = new()
        {
            Id = 2,
            UkPrn = 10011881,
            DisplayName = "Pearson",
            Name = "Pearson"
        };

        public override void Given()
        {
            LoggerFactory = Substitute.For<ILoggerFactory>();

            DbContext.AddRange(new[] { Ncfe, Pearson });
            DbContext.SaveChanges();

            Service = CreateService();
        }
    }
}