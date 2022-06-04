using MiniPloomes.Entities;

namespace MiniPloomes.Persistence
{
    public class MiniPloomesContext
    {
        public List<User> Users { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Deal> Deals { get; set; }
        public MiniPloomesContext()
        {
            Users = new List<User>();
            Contacts = new List<Contact>();
            Deals = new List<Deal>();
        }
        public User ValidateUser(string email, string password) 
        {
            var user = Users.Find(u => u.Email == email && u.Password == password);
            return user;
        }

        public User FindUserByToken(string token)
        {
            return Users.Find(u => u.Token == token);
        }
    }
}
