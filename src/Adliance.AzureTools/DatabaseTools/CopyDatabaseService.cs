using System;
using System.Threading.Tasks;
using Adliance.AzureTools.DatabaseTools.Parameters;

namespace Adliance.AzureTools.DatabaseTools
{
    public class CopyDatabaseService
    {
        private readonly CopyDatabaseParameters _parameters;

        public CopyDatabaseService(CopyDatabaseParameters parameters)
        {
            _parameters = parameters;
        }

        public async Task Run()
        {
            try
            {   var exportDatabaseParameters = ExportDatabaseParametersFromCopyDatabaseParameters(_parameters);
                var exportDatabaseService = new ExportDatabaseService(exportDatabaseParameters);
                var bacpacPath = await exportDatabaseService.Run();
                
                var importDatabaseParameters = ImportDatabaseParametersFromCopyDatabaseParameters(_parameters, bacpacPath);
                var importDatabaseService = new ImportDatabaseService(importDatabaseParameters);
                await importDatabaseService.Run();

            }
            catch (Exception ex)
            {
                Program.Exit(ex);
            }
        }

        public ExportDatabaseParameters ExportDatabaseParametersFromCopyDatabaseParameters(CopyDatabaseParameters parameters)
        {
            return new ExportDatabaseParameters
            {
                Source = parameters.Source,
                Force = !_parameters.UseLocalIfExists
            };
        } 
        
        public ImportDatabaseParameters ImportDatabaseParametersFromCopyDatabaseParameters(CopyDatabaseParameters parameters, string bacpacPath)
        {
            return new ImportDatabaseParameters
            { 
                Source = bacpacPath,
                Target = parameters.Target,
                Force = parameters.Force,
                ElasticPool = parameters.ElasticPool
            };
        } 
    }
}