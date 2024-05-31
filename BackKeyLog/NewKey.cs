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
    public partial class NewKey : Form
    {
        public NewKey()
        {
            InitializeComponent();
        }

        private void btnKeys_Click(object sender, EventArgs e)
        {
            Keys openKeys = new Keys();
            openKeys.ShowDialog();
        }
    }
}
