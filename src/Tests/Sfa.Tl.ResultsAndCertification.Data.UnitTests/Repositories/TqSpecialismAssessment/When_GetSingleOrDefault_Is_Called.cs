﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqSpecialismAssessment
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.TqSpecialismAssessment>
    {
        private Domain.Models.TqSpecialismAssessment _result;
        private Domain.Models.TqSpecialismAssessment _data;

        public override void Given()
        {
            _data = new TqSpecialismAssessmentBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }
        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TqRegistrationSpecialismId.Should().Be(_data.TqRegistrationSpecialismId);
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
