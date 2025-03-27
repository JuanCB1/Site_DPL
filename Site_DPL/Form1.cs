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
    public partial class Form1: Form
    {
        private int tempoRestante;
        private bool emDescanso;
        private int tempoEstudoTotal = 0;
        private int tempoForaDaTela = 0;
        private DateTime tempoSaiuDaTela;
        public Form1()
        {
            InitializeComponent();
            ConfigurarTimer();
        }

        [DllImport("user32.dll")]
        private static extern void ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }
        private void ConfigurarTimer()
        {
            timer1.Interval = 1000;
            timer1.Tick -= timer1_Tick;
            timer1.Tick += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tempoRestante > 0)
            {
                tempoRestante--;
                tempoEstudoTotal++;
                AtualizarLabelTempo(); // Atualiza o tempo no Label
            }
            else
            {
                timer1.Stop();
                if (!emDescanso)
                {
                    MessageBox.Show("Tempo de foco acabou! Iniciando descanso...");
                    IniciarDescanso();
                }
                else
                {
                    MessageBox.Show("Descanso concluído! Hora de voltar ao foco.");
                    btnIniciar.Visible = true;
                    btnPausar.Visible = false;
                    btnDescanso.Visible = false;
                }
            }
            AtualizarLblTimes();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            IniciarFoco();
        }
        private void IniciarFoco()
        {
            tempoRestante = 25 * 60; // 25 minutos
            emDescanso = false;
            btnIniciar.Visible = false;
            btnPausar.Visible = true;
            btnDescanso.Visible = true;
            AtualizarLabelTempo();
            timer1.Start();
        }
        private void IniciarDescanso()
        {
            tempoRestante = 5 * 60; // 5 minutos
            emDescanso = true;
            btnPausar.Visible = true;
            btnDescanso.Visible = false;
            AtualizarLabelTempo();
            timer1.Start();
        }
        private void btnPausar_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnIniciar.Visible = true;
            btnPausar.Visible = false;
        }

        private void btnDescanso_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            IniciarDescanso();
        }
        private void AtualizarLabelTempo()
        {
            int minutos = tempoRestante / 60;
            int segundos = tempoRestante % 60;
            lblTime.Text = $"{minutos:D2}:{segundos:D2}";
        }
        private void AtualizarLblTimes()
        {
            int minutosEstudo = tempoEstudoTotal / 60;
            int segundosEstudo = tempoEstudoTotal % 60;

            int minutosForaTela = tempoForaDaTela / 60;
            int segundosForaTela = tempoForaDaTela % 60;

            lblTimes2.Text = $"Estudo: {minutosEstudo:D2}:{segundosEstudo:D2} | Fora da tela: {minutosForaTela:D2}:{segundosForaTela:D2}";
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (chbModo.Checked && tempoRestante > 0) // Só continua se o modo estiver ativado e ainda houver tempo
            {
                timer1.Start();
            }
            if (tempoSaiuDaTela != DateTime.MinValue) 
            {
                TimeSpan tempoFora = DateTime.Now - tempoSaiuDaTela;
                tempoForaDaTela += (int)tempoFora.TotalSeconds; // Soma os segundos passados
                AtualizarLblTimes();
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (chbModo.Checked) // Se o modo estiver ativado, o timer para
            {
              timer1.Stop();
              tempoSaiuDaTela = DateTime.Now; // Guarda o horário de saída  
              AtualizarLblTimes(); 
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Texto (*.txt)|*.txt|Todos os Arquivos (*.*)|*.*";
            openFileDialog.Title = "Abrir Arquivo";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoArquivo = openFileDialog.FileName;

                using (StreamReader reader = new StreamReader(caminhoArquivo))
                {
                    txtBox.Text = reader.ReadToEnd();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Arquivos de Texto (*.txt)|*.txt|Todos os Arquivos (*.*)|*.*";
            saveFileDialog.Title = "Salvar Arquivo";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.FileName = "arquivo";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoArquivo = saveFileDialog.FileName;

                using (StreamWriter writer = new StreamWriter(caminhoArquivo))
                {
                    writer.WriteLine(txtBox.Text);
                }

                MessageBox.Show("Arquivo salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        
        }
    }
}
