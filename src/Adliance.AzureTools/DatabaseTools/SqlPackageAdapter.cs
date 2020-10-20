using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Adliance.AzureTools.DatabaseTools
{
    public class SqlPackageAdapter
    {
        public static void ExportDatabase(string connectionString, string fileName)
        {
            RunSqlPackage(
                "/Action:Export",
                $" /TargetFile:\"{fileName}\"",
                $" /SourceConnectionString:\"{connectionString}\"");
        }

        public static void ImportDatabase(string connectionString, string fileName)
        {
            RunSqlPackage(
                "/Action:Import",
                $" /SourceFile:\"{fileName}\"",
                $" /TargetConnectionString:\"{connectionString}\"");
        }

        private static void RunSqlPackage(params string[] arguments)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase ?? "";
            var assemblyPath = Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
            var sqlPackagePath = new FileInfo(Path.Combine(Path.GetDirectoryName(assemblyPath) ?? "", "CopyDatabase/sqlpackage/sqlpackage.exe"));

            if (!sqlPackagePath.Exists)
            {
                throw new Exception($"{sqlPackagePath.FullName} does not exist.");
            }

            var pi = new ProcessStartInfo(sqlPackagePath.FullName)
            {
                Arguments = string.Join(" ", arguments)
            };

            var currentConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            var process = Process.Start(pi);
            if (process == null)
            {
                throw new Exception("Process is null.");
            }

            process.WaitForExit();
            Console.ForegroundColor = currentConsoleColor;

            if (process.ExitCode != 0)
            {
                throw new Exception($"sqlpackage failed (exit code {process.ExitCode}.");
            }
        }
    }
}