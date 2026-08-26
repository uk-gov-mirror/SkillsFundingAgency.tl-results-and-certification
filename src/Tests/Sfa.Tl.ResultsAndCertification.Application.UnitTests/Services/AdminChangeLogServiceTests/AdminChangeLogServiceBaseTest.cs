using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminChangeLogServiceTests
{
    public abstract class AdminChangeLogServiceBaseTest : BaseTest<AdminChangeLogService>
    {
        protected IAdminChangeLogRepository AdminChangeLogRepository;
        protected AdminChangeLogService AdminChangeLogService;
        protected IMapper Mapper;
        protected ILoggerFactory LoggerFactory;

        public override void Setup()
        {
            LoggerFactory = Substitute.For<ILoggerFactory>();

            Mapper = CreateMapper(LoggerFactory);

            AdminChangeLogRepository = Substitute.For<IAdminChangeLogRepository>();
            AdminChangeLogService = new AdminChangeLogService(AdminChangeLogRepository, Mapper);
        }

        private static AutoMapper.Mapper CreateMapper(ILoggerFactory loggerFactory)
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ChangeLogMapper).Assembly), loggerFactory);
            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}