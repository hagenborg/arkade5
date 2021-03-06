using System;
using System.IO;
using System.Reflection;
using Arkivverket.Arkade.Core.Base;

namespace Arkivverket.Arkade.Core.Tests.Testing.Noark5
{
    public class TestUtil
    {
        public static string TestDataDirectory =  AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "TestData" + Path.DirectorySeparatorChar;

        public static Archive CreateArchiveExtraction(string testdataDirectory)
        {
            string workingDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\{testdataDirectory}";
            return new Base.ArchiveBuilder()
                .WithArchiveType(ArchiveType.Noark5)
                .WithWorkingDirectoryExternalContent(workingDirectory)
                .Build();
        }

        public static string ReadFromFileInTestDataDir(string fileName)
        {
            return File.ReadAllText(TestDataDirectory + fileName);
        }

    }
}
