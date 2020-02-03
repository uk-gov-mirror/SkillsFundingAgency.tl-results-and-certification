﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Specialism
{
    public class When_SpecialismRepository_Navigate_IsCalled : BaseTest<TlSpecialism>
    {
        private TlSpecialism _result;
        private TlSpecialism _data;

        public override void Given()
        {
            _data = new TlSpecialismBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Pathway.Should().NotBeNull();
            _result.TlPathwaySpecialismMars.Should().NotBeNull();
        }

        [Fact]
        public void Then_Pathway_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.Pathway;
            
            _result.Pathway.Id.Should().Be(expectedResult.Id);
            _result.Pathway.Name.Should().Be(expectedResult.Name);
            _result.Pathway.LarId.Should().Be(expectedResult.LarId);
            _result.Pathway.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.Pathway.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.Pathway.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.Pathway.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }

        [Fact]
        public void Then_TlPathwaySpecialismMars_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.TlPathwaySpecialismMars.FirstOrDefault();
            var actualResult = _result.TlPathwaySpecialismMars.FirstOrDefault();

            actualResult.PathwayId.Should().Be(expectedResult.PathwayId);
            actualResult.SpecialismId.Should().Be(expectedResult.SpecialismId);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
