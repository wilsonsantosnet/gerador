﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    

    public class DataItemGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DataItem> DataItems { get; set; }

    }
}
