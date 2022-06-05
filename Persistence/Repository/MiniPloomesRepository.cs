﻿using MiniPloomes.Entities;
using MiniPloomes.Models.Contacts;
using MiniPloomes.Models.Users;
using System.Data;
using System.Data.SqlClient;
//using Microsoft.Data.SqlClient;

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
                SqlCommand cmd = new SqlCommand("GetUserByToken", connection);
                cmd.CommandType = CommandType.StoredProcedure;
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
                SqlCommand cmd = new SqlCommand("ValidateUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
                SqlCommand cmd = new SqlCommand("AddUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@name", user.Name));
                cmd.Parameters.Add(new SqlParameter("@email", user.Email));
                cmd.Parameters.Add(new SqlParameter("@password", user.Password));
                cmd.Parameters.Add(new SqlParameter("@token", user.Token));
                cmd.Parameters.Add(new SqlParameter("@createDate", user.CreateDate));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateUser(User user)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@name", user.Name));
                cmd.Parameters.Add(new SqlParameter("@email", user.Email));
                cmd.Parameters.Add(new SqlParameter("@password", user.Password));
                cmd.Parameters.Add(new SqlParameter("@token", user.Token));

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void RemoveUser(string token)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("RemoveUserByToken", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
                SqlCommand cmd = new SqlCommand("RemoveToken", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
                SqlCommand cmd = new SqlCommand("UpdateTokenById", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
                SqlCommand cmd = new SqlCommand("GetContactByIdAndUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
        public int AddContact(Contact contact)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddContact", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@name", contact.Name));
                cmd.Parameters.Add(new SqlParameter("@creatorId", contact.CreatorId));
                cmd.Parameters.Add(new SqlParameter("@createDate", contact.CreateDate));

                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Direction = ParameterDirection.Output;

                connection.Open();
                cmd.ExecuteNonQuery();

                int id = Convert.ToInt32(cmd.Parameters["@id"].Value);
                connection.Close();
                return id;
            }
        }
        public void UpdateContact(Contact contact)
        {
            using (SqlConnection connection =
                                new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateContact", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
                SqlCommand cmd = new SqlCommand("RemoveContact", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
                SqlCommand cmd = new SqlCommand("GetDealByIdAndUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
        public int AddDeal(Deal deal)
        {
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddDeal", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@title", deal.Title));
                cmd.Parameters.Add(new SqlParameter("@amount", deal.Amount));
                cmd.Parameters.Add(new SqlParameter("@contactId", deal.ContactId));
                cmd.Parameters.Add(new SqlParameter("@creatorId", deal.CreatorId));
                cmd.Parameters.Add(new SqlParameter("@createDate", deal.CreateDate));

                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Direction = ParameterDirection.Output;

                connection.Open();
                cmd.ExecuteNonQuery();

                int id = Convert.ToInt32(cmd.Parameters["@id"].Value);
                connection.Close();
                return id;
            };
        }
        public void UpdateDeal(Deal deal)
        {
            using (SqlConnection connection =
                                new SqlConnection(connectionString))
            {
                Console.WriteLine(deal.Title);
                SqlCommand cmd = new SqlCommand("UpdateDeal", connection);
                cmd.CommandType = CommandType.StoredProcedure;

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
                SqlCommand cmd = new SqlCommand("RemoveDeal", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@id", id));
                
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
