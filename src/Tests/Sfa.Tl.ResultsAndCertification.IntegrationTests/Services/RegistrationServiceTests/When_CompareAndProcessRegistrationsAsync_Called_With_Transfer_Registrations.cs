﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    [Collection("BulkProcessTests")]
    public class When_CompareAndProcessRegistrationsAsync_Called_With_Transfer_Registrations : IClassFixture<BulkRegistrationsTextFixture>
    {
        private RegistrationProcessResponse _result;
        private BulkRegistrationsTextFixture _bulkRegistrationTestFixture;

        public When_CompareAndProcessRegistrationsAsync_Called_With_Transfer_Registrations(BulkRegistrationsTextFixture bulkRegistrationTestFixture)
        {
            _bulkRegistrationTestFixture = bulkRegistrationTestFixture;

            // Given
            _bulkRegistrationTestFixture.Uln = 1111111111;
            _bulkRegistrationTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);
            var barnsleyCollegeTqProvider = _bulkRegistrationTestFixture.TqProviders.FirstOrDefault(p => p.TlProvider.UkPrn == 10000536);
            var walsallCollegeTqProvider = _bulkRegistrationTestFixture.TqProviders.FirstOrDefault(p => p.TlProvider.UkPrn == 10007315);

            _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed = _bulkRegistrationTestFixture.SeedRegistrationData(_bulkRegistrationTestFixture.Uln, barnsleyCollegeTqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var seededRegistrationPathways = _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed.TqRegistrationPathways.ToList();
            tqPathwayAssessmentsSeedData.AddRange(_bulkRegistrationTestFixture.GetPathwayAssessmentsDataToProcess(seededRegistrationPathways));
            var _pathwayAssessments = _bulkRegistrationTestFixture.SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            // Results seed
            var results = _bulkRegistrationTestFixture.GetPathwayResultsDataToProcess(_pathwayAssessments);
            var _pathwayResults = _bulkRegistrationTestFixture.SeedPathwayResultsData(results);

            // Input param
            var registrationDataToProcess = _bulkRegistrationTestFixture.GetRegistrationsDataToProcess(_bulkRegistrationTestFixture.Uln, walsallCollegeTqProvider);
            _bulkRegistrationTestFixture.TqRegistrationProfilesData = new List<TqRegistrationProfile> { registrationDataToProcess };
        }

        [Fact]
        public async Task Then_Expected_Registrations_Are_Amended()
        {
            // when
            await _bulkRegistrationTestFixture.WhenAsync();

            // then
            _result = _bulkRegistrationTestFixture.Result;
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();
            _result.BulkUploadStats.Should().NotBeNull();
            _result.BulkUploadStats.TotalRecordsCount.Should().Be(_bulkRegistrationTestFixture.TqRegistrationProfilesData.Count);
            _result.BulkUploadStats.NewRecordsCount.Should().Be(0);
            _result.BulkUploadStats.AmendedRecordsCount.Should().Be(1);
            _result.BulkUploadStats.UnchangedRecordsCount.Should().Be(0);
            _result.ValidationErrors.Should().BeNullOrEmpty();

            var expectedRegistrationProfile = _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln);

            var actualRegistrationProfile = _bulkRegistrationTestFixture.DbContext.TqRegistrationProfile.AsNoTracking().Where(x => x.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                           .ThenInclude(x => x.TqRegistrationSpecialisms)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.TqPathwayAssessments)
                                                                                                                                .ThenInclude(x => x.TqPathwayResults)
                                                                                                                       .FirstOrDefault();
            // Assert registration profile data
            actualRegistrationProfile.Should().NotBeNull();
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);
            actualRegistrationProfile.Firstname.Should().Be(expectedRegistrationProfile.Firstname);
            actualRegistrationProfile.Lastname.Should().Be(expectedRegistrationProfile.Lastname);
            actualRegistrationProfile.DateofBirth.Should().Be(expectedRegistrationProfile.DateofBirth);
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);

            // Assert registration pathway data
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Active).ToList().Count.Should().Be(1);
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Transferred).ToList().Count.Should().Be(1);

            // Assert Transferred Pathway
            var actualTransferredPathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            var expectedTransferredPathway = _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed.TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            AssertRegistrationPathway(actualTransferredPathway, expectedTransferredPathway);

            // Assert Active Pathway
            var activePathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault().TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            var expectedActivePathway = _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault().TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            AssertRegistrationPathway(activePathway, expectedActivePathway);

            // Assert Active PathwayAssessment
            var actualActiveAssessment = activePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedActiveAssessment = expectedActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            AssertPathwayAssessment(actualActiveAssessment, expectedActiveAssessment);

            // Assert Transferred PathwayAssessment
            var actualTransferredAssessment = actualTransferredPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            var expectedTransferredAssessment = expectedTransferredPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayAssessment(actualTransferredAssessment, expectedTransferredAssessment);

            // Assert Active PathwayResult
            var actualActiveResult = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
            var expectedActiveResult = expectedActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
            AssertPathwayResults(actualActiveResult, expectedActiveResult);

            // Assert Transferred PathwayResult
            var actualTransferredResult = actualTransferredAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
            var expectedTransferredResult = expectedTransferredAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayResults(actualTransferredResult, expectedTransferredResult);
        }

        private static void AssertRegistrationPathway(TqRegistrationPathway actualPathway, TqRegistrationPathway expectedPathway)
        {
            actualPathway.Should().NotBeNull();
            actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);
            actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
            actualPathway.Status.Should().Be(expectedPathway.Status);
            actualPathway.IsBulkUpload.Should().Be(expectedPathway.IsBulkUpload);

            // Assert specialisms
            actualPathway.TqRegistrationSpecialisms.Count.Should().Be(expectedPathway.TqRegistrationSpecialisms.Count);

            foreach (var expectedSpecialism in expectedPathway.TqRegistrationSpecialisms)
            {
                var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == expectedSpecialism.TlSpecialismId);

                actualSpecialism.Should().NotBeNull();
                actualSpecialism.TlSpecialismId.Should().Be(expectedSpecialism.TlSpecialismId);
                actualSpecialism.IsOptedin.Should().Be(expectedSpecialism.IsOptedin);
                actualSpecialism.IsBulkUpload.Should().Be(expectedSpecialism.IsBulkUpload);
            }
        }

        private static void AssertPathwayAssessment(TqPathwayAssessment actualAssessment, TqPathwayAssessment expectedAssessment)
        {
            actualAssessment.Should().NotBeNull();
            actualAssessment.TqRegistrationPathwayId.Should().Be(expectedAssessment.TqRegistrationPathwayId);
            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId); 
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeTrue();

            if (actualAssessment.TqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        private static void AssertPathwayResults(TqPathwayResult actualResult, TqPathwayResult expectedResult)
        {
            actualResult.Should().NotBeNull();
            actualResult.TqPathwayAssessmentId.Should().Be(expectedResult.TqPathwayAssessmentId);
            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeTrue();

            if (actualResult.TqPathwayAssessment.TqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Active)
                actualResult.EndDate.Should().BeNull();
            else
                actualResult.EndDate.Should().NotBeNull();
        }
    }
}
