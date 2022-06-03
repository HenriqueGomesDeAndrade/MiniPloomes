namespace MiniPloomes.Entities
{
    public class Deal
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public decimal Amount { get; private set; }
        public int ContactId { get; private set; }
        public int CreatorId { get; private set; }
        public DateTime CreateDate { get; private set; }

        public Deal(string title, decimal amount, int contactId, int creatorId)
        {
            Title = title;
            Amount = amount;
            ContactId = contactId;
            CreatorId = creatorId;
            CreateDate = DateTime.Now;
        }
    }
}
