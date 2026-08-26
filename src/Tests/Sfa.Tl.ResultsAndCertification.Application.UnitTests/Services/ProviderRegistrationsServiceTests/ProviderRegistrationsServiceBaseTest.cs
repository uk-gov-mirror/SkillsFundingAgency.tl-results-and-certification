using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.ProviderRegistrationsServiceTests
{
    public abstract class ProviderRegistrationsServiceBaseTest : BaseTest<SearchRegistrationService>
    {
        protected IProviderRegistrationsRepository ProviderRegistrationsRepository;
        protected IBlobStorageService BlobStorageService;
        protected IMapper Mapper;
        protected ILoggerFactory LoggerFactory;

        protected ProviderRegistrationsService ProviderRegistrationsService;

        public override void Setup()
        {
            LoggerFactory = Substitute.For<ILoggerFactory>();
            ProviderRegistrationsRepository = Substitute.For<IProviderRegistrationsRepository>();
            BlobStorageService = Substitute.For<IBlobStorageService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderRegistrationsMapper).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);

            ProviderRegistrationsService = new ProviderRegistrationsService(ProviderRegistrationsRepository, BlobStorageService, Mapper);
        }
    }
}