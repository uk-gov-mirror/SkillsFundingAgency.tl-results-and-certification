﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IAssessmentController
    {
        Task<BulkAssessmentResponse> ProcessBulkAssessmentsAsync(BulkProcessRequest request);
        Task<AssessmentDetails> GetAssessmentDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
    }
}