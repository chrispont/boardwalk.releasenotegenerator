using System;
using System.Collections.Generic;
using System.Linq;

namespace boardwalk.releasenotegenerator
{
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    public class ReleaseNoteRunner
    {
        private QueryItem tfsQueryItem;

        private WorkItemStore workItemStore;

        /// <summary>
        /// Creates the release note.
        /// </summary>
        /// <param name="request">The request.</param>
        public void SaveReleaseNote(ReleaseNoteRequest request)
        {
            this.ConnectToTfs(request);

            var workItems = this.GenerateReleaseNoteWorkItems();

            var publisher = new PdfReleaseNotePublisher();
            publisher.Publish(request, workItems);
        }

        /// <summary>
        /// Connects to TFS.
        /// </summary>
        /// <param name="request">The request.</param>
        private void ConnectToTfs(ReleaseNoteRequest request)
        {
            // Connect to Team Foundation Server
            var tfsUri = new Uri(request.TfsServerUrl);
            var teamProjectCollection = new TfsTeamProjectCollection(tfsUri);
            this.workItemStore = teamProjectCollection.GetService<WorkItemStore>();
            var tfsTeamProject = workItemStore.Projects[request.TfsProject];
            var tfsQueryFolder = tfsTeamProject.QueryHierarchy[request.TfsQueryHierarchy] as QueryFolder;

            if (tfsQueryFolder != null)
            {
                this.tfsQueryItem = tfsQueryFolder[request.TfsQueryName];
            }
        }

        /// <summary>
        /// Generates the release note work items.
        /// </summary>
        /// <returns>A List of Release Note Work Items</returns>
        private List<WorkItem> GenerateReleaseNoteWorkItems()
        {
            var queryDefinition = this.workItemStore.GetQueryDefinition(this.tfsQueryItem.Id);
            var variables = new Dictionary<string, string> { { "project", this.tfsQueryItem.Project.Name } };

            var workItemCollection = this.workItemStore.Query(queryDefinition.QueryText, variables);

            return (from Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem workItem in workItemCollection
                    select
                        new WorkItem
                        {
                            Area = workItem.AreaPath,
                            ResolutionType = workItem.Reason,
                            Title = workItem.Title,
                            WorkItemId = workItem.Id,
                            Bug = workItem.Type.Name == "Bug",
                            CreatedBy = workItem.CreatedBy,
                            Uri = workItem.Uri.ToString(),
                            ProjectName = workItem.Project.Name,
                            Done = (workItem.State == "Done" || workItem.State == "Resolved"),
                            Iteration = workItem.IterationPath
                        }).ToList();
        }
    }
}
