using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineAddressBook.Models;
using OnlineAddressBook.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineAddressBook.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddContactAsync(PeopleViewModel _newPeople, String userId)
        {
            var NewPople = new People
            {
                Id   = new Guid(),
                FullName = _newPeople.FullName,
                NickName = _newPeople.NickName,
                Address = _newPeople.Address,
                WebSite = _newPeople.WebSite,
                BirthDate = _newPeople.BirthDate,
                UserId = userId 
            };

            _context.People.Add(NewPople);

            foreach(PhoneViewModel phoneNumber in _newPeople.Phones){
                var NewPhone = new Phone 
                {
                    Id = new Guid(),
                    Number = phoneNumber.Phone,
                    PeopleId = NewPople.Id
                };

                _context.Phone.Add(NewPhone);
            }

            var saveResult = await _context.SaveChangesAsync();
            var entityCount = _newPeople.Phones.Count + 1;
            return saveResult == entityCount;

        }
        
        public async Task<IEnumerable<People>> GetAllContactsAsync(String userId)
        {
            return await _context.People.Where(x=> x.UserId == userId).Include(x => x.Phones).ToArrayAsync();
        }

        public async Task<People> GetPeople(Guid peopleId)
        {
            return await _context.People.Where(X => X.Id == peopleId).Include(X => X.Phones).SingleAsync();
        }

        public async Task<bool> SaveChanges(PeopleViewModel _oldPeople, Guid peopleId)
        {
            var contex = await _context.People.Where(X => X.Id == peopleId).SingleAsync();
            contex.FullName = _oldPeople.FullName;
            contex.NickName = _oldPeople.NickName;
            contex.BirthDate = _oldPeople.BirthDate;
            contex.WebSite = _oldPeople.WebSite;
            contex.Address = _oldPeople.Address;
            try{
                await _context.SaveChangesAsync();
                try{
                    _context.Phone.Remove( await _context.Phone.Where(X => X.PeopleId == peopleId).FirstAsync());
                    await _context.SaveChangesAsync();
                    
                    foreach(PhoneViewModel phoneNumber in _oldPeople.Phones){
                        var NewPhone = new Phone 
                        {
                            Id = new Guid(),
                            Number = phoneNumber.Phone,
                            PeopleId = peopleId
                        };

                        _context.Phone.Add(NewPhone);
                    }

                    var saveResult = await _context.SaveChangesAsync();
                    var entityCount = _oldPeople.Phones.Count;
                    return saveResult == entityCount;
                }catch{
                    return false;
                }
            }catch{
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
