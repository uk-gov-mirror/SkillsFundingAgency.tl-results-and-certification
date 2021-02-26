using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class LearnerVerificationAndLearningEvents
    {
        private readonly IPersonalLearningRecordService _personalLearningRecordService;
        private readonly ICommonService _commonService;

        public LearnerVerificationAndLearningEvents(ICommonService commonService, IPersonalLearningRecordService personalLearningRecordService)
        {
            _commonService = commonService;
            _personalLearningRecordService = personalLearningRecordService;
        }

        [FunctionName("VerifyLearnerAndFetchLearningEvents")]
        public async Task VerifyLearnerAndFetchLearningEventsAsync([TimerTrigger("%LearnerVerificationAndLearningEventsTrigger%")]TimerInfo timer, ExecutionContext context, ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));

            var functionLogDetails = CommonHelper.CreateFunctionLogRequest(context.FunctionName);

            try
            {
                logger.LogInformation($"Function {context.FunctionName} started");

                var stopwatch = Stopwatch.StartNew();

                await _commonService.CreateFunctionLog(functionLogDetails);

                var response = await _personalLearningRecordService.ProcessLearnerVerificationAndLearningEvents();

                var message = $"Function {context.FunctionName} completed processing.\n" +
                                      $"\tStatus: {(response.IsSuccess ? FunctionStatus.Processed.ToString() : FunctionStatus.Failed.ToString())}\n" +
                                      $"\tTotal registration profiles to process: {response.RegistrationsRecordsCount}\n" +
                                      $"\tLearners retrieved from lrs: {response.LrsRecordsCount}\n" +
                                      $"\tModified registration record to process: {response.ModifiedRecordsCount}\n" +
                                      $"\tRows saved: {response.SavedRecordsCount}\n" +                                      
                                      $"\tAdditional message: {response.Message}";

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, response.IsSuccess ? FunctionStatus.Processed : FunctionStatus.Failed, message);

                await _commonService.UpdateFunctionLog(functionLogDetails);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} completed processing. Time taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch(Exception ex)
            {
                var errorMessage = $"Function {context.FunctionName} failed to process with the following exception = {ex}";
                logger.LogError(errorMessage);

                CommonHelper.UpdateFunctionLogRequest(functionLogDetails, FunctionStatus.Failed, errorMessage);

                _ = (functionLogDetails.Id > 0) ? await _commonService.UpdateFunctionLog(functionLogDetails) : await _commonService.CreateFunctionLog(functionLogDetails);
                
                throw;
            }
        }
    }
}