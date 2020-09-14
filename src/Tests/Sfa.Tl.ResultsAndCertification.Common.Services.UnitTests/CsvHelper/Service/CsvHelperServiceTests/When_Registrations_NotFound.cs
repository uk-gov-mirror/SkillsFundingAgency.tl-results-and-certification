﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public class When_Registrations_NotFound : TestSetup
    {
        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
        }

        [Fact]
        public void Then_Returns_Erorr_HeaderNotFound()
        {
            var actualResult = Response;
            actualResult.IsDirty.Should().BeTrue();
            actualResult.Rows.Count().Should().Be(0);
            actualResult.ErrorMessage.Should().Be(ValidationMessages.NoRecordsFound);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("ULN,First Name,Last Name,Date of Birth,UKPRN,Academic year,Core code,Specialism codes");
            return csvData;
        }
    }
}
