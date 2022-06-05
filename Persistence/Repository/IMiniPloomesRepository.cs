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


        Contact GetContactByIdAndUser(int contactId, int userId);
        void AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void RemoveContact(int id);


        Deal GetDealByIdAndUser(int dealId, int userId);
        void AddDeal(Deal deal);
        void UpdateDeal(Deal deal);
        void RemoveDeal(int id);
    }
}
