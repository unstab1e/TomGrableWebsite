using System.Data;
using Sitecore.Cintel.Reporting;
using Sitecore.Cintel.Reporting.Processors;

namespace TomGrable.Website.Extensions.Pipelines.DeveloperData
{
    public class ConstructDeveloperDataTable : ReportProcessorBase
    {
        public override void Process(ReportProcessorArgs args)
        {
            args.ResultTableForView = new DataTable();
            var viewField = new ViewField<string>("DeveloperId");
            args.ResultTableForView.Columns.Add(viewField.ToColumn());
        }
    }
}