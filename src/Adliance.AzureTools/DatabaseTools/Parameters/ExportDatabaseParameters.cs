using CommandLine;

namespace Adliance.AzureTools.DatabaseTools.Parameters
{
    [Verb("export-database", HelpText = "Exports an existing database (structure and data) to a bacpac file. Please note that this uses SqlPackage and currently only works on Windows.")]
    public class ExportDatabaseParameters
    {
        [Option('s', "source", Required = true, Default = "", HelpText = "The connection string to the source database.")] public string Source { get; set; } = "";
        [Option('t', "target", Required = true, Default = "", HelpText = "The filename of the bacpac file. If empty, the database name of the source will be used.")] public string Target { get; set; } = "";
        
        [Option('f', "force", Required = false, Default = false, HelpText = "Overwrite existing target file if it exists.")] public bool Force { get; set; } = false;
    }
}
