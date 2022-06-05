using MiniPloomes.Entities;
using MiniPloomes.Models.Contacts;
using MiniPloomes.Models.Users;

namespace MiniPloomes.Persistence.Repository
{
    public interface IMiniPloomesRepository
    {
        ExposableUserModel GetExposableUserByToken(string token);
        ExposableUserModel ValidateUser(string email, string password);
        void AddUser(User user);
        void UpdateUser(User user);
        void RemoveUser(string token);
        void RemoveToken(string token);
        string UpdateTokenById(int id);


        List<Contact> GetContacts(int userId);
        Contact GetContactByIdAndUser(int contactId, int userId);
        int AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void RemoveContact(int id);


        List<Deal> GetDeals(int userId);
        Deal GetDealByIdAndUser(int dealId, int userId);
        int AddDeal(Deal deal);
        void UpdateDeal(Deal deal);
        void RemoveDeal(int id);
    }
}
