using MiniPloomes.Entities;

namespace MiniPloomes.Models.Users
{
    public class ExposableUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Token { get; set; }
        public DateTime CreateDate { get; set; }
       
        public ExposableUserModel(int id, string name, string email, DateTime createDate, string token)
        {
            Id = id;
            Name = name;
            Email = email;
            CreateDate = createDate;
            Token = token;  
        }
    }
}
