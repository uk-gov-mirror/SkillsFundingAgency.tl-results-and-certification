﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ResultService : IResultService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRepository<AssessmentSeries> _assessmentSeriesRepository;
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        public ResultService(IAssessmentRepository assessmentRepository, IRepository<AssessmentSeries> assessmentSeriesRepository, IRepository<TlLookup> tlLookupRepository, IResultRepository resultRepository, IMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _tlLookupRepository = tlLookupRepository;
            _resultRepository = resultRepository;
            _mapper = mapper;
        }

        public async Task<IList<ResultRecordResponse>> ValidateResultsAsync(long aoUkprn, IEnumerable<ResultCsvRecordResponse> csvResults)
        {
            var response = new List<ResultRecordResponse>();
            var dbRegistrations = await _resultRepository.GetBulkResultsAsync(aoUkprn, csvResults.Select(x => x.Uln));
            var dbAssessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            var tlLookup = await _tlLookupRepository.GetManyAsync().ToListAsync();
            
            foreach (var result in csvResults)
            {
                // 1. ULN not recognised with AO
                var dbRegistration = dbRegistrations.FirstOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == result.Uln);
                if (dbRegistration == null)
                {
                    response.Add(AddStage3ValidationError(result.RowNum, result.Uln, ValidationMessages.UlnNotRegistered));
                    continue;
                }

                // 2. ULN is withdrawn
                var isWithdrawn = dbRegistration.Status == RegistrationPathwayStatus.Withdrawn;
                if (isWithdrawn)
                {
                    response.Add(AddStage3ValidationError(result.RowNum, result.Uln, ValidationMessages.CannotAddResultToWithdrawnRegistration));
                    continue;
                }

                var validationErrors = new List<BulkProcessValidationError>();
                // 3. Core Code is incorrect
                if (!string.IsNullOrEmpty(result.CoreCode))
                {
                    var isValidCoreCode = dbRegistration.TqProvider.TqAwardingOrganisation.TlPathway.LarId.Equals(result.CoreCode, StringComparison.InvariantCultureIgnoreCase);
                    if (!isValidCoreCode)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreComponentCode));
                        continue;
                    }
                }

                // 4. Assessment Series does not exists
                var isSeriesFound = dbAssessmentSeries.Any(x => x.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                if (!isSeriesFound)
                {
                    validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreAssessmentSeriesEntry));
                    continue;
                }

                // 5. No assessment entry is currently active
                var hasAnyActiveCoreAssessment = dbRegistration.TqPathwayAssessments.Any(pa => pa.IsOptedin && pa.EndDate == null);
                if (!hasAnyActiveCoreAssessment)
                {
                    validationErrors.Add(BuildValidationError(result, ValidationMessages.NoCoreAssessmentEntryCurrentlyActive));
                    continue;
                }

                // 6. Assessment entry mapping error 
                var hasAssessmentSeriesMatchTheSeriesOnRegistrationCore = dbRegistration.TqPathwayAssessments.Any(pa => pa.IsOptedin && pa.EndDate == null && pa.AssessmentSeries.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                if(!hasAssessmentSeriesMatchTheSeriesOnRegistrationCore)
                {
                    validationErrors.Add(BuildValidationError(result, ValidationMessages.AssessmentSeriesDoesNotMatchTheSeriesOnTheRegistration));
                    continue;
                }

                // 7. Core component grade not valid - needs to be A* to E, or Unclassified
                if (!string.IsNullOrEmpty(result.CoreGrade))
                {
                    var tlPathwayCoreComponentGrades = tlLookup.Where(lr => lr.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    var isValidCoreComponentGrade = tlPathwayCoreComponentGrades.Any(pcg => pcg.Value.Equals(result.CoreGrade, StringComparison.InvariantCultureIgnoreCase));
                    if(!isValidCoreComponentGrade)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreComponentGrade));
                        continue;
                    }
                }
            }
            return response;
        }

        public async Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var tqRegistration = await _assessmentRepository.GetAssessmentsAsync(aoUkprn, profileId);

            if (tqRegistration == null || (status != null && tqRegistration.Status != status)) return null;

            return _mapper.Map<ResultDetails>(tqRegistration);
        }

        #region Private Methods

        private ResultRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new ResultRecordResponse()
            {
                ValidationErrors = new List<BulkProcessValidationError>()
                {
                    new BulkProcessValidationError
                    {
                        RowNum = rowNum.ToString(),
                        Uln = uln.ToString(),
                        ErrorMessage = errorMessage
                    }
                }
            };
        }

        private BulkProcessValidationError BuildValidationError(ResultCsvRecordResponse result, string message)
        {
            return new BulkProcessValidationError { RowNum = result.RowNum.ToString(), Uln = result.Uln.ToString(), ErrorMessage = message };
        }

        #endregion
    }
}
