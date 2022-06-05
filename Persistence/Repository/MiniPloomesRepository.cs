using MiniPloomes.Entities;
using MiniPloomes.Models.Contacts;
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
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Contacts WHERE Id = @contactId AND CreatorId = @userId;", connection);
                cmd.Parameters.Add(new SqlParameter("@contactId", contactId));
                cmd.Parameters.Add(new SqlParameter("@userId", userId));

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Contact contact =
                        new Contact(
                            (int)reader["Id"],
                            reader["Name"].ToString(),
                            (int)reader["CreatorId"],
                            Convert.ToDateTime(reader["CreateDate"].ToString())
                    );
                    connection.Close();
                    return contact;
                }
            }
            return null;
        }
        public void AddContact(Contact contact)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Contacts OUTPUT INSERTED.Id VALUES (@name, @creatorId, @createDate)  ", connection);
                cmd.Parameters.Add(new SqlParameter("@name", contact.Name));
                cmd.Parameters.Add(new SqlParameter("@creatorId", contact.CreatorId));
                cmd.Parameters.Add(new SqlParameter("@createDate", contact.CreateDate));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateContact(Contact contact)
        {
            using (SqlConnection connection =
                                new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Contacts SET Name = @name WHERE Id = @id", connection);
                cmd.Parameters.Add(new SqlParameter("@name", contact.Name));
                cmd.Parameters.Add(new SqlParameter("@id", contact.Id));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void RemoveContact(int id)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Contacts WHERE Id = @id", connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }


        //Deal
        public Deal GetDealByIdAndUser(int dealId, int userId)
        {
            using (SqlConnection connection =
                  new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Deals WHERE Id = @dealId AND CreatorId = @userId;", connection);
                cmd.Parameters.Add(new SqlParameter("@dealId", dealId));
                cmd.Parameters.Add(new SqlParameter("@userId", userId));

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Deal deal =
                        new Deal(
                            (int)reader["Id"],
                            reader["Title"].ToString(),
                            (decimal)reader["Amount"],
                            (int)reader["ContactId"],
                            (int)reader["CreatorId"],
                            Convert.ToDateTime(reader["CreateDate"].ToString())
                    );
                    connection.Close();
                    return deal;
                }
                return null;
            }
        }
        public void AddDeal(Deal deal)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Deals VALUES (@title, @amount, @contactId, @creatorId, @createdate ) ", connection);
                cmd.Parameters.Add(new SqlParameter("@title", deal.Title));
                cmd.Parameters.Add(new SqlParameter("@amount", deal.Amount));
                cmd.Parameters.Add(new SqlParameter("@contactId", deal.ContactId));
                cmd.Parameters.Add(new SqlParameter("@creatorId", deal.CreatorId));
                cmd.Parameters.Add(new SqlParameter("@createDate", deal.CreateDate));

                connection.Open();
                cmd.ExecuteNonQuery();
            };
        }
        public void UpdateDeal(Deal deal)
        {
            using (SqlConnection connection =
                                new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Deals SET Title = @title, Amount = @amount, ContactId = @contactId  WHERE Id = @id", connection);
                cmd.Parameters.Add(new SqlParameter("@title", deal.Title));
                cmd.Parameters.Add(new SqlParameter("@amount", deal.Amount));
                cmd.Parameters.Add(new SqlParameter("@contactId", deal.ContactId));
                cmd.Parameters.Add(new SqlParameter("@id", deal.Id));

                connection.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public void RemoveDeal(int id)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Deals WHERE Id = @id", connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }




        
    }
}
