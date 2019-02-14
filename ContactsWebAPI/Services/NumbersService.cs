using ContactsWebAPI.Models;
using ContactsWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ContactsWebAPI.Services
{
    public class NumbersService
    {
        private readonly ContactsDatabaseEntities _db;

        public NumbersService()
        {
            _db = new ContactsDatabaseEntities();
        }

        public void AddNewNumbers(ContactViewModel newContact)
        {
            foreach (var r in newContact.Numbers)
            {
                var create = new NumbersTable
                {
                    Number = r.Number,
                    ContactId = newContact.ContactId
                };

                _db.NumbersTable.Add(create);
            }
            _db.SaveChanges();
        }
    
        public void UpdateNumbers(ContactViewModel newContact)
        {
            var numbers = _db.NumbersTable.Where(r => r.ContactId == newContact.ContactId);
            var createNumbers = newContact.Numbers.Where(r => r.IdNumber == 0)
                .Select(r => new NumbersTable
                { Number = r.Number, ContactId = newContact.ContactId })
                .ToList();
            _db.NumbersTable.AddRange(createNumbers);

            var updateNumbers = newContact.Numbers.Where(r => r.IdNumber != 0).ToList();
            foreach (var item in updateNumbers)
            {
                var old = numbers.Single(r => r.IdNumber == item.IdNumber);
                old.Number = item.Number;
                _db.Entry(old).State = EntityState.Modified;
            }
            var newNumberIds = updateNumbers.Select(r => r.IdNumber);
            var deletedNumbers = numbers.Where(r => !newNumberIds.Contains(r.IdNumber));
            foreach (var number in deletedNumbers)
            {
                _db.Entry(number).State = EntityState.Deleted;
            }
            _db.SaveChanges();
        }

        public void DeleteNumbers(Guid guid)
        {
            var numbersForDelete = _db.NumbersTable.Where(r => r.ContactId == guid).ToList();
            foreach (var item in numbersForDelete)
            {
                _db.Entry(item).State = EntityState.Deleted;
            }
            _db.SaveChanges();
        }
    }
}