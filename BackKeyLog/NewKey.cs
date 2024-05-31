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
    public partial class NewKey : Form
    {
        int idKey = AuthenticatedUser.IdKey;

        private Keys parentForm; // Agregar referencia al formulario padre

        // Cadena de conexión a la base de datos
        string connectionString = "Data Source=ASTACIO\\SQLEXPRESS;Initial Catalog=BackKeyLog;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        // Constructor modificado para aceptar la referencia del formulario padre
        public NewKey(Keys parent)
        {
            InitializeComponent();
            parentForm = parent;
            this.FormClosed += NewKey_FormClosed; // Suscribir al evento FormClosed
        }

        private void btnKeys_Click(object sender, EventArgs e)
        {
            Keys openKeys = new Keys();
            openKeys.ShowDialog();
        }

        private void btnGenerete_Click(object sender, EventArgs e)
        {
            // Verificar si al menos un checkbox está seleccionado
            if (!chkMax.Checked && !chkMin.Checked && !chkNum.Checked && !chkSpecial.Checked)
            {
                MessageBox.Show("Please select at least one option.");
                return;
            }

            // Generar la contraseña aleatoria
            string password = GeneratePassword();

            // Mostrar la contraseña en el textbox
            txtKey.Text = password;
        }

        private string GeneratePassword()
        {
            // Definir los caracteres posibles según las opciones seleccionadas
            string chars = "";
            if (chkMax.Checked)
                chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (chkMin.Checked)
                chars += "abcdefghijklmnopqrstuvwxyz";
            if (chkNum.Checked)
                chars += "0123456789";
            if (chkSpecial.Checked)
                chars += "!@#$%^&*()-_=+[]{}|;:'\",.<>/?";

            // Generar la contraseña aleatoria
            Random random = new Random();
            StringBuilder password = new StringBuilder();
            int length = 128; // Longitud máxima de la contraseña
            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                // Seleccionar todo el texto en el TextBox
                txtKey.SelectAll();
                // Copiar el texto al portapapeles
                Clipboard.SetText(txtKey.Text);
                MessageBox.Show("Copied to clipboard.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying to clipboard: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int idKey = AuthenticatedUser.IdKey;
                string password = txtKey.Text;
                string title = txtTitle.Text; // Obtener el título del TextBox

                // Verificar si la contraseña está vacía
                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please generate a password first.");
                    return;
                }

                // Guardar la contraseña y el título en la base de datos
                SavePasswordToDatabase(idKey, password, title);

                // Limpiar los controles
                ClearControls();

                // Cerrar el formulario
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SavePasswordToDatabase(int idKey, string password, string title)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Password (IdKey, Password, TituloKey) VALUES (@IdKey, @Password, @Title)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdKey", idKey);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Title", title); // Agregar el título como parámetro
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password saved successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Error saving password.");
                    }
                }
            }
        }

        private void SavePasswordToDatabase(int idKey, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Password (IdKey, Password) VALUES (@IdKey, @Password)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdKey", idKey);
                    command.Parameters.AddWithValue("@Password", password);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password saved successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Error saving password.");
                    }
                }
            }
        }

        private void ClearControls()
        {
            // Limpiar los controles
            txtKey.Text = "";
            chkMax.Checked = false;
            chkMin.Checked = false;
            chkNum.Checked = false;
            chkSpecial.Checked = false;
        }

        // Método para manejar el evento FormClosed
        private void NewKey_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.ShowPasswordRecords(idKey); // Llamar al método para actualizar el DataGridView en el formulario padre
        }

    }
}
