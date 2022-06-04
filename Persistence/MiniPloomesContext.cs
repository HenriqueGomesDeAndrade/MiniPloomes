using MiniPloomes.Entities;

namespace MiniPloomes.Persistence
{
    public class MiniPloomesContext
    {
        public List<User> Users { get; set; }
        public MiniPloomesContext()
        {
            Users = new List<User>();
        }
        public User ValidateUser(string email, string password) 
        {
            var user = Users.Find(u => u.Email == email && u.Password == password);
            return user;
        }
    }
}
