using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsWebAPI.ViewModels
{
    public class ContactItemList
    {
        public System.Guid ContactId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Tag { get; set; }

    }
}