using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.Analytics.Rules.SegmentBuilder;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.Rules.Conditions;
using Sitecore.ContentSearch.Utilities;

namespace TomGrable.Website.Extensions.Rules.SegmentBuilder.Condition
{
    public class PageEventIdCondition<T> : TypedQueryableOperatorCondition<T, IndexedContact> where T : VisitorRuleContext<IndexedContact>
    {
        public string Value { get; set; }
        protected override Expression<Func<IndexedContact, bool>> GetResultPredicate(T ruleContext)
        {
            return GetCompareExpression(c => c["visitpageevent.pageeventdefinitionid"], IdHelper.NormalizeGuid(Value));
        }
    }
}
