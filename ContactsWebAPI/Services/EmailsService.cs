using ContactsWebAPI.Models;
using ContactsWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ContactsWebAPI.Services
{
    public class EmailsService
    {
        private readonly ContactsDatabaseEntities _db;

        public EmailsService()
        {
            _db = new ContactsDatabaseEntities();
        }

        public void AddNewMail (ContactViewModel newContact)
        {
            foreach (var r in newContact.Emails)
            {
                var create = new EmailsTable
                {
                    Email = r.Email,
                    ContactId = newContact.ContactId
                };
                _db.EmailsTable.Add(create);
            }
             _db.SaveChanges();
        }

        public void UpdateMails (ContactViewModel newContact)
        {
            var emails = _db.EmailsTable.Where(r => r.ContactId == newContact.ContactId);

            var create = newContact.Emails.Where(r => r.IdEmail == 0)
                .Select(r => new EmailsTable
                { Email = r.Email, ContactId = newContact.ContactId })
                .ToList();
            _db.EmailsTable.AddRange(create);

            var update = newContact.Emails.Where(r => r.IdEmail != 0).ToList();
            foreach (var item in update)
            {
                var old = emails.Single(r => r.IdEmail == item.IdEmail);
                old.Email = item.Email;
                _db.Entry(old).State = EntityState.Modified;
            }

            var newIds = update.Select(r => r.IdEmail);
            var deleted = emails.Where(r => !newIds.Contains(r.IdEmail));
            foreach (var email in deleted)
            {
                _db.Entry(email).State = EntityState.Deleted;
            }
            _db.SaveChanges();
        }

        public void DeleteMails(Guid guid)
        {
            var mailsForDelete = _db.EmailsTable.Where(r => r.ContactId == guid).ToList();
            foreach(var item in mailsForDelete)
            {
                _db.Entry(item).State = EntityState.Deleted;
            }
            _db.SaveChanges();
        }

    }
}