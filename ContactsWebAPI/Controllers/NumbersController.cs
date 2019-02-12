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
    public class NumbersController : ApiController
    {
        private ContactsDatabaseEntities db = new ContactsDatabaseEntities();

        // GET: api/Numbers
        public IQueryable<NumbersTable> GetNumbersTable()
        {
            return db.NumbersTable;
        }

        // GET: api/Numbers/5
        [ResponseType(typeof(NumbersTable))]
        public IHttpActionResult GetNumbersTable(int id)
        {
            NumbersTable numbersTable = db.NumbersTable.Find(id);
            if (numbersTable == null)
            {
                return NotFound();
            }

            return Ok(numbersTable);
        }

        // PUT: api/Numbers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNumbersTable(int id, NumbersTable numbersTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != numbersTable.IdNumber)
            {
                return BadRequest();
            }

            db.Entry(numbersTable).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NumbersTableExists(id))
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

        // POST: api/Numbers
        [ResponseType(typeof(NumbersTable))]
        public IHttpActionResult PostNumbersTable(NumbersTable numbersTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           if(numbersTable.IdNumber==0)

        db.NumbersTable.Add(numbersTable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = numbersTable.IdNumber }, numbersTable);
        }

        // DELETE: api/Numbers/5
        [ResponseType(typeof(NumbersTable))]
        public IHttpActionResult DeleteNumbersTable(int id)
        {
            NumbersTable numbersTable = db.NumbersTable.Find(id);
            if (numbersTable == null)
            {
                return NotFound();
            }

            db.NumbersTable.Remove(numbersTable);
            db.SaveChanges();

            return Ok(numbersTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NumbersTableExists(int id)
        {
            return db.NumbersTable.Count(e => e.IdNumber == id) > 0;
        }
    }
}