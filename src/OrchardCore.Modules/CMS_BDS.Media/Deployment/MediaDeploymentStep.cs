using OrchardCore.Deployment;

namespace CMS_BDS.Media.Deployment
{
    /// <summary>
    /// Adds layers to a <see cref="DeploymentPlanResult"/>.
    /// </summary>
    public class MediaDeploymentStep : DeploymentStep
    {
        public MediaDeploymentStep()
        {
            Name = "Media";
        }

        public bool IncludeAll { get; set; } = true;

        public string[] FilePaths { get; set; }

        public string[] DirectoryPaths { get; set; }
    }
}