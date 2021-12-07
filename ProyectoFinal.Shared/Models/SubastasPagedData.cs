using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Models
{
    public class SubastasPagedData: PagedData<SubastaItemDto>
    {
        public string SortOrder { get; set; }
        public bool HideEnded { get; set; }
        public bool HideMySubastas { get; set; }
        public bool ShowAll { get; set; }

        public SubastasPagedData(IEnumerable<SubastaItemDto> data, int page, int size, int pageCount, string search, string sortOrder, bool hideEnded, bool hideMySubastas, bool showAll): base(data, page, size, pageCount, search)
        {
            SortOrder = sortOrder;
            HideEnded = hideEnded;
            HideMySubastas = hideMySubastas;
            ShowAll = showAll;
        }
    }
}
