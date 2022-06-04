namespace MiniPloomes.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string? Token { get; private set; }
        public DateTime CreateDate { get; private set; }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            Token = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }



        public User UpdateUser(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            return this;
        }
    }
}
