using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{

    public class ExternalResource
    {

        public ExternalResource()
        {
            this.ReplaceLocalFilesApplication = true;
            this.Seed = false;
            this.UpdateContinuos = true;
        }

        public bool ReplaceLocalFilesApplication { get; set; }
        public bool Seed { get; set; }
        public bool UpdateContinuos { get; set; }

        public string ResouceRepositoryName { get; set; }
        public string ResourceUrlRepository { get; set; }
        public string ResourceLocalPathFolderExecuteCloning { get; set; }
        public string ResourceLocalPathDestinationFolrderApplication { get; set; }
        public string OnlyFoldersContainsThisName { get; set; }

        public string ResourceLocalPathFolderCloningRepository
        {
            get
            {
                return string.Format("{0}\\{1}", this.ResourceLocalPathFolderExecuteCloning, this.ResouceRepositoryName);
            }
        }


    }

    public static class HelperExternalResources
    {

        public static void Clone(IEnumerable<ExternalResource> resources)
        {
            foreach (var resource in resources)
            {
                if (!resource.UpdateContinuos)
                    continue;

                var bkpPathFolder = string.Format("{0}\\{1}-BKP", AppDomain.CurrentDomain.BaseDirectory, resource.ResouceRepositoryName);

                if (resource.OnlyFoldersContainsThisName.IsNotNullOrEmpty())
                {
                    var foldersActual = new DirectoryInfo(resource.ResourceLocalPathDestinationFolrderApplication).GetDirectories()
                        .Where(_ => _.Name.Contains(resource.OnlyFoldersContainsThisName));

                    foreach (var folderActual in foldersActual)
                    {
                        ExecuteCommand(string.Format("robocopy {0} {1} /s /e /xd *\"bin\" *\"obj\"", folderActual.FullName, string.Format("{0}\\{1}", bkpPathFolder, folderActual.Name)));
                    }
                }
                else
                {
                    ExecuteCommand(string.Format("robocopy {0} {1} /s /e", resource.ResourceLocalPathDestinationFolrderApplication, bkpPathFolder));
                }

                if (Directory.Exists(resource.ResourceLocalPathFolderCloningRepository))
                    ExecuteCommand(string.Format("RMDIR {0} /S /Q", resource.ResourceLocalPathFolderCloningRepository));

                ExecuteCommand(string.Format("git clone {0} {1}", resource.ResourceUrlRepository, resource.ResourceLocalPathFolderCloningRepository));

                if (resource.ReplaceLocalFilesApplication)
                    ExecuteCommand(string.Format("robocopy {0} {1}", resource.ResourceLocalPathFolderCloningRepository, resource.ResourceLocalPathDestinationFolrderApplication));

            }

        }

        public static void Update(IEnumerable<ExternalResource> resources)
        {
            foreach (var resource in resources)
            {
                if (resource.Seed)
                    continue;

                if (resource.OnlyFoldersContainsThisName.IsNotNullOrEmpty())
                {
                    var foldersActual = new DirectoryInfo(resource.ResourceLocalPathDestinationFolrderApplication).GetDirectories()
                            .Where(_ => _.Name.Contains(resource.OnlyFoldersContainsThisName));

                    foreach (var folderActual in foldersActual)
                    {
                        ExecuteCommand(string.Format("robocopy {0} {1} /s /e *\"bin\" *\"obj\"", folderActual.FullName, string.Format("{0}\\{1}", resource.ResourceLocalPathFolderCloningRepository, folderActual.Name)));
                    }
                }
                else
                {
                    ExecuteCommand(string.Format("robocopy {0} {1} /s /e", resource.ResourceLocalPathDestinationFolrderApplication, resource.ResourceLocalPathFolderCloningRepository));
                }

            }

        }

        static void ExecuteCommand(string command)
        {
            Console.WriteLine("Execute {0}", command);

            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);

            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);
            process.WaitForExit(10000);

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            var exitCode = process.ExitCode;

            if (exitCode == 0)
            {
                Console.WriteLine("Command {0} executed success", command);
            }
            else
            {

                if (!String.IsNullOrEmpty(output)) Console.WriteLine("output: {0}", output);
                if (!String.IsNullOrEmpty(error)) Console.WriteLine("error: {0}", error);

                Console.WriteLine("ExitCode: {0} ", exitCode.ToString());
            }

            PrinstScn.WriteLine("");
            System.Threading.Thread.Sleep(3000);

            process.Close();

        }

    }
}
