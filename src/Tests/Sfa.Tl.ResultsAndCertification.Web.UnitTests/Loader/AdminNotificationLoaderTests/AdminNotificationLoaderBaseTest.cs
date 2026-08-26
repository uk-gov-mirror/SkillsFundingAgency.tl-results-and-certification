using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminNotificationLoaderTests
{
    public abstract class AdminNotificationLoaderBaseTest : BaseTest<AdminProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminNotificationLoader Loader;
        protected ILoggerFactory LoggerFactory;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            Loader = new AdminNotificationLoader(ApiClient, CreateMapper(LoggerFactory));
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
                    if (type.Equals(typeof(UserNameResolver<AdminEditNotificationViewModel, UpdateNotificationRequest>)))
                    {
                        return new UserNameResolver<AdminEditNotificationViewModel, UpdateNotificationRequest>(httpContextAccessor);
                    }
                    if (type.Equals(typeof(UserNameResolver<AdminAddNotificationViewModel, AddNotificationRequest>)))
                    {
                        return new UserNameResolver<AdminAddNotificationViewModel, AddNotificationRequest>(httpContextAccessor);
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