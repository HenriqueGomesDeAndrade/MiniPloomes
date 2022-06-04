using MiniPloomes.Entities;

namespace MiniPloomes.Persistence.Repository
{
    public interface IMiniPloomesRepository
    {
        User GetUserByToken(string token);
        User ValidateUser(string email, string password);
        void AddUser(User user);
        void RemoveUser(User user);

        Contact GetContactByIdAndUser(int contactId, int userId);
        void AddContact(Contact contact);
        void RemoveContact(Contact contact);

        Deal GetDealByIdAndUser(int dealId, int userId);
        void AddDeal(Deal deal);
        void RemoveDeal(Deal deal);
    }
}
