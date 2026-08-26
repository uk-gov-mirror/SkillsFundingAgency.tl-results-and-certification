using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver.AdminAssessmentResult;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Mapper.AssessmentToAdminAssessmentViewModel
{
    public abstract class AdminDashboardMapperTestBase : BaseTest<AdminDashboardMapper>
    {
        protected IMapper Mapper;
        protected ILoggerFactory LoggerFactory;
        private ISystemProvider _systemProvider;

        protected int RegistrationPathwayId;
        protected DateTime Today;
        protected Assessment Source;

        protected AdminAssessmentViewModel Result;

        public void Setup(int registrationPathwayId, DateTime today, Assessment source)
        {
            _systemProvider = Substitute.For<ISystemProvider>();

            LoggerFactory = Substitute.For<ILoggerFactory>();

            MapperConfiguration configuration = new(cfg =>
            {
                cfg.AddProfile<AdminDashboardMapper>();
                cfg.ConstructServicesUsing(type =>
                {
                    if (Equals(type, typeof(TableButtonResolver)))
                    {
                        return new TableButtonResolver(_systemProvider);
                    }
                    else if (Equals(type, typeof(IsChangeAllowedResolver)))
                    {
                        return new IsChangeAllowedResolver(_systemProvider);
                    }
                    else
                    {
                        return null;
                    }
                });
            }, LoggerFactory);

            Mapper = configuration.CreateMapper();

            RegistrationPathwayId = registrationPathwayId;
            Today = today;
            Source = source;
        }

        public override void Given()
        {
            _systemProvider.Today.Returns(Today);
        }

        public override Task When()
        {
            Result = Mapper.Map<AdminAssessmentViewModel>(Source, opt =>
            {
                opt.Items[Constants.RegistrationPathwayId] = RegistrationPathwayId;
            });

            return Task.CompletedTask;
        }
    }
}