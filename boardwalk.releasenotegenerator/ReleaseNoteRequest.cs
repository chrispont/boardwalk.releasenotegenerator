namespace boardwalk.releasenotegenerator
{
    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    public class ReleaseNoteRequest
    {
        /// <summary>
        /// Gets or sets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public string BuildNumber { get; set; }

        /// <summary>
        /// Gets or sets the TFS project.
        /// </summary>
        /// <value>
        /// The TFS project.
        /// </value>
        public string TfsProject { get; set; }

        /// <summary>
        /// Gets or sets the TFS query hierarchy.
        /// </summary>
        /// <value>
        /// The TFS query hierarchy.
        /// </value>
        public string TfsQueryHierarchy { get; set; }

        /// <summary>
        /// Gets or sets the name of the TFS query.
        /// </summary>
        /// <value>
        /// The name of the TFS query.
        /// </value>
        public string TfsQueryName { get; set; }

        /// <summary>
        /// Gets or sets the TFS server URL.
        /// </summary>
        /// <value>
        /// The TFS server URL.
        /// </value>
        public string TfsServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the TFS query.
        /// </summary>
        /// <value>
        /// The TFS query.
        /// </value>
        public QueryDefinition TfsQuery { get; set; }

        /// <summary>
        /// Gets or sets the iteration number.
        /// </summary>
        /// <value>
        /// The iteration number.
        /// </value>
        public int IterationNumber { get; set; }

        /// <summary>
        /// Gets or sets the front page template.
        /// </summary>
        /// <value>
        /// The front page template.
        /// </value>
        public string FrontPageTemplate { get; set; }

        /// <summary>
        /// Gets or sets the page template.
        /// </summary>
        /// <value>
        /// The page template.
        /// </value>
        public string PageTemplate { get; set; }

        /// <summary>
        /// Gets or sets the back page template.
        /// </summary>
        /// <value>
        /// The back page template.
        /// </value>
        public string BackPageTemplate { get; set; }

        /// <summary>
        /// Gets or sets the output file.
        /// </summary>
        /// <value>
        /// The output file.
        /// </value>
        public string OutputFile { get; set; }
    }
}
