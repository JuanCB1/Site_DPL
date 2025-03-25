using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Site_DPL
{
    public partial class Login: Form
    {
        public Login()
        {
            InitializeComponent();
            btnEntrar.Enabled = false;
            txtUsuario.TextChanged += ValidarCampos;
            txtSenha.TextChanged += ValidarCampos;
            txtSenha2.TextChanged += ValidarCampos;
        }

        [DllImport("user32.dll")]
        private static extern void ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text;
            string email = txtEmail.Text;
            string pass = txtSenha.Text;
            string pass2 = txtSenha2.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Preencha todos os campos antes de continuar.");
                return;
            }

            if (pass != pass2)
            {
                MessageBox.Show("As senhas não coincidem.");
                return;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains(".com"))
            {
                MessageBox.Show("E-mail inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveUser(user, email, pass);
            MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
        private void SaveUser(string user, string email, string pass)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("users.txt", true))
                {
                    sw.WriteLine($"{user},{email},{pass}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar usuário: " + ex.Message);
            }
        }
        private void ValidarCampos(object sender, EventArgs e)
        {
            btnEntrar.Enabled = !string.IsNullOrWhiteSpace(txtUsuario.Text) && !string.IsNullOrWhiteSpace(txtSenha.Text) && !string.IsNullOrWhiteSpace(txtSenha2.Text) && !string.IsNullOrWhiteSpace(txtEmail.Text);
        }

        private void chbSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha2.UseSystemPasswordChar = !chbSenha2.Checked;
        }

        private void chbSenha2_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha2.UseSystemPasswordChar = !chbSenha2.Checked;
        }
    }
}
