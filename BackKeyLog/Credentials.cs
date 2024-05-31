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
    public partial class Credentials : Form
    {
        int idKey = AuthenticatedUser.IdKey;

        // Cadena de conexión a la base de datos
        string connectionString = "Data Source=ASTACIO\\SQLEXPRESS;Initial Catalog=BackKeyLog;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
        
        private const int MaxPasswordLength = 128;
        public Credentials()
        {
            InitializeComponent();

            txtOld.MaxLength = MaxPasswordLength;
            txtNew.MaxLength = MaxPasswordLength;
            txtNew2.MaxLength = MaxPasswordLength;
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ActionToChance();
        }

        private void ActionToChance()
        {
            try
            {
                string oldPassword = txtOld.Text;
                string newPassword = txtNew.Text;
                string confirmPassword = txtNew2.Text;

                // Verificar si el campo de la contraseña actual está vacío
                if (string.IsNullOrEmpty(oldPassword))
                {
                    MessageBox.Show("Please enter your current password.");
                    return;
                }

                // Verificar si la contraseña nueva y la confirmación son iguales
                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("New passwords do not match.");
                    return;
                }

                // Verificar la contraseña actual
                if (VerifyCurrentPassword(idKey, oldPassword))
                {
                    // Actualizar la contraseña en la base de datos
                    UpdatePassword(idKey, newPassword);
                }
                else
                {
                    MessageBox.Show("The current password is incorrect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private bool VerifyCurrentPassword(int idKey, string oldPassword)
        {
            bool isPasswordCorrect = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM BackKeyLog WHERE IdKey = @IdKey AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdKey", idKey);
                    command.Parameters.AddWithValue("@Password", oldPassword);

                    int count = (int)command.ExecuteScalar();
                    isPasswordCorrect = (count > 0);
                }
            }
            return isPasswordCorrect;
        }

        private void UpdatePassword(int idKey, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE BackKeyLog SET Password = @NewPassword WHERE IdKey = @IdKey";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword);
                    command.Parameters.AddWithValue("@IdKey", idKey);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully.");

                        Clean();

                        // Cerrar el formulario
                        this.Close();
                    }
                    
                    else
                    {
                        MessageBox.Show("Error updating password.");
                    }
                }
            }

        }

        private void Clean()
        {
            txtOld.Text = "";
            txtNew.Text = "";
            txtNew2.Text = "";
        }

        
    }
}
