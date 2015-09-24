using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace Quizz
{
    public partial class Form1 : Form
    {
        int questao = 0;
        int acertos = 0;
        string alternativa = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            /*
            MessageBox.Show(alternativa);*/
            //limpaAlternativas();
            //carregaAlernativas(1);
            limpaQuiz();
        }

        private void recebeAlternativa(RadioButton rdoButton)
        {
            if (rdoButton.Checked)
            {
                alternativa = rdoButton.Text;
            }
        }

   
       

        private void carregaQuiz(int id)
        {
            MySqlConnection oConn = new MySqlConnection("Server=localhost;Database=quiz;Uid=root;Pwd=;");
            MySqlConnection oConn2 = new MySqlConnection("Server=localhost;Database=quiz;Uid=root;Pwd=;");
            oConn.Open();
            oConn2.Open();

            MySqlCommand oCmd = new MySqlCommand("SELECT * FROM PERGUNTAS WHERE ID = "+ id, oConn);
            MySqlCommand oCmd2 = new MySqlCommand("SELECT * FROM RESPOSTAS WHERE ID_PERGUNTA = "+ id, oConn2);

            MySqlDataReader oDr = oCmd2.ExecuteReader();
            MySqlDataReader oDr2 = oCmd.ExecuteReader();
            
            while (oDr2.Read())
            {
                if (lblPergunta.Text == "")
                {
                    lblPergunta.Text = oDr2["PERGUNTA"].ToString();
                }
            }

            while (oDr.Read())
            {
                if (lblA.Text == "")
                {
                    lblA.Text = oDr["RESPOSTA"].ToString();
                    oDr.Read();
                }



                if (lblB.Text == "")
                {
                    lblB.Text = oDr["RESPOSTA"].ToString();
                    oDr.Read();
                }

                if (lblC.Text == "")
                {
                    lblC.Text = oDr["RESPOSTA"].ToString();
                    oDr.Read();
                }
            }
            oConn.Close();
        }

        private void limpaQuiz()
        {
            if (questao <= 10)
            {
                checaQuiz();
                questao++;
                lblA.Text = "";
                lblB.Text = "";
                lblC.Text = "";
                lblPergunta.Text = "";
                carregaQuiz(questao);
                
            }

            if(questao == 11)
            {
                groupBox2.Visible = false;
                groupBox1.Visible = false;
                lblResult.Text = "Você acertou " + acertos + " questões!";
                lblResult.Visible = true;
                btnAgain.Visible = true;
            }


            
        }


        private void checaQuiz()
        {
            recebeAlternativa(radioButton1);
            recebeAlternativa(radioButton2);
            recebeAlternativa(radioButton3);

            MySqlConnection oConn = new MySqlConnection("Server=localhost;Database=quiz;Uid=root;Pwd=;");
            oConn.Open();
            MySqlCommand oCmd = new MySqlCommand("SELECT * FROM RESPOSTAS WHERE ID_PERGUNTA = " + questao +" AND LETRA= '" + alternativa +"'", oConn);
            MySqlDataReader oDr = oCmd.ExecuteReader();

            while (oDr.Read())
            {
                if (oDr.GetBoolean("CORRETA")  == true)
                {
                    acertos++;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            questao = 1;
            carregaQuiz(questao);
            lblPergunta.ReadOnly = true;
        }

        private void lblA_Click(object sender, EventArgs e)
        {
            radioButton1.Select();
        }

        private void lblB_Click(object sender, EventArgs e)
        {
            radioButton2.Select();
        }

        private void lblC_Click(object sender, EventArgs e)
        {
            radioButton3.Select();
        }

        private void btnAgain_Click(object sender, EventArgs e)
        {
            questao = 1;
            acertos = 0;
            carregaQuiz(questao);
            groupBox2.Visible = true;
            groupBox1.Visible = true;
            lblResult.Visible = false;
            btnAgain.Visible = false;
        }
    }
}
