using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Collections.Generic;

namespace Assignment10
{
    class DBManager
    {
        private string connStr = Utility.GetConnectionString();

        // Query everything from the person table, as well as their hobby and job
        public DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT p.Id, p.fname, p.lname, p.address, p.facepiece1, p.facepiece2, p.facepiece3, p.facepiece4, h.Name AS Hobby, j.Name AS Job FROM Person p LEFT JOIN Hobby h ON p.HobbyId = h.Id LEFT JOIN Job j ON p.JobId = j.id";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message);
                    }
                }
            }
            return dt;
        }

        // Insert everything into the db, use ids for hobby and job
        public void AddPerson(string firstName, string lastName, string address, string facepiece1, string facepiece2, string facepiece3, string facepiece4, int hobbyId, int jobId)
        {

            string query = "INSERT INTO Person(fname, lname, address, facepiece1, facepiece2, facepiece3, facepiece4, hobbyId, jobId) VALUES(@fn, @ln, @address, @facepiece1, @facepiece2, @facepiece3, @facepiece4, @hobbyId, @jobId)";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fn", firstName);
                    cmd.Parameters.AddWithValue("@ln", lastName);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@facepiece1", facepiece1);
                    cmd.Parameters.AddWithValue("@facepiece2", facepiece2);
                    cmd.Parameters.AddWithValue("@facepiece3", facepiece3);
                    cmd.Parameters.AddWithValue("@facepiece4", facepiece4);
                    cmd.Parameters.AddWithValue("@hobbyId", hobbyId);
                    cmd.Parameters.AddWithValue("@jobId", jobId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding person: " + ex.Message);
                    }
                }
            }
        }

        // Delete person by ID, that's it
        public void DeletePerson(int id)
        {
            string query = "DELETE FROM Person WHERE Id = @id";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting person: " + ex.Message);
                    }
                }
            }
        }

        // Update is similar to add, but changes info by ID instead 
        public void UpdatePerson(int id, string firstName, string lastName, string address, string facepiece1, string facepiece2, string facepiece3, string facepiece4, int hobbyId, int jobId)
        {
            string query = "UPDATE Person SET fname = @fn, lname = @ln, address = @address, facepiece1 = @facepiece1, facepiece2 = @facepiece2, facepiece3 = @facepiece3, facepiece4 = @facepiece4, hobbyId = @hobbyId, jobId = @jobId WHERE Id = @id";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fn", firstName);
                    cmd.Parameters.AddWithValue("@ln", lastName);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@facepiece1", facepiece1);
                    cmd.Parameters.AddWithValue("@facepiece2", facepiece2);
                    cmd.Parameters.AddWithValue("@facepiece3", facepiece3);
                    cmd.Parameters.AddWithValue("@facepiece4", facepiece4);
                    cmd.Parameters.AddWithValue("@hobbyId", hobbyId);
                    cmd.Parameters.AddWithValue("@jobId", jobId);
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating person: " + ex.Message);
                    }
                }
            }
        }

        // Similar to getting all people, but use where to do so by last name
        public DataTable SearchPerson(string lastName)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT p.Id, p.fname, p.lname, p.address, p.facepiece1, p.facepiece2, p.facepiece3, p.facepiece4, h.Name AS Hobby, j.Name AS Job FROM Person p LEFT JOIN Hobby h ON p.HobbyId = h.Id LEFT JOIN Job j ON p.JobId = j.Id WHERE p.lname LIKE @lastName";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@lastName", "%" + lastName + "%");

                    try
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error searching database: " + ex.Message);
                    }
                }
            }

            return dt;
        }

        // Get all hobbies so they aren't hardcoded
        public List<string> GetHobbies()
        {
            List<string> hobbies = new List<string>();
            string query = "SELECT Name FROM Hobby";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            hobbies.Add(reader["Name"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message);
                    }
                }
            }

            return hobbies;
        }

        // Get all jobs so they aren't hardcoded
        public List<string> GetJobs()
        {
            List<string> job = new List<string>();
            string query = "SELECT Name FROM Job";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            job.Add(reader["Name"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message);
                    }
                }
            }

            return job;
        }

    }
}
