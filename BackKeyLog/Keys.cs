using System;
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
        }

        private void ShowPasswordRecords(int idKey)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Password FROM Password WHERE IdKey = @IdKey"; // Seleccionar solo la columna Password
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
            NewKey openNewKey = new NewKey();
            openNewKey.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            Credentials openUser = new Credentials();
            openUser.ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // Cerrar la aplicación cuando el formulario se cierre
        }
    }
}
