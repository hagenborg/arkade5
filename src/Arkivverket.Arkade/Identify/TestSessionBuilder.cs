using Arkivverket.Arkade.Core;
using Serilog;
using System.IO;

namespace Arkivverket.Arkade.Identify
{
    public class TestSessionBuilder : ITestSessionBuilder
    {
        private readonly IArchiveExtractor _archiveExtractor;
        private readonly IArchiveIdentifier _archiveIdentifier;

        public TestSessionBuilder(IArchiveExtractor archiveExtractor, IArchiveIdentifier archiveIdentifier)
        {
            _archiveExtractor = archiveExtractor;
            _archiveIdentifier = archiveIdentifier;
        }

        public TestSession NewSessionFromTarFile(string archiveFileName, string metadataFileName)
        {
            Log.Information($"Building new TestSession with [archiveFileName: {archiveFileName}] [metadataFileName: {metadataFileName}");
            FileInfo archiveFileInfo = new FileInfo(archiveFileName);

            var uuid = Uuid.Of(Path.GetFileNameWithoutExtension(archiveFileName));
            var archiveType = _archiveIdentifier.Identify(metadataFileName);

            DirectoryInfo targetFolderName = _archiveExtractor.Extract(archiveFileInfo);
            Archive archive = new Archive(archiveType, uuid, targetFolderName);
            return new TestSession(archive);
        }
    }
}