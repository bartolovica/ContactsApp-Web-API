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

namespace ContactsWebAPI.Controllers
{
    public class EmailsController : ApiController
    {
        private ContactsDatabaseEntities db = new ContactsDatabaseEntities();

        // GET: api/Emails
        public IQueryable<EmailsTable> GetEmailsTable()
        {
            return db.EmailsTable;
        }

        // GET: api/Emails/5
        [ResponseType(typeof(EmailsTable))]
        public IHttpActionResult GetEmailsTable(int id)
        {
            EmailsTable emailsTable = db.EmailsTable.Find(id);
            if (emailsTable == null)
            {
                return NotFound();
            }

            return Ok(emailsTable);
        }

        // PUT: api/Emails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmailsTable(int id, EmailsTable emailsTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emailsTable.IdEmail)
            {
                return BadRequest();
            }

            db.Entry(emailsTable).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailsTableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Emails
        [ResponseType(typeof(EmailsTable))]
        public IHttpActionResult PostEmailsTable(EmailsTable emailsTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            emailsTable.ContactsTable.ContactId = emailsTable.ContactId;
            db.EmailsTable.Add(emailsTable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {controller="addAnotherMail", id = emailsTable.IdEmail }, emailsTable);
        }

        // DELETE: api/Emails/5
        [ResponseType(typeof(EmailsTable))]
        public IHttpActionResult DeleteEmailsTable(int id)
        {
            EmailsTable emailsTable = db.EmailsTable.Find(id);
            if (emailsTable == null)
            {
                return NotFound();
            }

            db.EmailsTable.Remove(emailsTable);
            db.SaveChanges();

            return Ok(emailsTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailsTableExists(int id)
        {
            return db.EmailsTable.Count(e => e.IdEmail == id) > 0;
        }
    }
}