﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationSpecialism
{
    public class When_TqRegistrationSpecialism_Create_Is_Called : BaseTest<Domain.Models.TqRegistrationSpecialism>
    {
        private int _result;
        private Domain.Models.TqRegistrationSpecialism _data;

        public override void Given()
        {
            _data = new TqRegistrationSpecialismBuilder().Build();

        }
        public override void When()
        {
            _result = Repository.CreateAsync(_data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() =>
                _result.Should().Be(1);
    }
}