using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackKeyLog
{
    public partial class Confirm : Form
    {
        int idKey = AuthenticatedUser.IdKey;

        // Cadena de conexión a la base de datos
        string connectionString = "Data Source=ASTACIO\\SQLEXPRESS;Initial Catalog=BackKeyLog;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public bool IsConfirmed { get; private set; } = false;

        // Constructor con parámetros
        public Confirm(int userId)
        {
            InitializeComponent();
            idKey = userId;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string confirmPassword = txtConfirm.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Password FROM BackKeyLog WHERE IdKey = @IdKey";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdKey", idKey);
                        string storedPassword = command.ExecuteScalar()?.ToString();

                        if (storedPassword == confirmPassword)
                        {
                            IsConfirmed = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("The password does not match.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private bool ValidatePassword(int idKey, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM BackKeyLog WHERE IdKey = @IdKey AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdKey", idKey);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}