﻿using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IDfeSignInApiClient
    {
        Task<DfeClaims> GetUserInfo(string organisationId, string userId);
    }
}