using Sitecore.Cintel.Reporting;
using Sitecore.Cintel.Reporting.Processors;
using Sitecore.Cintel.Reporting.ReportingServerDatasource;

namespace TomGrable.Website.Extensions.Pipelines.DeveloperData
{
    public class GetDeveloperData : ReportProcessorBase
    {
        public override void Process(ReportProcessorArgs args)
        {
            var queryExpression = this.CreateQuery().Build();
            var table = base.GetTableFromContactQueryExpression(queryExpression, args.ReportParameters.ContactId, null);
            args.QueryResult = table;
        }
        protected virtual QueryBuilder CreateQuery()
        {
            var builder = new QueryBuilder
            {
                collectionName = "Contacts"
            };
            builder.Fields.Add("_id");
            builder.Fields.Add("DeveloperData_DeveloperId");
            builder.QueryParms.Add("_id", "@contactid");
            return builder;
        }
    }
}