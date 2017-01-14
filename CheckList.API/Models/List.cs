using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckList.API.Models
{
    public class List
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public Boolean complete { get; set; }
    }
}