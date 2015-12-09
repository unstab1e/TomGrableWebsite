using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Analytics.Rules.SegmentBuilder;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.Rules.Conditions;

namespace TomGrable.Website.Extensions.Rules.SegmentBuilder.Condition
{
    public class PageUrlCondition<T> : TypedQueryableStringOperatorCondition<T, IndexedContact> where T : VisitorRuleContext<IndexedContact>
    {
        protected override Expression<Func<IndexedContact, bool>> GetResultPredicate(T ruleContext)
        {
            return GetCompareExpression(c => c["visitpageevent.url"], Value);
        }
    }
}
