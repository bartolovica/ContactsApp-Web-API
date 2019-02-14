using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsWebAPI.ViewModels
{
    public class NumberViewModel
    {
        public int IdNumber { get; set; }
        public string Number { get; set; }
        public System.Guid ContactId { get; set; }
    }
}