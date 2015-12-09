using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sitecore;
using Sitecore.Analytics.Rules.SegmentBuilder;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.Rules.Conditions;
using Sitecore.ContentSearch.SearchTypes;

namespace TomGrable.Website.Extensions.Rules.SegmentBuilder.Condition
{
    public class PageVisitDateTimeCondition<T> : TypedQueryableOperatorCondition<T, IndexedContact> where T : VisitorRuleContext<IndexedContact>
    {
        public string Value { get; set; }
        protected override Expression<Func<IndexedContact, bool>> GetResultPredicate(T ruleContext)
        {
            if (this.Value == null)
                return c => false;
            var dateTime = DateUtil.ParseDateTime(this.Value, DateTime.MinValue);
            return this.GetCompareExpression((c => c[(ObjectIndexerKey)"visitpageevent.datetime"]), dateTime);

        }
    }
}
