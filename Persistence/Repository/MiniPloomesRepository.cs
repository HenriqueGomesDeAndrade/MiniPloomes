using MiniPloomes.Entities;
using MiniPloomes.Models.Users;
using System.Data;
using System.Data.SqlClient;

namespace MiniPloomes.Persistence.Repository
{
    public class MiniPloomesRepository : IMiniPloomesRepository
    {
        private readonly string connectionString = @"Server=Localhost; Database = Miniploomes; User Id = sa; Password =#gcFrgeux";
        private readonly MiniPloomesContext _context;
        public MiniPloomesRepository(MiniPloomesContext context)
        {
            _context = context;
        }

        //User
        public ExposableUserModel GetExposableUserByToken(string token)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Token = @token", connection);
                cmd.Parameters.Add(new SqlParameter("@token", token));

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ExposableUserModel exposableUser =
                        new ExposableUserModel(
                            (int)reader["Id"],
                            reader["Name"].ToString(),
                            reader["Email"].ToString(),
                            Convert.ToDateTime(reader["CreateDate"].ToString()),
                            reader["Token"].ToString()
                    );
                    connection.Close();
                    return exposableUser;
                }
            }
            return null;
            //return _context.Users.FirstOrDefault(u => u.Token == token);
        }
        public ExposableUserModel ValidateUser(string email, string password)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Email = @email AND Password = @password", connection);
                cmd.Parameters.Add(new SqlParameter("@email", email));
                cmd.Parameters.Add(new SqlParameter("@password", password));

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ExposableUserModel exposableUser =
                        new ExposableUserModel(
                            (int)reader["Id"],
                            reader["Name"].ToString(),
                            reader["Email"].ToString(),
                            Convert.ToDateTime(reader["CreateDate"].ToString()),
                            reader["Token"].ToString()
                    );
                    connection.Close();
                    return exposableUser;
                }
            }
            return null;
            //return _context.Users.Find(u => u.Email == email && u.Password == password);
        }
        public void AddUser(User user)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Users VALUES (@name, @email, @password, @token, @createDate)", connection);
                cmd.Parameters.Add(new SqlParameter("@name", user.Name));
                cmd.Parameters.Add(new SqlParameter("@email", user.Email));
                cmd.Parameters.Add(new SqlParameter("@password", user.Password));
                cmd.Parameters.Add(new SqlParameter("@token", user.Token));
                cmd.Parameters.Add(new SqlParameter("@createDate", user.CreateDate));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateUser(string name, string email, string password, string token)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Users SET Name = @name, Email = @email, Password = @password WHERE Token = @token", connection);
                cmd.Parameters.Add(new SqlParameter("@name", name));
                cmd.Parameters.Add(new SqlParameter("@email", email));
                cmd.Parameters.Add(new SqlParameter("@password", password));
                cmd.Parameters.Add(new SqlParameter("@token", token));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void RemoveUser(string token)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Token = @token", connection);
                cmd.Parameters.Add(new SqlParameter("@token", token));

                connection.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public void RemoveToken(string token)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Users SET Token = NULL WHERE Token = @token", connection);
                cmd.Parameters.Add(new SqlParameter("@token", token));

                connection.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public string UpdateTokenById(int id)
        {
            string newToken = Guid.NewGuid().ToString();

            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Users SET Token = @newToken WHERE Id = @id", connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@newToken", newToken));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            return newToken;
        }


        //Contact
        public Contact GetContactByIdAndUser(int contactId, int userId)
        {


            return _context.Contacts.FirstOrDefault(c => c.Id == contactId && c.CreatorId == userId);
        }

        public void AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
        }

        public void RemoveContact(Contact contact)
        {
            _context.Contacts.Remove(contact);
        }


        //Deal
        public Deal GetDealByIdAndUser(int dealId, int userId)
        {
            return _context.Deals.FirstOrDefault(c => c.Id == dealId && c.CreatorId == userId);
        }

        public void AddDeal(Deal deal)
        {
            _context.Deals.Add(deal);
        }

        public void RemoveDeal(Deal deal)
        {
            _context.Deals.Remove(deal);
        }




        
    }
}
