using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
        [DefaultValue(1)]
        public int CurrentPage { get; set; } = 1;
        [DefaultValue("")]
        public string TextSearch { get; set; } = string.Empty;
    } 
}
