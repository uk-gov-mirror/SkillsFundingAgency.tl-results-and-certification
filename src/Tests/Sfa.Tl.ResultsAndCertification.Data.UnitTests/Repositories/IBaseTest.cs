﻿using System;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories
{
    public interface IBaseTest<T> : IDisposable
    {
        public void Setup();
        public void Given();
        public void When();

    }
}