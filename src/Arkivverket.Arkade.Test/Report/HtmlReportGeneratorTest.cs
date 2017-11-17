﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arkivverket.Arkade.Core;
using Arkivverket.Arkade.Report;
using Arkivverket.Arkade.Test.Core;
using Arkivverket.Arkade.Tests;
using FluentAssertions;
using Xunit;

namespace Arkivverket.Arkade.Test.Report
{
    public class HtmlReportGeneratorTest
    {
        private static TestSession CreateTestSessionWithTwoTestRuns()
        {
            TestRun testRun1 = new TestRunBuilder()
                .WithDurationMillis(100L)
                .WithTestName("Test 1")
                .WithTestDescription("Test description 1")
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 1"))
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 2"))
                .Build();

            TestRun testRun2 = new TestRunBuilder()
                .WithDurationMillis(100L)
                .WithTestName("Test 2")
                .WithTestDescription("Test description 2")
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 1"))
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 2"))
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 3"))
                .Build();

            var testRuns = new List<TestRun> {testRun1, testRun2};

            TestSession testSession = new TestSessionBuilder()
                .WithTestRuns(testRuns)
                .WithTestSummary(new TestSummary(0, 0, 0))
                .Build();
            return testSession;
        }

        private static string GenerateReport(TestSession testSession)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var htmlReportGenerator = new HtmlReportGenerator(sw);
            htmlReportGenerator.Generate(testSession);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        [Fact]
        public void ShouldGenerateReport()
        {
            TestSession testSession = CreateTestSessionWithTwoTestRuns();

            string html = GenerateReport(testSession);

            html.Should().Contain("<html");
            html.Should().Contain("Test 1");
            html.Should().Contain("Test 2");
            html.Should().Contain("</html>");
        }

        [Fact]
        public void ShouldGenerateReportWithSummaryForAddmlFlatFile()
        {
            TestRun testRun1 = new TestRunBuilder()
                .WithDurationMillis(100L)
                .WithTestName("Test 1")
                .WithTestDescription("Test description 1")
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 1"))
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 2"))
                .Build();

            var testRuns = new List<TestRun> {testRun1};

            TestSession testSession = new TestSessionBuilder()
                .WithArchive(new Archive(ArchiveType.Noark3, null, null))
                .WithTestSummary(new TestSummary(41, 42, 0))
                .WithTestRuns(testRuns)
                .Build();

            testSession.TestSummary = new TestSummary(42, 43, 44);

            string html = GenerateReport(testSession);

            // xunit was not very happy to report errors on a very huge string
            html.Contains("Antall filer").Should().BeTrue();
            html.Contains("Antall poster").Should().BeTrue();
        }

        [Fact]
        public void ShouldGenerateReportWithSummaryForNoark5()
        {
            TestRun testRun1 = new TestRunBuilder()
                .WithDurationMillis(100L)
                .WithTestName("Test 1")
                .WithTestDescription("Test description 1")
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 1"))
                .WithTestResult(new TestResult(ResultType.Error, new Location("location"), "Test result 2"))
                .Build();

            var testRuns = new List<TestRun> {testRun1};

            TestSession testSession = new TestSessionBuilder()
                .WithArchive(new Archive(ArchiveType.Noark5, null, null))
                .WithTestSummary(new TestSummary(0, 0, 44))
                .WithTestRuns(testRuns)
                .Build();

            string html = GenerateReport(testSession);

            // xunit was not very happy to report errors on a very huge string
            html.Contains("Antall filer").Should().BeTrue();
            html.Contains("Antall poster").Should().BeFalse();
        }

        [Fact]
        public void ShouldShowArkadeVersionNumberInReport()
        {
            TestSession testSession = CreateTestSessionWithTwoTestRuns();

            string html = GenerateReport(testSession);
            html.Contains(String.Format(Resources.Report.FooterArkadeVersion, "0.0.0.0")).Should().BeTrue();
        }
    }
}