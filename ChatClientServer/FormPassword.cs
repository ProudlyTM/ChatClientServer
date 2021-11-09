using System;
using System.Windows.Forms;

namespace ChatClientServer
{
    public partial class FormPassword : Form
    {
        public bool IsBtnOKPressed = false;

        public FormPassword()
        {
            InitializeComponent();
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOKPassword.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void btnShowHidePassword_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.PasswordChar == '*')
            {
                textBoxPassword.PasswordChar = '\0';
            }
            else
            {
                textBoxPassword.PasswordChar = '*';
            }
        }

        private void btnOKPassword_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text != "")
            {
                IsBtnOKPressed = true;
                Close();
            }
            else
            {
                MessageBox.Show("You need to enter a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelPassword_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
