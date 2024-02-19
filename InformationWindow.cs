using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoZeta1
{
    public partial class InformationWindow : Form
    {
        string spliceInfoString;
        public InformationWindow(string string1)
        {
            InitializeComponent();
            spliceInfoString = string1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            txtDialog1.Text = spliceInfoString;
            Clipboard.SetText(txtDialog1.Text);
        }
    }
}
