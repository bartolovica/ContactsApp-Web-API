using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsWebAPI.ViewModels
{
    public class ContactViewModel
    {
        public System.Guid ContactId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Tag { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public List<EmailViewModel> Emails { get; set; }
        public List<NumberViewModel> Numbers { get; set; }
    }
}