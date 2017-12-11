﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arkivverket.Arkade.Core.Noark5;
using Arkivverket.Arkade.Resources;

namespace Arkivverket.Arkade.Tests.Noark5
{
    public class NumberOfEachCaseFolderStatus : Noark5XmlReaderBaseTest
    {
        private string _currentArchivePartSystemId;
        private CaseFolder _currentCaseFolder; // TODO: Support nested case folders?
        private readonly List<CaseFolder> _folders = new List<CaseFolder>();

        public override string GetName()
        {
            return Noark5Messages.NumberOfEachCaseFolderStatus;
        }

        public override TestType GetTestType()
        {
            return TestType.ContentAnalysis;
        }

        protected override List<TestResult> GetTestResults()
        {
            var testResults = new List<TestResult>();

            var folderQuery = from folder in _folders
                group folder by new
                {
                    folder.ArchivePartSystemId,
                    Status = folder.CaseStatus
                }
                into grouped
                select new
                {
                    grouped.Key.ArchivePartSystemId,
                    grouped.Key.Status,
                    Count = grouped.Count()
                };

            bool multipleArchiveParts = _folders.GroupBy(j => j.ArchivePartSystemId).Count() > 1;

            foreach (var item in folderQuery)
            {
                var message = new StringBuilder(
                    string.Format(Noark5Messages.NumberOfEachCaseFolderStatusMessage, item.Status, item.Count));

                if (multipleArchiveParts)
                    message.Insert(0,
                        string.Format(Noark5Messages.ArchivePartSystemId, item.ArchivePartSystemId) + " - ");

                ResultType resultType = item.Status.Equals("Avsluttet") || item.Status.Equals("Utgår")
                    ? ResultType.Success
                    : ResultType.Error;

                testResults.Add(new TestResult(resultType, new Location(string.Empty), message.ToString()));
            }

            return testResults;
        }

        protected override void ReadStartElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadAttributeEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (IdentifiesCaseFolder(eventArgs))
                _currentCaseFolder = new CaseFolder { ArchivePartSystemId = _currentArchivePartSystemId };
        }

        protected override void ReadElementValueEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.Path.Matches("systemID", "arkivdel"))
                _currentArchivePartSystemId = eventArgs.Value;

            if (eventArgs.Path.Matches("saksstatus", "mappe") && _currentCaseFolder != null)
                _currentCaseFolder.CaseStatus = eventArgs.Value;
        }

        protected override void ReadEndElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            // TODO: Handle non-casefolder-type subfolders?

            if (eventArgs.NameEquals("mappe") && _currentCaseFolder != null)
            {
                _folders.Add(_currentCaseFolder);
                _currentCaseFolder = null;
            }
        }

        private static bool IdentifiesCaseFolder(ReadElementEventArgs eventArgs)
        {
            return eventArgs.Path.Matches("mappe") &&
                   eventArgs.Name.Equals("xsi:type") &&
                   eventArgs.Value.Equals("saksmappe");
        }

        private class CaseFolder
        {
            public string ArchivePartSystemId { get; set; }
            public string CaseStatus { get; set; }
        }
    }
}
