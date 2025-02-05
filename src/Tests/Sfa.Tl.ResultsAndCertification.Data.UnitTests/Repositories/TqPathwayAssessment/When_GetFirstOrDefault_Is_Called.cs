﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqPathwayAssessment
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.TqPathwayAssessment>
    {
        private Domain.Models.TqPathwayAssessment _result;
        private Domain.Models.TqPathwayAssessment _data;

        public override void Given()
        {
            _data = new TqPathwayAssessmentBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TqRegistrationPathwayId.Should().Be(_data.TqRegistrationPathwayId);
            _result.AssessmentSeriesId.Should().Be(_data.AssessmentSeriesId);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.IsOptedin.Should().Be(_data.IsOptedin);
            _result.IsBulkUpload.Should().Be(_data.IsBulkUpload);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
