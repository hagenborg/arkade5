﻿using System.Linq;
using Arkivverket.Arkade.Core.Base;
using Arkivverket.Arkade.Core.Testing.Noark5;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Core.Tests.Testing.Noark5
{
    public class NumberOfChangesLoggedTest
    {
        [Fact]
        public void FindsLogEntries()
        {
            const string testdataDirectory = "TestData\\Noark5\\LogsControl";

            Archive testArchive = TestUtil.CreateArchiveExtraction(testdataDirectory);

            TestRun testRun = new XmlElementHelper().RunEventsOnTest(new NumberOfChangesLogged(testArchive));

            testRun.Results.First().Message.Should().Be("Antall endringer: 2");
        }
    }
}
