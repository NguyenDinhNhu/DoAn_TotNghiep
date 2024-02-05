using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }

    public class SearchVM
    {
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public string TextSearch { get; set; }
    } 
}
