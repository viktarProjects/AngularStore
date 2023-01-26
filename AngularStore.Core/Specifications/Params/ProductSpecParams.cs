using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Params
{
    public class ProductSpecParams
    {
        private const int _maxPageSize = 24;

        private int _pageSize { get; set; } = 6;

        private string _search { get; set; } = "";

        public int PageNumber { get; set; } = 1;

        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        public string Sort { get; set; } = "";

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > _maxPageSize ? _maxPageSize : value;
        }

        public string? Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
