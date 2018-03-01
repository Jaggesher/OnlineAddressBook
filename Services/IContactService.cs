using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAddressBook.Models;

namespace OnlineAddressBook.Services
{
    public interface IContactService
    {
        Task<bool> AddContactAsync(PeopleViewModel _newPeople, String userId);
        
        Task<IEnumerable<People>> GetAllContactsAsync(String userId);

        Task<People> GetPeople(Guid peopleId);

        Task<bool> SaveChanges(PeopleViewModel _oldPeople, Guid peopleId);

        Task<bool> DeleteIt(Guid peopleId);
    }
}