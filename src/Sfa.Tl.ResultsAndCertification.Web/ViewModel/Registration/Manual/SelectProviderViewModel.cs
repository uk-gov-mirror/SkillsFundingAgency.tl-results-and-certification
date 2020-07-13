﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SelectProviderViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SelectProvider), ErrorMessageResourceName = "Validation_Select_Provider_Required")]
        public string SelectedProviderId { get; set; }

        public IList<SelectListItem> ProvidersSelectList { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AddRegistrationDateofBirth,
                };
            }
        }
    }
}