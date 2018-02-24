using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAddressBook.Models;

namespace OnlineAddressBook.Services
{
    public interface IContactService
    {
        Task<bool> AddContactAsync(PeopleViewModel _newPeople, String userId);
    }
}