﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    interface IResultsAndCertificationInternalApiClient
    {
        Task<IEnumerable<string>> GetAllTlevelsByAwardingOrganisationIdAsync(int id);
    }
}