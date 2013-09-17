using System;

namespace boardwalk.releasenotegenerator
{
    public class WorkItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>
        /// The area.
        /// </value>
        public string Area { get; set; }

        /// <summary>
        /// Gets or sets the work item id.
        /// </summary>
        /// <value>
        /// The work item id.
        /// </value>
        public int WorkItemId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type of the resolution.
        /// </summary>
        /// <value>
        /// The type of the resolution.
        /// </value>
        public string ResolutionType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkItem"/> is bug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if bug; otherwise, <c>false</c>.
        /// </value>
        public bool Bug { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkItem"/> is done.
        /// </summary>
        /// <value>
        ///   <c>true</c> if done; otherwise, <c>false</c>.
        /// </value>
        public bool Done { get; set; }

        /// <summary>
        /// Gets or sets the iteration.
        /// </summary>
        /// <value>
        /// The iteration.
        /// </value>
        public string Iteration { get; set; }

        /// <summary>
        /// Gets or sets the date completed.
        /// </summary>
        /// <value>
        /// The date completed.
        /// </value>
        public DateTime DateCompleted { get; set; }
    }
}
