﻿using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetTlevelDetailsByPathwayIdAsync_IsCalled_With_Valid_PathwayId_And_Invalid_Ukprn : PathwayServiceBaseTest
    {
        private readonly long _ukprn = 00000;

        public override void Given()
        {
            SeedTlevelTestData(EnumAwardingOrganisation.Pearson);
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TlPathway>>>();
            _service = new PathwayService(Repository, _mapper);
        }

        public override void When()
        {
            _result = _service.GetTlevelDetailsByPathwayIdAsync(_ukprn, _pathway.Id).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Not_Returned()
        {
            _result.Should().BeNull();
        }
    }
}
