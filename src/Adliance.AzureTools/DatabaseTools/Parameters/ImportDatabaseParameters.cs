using CommandLine;

namespace Adliance.AzureTools.DatabaseTools.Parameters
{
    [Verb("import-database", HelpText = "Imports structure and data from a backup (bacpac) to a target database. If the target database already exists, it will be overwritten. Please note that this uses SqlPackage and currently only works on Windows.")]
    public class ImportDatabaseParameters
    {
        [Option('s', "source", Required = true, Default = "", HelpText = "The source file path of the *.bacpac.")] public string Source { get; set; } = "";
        [Option('t', "target", Required = true, Default = "", HelpText = "The connection string to the target database.")] public string Target { get; set; } = "";
        
        [Option( 'p',"elastic-pool", Required = false, Default = "", HelpText = "If set, the target database will be moved to the specified elastic pool.")] public string ElasticPool { get; set; } = "";

        [Option('f', "force", Required = false, Default = false, HelpText = "Force import-database operation and skip user confirmation or interaction")] public bool Force { get; set; } = false;
    }
}