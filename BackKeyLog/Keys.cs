using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackKeyLog
{
    public partial class Keys : Form
    {
        public Keys()
        {
            InitializeComponent();
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
    }
    
}
