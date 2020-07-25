﻿using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem
{
    public class SummaryItemModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string RouteName { get; set; }
        public Dictionary<string, string> RouteAttributes { get; set; }
        public string ActionText { get; set; }
    }
}