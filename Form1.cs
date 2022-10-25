using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubirFotoBanco
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
		//string da conexão com o banco de dados informando parametros do banco. 
        //uid = usuario
        //server = IP do servidor do banco
        //database = é o banco
        //pwd = senha do banco
        string conexao = "server=localhost;database=database;uid=usuario;pwd=senhaAqui";
		
        private void button2_Click(object sender, EventArgs e)
        {

            //carregar foto do banco

            MySqlConnection conn = new MySqlConnection(conexao);
            conn.Open();

            MemoryStream ms = new MemoryStream();
            FileStream fs;
            Byte[] bindata;

            //Você precisa dar uma condição de algum registro já existente no banco para poder carregar a imagem. 
            //Se atentar para a tabela do banco e a coluna em que será requisitado a foto que será carregada
            MySqlCommand cmd = new MySqlCommand("SELECT foto FROM estoque WHERE id=5", conn);

            bindata = (byte[])(cmd.ExecuteScalar());

            ms.Write(bindata, 0, bindata.Length);
            pictureBox1.Image = new Bitmap(ms);

            fs = new FileStream("Foto Carregada", FileMode.Create, FileAccess.Write);
            ms.WriteTo(fs);

            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //SELECIONANDO ARQUIVO DE IMAGEM PARA SUBIR NO BANCO
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

                try
                {
                    pictureBox1.Load(textBox1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao anexar foto: " + ex);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("Favor selecionar foto!");
                return;
            }
            else
            {
                MySqlConnection conn = new MySqlConnection(conexao);
                conn.Open();
                FileStream fs;
                Byte[] bindata;
                MySqlParameter picpara;
                MySqlCommand cmd = new MySqlCommand("INSERT INTO estoque (foto) VALUES(?foto)", conn);
                picpara = cmd.Parameters.Add("?foto", MySqlDbType.MediumBlob);
                cmd.Prepare();

                //txtPicPath is the path of the image, e.g. C:\MyPic.png

                fs = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Read);
                bindata = new byte[Convert.ToInt32(fs.Length)];
                fs.Read(bindata, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                picpara.Value = bindata;
                cmd.ExecuteNonQuery();

                conn.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox1.Text = "";
        }
    }
}
