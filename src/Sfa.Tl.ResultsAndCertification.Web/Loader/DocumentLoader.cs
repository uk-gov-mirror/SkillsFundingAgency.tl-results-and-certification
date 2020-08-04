﻿using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class DocumentLoader : IDocumentLoader
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<DocumentLoader> _logger;

        public DocumentLoader(ILogger<DocumentLoader> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        public async Task<Stream> GetBulkUploadRegistrationsTechSpecFileAsync(string fileName)
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.Documents.ToString(),
                BlobFileName = fileName,
                SourceFilePath = $"{BlobStorageConstants.TechSpecFolderName}/{BlobStorageConstants.RegistrationsFolderName}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download bulkupload registration tech spec. Method: DownloadFileAsync(ContainerName: {DocumentType.Documents}, BlobFileName = {fileName}, SourceFilePath = {BlobStorageConstants.TechSpecFolderName}/{BlobStorageConstants.RegistrationsFolderName})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }
    }
}
