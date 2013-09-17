namespace boardwalk.releasenotegenerator
{
    using MigraDoc.DocumentObjectModel;
    using MigraDoc.DocumentObjectModel.Tables;
    using MigraDoc.Rendering;

    using PdfSharp.Drawing;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.IO;

    using System.Collections.Generic;
    using System.Linq;

    public class PdfReleaseNotePublisher
    {
        /// <summary>
        /// The request
        /// </summary>
        private ReleaseNoteRequest request;

        /// <summary>
        /// The doc
        /// </summary>
        private PdfDocument doc;

        /// <summary>
        /// The work items
        /// </summary>
        private List<WorkItem> workItems;

        /// <summary>
        /// Publishes the specified release note request.
        /// </summary>
        /// <param name="releaseNoteRequest">The release note request.</param>
        /// <param name="releaseNoteWorkItems">The release note work items.</param>
        public void Publish(ReleaseNoteRequest releaseNoteRequest, List<WorkItem> releaseNoteWorkItems)
        {
            this.request = releaseNoteRequest;
            this.workItems = releaseNoteWorkItems.Where(c=> c.Iteration.EndsWith(string.Format(" {0}", request.IterationNumber))).ToList();

            // Start the document by generating the front cover
            this.GenerateCover();

            this.doc.Info.Title = string.Format(
                "FitApp Release Note - Build {0} - Iteration {1}",
                request.BuildNumber,
                request.IterationNumber);
            this.doc.Info.Author = "Boardwalk IT";

            // Write the work items
            WriteWorkItems();
            
            // Save
            this.doc.Save(request.OutputFile);
        }

        /// <summary>
        /// Generates the cover.
        /// </summary>
        private void GenerateCover()
        {
            this.doc = PdfReader.Open(this.request.FrontPageTemplate, PdfDocumentOpenMode.Modify);
            var page = doc.Pages[0];
            var gfx = XGraphics.FromPdfPage(page);

            // HACK²
            gfx.MUH = PdfFontEncoding.Unicode;
            gfx.MFEH = PdfFontEmbedding.Default;
 
            var font = new XFont("Tahoma", 13, XFontStyle.Bold);

            gfx.DrawString(
                string.Format(
                    "Build {0} - Iteration {1}",
                    this.request.BuildNumber,
                    this.request.IterationNumber),
                font,
                XBrushes.Black,
                new XRect(100, 200, page.Width - 200, 300),
                XStringFormats.Center);
        }

        /// <summary>
        /// Writes the work items.
        /// </summary>
        private void WriteWorkItems()
        {
            // Create document
            var migraDoc = new Document();

            var areas = workItems.Select(c => c.Area).Distinct();

            foreach (var area in areas)
            {
                Section sec = migraDoc.AddSection();

                // Add a single paragraph with some text and format information.
                Paragraph para = sec.AddParagraph();
                para.Format.Alignment = ParagraphAlignment.Justify;
                para.Format.Font.Name = "Tahoma";
                para.Format.Font.Size = 14;
                para.Format.Font.Color = Colors.Black;

                // Are we looking at a sub area?
                var startChar = area.LastIndexOf(@"\", System.StringComparison.Ordinal) == -1
                    ? 0
                    : area.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1;

                // Add the area name
                para.AddText(area.Substring(startChar));
                para.AddLineBreak();
                para.AddLineBreak();

                WriteWorkItemTable(migraDoc, area);
            }

            // Create a renderer and prepare (=layout) the document
            var docRenderer = new DocumentRenderer(migraDoc);
            docRenderer.PrepareDocument();

            int pageCount = docRenderer.FormattedDocument.PageCount;
            for (int idx = 0; idx < pageCount; idx++)
            {
                // Template Page
                var pageTemplateDoc = PdfReader.Open(this.request.PageTemplate, PdfDocumentOpenMode.Import);
                var page = pageTemplateDoc.Pages[0];

                page = this.doc.AddPage(page);
                XGraphics gfx = XGraphics.FromPdfPage(page);

                var font = new XFont("Tahoma", 9, XFontStyle.Regular);

                gfx.DrawString(
                    string.Format(
                        "Project: {0} - Build: {1} - Iteration: {2}",
                        this.request.TfsProject,
                        this.request.BuildNumber,
                        this.request.IterationNumber),
                font,
                XBrushes.Gray,
                new XRect(50, 800, page.Width - 200, 50),
                XStringFormats.TopLeft);

                gfx.DrawString(
                    string.Format(
                        "Page {0}",
                        idx + 2),
                font,
                XBrushes.Gray,
                new XRect(page.Width - 100, 800, page.Width - 50, 50),
                XStringFormats.TopLeft);

                // HACK²
                gfx.MUH = PdfFontEncoding.Unicode;
                gfx.MFEH = PdfFontEmbedding.Default;

                // Render the page. Note that page numbers start with 1.
                docRenderer.RenderPage(gfx, idx + 1);
            }
        }

        /// <summary>
        /// Writes the work item table.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="area">The area.</param>
        private void WriteWorkItemTable(Document document, string area)
        {
            var table = new Table
                          {
                              Borders = { Width = 0.75 }, 
                          };
            table.AddColumn(Unit.FromCentimeter(2));
            table.AddColumn(Unit.FromCentimeter(10));
            table.AddColumn(Unit.FromCentimeter(5));

            // Create the header
            var row = table.AddRow();
            row.Shading.Color = Colors.Black;
            
            var cell = row.Cells[0];
            cell.Format.Font.Color = Colors.White;
            cell.AddParagraph("Work Item ID");
            cell = row.Cells[1];
            cell.Format.Font.Color = Colors.White;
            cell.AddParagraph("Title");
            cell = row.Cells[2];
            cell.Format.Font.Color = Colors.White;
            cell.AddParagraph("Created By");

            // Loop around the workitems for a given area
            foreach (var workItem in this.workItems.Where(a=> a.Area == area))
            {
                row = table.AddRow();
                cell = row.Cells[0];
                cell.AddParagraph(workItem.WorkItemId.ToString());
                cell = row.Cells[1];
                cell.AddParagraph(workItem.Title);
                cell = row.Cells[2];
                cell.AddParagraph(workItem.CreatedBy);
            }
            
            // Add the table to the doc
            document.LastSection.Add(table);
        }
    }
}
