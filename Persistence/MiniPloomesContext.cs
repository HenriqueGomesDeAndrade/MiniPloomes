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
    }
}
