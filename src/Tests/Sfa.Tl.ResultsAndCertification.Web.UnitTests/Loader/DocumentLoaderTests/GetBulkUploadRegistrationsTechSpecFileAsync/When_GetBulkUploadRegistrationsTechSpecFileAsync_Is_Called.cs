﻿using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.IO;
using System.Text;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DocumentLoaderTests.GetBulkUploadRegistrationsTechSpecFileAsync
{
    public abstract class When_GetBulkUploadRegistrationsTechSpecFileAsync_Is_Called : BaseTest<DocumentLoader>
    {
        protected ILogger<DocumentLoader> Logger;
        protected IDocumentLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected Stream ActualResult;
        protected string FileName;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<DocumentLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
        }

        public override void Given()
        {
            FileName = DocumentResource.RegistrationDataFormatAndRulesGuide.Registrations_Data_Format_And_Rules_Guide_File_Name_Text;
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test file for registration tech spec")));
            Loader = new DocumentLoader(Logger, BlobStorageService);
        }

        public override void When()
        {
            ActualResult = Loader.GetBulkUploadRegistrationsTechSpecFileAsync(FileName).Result;
        }
    }
}