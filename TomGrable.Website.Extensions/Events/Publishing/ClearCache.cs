using System;
using System.Collections;
using Sitecore.Caching;
using Sitecore.Diagnostics;
using Sitecore.StringExtensions;

namespace TomGrable.Website.Extensions.Events.Publishing
{
    public class ClearCache
    {
        private readonly ArrayList _cacheNameList;

        public ArrayList CacheNameList
        {
            get
            {
                return _cacheNameList;
            }
        }

        public ClearCache()
        {
            _cacheNameList = new ArrayList();
        }

        public void RemoveCache(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull((object)args, "args");
            Log.Info("Clearning Cache", this);
            foreach (string name in this.CacheNameList)
            {
                var cacheByName = CacheManager.FindCacheByName(name);
                if (cacheByName != null)
                {
                    Log.Info("deleting cache: {0}".FormatWith(name), this);
                    cacheByName.Clear();
                }
                else
                {
                    Log.Info("could not find cache: {0}".FormatWith(name), this);
                }
            }
            Log.Info("Cleared all cache", this);
        }
    }
}
