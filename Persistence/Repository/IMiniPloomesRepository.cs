using MiniPloomes.Entities;
using MiniPloomes.Models.Users;

namespace MiniPloomes.Persistence.Repository
{
    public interface IMiniPloomesRepository
    {
        ExposableUserModel GetExposableUserByToken(string token);
        ExposableUserModel ValidateUser(string email, string password);
        void AddUser(User user);
        void UpdateUser(string name, string email, string password, string token);
        void RemoveUser(string token);
        void RemoveToken(string token);
        string UpdateTokenById(int id);


        Contact GetContactByIdAndUser(int contactId, int userId);
        void AddContact(Contact contact);
        void RemoveContact(Contact contact);


        Deal GetDealByIdAndUser(int dealId, int userId);
        void AddDeal(Deal deal);
        void RemoveDeal(Deal deal);
    }
}
