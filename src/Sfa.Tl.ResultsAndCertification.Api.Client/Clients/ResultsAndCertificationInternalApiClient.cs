﻿using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class ResultsAndCertificationInternalApiClient : IResultsAndCertificationInternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _internalApiUri;
        private readonly ITokenServiceClient _tokenServiceClient;
        private readonly bool _isDevevelopment;

        public ResultsAndCertificationInternalApiClient(HttpClient httpClient, ITokenServiceClient tokenService, ResultsAndCertificationConfiguration configuration)
        {
            _isDevevelopment = configuration.IsDevevelopment;
            _tokenServiceClient = tokenService;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _internalApiUri = configuration.ResultsAndCertificationInternalApiSettings.Uri.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_internalApiUri);
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            return await GetAsync<IEnumerable<AwardingOrganisationPathwayStatus>>(string.Format(ApiConstants.GetAllTLevelsUri, ukprn));
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            return await GetAsync<IEnumerable<AwardingOrganisationPathwayStatus>>(string.Format(ApiConstants.GetTlevelsByStatus, ukprn, statusId));
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var requestUri = string.Format(ApiConstants.TlevelDetailsUri, ukprn, id);
            return await GetAsync<TlevelPathwayDetails>(requestUri);
        }

        public async Task<bool> VerifyTlevelAsync(VerifyTlevelDetails model)
        {
            var requestUri = ApiConstants.VerifyTlevelUri;
            return await PutAsync<VerifyTlevelDetails, bool>(requestUri, model);
        }

        public async Task<bool> AddProviderTlevelsAsync(IList<ProviderTlevel> model)
        {
            var requestUri = ApiConstants.AddProviderTlevelsUri;
            return await PostAsync<IList<ProviderTlevel>, bool>(requestUri, model);
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            var requestUri = string.Format(ApiConstants.IsAnyProviderSetupCompletedUri, ukprn);
            return await GetAsync<bool>(requestUri);
        }

        public async Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch)
        {
            var requestUri = string.Format(ApiConstants.FindProviderAsyncUri, name, isExactMatch);
            return await GetAsync<IEnumerable<ProviderMetadata>>(requestUri);
        }

        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var requestUri = string.Format(ApiConstants.GetSelectProviderTlevelsUri, aoUkprn, providerId);
            return await GetAsync<ProviderTlevels>(requestUri);
        }

        public async Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var requestUri = string.Format(ApiConstants.GetAllProviderTlevelsAsyncUri, aoUkprn, providerId);
            return await GetAsync<ProviderTlevels>(requestUri);
        }

        public async Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn)
        {
            var requestUri = string.Format(ApiConstants.GetTqAoProviderDetailsAsyncUri, aoUkprn);
            return await GetAsync<IList<ProviderDetails>>(requestUri);
        }

        public async Task<ProviderTlevelDetails> GetTqProviderTlevelDetailsAsync(long aoUkprn, int tqProviderId)
        {
            var requestUri = string.Format(ApiConstants.GetTqProviderTlevelDetailsAsyncUri, aoUkprn, tqProviderId);
            return await GetAsync<ProviderTlevelDetails>(requestUri);
        }

        public async Task<bool> RemoveTqProviderTlevelAsync(long aoUkprn, int tqProviderId)
        {
            var requestUri = string.Format(ApiConstants.RemoveTqProviderTlevelAsyncUri, aoUkprn, tqProviderId);
            return await DeleteAsync<bool>(requestUri);
        }

        public async Task<bool> HasAnyTlevelSetupForProviderAsync(long aoUkprn, int tlProviderId)
        {
            var requestUri = string.Format(ApiConstants.HasAnyTlevelSetupForProviderAsyncUri, aoUkprn, tlProviderId);
            return await GetAsync<bool>(requestUri);
        }

        public async Task<BulkProcessResponse> ProcessBulkRegistrationsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkRegistrationsUri;
            return await PostAsync<BulkProcessRequest, BulkProcessResponse>(requestUri, model);
        }

        public async Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetailsAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var requestUri = string.Format(ApiConstants.GetDocumentUploadHistoryDetailsAsyncUri, aoUkprn, blobUniqueReference);
            return await GetAsync<DocumentUploadHistoryDetails>(requestUri);
        }

        public async Task<IList<PathwayDetails>> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn)
        {
            var requestUri = string.Format(ApiConstants.GetRegisteredProviderPathwayDetailsAsyncUri, aoUkprn, providerUkprn);
            return await GetAsync<IList<PathwayDetails>>(requestUri);
        }

        public async Task<PathwaySpecialisms> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId)
        {
            var requestUri = string.Format(ApiConstants.GetPathwaySpecialismsByPathwayLarIdAsyncUri, aoUkprn, pathwayLarId);
            return await GetAsync<PathwaySpecialisms>(requestUri);
        }
        
        public async Task<bool> AddRegistrationAsync(RegistrationRequest model)
        {
            var requestUri = ApiConstants.AddRegistrationUri;
            return await PostAsync<RegistrationRequest, bool>(requestUri, model);
        }

        public async Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln)
        {
            var requestUri = string.Format(ApiConstants.FindUlnUri, aoUkprn, uln);
            return await GetAsync<FindUlnResponse>(requestUri);
        }

        public async Task<RegistrationDetails> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var requestUri = string.Format(ApiConstants.GetRegistrationDetailsUri, aoUkprn, profileId, (int?)status);
            return await GetAsync<RegistrationDetails>(requestUri);
        }

        public async Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId)
        {
            var requestUri = string.Format(ApiConstants.DeleteRegistrationUri, aoUkprn, profileId);
            return await DeleteAsync<bool>(requestUri);
        }

        public async Task<bool> UpdateRegistrationAsync(ManageRegistration model)
        {
            var requestUri = ApiConstants.UpdateRegistrationUri;
            return await PutAsync<ManageRegistration, bool>(requestUri, model);
        }

        public async Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model)
        {
            var requestUri = ApiConstants.WithdrawRegistrationUri;
            return await PutAsync<WithdrawRegistrationRequest, bool>(requestUri, model);
        }

        public async Task<bool> RejoinRegistrationAsync(RejoinRegistrationRequest model)
        {
            var requestUri = ApiConstants.RejoinRegistrationUri;
            return await PutAsync<RejoinRegistrationRequest, bool>(requestUri, model);
        }

        public async Task<bool> ReregistrationAsync(ReregistrationRequest model)
        {
            var requestUri = ApiConstants.ReregistrationUri;
            return await PostAsync<ReregistrationRequest, bool>(requestUri, model);
        }

        // Assessment Related endpoints
        public async Task<BulkAssessmentResponse> ProcessBulkAssessmentsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkAssessmentsUri;
            return await PostAsync<BulkProcessRequest, BulkAssessmentResponse>(requestUri, model);
        }

        public async Task<AssessmentDetails> GetAssessmentDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var requestUri = string.Format(ApiConstants.GetAssessmentDetailsUri, aoUkprn, profileId, (int?)status);
            return await GetAsync<AssessmentDetails>(requestUri);
        }
        
        public async Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, ComponentType componentType)
        {
            var requestUri = string.Format(ApiConstants.GetAvailableAssessmentSeriesUri, aoUkprn, profileId, (int)componentType);
            return await GetAsync<AvailableAssessmentSeries>(requestUri);
        }

        public async Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request)
        {
            var requestUri = ApiConstants.AddAssessmentEntryUri;
            return await PostAsync<AddAssessmentEntryRequest, AddAssessmentEntryResponse>(requestUri, request);
        }

        public async Task<AssessmentEntryDetails> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, ComponentType componentType)
        {
            var requestUri = string.Format(ApiConstants.GetActiveAssessmentEntryDetailsUri, aoUkprn, assessmentId, (int)componentType);
            return await GetAsync<AssessmentEntryDetails>(requestUri);
        }

        public async Task<bool> RemoveAssessmentEntryAsync(RemoveAssessmentEntryRequest model)
        {
            var requestUri = ApiConstants.RemoveAssessmentEntryUri;
            return await PutAsync<RemoveAssessmentEntryRequest, bool>(requestUri, model);
        }

        // Results endpoints
        public async Task<BulkResultResponse> ProcessBulkResultsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkResultsUri;
            return await PostAsync<BulkProcessRequest, BulkResultResponse>(requestUri, model);
        }

        public async Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var requestUri = string.Format(ApiConstants.GetResultDetailsUri, aoUkprn, profileId, (int?)status);
            return await GetAsync<ResultDetails>(requestUri);
        }

        public async Task<AddResultResponse> AddResultAsync(AddResultRequest request)
        {
            var requestUri = ApiConstants.AddResultUri;
            return await PostAsync<AddResultRequest, AddResultResponse>(requestUri, request);
        }

        public async Task<ChangeResultResponse> ChangeResultAsync(ChangeResultRequest model)
        {
            var requestUri = ApiConstants.ChangeResultUri;
            return await PutAsync<ChangeResultRequest, ChangeResultResponse>(requestUri, model);
        }

        public async Task<IList<LookupData>> GetLookupDataAsync(LookupCategory pathwayComponentGrade)
        {
            var requestUri = string.Format(ApiConstants.GetLookupDataUri, (int)pathwayComponentGrade);
            return await GetAsync<IList<LookupData>>(requestUri);
        }

        #region Private Methods

        /// <summary>
        /// Sets the bearer token.
        /// </summary>
        private async Task SetBearerToken()
        {
            if (!_isDevevelopment)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenServiceClient.GetInternalApiToken());
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        private async Task<T> GetAsync<T>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            await SetBearerToken();
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent<TRequest>(content));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

           /// <summary>
        /// Puts the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            await SetBearerToken();
            var response = await _httpClient.PutAsync(requestUri, CreateHttpContent<TRequest>(content));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        private async Task<TResponse> DeleteAsync<TResponse>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.DeleteAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, IsoDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Gets the microsoft date format settings.
        /// </summary>
        /// <value>
        /// The microsoft date format settings.
        /// </value>
        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        /// <summary>
        /// Gets the microsoft date format settings.
        /// </summary>
        /// <value>
        /// The microsoft date format settings.
        /// </value>
        private static JsonSerializerSettings IsoDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                };
            }
        }

        #endregion
    }
}
