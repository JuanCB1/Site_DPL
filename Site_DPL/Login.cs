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
            string user1 = txtUsuario.Text;
            string pass2 = txtSenha.Text;

            if (string.IsNullOrWhiteSpace(user1) || string.IsNullOrWhiteSpace(pass2))
            {
                MessageBox.Show("Preencha todos os campos antes de continuar.");
                return;
            }

            SaveUser(user1, pass2);
            MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
        private void SaveUser(string user1, string pass2)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("users.txt", true))
                {
                    sw.WriteLine($"{user1},{pass2}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar usuário: " + ex.Message);
            }
        }

        private void ValidarCampos(object sender, EventArgs e)
        {
            btnEntrar.Enabled = !string.IsNullOrWhiteSpace(txtUsuario.Text) && !string.IsNullOrWhiteSpace(txtSenha.Text);
        }

        private void chbSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chbSenha.Checked;
        }
    }
}
