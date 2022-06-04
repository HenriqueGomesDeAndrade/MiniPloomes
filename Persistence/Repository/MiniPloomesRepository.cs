using MiniPloomes.Entities;

namespace MiniPloomes.Persistence.Repository
{
    public class MiniPloomesRepository : IMiniPloomesRepository
    {
        private readonly MiniPloomesContext _context;
        public MiniPloomesRepository(MiniPloomesContext context)
        {
            _context = context;
        }

        public void AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
        }

        public void AddDeal(Deal deal)
        {
            _context.Deals.Add(deal);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public Contact GetContactByIdAndUser(int contactId, int userId)
        {
            return _context.Contacts.FirstOrDefault(c => c.Id == contactId && c.CreatorId == userId);
        }

        public Deal GetDealByIdAndUser(int dealId, int userId)
        {
            return _context.Deals.FirstOrDefault(c => c.Id == dealId && c.CreatorId == userId);
        }

        public User GetUserByToken(string token)
        {
            return _context.Users.FirstOrDefault(u => u.Token == token);
        }

        public void RemoveContact(Contact contact)
        {
            _context.Contacts.Remove(contact);
        }

        public void RemoveDeal(Deal deal)
        {
            _context.Deals.Remove(deal);
        }

        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }

        public User ValidateUser(string email, string password)
        {
            return _context.Users.Find(u => u.Email == email && u.Password == password);
        }
    }
}
