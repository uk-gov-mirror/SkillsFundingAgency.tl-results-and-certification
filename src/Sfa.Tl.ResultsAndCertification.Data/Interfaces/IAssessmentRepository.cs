﻿using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IAssessmentRepository : IRepository<TqPathwayAssessment>
    {
        Task<bool> BulkInsertOrUpdateAssessments(List<TqPathwayAssessment> pathwayAssessmentEntities, List<TqSpecialismAssessment> specialismAssessmentEntities);
    }
}
