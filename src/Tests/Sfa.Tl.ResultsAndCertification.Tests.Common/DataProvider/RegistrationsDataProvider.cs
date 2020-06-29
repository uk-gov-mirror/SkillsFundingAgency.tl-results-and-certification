﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class RegistrationsDataProvider
    {
        #region TqRegistrationProfile

        public static TqRegistrationProfile CreateTqRegistrationProfile(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tqRegistrationProfile = new TqRegistrationProfileBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationProfile);
            }
            return tqRegistrationProfile;
        }

        public static TqRegistrationProfile CreateTqRegistrationProfile(ResultsAndCertificationDbContext _dbContext, TqRegistrationProfile tqRegistrationProfile, bool addToDbContext = true)
        {
            if (tqRegistrationProfile == null)
            {
                tqRegistrationProfile = new TqRegistrationProfileBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationProfile);
            }
            return tqRegistrationProfile;
        }

        public static TqRegistrationProfile CreateTqRegistrationProfile(ResultsAndCertificationDbContext _dbContext, long uniqueLearnerNumber, string firstName, string lastName, DateTime dateOfBirth, bool addToDbContext = true)
        {
            var tqRegistrationProfile = new TqRegistrationProfile
            {
                UniqueLearnerNumber = uniqueLearnerNumber,
                Firstname = firstName,
                Lastname = lastName,
                DateofBirth = dateOfBirth
            };

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationProfile);
            }
            return tqRegistrationProfile;
        }

        public static IList<TqRegistrationProfile> CreateTqRegistrationProfiles(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tqRegistrationProfiles = new TqRegistrationProfileBuilder().BuildList();
            
            if (addToDbContext && tqRegistrationProfiles != null)
            {
                foreach (var tqRegistrationProfile in tqRegistrationProfiles)
                {
                    _dbContext.Add(tqRegistrationProfile);
                }
            }
            return tqRegistrationProfiles;
        }

        #endregion

        #region TqRegistrationPathway

        public static TqRegistrationPathway CreateTqRegistrationPathway(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tqRegistrationPathway = new TqRegistrationPathwayBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationPathway);
            }
            return tqRegistrationPathway;
        }

        public static TqRegistrationPathway CreateTqRegistrationPathway(ResultsAndCertificationDbContext _dbContext, TqRegistrationPathway tqRegistrationPathway, bool addToDbContext = true)
        {
            if (tqRegistrationPathway == null)
            {
                tqRegistrationPathway = new TqRegistrationPathwayBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationPathway);
            }
            return tqRegistrationPathway;
        }

        public static TqRegistrationPathway CreateTqRegistrationPathway(ResultsAndCertificationDbContext _dbContext, TqRegistrationProfile tqRegistrationProfile, bool addToDbContext = true)
        {
            if (tqRegistrationProfile == null)
            {
                tqRegistrationProfile = new TqRegistrationProfileBuilder().Build();
            }

            var tqRegistrationPathway = new TqRegistrationPathwayBuilder().Build(tqRegistrationProfile);

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationPathway);
            }
            return tqRegistrationPathway;
        }

        public static TqRegistrationPathway CreateTqRegistrationPathway(ResultsAndCertificationDbContext _dbContext, TqRegistrationProfile tqRegistrationProfile, int tqProviderId, DateTime registrationDate, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, bool addToDbContext = true)
        {
            if (tqRegistrationProfile == null)
            {
                tqRegistrationProfile = new TqRegistrationProfileBuilder().Build();
            }

            var tqRegistrationPathway = new TqRegistrationPathway
            {
                TqRegistrationProfileId = tqRegistrationProfile.Id,
                TqProviderId = tqProviderId,
                RegistrationDate = registrationDate,
                AcademicYear = registrationDate.Year,
                StartDate = DateTime.UtcNow,
                Status = status,
                IsBulkUpload = true
            };

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationPathway);
            }
            return tqRegistrationPathway;
        }

        #endregion

        #region TqRegistrationSpecialism

        public static TqRegistrationSpecialism CreateTqRegistrationSpecialism(ResultsAndCertificationDbContext _dbContext, TqRegistrationSpecialism tqRegistrationSpecialism, bool addToDbContext = true)
        {
            if (addToDbContext && tqRegistrationSpecialism == null)
            {
                _dbContext.Add(tqRegistrationSpecialism);
            }
            return tqRegistrationSpecialism;
        }

        public static TqRegistrationSpecialism CreateTqRegistrationSpecialism(ResultsAndCertificationDbContext _dbContext, TqRegistrationPathway tqRegistrationPathway, int tlSpecialismId, RegistrationSpecialismStatus registrationSpecialismStatus, bool addToDbContext = true)
        {
            if (tqRegistrationPathway == null)
                return null;

            var tqRegistrationSpecialism = new TqRegistrationSpecialism
            {
                TqRegistrationPathwayId = tqRegistrationPathway.Id,
                TqRegistrationPathway = tqRegistrationPathway,
                TlSpecialismId = tlSpecialismId,
                StartDate = DateTime.UtcNow,
                Status = registrationSpecialismStatus,
                IsBulkUpload = true
            };

            if (addToDbContext)
            {
                _dbContext.Add(tqRegistrationSpecialism);
            }
            return tqRegistrationSpecialism;
        }

        public static IList<TqRegistrationSpecialism> CreateTqRegistrationSpecialism(ResultsAndCertificationDbContext _dbContext, TqRegistrationPathway tqRegistrationPathway, bool addToDbContext = true)
        {
            var tqRegistrationSpecialisms = new TqRegistrationSpecialismBuilder().BuildList(tqRegistrationPathway);

            if (addToDbContext && tqRegistrationSpecialisms != null)
            {
                _dbContext.AddRange(tqRegistrationSpecialisms);
            }
            return tqRegistrationSpecialisms;
        }

        #endregion
    }
}