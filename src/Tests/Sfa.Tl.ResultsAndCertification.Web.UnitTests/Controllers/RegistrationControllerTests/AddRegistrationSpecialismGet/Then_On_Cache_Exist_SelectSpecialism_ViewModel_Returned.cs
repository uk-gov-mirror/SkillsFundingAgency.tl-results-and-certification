﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismGet
{
    public class Then_On_Cache_Exist_SelectSpecialism_ViewModel_Returned : When_AddRegistrationSpecialism_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private SelectCoreViewModel _selectCoreViewModel;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private string _coreCode = "12345678";

        public override void Given()
        {
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreId = _coreCode, CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            cacheResult = new RegistrationViewModel
            {
                SelectCore = _selectCoreViewModel,
                SpecialismQuestion = _specialismQuestionViewModel
            };

            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayName = "Test Pathway", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Id = 1, Code = "345678", Name = "Test Specialism", DisplayName = "Test Specialism (345678)", IsSelected = true } } };
            RegistrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(Ukprn, _coreCode).Returns(_pathwaySpecialismsViewModel);
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_SelectSpecialism_ViewModel_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectSpecialismViewModel));

            var model = viewResult.Model as SelectSpecialismViewModel;
            model.Should().NotBeNull();

            model.HasSpecialismSelected.Should().NotBeNull();
            model.PathwaySpecialisms.Specialisms.Should().NotBeNull();
            model.PathwaySpecialisms.Specialisms.Count.Should().Be(_pathwaySpecialismsViewModel.Specialisms.Count);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationSpecialismQuestion);
        }
    }
}