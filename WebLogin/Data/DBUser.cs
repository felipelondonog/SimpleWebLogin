using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLogin.Models;

namespace WebLogin.Data
{
    public static class DBUser
    {
        // SQL Server DB connection
        private static string SQLConn = "Paste here your connection.";

        public static bool Register(UserDTO user)
        {
            bool result = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(SQLConn))
                {
                    string query = "INSERT INTO USERS(UserName, Email, Pwd, ResetPwd, Confirmed, Token)";
                    query += "VALUES (@userName, @email, @pwd, @resetPwd, @confirmed, @token)";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@userName", user.UserName);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@pwd", user.Pwd);
                    cmd.Parameters.AddWithValue("@resetPwd", user.ResetPwd);
                    cmd.Parameters.AddWithValue("@confirmed", user.Confirmed);
                    cmd.Parameters.AddWithValue("@token", user.Token);
                    cmd.CommandType = System.Data.CommandType.Text;

                    connection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();

                    if(affectedRows > 0) result = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserDTO Validate(string email, string pwd)
        {
            UserDTO user = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQLConn))
                {
                    string query = "SELECT UserName, ResetPwd, Confirmed from USERS WHERE Email = @email and Pwd = @pwd";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@pwd", pwd);
                    cmd.CommandType = System.Data.CommandType.Text;

                    connection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if(dr.Read())
                        {
                            user = new UserDTO()
                            {
                                UserName = dr["UserName"].ToString(),
                                ResetPwd = (bool)dr["ResetPwd"],
                                Confirmed = (bool)dr["Confirmed"]
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }

        public static UserDTO GetUser(string email)
        {
            UserDTO user = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQLConn))
                {
                    string query = "SELECT UserName, Pwd, ResetPwd, Confirmed, Token from USERS WHERE Email = @email";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.CommandType = System.Data.CommandType.Text;

                    connection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new UserDTO()
                            {
                                UserName = dr["UserName"].ToString(),
                                Pwd = dr["Pwd"].ToString(),
                                ResetPwd = (bool)dr["ResetPwd"],
                                Confirmed = (bool)dr["Confirmed"],
                                Token = dr["Token"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }

        public static bool ResetUser(int resetPwd, string pwd, string token)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQLConn))
                {
                    string query = @"UPDATE USERS set "+
                        "ResetPwd = @reset, " +
                        "Pwd = @pwd " +
                        "WHERE Token = @token";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@reset", resetPwd);
                    cmd.Parameters.AddWithValue("@pwd", pwd);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = System.Data.CommandType.Text;

                    connection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0) result = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ConfirmUser(string token)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(SQLConn))
                {
                    string query = "UPDATE USERS set " +
                        "Confirmed = 1 " +
                        "WHERE Token=@token";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = System.Data.CommandType.Text;

                    connection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0) result = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}