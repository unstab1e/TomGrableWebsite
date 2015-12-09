using System.Data;
using Sitecore.Cintel.Reporting;
using Sitecore.Cintel.Reporting.Processors;

namespace TomGrable.Website.Extensions.Pipelines.DeveloperData
{
    class PopulateWithDeveloperData : ReportProcessorBase
    {
        public override void Process(ReportProcessorArgs args)
        {
            var result = args.QueryResult;
            var table = args.ResultTableForView;
            if (table.Columns.Contains("DeveloperId"))
            {
                foreach (DataRow row in result.AsEnumerable())
                {
                    var id = row["DeveloperData_DeveloperId"];
                    if (id == null || string.IsNullOrEmpty(id.ToString()))
                    {
                        continue;
                    }
                    var targetRow = table.NewRow();
                    targetRow["DeveloperId"] = id;
                    table.Rows.Add(targetRow);
                }
            }
            args.ResultSet.Data.Dataset[args.ReportParameters.ViewName] = table;
        }
    }
}