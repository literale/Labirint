using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labirint
{
    public partial class Form1 : Form
    {
        Labirints labirint = new Labirints();
        Genetics genetic = new Genetics();
        Random r = new Random();       
        int counter = 0;
        int way = 0;
        bool onlyWall = false;
        List <Way> arcive=new List<Way>();
        List<Way> alive=new List<Way>();
        int time=0;
        int size = 12;//размерность массива живых лабиринтов в конце каждой итерации
        int x; int y;
        public Form1()
        {
            InitializeComponent();
            btnSearch.Enabled = false;
            radioButton2.Checked = true;
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnGeneric_Click(object sender, EventArgs e)//Генерируем лабиринт
        {
            DeletLabirint();
            
            try
            {

                labirint.width = Convert.ToInt32(tbWidth.Text);
                labirint.height = Convert.ToInt32(tbHeight.Text);
                if (labirint.height == 0 || labirint.width == 0)
                {
                    MessageBox.Show("Введите размеры лабиринта!");
                }
                else
                {
                    labirint.labirint = new int[labirint.height, labirint.width];                    
                    counter = 1;
                    labirint.DoLabirint();
                    DrawLabirint();
                    btnSearch.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Введите размеры лабиринта!");
            }
        }

        private void DrawLabirint()//Рисуем лабиринт
        {
            x = 10;
            y = 10;
            DeletLabirint();
            
            for (int i = 0; i <= labirint.width; i++)
            {
                PictureBox pb = new PictureBox();
                pb.Location = new System.Drawing.Point(x, y);
                if (i == 0)
                { 
                    pb.Size = new System.Drawing.Size(2, 2);
                    pb.Visible = true;                    
                    pb.Image = Properties.Resources._04;
                    x += 2;
                    
                }
                else
                {
                    pb.Size = new System.Drawing.Size(16, 2);
                    pb.Visible = true;
                    pb.Image = Properties.Resources._01;
                    x += 16;
                }
                this.panel2.Controls.Add(pb);
            }
            y += 2;
            for (int i=0; i < labirint.height; i++)
            {
                x = 10;
                
                PictureBox pb = new PictureBox();
                pb.Location = new System.Drawing.Point(x, y);
                pb.Size = new System.Drawing.Size(2, 16);
                pb.Visible = true;
                pb.Image = Properties.Resources._02;
                this.panel2.Controls.Add(pb);
                x += 2;
                for (int j = 0; j < labirint.width; j++)
                {
                    switch (labirint.labirint[i,j])
                    {
                        case 0:
                            {
                                PictureBox pb2 = new PictureBox();
                                pb2.Location = new System.Drawing.Point(x, y);
                                pb2.Size = new System.Drawing.Size(16, 16);
                                pb2.Visible = true;
                                pb2.Image = Properties.Resources._5;
                                x += 16;
                                this.panel2.Controls.Add(pb2);
                                break;
                            }
                        case 1:
                            {
                                PictureBox pb2 = new PictureBox();
                                pb2.Location = new System.Drawing.Point(x, y);
                                pb2.Size = new System.Drawing.Size(16, 16);
                                pb2.Visible = true;
                                pb2.Image = Properties.Resources._1;
                                x += 16;
                                this.panel2.Controls.Add(pb2);
                                break;
                            }
                        case 2:
                            {
                                PictureBox pb2 = new PictureBox();
                                pb2.Location = new System.Drawing.Point(x, y);
                                pb2.Size = new System.Drawing.Size(16, 16);
                                pb2.Visible = true;
                                pb2.Image = Properties.Resources._2;
                                x += 16;
                                this.panel2.Controls.Add(pb2);
                                break;
                            }
                        case 3:
                            {
                                PictureBox pb2 = new PictureBox();
                                pb2.Location = new System.Drawing.Point(x, y);
                                pb2.Size = new System.Drawing.Size(16, 16);
                                pb2.Visible = true;
                                pb2.Image = Properties.Resources._3;
                                x += 16;
                                this.panel2.Controls.Add(pb2);
                                break;
                            }
                    }

                }
                y += 16;
            }
        }

        private void btnFall_Click(object sender, EventArgs e)//Удаляем все
        {
            onlyWall = false;
            labirint = new Labirints();
           radioButton2.Checked = true;
            radioButton1.Checked = false;
            labirint.height = 0;
            tbHeight.Text = "";
            labirint.width = 0;
            tbWidth.Text = "";
            way = 0;
            tbWay.Text = "";
            lbTime.Text = "Время: ";
            label4.Text = "";
            lbWay.Text = "Длиина пути итого: ";
            DeletLabirint();
                        
            btnSearch.Enabled = false;
            int counter = 1;
        }

        private void DeletLabirint()//Удаляем рисунок лабиринта
        { 
            int length = panel2.Controls.Count;
            for (int i = 0; i < length; i++)
            {
                var ctrl = panel2.Controls[i];
                if (ctrl is PictureBox)
                {
                    panel2.Controls.RemoveAt(i);
                    i--;
                    length--;
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            labirint.Make2xLabirint();
             arcive = new List<Way>();
             alive = new List<Way>();
            bool flag = false;
           try
            {
                way = Convert.ToInt32(tbWay.Text);
                Stopwatch timer = new Stopwatch();
                timer.Start();

                genetic.Genetic(way, alive, labirint, arcive, size, onlyWall);
                timer.Stop();
                lbTime.Text = "Время: " + timer.Elapsed.TotalSeconds.ToString();
                flag = true;
            }
            catch
            {
                MessageBox.Show("Введите длинну пути!");
            }
            if (flag)
            {
                genetic.SortAlive(alive);
                lbWay.Text = "Длинна пути итого: " + alive[0].way;
                labirint.labirintCopy = new int[labirint.height * 2, labirint.width * 2];
                labirint.Copy();             
                alive[0].Wave(labirint.labirintCopy, labirint.width * 2, labirint.height * 2);
                alive[0].ToDrawWawe(labirint.labirintCopy);
                DrawWays(labirint.labirintCopy);
                label4.Text = "Итерации: " + genetic.counter + '\n'+ "Вход " + (alive[0].Xentry+1) + "," + (alive[0].Yentry+1) + '\n' + "Выход: " + (alive[0].Xexit+1) + "," + (alive[0].Yexit+1);
            }
        }//ищем

        public void DrawWays(int[,] labirintx2)//рисуем с путем;
        {
            int x = 10; int y = 10;
            DeletLabirint();
            for (int i = 0; i <= labirint.width; i++)
            {
                PictureBox pb = new PictureBox();
                pb.Location = new System.Drawing.Point(x, y);
                if (i == 0)
                {
                    pb.Size = new System.Drawing.Size(2, 2);
                    pb.Visible = true;
                    pb.Image = Properties.Resources._04;
                    x += 2;

                }
                else
                {
                    pb.Size = new System.Drawing.Size(16, 2);
                    pb.Visible = true;
                    pb.Image = Properties.Resources._01;
                    x += 16;
                }
                this.panel2.Controls.Add(pb);
            }
            y += 2;
            for (int i = 0; i < labirint.height; i++)
            {
                x = 10;

                PictureBox pb = new PictureBox();
                pb.Location = new System.Drawing.Point(x, y);
                pb.Size = new System.Drawing.Size(2, 16);
                pb.Visible = true;
                pb.Image = Properties.Resources._02;
                this.panel2.Controls.Add(pb);
                x += 2;
                for (int j = 0; j < labirint.width; j++)
                {
                    if (labirintx2[i*2, j*2]==1)
                    {
                        if(labirintx2[i * 2, j * 2 + 1]==-1)
                        {
                            if (labirintx2[i * 2 + 1, j * 2] ==-1)
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._30;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                            else
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._10;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                        }
                        else
                        {
                            if (labirintx2[i * 2 + 1, j * 2] ==-1)
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._20;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                            else
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._50;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                        }
                    }
                    else
                    {
                        if (labirintx2[i * 2, j * 2 + 1] == -1)
                        {
                            if (labirintx2[i * 2 + 1, j * 2] == -1)
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._3;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                            else
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._1;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                        }
                        else
                        {
                            if (labirintx2[i * 2 + 1, j * 2] == -1)
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._2;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                            else
                            {
                                pb = new PictureBox();
                                pb.Location = new System.Drawing.Point(x, y);
                                pb.Size = new System.Drawing.Size(16, 16);
                                pb.Visible = true;
                                pb.Image = Properties.Resources._5;
                                this.panel2.Controls.Add(pb);
                                x += 16;
                            }
                        }
                    }

                }
                y += 16;
            }
        }    

        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            onlyWall = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            onlyWall = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
