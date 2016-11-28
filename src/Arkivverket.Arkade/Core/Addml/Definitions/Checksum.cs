﻿
using Arkivverket.Arkade.Util;

namespace Arkivverket.Arkade.Core.Addml.Definitions
{
    public class Checksum
    {
        public string Algorithm { get;  }
        public string Value { get;  }

        public Checksum(string algorithm, string value)
        {
            Assert.AssertNotNullOrEmpty("algorithm", algorithm);
            Assert.AssertNotNullOrEmpty("value", value);

            Algorithm = algorithm;
            Value = value;
        }

    }
}
