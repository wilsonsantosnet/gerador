using Common.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



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

        public static void CloneAndCopy(IEnumerable<ExternalResource> resources)
        {
            foreach (var resource in resources)
            {
                if (!resource.UpdateContinuos)
                    continue;

                clone(resource);

                if (resource.ReplaceLocalFilesApplication)
                    HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1}", resource.ResourceLocalPathFolderCloningRepository, resource.ResourceLocalPathDestinationFolrderApplication), 10000);

            }

        }

        public static void CloneOnly(IEnumerable<ExternalResource> resources)
        {
            foreach (var resource in resources)
            {
                if (!resource.UpdateContinuos)
                    continue;

                clone(resource);
            }

        }

        public static void UpdateLocalRepository(IEnumerable<ExternalResource> resources)
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
                        HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e /xd *\"bin\" *\"obj\"", folderActual.FullName, string.Format("{0}\\{1}", resource.ResourceLocalPathFolderCloningRepository, folderActual.Name)), 10000);
                    }
                }
                else
                {
                    HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e", resource.ResourceLocalPathDestinationFolrderApplication, resource.ResourceLocalPathFolderCloningRepository), 10000);
                }

            }

        }

        # region helper

        private static void clone(ExternalResource resource)
        {
            var bkpPathFolder = string.Format("{0}\\{1}-BKP", AppDomain.CurrentDomain.BaseDirectory, resource.ResouceRepositoryName);

            if (resource.OnlyFoldersContainsThisName.IsNotNullOrEmpty())
            {
                var foldersActual = new DirectoryInfo(resource.ResourceLocalPathDestinationFolrderApplication).GetDirectories()
                    .Where(_ => _.Name.Contains(resource.OnlyFoldersContainsThisName));

                foreach (var folderActual in foldersActual)
                {
                    HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e /xd *\"bin\" *\"obj\"", folderActual.FullName, string.Format("{0}\\{1}", bkpPathFolder, folderActual.Name)), 10000);
                }
            }
            else
            {
                HelperCmd.ExecuteCommand(string.Format("robocopy {0} {1} /s /e", resource.ResourceLocalPathDestinationFolrderApplication, bkpPathFolder), 10000);
            }

            if (Directory.Exists(resource.ResourceLocalPathFolderCloningRepository))
                HelperCmd.ExecuteCommand(string.Format("RMDIR {0} /S /Q", resource.ResourceLocalPathFolderCloningRepository), 10000);

            HelperCmd.ExecuteCommand(string.Format("git clone {0} {1}", resource.ResourceUrlRepository, resource.ResourceLocalPathFolderCloningRepository), 10000);
        }

        #endregion

    }
}
