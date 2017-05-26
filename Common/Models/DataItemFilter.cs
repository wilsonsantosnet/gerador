using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class DataItemFilter : IFilter
    {
        public DataItemFilter()
        {
            this.ByCache = false;
        }

        public int CacheGroupId { get; set; }

        public string DataItemType { get; set; }

        public int FilterId { get; set; }

        public int SubFilterId { get; set; }

        public string FilterValue { get; set; }

        public string SubFilterValue { get; set; }

        private bool _byCache;

        public bool ByCache
        {
            get
            {
                if (ConfigurationManager.AppSettings["DesabledDataItemCached"] == "true")
                    return false;

                return _byCache;
            }

            set
            {
                _byCache = value;
            }
        }
    }
}
