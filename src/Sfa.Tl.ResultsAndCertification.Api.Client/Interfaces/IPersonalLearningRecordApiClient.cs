﻿using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IPersonalLearningRecordApiClient
    {
        Task<bool> GetLearnerEventsAsync(string uln, string firstName, string lastName, string dateOfBirth);
    }
}
