using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsWebAPI.ViewModels
{
    public class EmailViewModel
    {
        public int IdEmail { get; set; }
        public string Email { get; set; }
        public System.Guid ContactId { get; set; }
    }
}