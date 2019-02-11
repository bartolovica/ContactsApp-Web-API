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
using EntityState = System.Data.Entity.EntityState;

namespace ContactsWebAPI.Controllers
{
    public class ContactsGeneralController : ApiController
    {
        private ContactsDatabaseEntities db = new ContactsDatabaseEntities();

        // GET: api/ContactsGeneral
        [Route("api/getAllContacts")]
        [HttpGet]
        public IQueryable<ContactsTable> GetContactsTable()
        {
            return db.ContactsTable;
        }

        // GET: api/ContactsGeneral/5
        [ResponseType(typeof(ContactsTable))]

        public IHttpActionResult GetContactsTable(Guid id)
        {
            ContactsTable contactsTable = db.ContactsTable.Find(id);
            if (contactsTable == null)
            {
                return NotFound();
            }

            return Ok(contactsTable);
        }

        // PUT: api/ContactsGeneral/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContactsTable(Guid id, ContactsTable contactsTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ContactsTable contactsTableOld = db.ContactsTable.Find(id);
            if (contactsTableOld == null)
            {
                return BadRequest();
            }

            //if (id != contactsTableOld.ContactsTable.Find(ContactId))
            //{
            //    return BadRequest();
            //}



            //var idTable = db.ContactsTable.Find(id);
            // if (idTable == null)
            // {
            //     return NotFound();
            // }

            //contactsTable.ContactId=id;

            db.ContactsTable.Remove(contactsTableOld);
            db.ContactsTable.Add(contactsTable);
            db.SaveChanges();

            return Ok(contactsTable);

          
        }

        // POST: api/ContactsGeneral
        [Route("api/addContact")]
        [ResponseType(typeof(ContactsTable))]
        [HttpPost]
        public IHttpActionResult PostContactsTable(ContactsTable contactsTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (contactsTable.ContactId == Guid.Empty)
            {
                contactsTable.ContactId = Guid.NewGuid();
            }
            //contactsTable.ContactId = Guid.NewGuid();
            db.ContactsTable.Add(contactsTable);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ContactsTableExists(contactsTable.ContactId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = contactsTable.ContactId }, contactsTable);
        }

        // DELETE: api/ContactsGeneral/5
        [ResponseType(typeof(ContactsTable))]
        public IHttpActionResult DeleteContactsTable(Guid id)
        {
            ContactsTable contactsTable = db.ContactsTable.Find(id);
            if (contactsTable == null)
            {
                return NotFound();
            }

            db.ContactsTable.Remove(contactsTable);
            db.SaveChanges();

            return Ok(contactsTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactsTableExists(Guid id)
        {
            return db.ContactsTable.Count(e => e.ContactId == id) > 0;
        }
    }
}