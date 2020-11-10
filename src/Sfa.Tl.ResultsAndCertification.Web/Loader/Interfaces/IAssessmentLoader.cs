﻿using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAssessmentLoader
    {
        Task<UploadAssessmentsResponseViewModel> ProcessBulkAssessmentsAsync(UploadAssessmentsRequestViewModel viewModel);
        Task<Stream> GetAssessmentValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference);
    }
}