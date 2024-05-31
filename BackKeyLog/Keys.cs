﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BackKeyLog
{
    public partial class Keys : Form
    {
        int idKey = AuthenticatedUser.IdKey;

        // Cadena de conexión a la base de datos
        string connectionString = "Data Source=ASTACIO\\SQLEXPRESS;Initial Catalog=BackKeyLog;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public Keys()
        {
            InitializeComponent();

            // Suscribir al evento FormClosed
            this.FormClosed += MainForm_FormClosed;
        }

        private void Keys_Load(object sender, EventArgs e)
        {
            // Mostrar los registros de la tabla Password para el usuario actual
            ShowPasswordRecords(idKey);

            // Modificar el ancho de la columna "Password"
            dgvKeys.Columns["TituloKey"].Width = 53; // Establece el ancho a 200 píxeles, ajusta según tus necesidades
            dgvKeys.Columns["Password"].Width = 250;

        }

        public void ShowPasswordRecords(int idKey)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT TituloKey, Password FROM Password WHERE IdKey = @IdKey"; // Seleccionar solo la columna Password
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdKey", idKey);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvKeys.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void btnNewKey_Click(object sender, EventArgs e)
        {
            NewKey openNewKey = new NewKey(this);
            openNewKey.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            Credentials openUser = new Credentials();
            openUser.ShowDialog();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvKeys.SelectedRows.Count > 0)
                {
                    // Obtener el valor de la contraseña de la fila seleccionada
                    string password = dgvKeys.SelectedRows[0].Cells["Password"].Value.ToString();

                    // Copiar el texto al portapapeles
                    Clipboard.SetText(password);
                    MessageBox.Show("Password copied to clipboard.");
                }
                else
                {
                    MessageBox.Show("Please select a password to copy.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying password: " + ex.Message);
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // Cerrar la aplicación cuando el formulario se cierre
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

    }
}
