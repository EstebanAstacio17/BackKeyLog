using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace BackKeyLog
{
    public partial class Create : Form
    {
        // Cambia esta cadena de conexión según tu configuración de SQL Server
        string connectionString = "Data Source=ASTACIO\\SQLEXPRESS;Initial Catalog=BackKeyLog;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        private const int MaxPasswordLength = 128;
        public Create()
        {
            InitializeComponent();

            txtPassword.MaxLength = MaxPasswordLength;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateU();
        }

        private void CreateU()
        {
            string userKey = txtUser.Text;
            string password = txtPassword.Text;

            if (!string.IsNullOrEmpty(userKey) && !string.IsNullOrEmpty(password))
            {
                if (!UserExists(userKey))
                {
                    SaveToDatabase(userKey, password);
                }
                else
                {
                    MessageBox.Show("User key already exists. Please choose a different user key.");
                }
                Clean();
            }
            else
            {
                MessageBox.Show("Please enter a user key and a password.");
            }
        }

        private bool UserExists(string userKey)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM BackKeyLog WHERE UserKey = @UserKey";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserKey", userKey);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void SaveToDatabase(string userKey, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO BackKeyLog (UserKey, Password) VALUES (@UserKey, @Password)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserKey", userKey);
                        command.Parameters.AddWithValue("@Password", password);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User key and password saved successfully.");
                            Clean();
                            this.Close(); // Cerrar el formulario después de guardar correctamente
                        }
                        else
                        {
                            MessageBox.Show("Error saving user key and password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Clean()
        {
            txtUser.Text = "";
            txtPassword.Text = "";
        }
    }
}
