namespace MiniPloomes.Entities
{
    public class Contact
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int CreatorId { get; private set; }
        public DateTime CreateDate { get; private set; }

        public Contact(string name, int creatorId)
        {
            Name = name;
            CreatorId = creatorId;
            CreateDate = DateTime.Now;  
        }

        public Contact(int id, string name, int creatorId, DateTime createDate)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            CreateDate = createDate;
        }
    }
}
