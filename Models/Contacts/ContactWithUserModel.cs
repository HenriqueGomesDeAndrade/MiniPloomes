using MiniPloomes.Models.Users;

namespace MiniPloomes.Models.Contacts
{
    public class ContactWithUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreateDate { get; set; }
        public ExposableUserModel User { get; set; }

        public ContactWithUserModel(int id, string name, int creatorId, DateTime createDate, ExposableUserModel exposableUser)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            CreateDate = createDate;
            User = exposableUser;
        }
    }
}
