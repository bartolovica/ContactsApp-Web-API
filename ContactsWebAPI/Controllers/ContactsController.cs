using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ContactsWebAPI.Models;
using ContactsWebAPI.Services;
using ContactsWebAPI.ViewModels;
using EntityState = System.Data.Entity.EntityState;

namespace ContactsWebAPI.Controllers
{
    public class ContactsController : ApiController
    {
        private readonly ContactsDatabaseEntities _db;
        private readonly ContactsService _contactsService;

        public ContactsController()
        {
            _db = new ContactsDatabaseEntities();
            _contactsService = new ContactsService();
        }

        [Route("api/getAllContacts")]
        [HttpGet]
        public IHttpActionResult GetContactsTable()
        {
            return Ok(_contactsService.GetAllContacts());
        }

        public IHttpActionResult GetContactsTable(Guid id)
        {
            var contactsTable = _contactsService.GetContactById(id);
            if (contactsTable == null)
            {
                return NotFound();
            }
            return Ok(contactsTable);
        }


        [Route("api/searchContacts/{searchParametar}")]
        [HttpGet]
        public IHttpActionResult SearchContacts(string searchParametar)
        {
            var searchResults = _contactsService.Search(searchParametar);
            return Ok(searchResults);
        }

        [Route("api/updateContacts/{guid}")]
        [HttpPut]
        public IHttpActionResult PutContactsTable(Guid guid,ContactViewModel contact)
        {
            _contactsService.UpdateContact(guid,contact);
            return Ok(contact);
        }

        [Route("api/addContact",Name = "addContact")]
        [HttpPost]
        public IHttpActionResult PostContactsTable(ContactViewModel newContact)
        {
            _contactsService.AddContact(newContact);
            return Ok(newContact);
        }
        
        [Route("api/deleteContact/{guid}", Name = "deleteContact")]
        [HttpDelete]
        public IHttpActionResult DeleteContactsTable(Guid guid)
        {
            _contactsService.DeleteContact(guid);
            return Ok(guid);
        }
    }
}