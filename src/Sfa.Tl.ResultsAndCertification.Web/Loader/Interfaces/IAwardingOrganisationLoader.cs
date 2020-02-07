﻿using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAwardingOrganisationLoader
    {
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByAwardingOrganisationAsync(int id);
    }
}