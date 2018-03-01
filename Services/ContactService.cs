using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineAddressBook.Models;
using OnlineAddressBook.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace OnlineAddressBook.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ContactService (ApplicationDbContext context , IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

        public async Task<MemoryStream> BuildCSV(AllContactViewModel data)
        {

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"ContactList.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("ContactList");

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Full Name");
                row.CreateCell(1).SetCellValue("Nick Name");
                row.CreateCell(2).SetCellValue("Phone");
                row.CreateCell(3).SetCellValue("Address");
                row.CreateCell(4).SetCellValue("Website");
                row.CreateCell(5).SetCellValue("Birth Date");

                var i = 1;
                foreach(People person in data.MyContacts)
                {
                    string phones = "";
                    foreach (var phone in person.Phones)
                    {
                        phones += phone.Number.ToString()+" ";
                    }

                    row = excelSheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue(person.FullName);
                    row.CreateCell(1).SetCellValue(person.NickName);
                    row.CreateCell(2).SetCellValue(phones);
                    row.CreateCell(3).SetCellValue(person.Address);
                    row.CreateCell(4).SetCellValue(person.WebSite);
                    row.CreateCell(5).SetCellValue(person.BirthDate.Date.ToString("yyyy/MM/dd"));

                    i++;
                }
                
                workbook.Write(fs);


                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

            }

            return memory;

            throw new NotImplementedException();
        }

        public async Task<bool> DeleteIt(Guid peopleId)
        {
            _context.People.Remove(await _context.People.FindAsync(peopleId));

            var result = await _context.SaveChangesAsync();

            return result == 1;
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
            var contex = await _context.People.Where(X => X.Id == peopleId).Include(X => X.Phones ).SingleAsync();
            contex.FullName = _oldPeople.FullName;
            contex.NickName = _oldPeople.NickName;
            contex.BirthDate = _oldPeople.BirthDate;
            contex.WebSite = _oldPeople.WebSite;
            contex.Address = _oldPeople.Address;
            contex.Phones.Clear();

            try{
                await _context.SaveChangesAsync();
                try{
                    
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
        }
    }
}
