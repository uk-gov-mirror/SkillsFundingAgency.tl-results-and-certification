using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests
{
    public abstract class StatementOfAchievementLoaderTestBase : BaseTest<StatementOfAchievementLoader>
    {
        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";

        // Dependencies
        protected IResultsAndCertificationInternalApiClient InternalApiClient;

        protected IMapper Mapper;
        protected ILoggerFactory LoggerFactory;
        protected IHttpContextAccessor HttpContextAccessor;
        protected StatementOfAchievementLoader Loader;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, Givenname),
                    new Claim(ClaimTypes.Surname, Surname),
                    new Claim(ClaimTypes.Email, Email)
                }))
            });

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            LoggerFactory = Substitute.For<ILoggerFactory>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(StatementOfAchievementMapper).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);
            Loader = new StatementOfAchievementLoader(InternalApiClient, Mapper);
        }

        public void CreateMapper()
        {
            LoggerFactory = Substitute.For<ILoggerFactory>();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(StatementOfAchievementMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<SoaLearnerRecordDetailsViewModel, SoaPrintingRequest>(HttpContextAccessor) : null);
            }, LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}