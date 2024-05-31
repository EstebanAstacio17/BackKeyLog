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
    public partial class Login : Form
    {
        // Cadena de conexión a la base de datos
        string connectionString = "Data Source=ASTACIO\\SQLEXPRESS;Initial Catalog=BackKeyLog;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        private const int MaxPasswordLength = 128;
        private const int MaxUserLength = 128;

        public Login()
        {
            InitializeComponent();

            txtPassword.MaxLength = MaxPasswordLength;
            txtUser.MaxLength = MaxUserLength;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Create openCreate = new Create();
            openCreate.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a user key and a password.");
                return;
            }

            try
            {
                int idKey = AuthenticateUser(user, password);
                if (idKey != -1)
                {
                    AuthenticatedUser.IdKey = idKey;
                    Keys openKeys = new Keys();
                    Clean();
                    openKeys.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Incorrect user key or password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private int AuthenticateUser(string user, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT idKey FROM BackKeyLog WHERE UserKey = @UserKey AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserKey", user);
                    command.Parameters.AddWithValue("@Password", password);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        private void Clean()
        {
            txtUser.Text = "";
            txtPassword.Text = "";
        }
    }
}
