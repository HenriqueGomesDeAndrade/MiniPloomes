using MiniPloomes.Entities;
using MiniPloomes.Models.Users;

namespace MiniPloomes.Models.Deals
{
    public class DealWithUserAndContactModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public int ContactId { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreateDate { get; set; }
        public Contact Contact { get; set; }
        public ExposableUserModel User { get; set; }

        public DealWithUserAndContactModel(int id, string title, decimal amount, int contactId, int creatorId, DateTime createDate, Contact contact, ExposableUserModel user)
        {
            Id = id;
            Title = title;
            Amount = amount;
            ContactId = contactId;
            CreatorId = creatorId;
            CreateDate = createDate;
            Contact = contact;
            User = user; 
        }
    }
}
