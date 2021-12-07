using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Models
{
    public class PagedData<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int PageCount { get; set; }
        public string Search { get; set; }

        public PagedData(IEnumerable<T> data, int page, int size, int pageCount, string search)
        {
            Data = data;
            Page = page;
            Size = size;
            PageCount = pageCount;
            Search = search;
        }
    }

}
