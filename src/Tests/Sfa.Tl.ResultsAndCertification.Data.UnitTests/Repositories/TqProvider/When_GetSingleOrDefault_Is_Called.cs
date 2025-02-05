﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqProvider
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.TqProvider>
    {
        private Domain.Models.TqProvider _result;
        private Domain.Models.TqProvider _data;

        public override void Given()
        {
            _data = new TqProviderBuilder().Build();
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
            _result.TqAwardingOrganisationId.Should().Be(_data.TqAwardingOrganisationId);
            _result.TlProviderId.Should().Be(_data.TlProviderId);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
