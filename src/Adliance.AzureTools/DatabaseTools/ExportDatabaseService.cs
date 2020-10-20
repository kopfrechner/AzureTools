using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Adliance.AzureTools.DatabaseTools.Parameters;

namespace Adliance.AzureTools.DatabaseTools
{
    public class ExportDatabaseService
    {
        private readonly ExportDatabaseParameters _parameters;

        public ExportDatabaseService(ExportDatabaseParameters parameters)
        {
            _parameters = parameters;
        }

        public async Task<string> Run()
        {
            var targetFileName = "";
            try
            {
                var sourceDbName = FindDatabaseName(_parameters.Source);
                targetFileName = FindTargetFilePath(_parameters.Target, sourceDbName);
                
                var targetFileInfo = new FileInfo(targetFileName);
                if (!_parameters.Force && targetFileInfo.Exists)
                {
                    Console.WriteLine($"File \"{targetFileInfo.FullName}\" exists. Use --force to overwrite existing bacpac.");
                    return "";
                }
                
                Console.WriteLine($"Downloading database to \"{targetFileName}\" ...");
                SqlPackageAdapter.ExportDatabase(_parameters.Source, targetFileName);
                
                Console.WriteLine("Everything done.");
            }
            catch (Exception ex)
            {
                Program.Exit(ex);
            }
            
            return await Task.FromResult(targetFileName);
        }

        public string FindTargetFilePath(string parametersTarget, string sourceDbName)
        {
            var noTargetNameProvided = string.IsNullOrEmpty(parametersTarget);
            if (noTargetNameProvided)
            {
                parametersTarget = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), $"{sourceDbName}.bacpac");;
            }
            else if (!parametersTarget.EndsWith(".bacpac", StringComparison.InvariantCultureIgnoreCase))
            {
                parametersTarget = Path.Combine(Environment.CurrentDirectory,  $"{parametersTarget}.bacpac");
            }
            else
            {
                parametersTarget = Path.Combine(Environment.CurrentDirectory,  $"{parametersTarget}");
            }
            
            return parametersTarget;
        }


        private string FindDatabaseName(string connectionString)
        {
            var match = Regex.Match(connectionString, @"[ ;]*Initial Catalog[ ]*\=(.*?)[;$]", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            throw new Exception($"No database name found in \"{connectionString}\".");
        }
    }
}