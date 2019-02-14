using ContactsWebAPI.Models;
using ContactsWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ContactsWebAPI.Services
{
    public class ContactsService
    {
        private readonly ContactsDatabaseEntities _db;
        private readonly EmailsService _emailsService;
        private readonly NumbersService _numbersService;

       public ContactsService()
        {
            _db = new ContactsDatabaseEntities();
            _emailsService = new EmailsService();
            _numbersService = new NumbersService();
        }

        public List<ContactItemList> GetAllContacts()
        {
            return _db.ContactsTable
                 .Select(r => new ContactItemList
                 {
                     ContactId = r.ContactId,
                     Name = r.Name,
                     Surname = r.Surname,
                     Tag = r.Tag
                 })
                .ToList();
        }
        public List<ContactItemList> Search(string searchParametar) {
            return _db.ContactsTable.
                 Where(r => r.Surname.Contains(searchParametar) ||
                 r.Name.Contains(searchParametar) ||
                 r.Tag.Contains(searchParametar))
                 .Select(r => new ContactItemList
                 {
                     ContactId = r.ContactId,
                     Name = r.Name,
                     Surname = r.Surname,
                     Tag = r.Tag
                 })
                .ToList();
        }

        public ContactViewModel GetContactById(Guid ContactId)
        {
            return _db.ContactsTable
                .Include(r => r.EmailsTable)
                .Include(r => r.NumbersTable)
                .Where(r => r.ContactId == ContactId)
                .Select(r => new ContactViewModel {
                    ContactId = r.ContactId,
                    Name = r.Name,
                    Surname = r.Surname,
                    Tag = r.Tag,
                    City=r.City,
                    Adress=r.Adress,
                    PostalCode=r.PostalCode,
                    Country=r.Country,
                    Emails = r.EmailsTable.Select(x => new EmailViewModel { IdEmail = x.IdEmail, Email = x.Email}).ToList(),
                    Numbers = r.NumbersTable.Select(x => new NumberViewModel { IdNumber = x.IdNumber, Number = x.Number }).ToList(),
                } )
                .SingleOrDefault();
        }

        public void AddContact (ContactViewModel newContact)
        {

            newContact.ContactId = Guid.NewGuid();
            var contact = new ContactsTable
            {
                ContactId = newContact.ContactId,
                Name = newContact.Name,
                Surname = newContact.Surname,
                Tag = newContact.Tag,
                Adress = newContact.Adress,
                PostalCode = newContact.PostalCode,
                City = newContact.City,
                Country = newContact.Country,
                EmailsTable = null,
                NumbersTable = null
            };
            _db.ContactsTable.Add(contact);
            _db.SaveChanges();
            _emailsService.AddNewMail(newContact);
            _numbersService.AddNewNumbers(newContact);
        }

        public void UpdateContact(Guid guid, ContactViewModel newContact) {
            var oldContact = _db.ContactsTable.SingleOrDefault(r => r.ContactId == guid);
            oldContact.Name = newContact.Name;
            oldContact.Surname = newContact.Surname;
            oldContact.Adress = newContact.Adress;
            oldContact.PostalCode = newContact.PostalCode;
            oldContact.City = newContact.City;
            oldContact.Country = newContact.Country;
            _emailsService.UpdateMails(newContact);
            _numbersService.UpdateNumbers(newContact);
            _db.Entry(oldContact).State = EntityState.Modified;
            _db.SaveChanges();
        }


        public void DeleteContact(Guid guid)
        {
            var contactforDelete = _db.ContactsTable.SingleOrDefault(r => r.ContactId == guid);
            _emailsService.DeleteMails(guid);
            _numbersService.DeleteNumbers(guid);
            _db.Entry(contactforDelete).State = EntityState.Deleted;
            _db.SaveChanges();
        }
    }
}
