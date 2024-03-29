﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }

        private const int MaxPageSize = 10;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value>MaxPageSize?MaxPageSize:value; }
        }
        public int PageIndex { get; set; } = 1;

        public string? Search { get; set; }

    }
}
