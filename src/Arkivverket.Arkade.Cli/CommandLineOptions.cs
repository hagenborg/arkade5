﻿using Arkivverket.Arkade.Core;
using CommandLine;
using CommandLine.Text;

namespace Arkivverket.Arkade.Cli
{
    /// <summary>
    ///     Using CommandLine library for parsing options. See https://github.com/gsscoder/commandline/wiki/
    /// </summary>
    internal class CommandLineOptions
    {
        [Option('a', "archive", HelpText = "Archive directory or file (.tar) to process.")]
        public string Archive { get; set; }

        [Option('t', "type", HelpText = "Archive type, valid values: noark3, noark4, noark5 or fagsystem")]
        public string ArchiveType { get; set; }

        [Option('m', "metadata-file", HelpText = "File with metadata to add to package.")]
        public string MetadataFile { get; set; }

        [Option('g', "generate-metadata-example", HelpText = "Generate example metadata file. Argument is output file name.")]
        public string GenerateMetadataExample { get; set; }

        [Option('v', "verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
        
        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
        
    }
}