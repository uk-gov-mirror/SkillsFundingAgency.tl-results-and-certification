using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminProviderLoaderTests
{
    public abstract class AdminProviderLoaderBaseTest : BaseTest<AdminProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminProviderLoader Loader;
        protected ILoggerFactory LoggerFactory;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            Loader = new AdminProviderLoader(ApiClient, CreateMapper(LoggerFactory));
        }

        private static AutoMapper.Mapper CreateMapper(ILoggerFactory loggerFactory)
        {
            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "test"),
                    new Claim(ClaimTypes.Surname, "user"),
                    new Claim(ClaimTypes.Email, "test.user@test.com")
                }))
            });

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(AdminProviderLoader).Assembly);

                c.ConstructServicesUsing(type =>
                {
                    if (type.Equals(typeof(UserNameResolver<AdminEditProviderViewModel, UpdateProviderRequest>)))
                    {
                        return new UserNameResolver<AdminEditProviderViewModel, UpdateProviderRequest>(httpContextAccessor);
                    }
                    if (type.Equals(typeof(UserNameResolver<AdminAddProviderViewModel, AddProviderRequest>)))
                    {
                        return new UserNameResolver<AdminAddProviderViewModel, AddProviderRequest>(httpContextAccessor);
                    }
                    else
                    {
                        return null;
                    }
                });
            }, loggerFactory);

            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}