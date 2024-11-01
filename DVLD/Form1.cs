using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void applicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Application");
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("people");

        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("drivers");

        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" users");

        }

        private void acountSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("acountSettings");

        }
    }
}
