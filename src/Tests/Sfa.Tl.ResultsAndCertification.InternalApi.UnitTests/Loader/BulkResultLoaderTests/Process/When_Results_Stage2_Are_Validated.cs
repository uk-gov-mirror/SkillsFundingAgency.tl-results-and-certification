﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkResultLoaderTests.Process
{
    public class When_Results_Stage2_Are_Validated : TestSetup
    {
        public override void Given()
        {
            var csvResponse = new CsvResponseModel<ResultCsvRecordResponse> { IsDirty = true, ErrorCode = CsvFileErrorCode.HeaderInvalid };
            CsvService.ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>()).Returns(csvResponse);

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));

            var expectedWriteFileBytes = new byte[5];
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>());
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}