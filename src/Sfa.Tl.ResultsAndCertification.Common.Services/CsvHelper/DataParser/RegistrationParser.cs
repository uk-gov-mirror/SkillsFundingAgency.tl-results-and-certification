﻿using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class RegistrationParser : BaseParser, IDataParser<RegistrationCsvRecordResponse>
    {
        public RegistrationCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is RegistrationCsvRecordRequest reg))
                return null;

            return new RegistrationCsvRecordResponse
            {
                Uln = reg.Uln.Trim().ToLong(),
                FirstName = reg.FirstName.Trim(),
                LastName = reg.LastName.Trim(),
                DateOfBirth = reg.DateOfBirth.Trim().ParseStringToDateTime(),
                ProviderUkprn = reg.Ukprn.Trim().ToLong(),
                AcademicYear = EnumExtensions.GetEnumValueByDisplayName<AcademicYear>(reg.AcademicYear.Trim()).Value,
                CoreCode = reg.Core.Trim(),
                SpecialismCodes = reg.Specialism.Trim().Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())),
                RowNum = rownum,
                ValidationErrors = new List<BulkProcessValidationError>()
            };
        }

        public RegistrationCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult, string errorMessage = null)
        {
            var reg = model as RegistrationCsvRecordRequest;
            var ulnValue = reg != null && reg.Uln.IsLong() ? reg.Uln.ToLong() : 0;

            return new RegistrationCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnValue,
                RowNum = rownum,
                ValidationErrors = BuildValidationError(rownum, ulnValue, validationResult, errorMessage)
            };
        }
    }
}
