﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Registrations
{
    public class When_Upload_File_Header_IsInvalid : RegistrationsCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\Registrations\TestData\Registrations_Stage_2_Header_Validation.csv";

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new RegistrationParser();
            Validator = new RegistrationValidator();
            Service = new CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>(Validator, DataParser, Logger);
            FilePath = Path.Combine(Path.GetDirectoryName(GetCodeBaseAbsolutePath()), _dataFilePath);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ReadAndParseFileResponse.Should().NotBeNull();
            ReadAndParseFileResponse.IsDirty.Should().BeTrue();
            ReadAndParseFileResponse.ErrorMessage.Should().Be(ValidationMessages.FileHeaderNotFound);
        }
    }
}