using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;
using Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationSpecialismToSpecialismRommExtractData
{
    public class When_Map_Is_Called
    {
        protected IMapper _mapper;
        protected ILoggerFactory _loggerFactory;

        public When_Map_Is_Called()
        {
            _loggerFactory = Substitute.For<ILoggerFactory>();
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(SpecialismRommExtractMapper).Assembly), _loggerFactory);
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Then_Map_Without_Throwing()
        {
            _mapper.Map<SpecialRommExtractionData>(new TqRegistrationSpecialism());
        }
    }
}