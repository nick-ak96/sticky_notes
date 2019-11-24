using System;
using System.Collections.Generic;

namespace api.Models
{
    public class Paginated<TModel> where TModel : class
    {
        public Paginated(Pagination paginationDetails, IEnumerable<TModel> data)
        {
            PaginationDetails = paginationDetails;
            Data = data;
        }

        public Pagination PaginationDetails { get; private set; }

        public IEnumerable<TModel> Data { get; private set; }
    }


    public class Pagination
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int Total { get; set; }

        public int Offset =>
            Page * ItemsPerPage;

        public int TotalPages =>
            (int)Math.Ceiling(1.0 * Total / ItemsPerPage);
    }
}
