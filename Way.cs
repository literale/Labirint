using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.ComponentModel;
using System.Data;

using System.Windows.Forms;

namespace Labirint
{
    class Way
    {
        public int Xentry; public int Yentry;
        public int Xexit; public int Yexit;
        public int way = 0;
        public int ratio;

        public void setE(int xen, int yen, int xex, int yex)
        {
            Xentry = xen;
            Yentry = yen;
            Xexit = xex;
            Yexit = yex;
        }//Ставит вход и выход
        public void Wave(int[,] labirintx2, int wight, int hight)
        {
            int k = 1;
            bool flag = false;
            labirintx2[Xentry * 2, Yentry * 2] = 1;

            for (int l = 0; l <= (wight * hight); l++)//потому что максимальный путь - путь через весь лабиринт
            {
                for (int i = 0; i < hight; i++)
                {
                    for (int j = 0; j < wight; j++)
                    {
                        if (labirintx2[i, j] == k)
                        {
                            if (i + 1 <= hight)
                            {
                                if (labirintx2[i + 1, j] == 0)
                                    labirintx2[i + 1, j] = k + 1;
                                if (i + 1 == Xexit * 2 && j == Yexit * 2)
                                {
                                    flag = true;
                                    break;
                                }

                            }

                            if (i - 1 >= 0)
                            {
                                if (labirintx2[i - 1, j] == 0)
                                    labirintx2[i - 1, j] = k + 1;
                                if (i - 1 == Xexit * 2 && j == Yexit * 2)
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if (j + 1 <= wight)
                            {
                                if (labirintx2[i, j + 1] == 0)
                                    labirintx2[i, j + 1] = k + 1;
                                if (i == Xexit * 2 && j + 1 == Yexit * 2)
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if (j - 1 >= 0)
                            {
                                if (labirintx2[i, j - 1] == 0)
                                    labirintx2[i, j - 1] = k + 1;
                                if (i == Xexit * 2 && j + 1 == Yexit * 2)
                                {
                                    flag = true;
                                    break;
                                }
                            }

                        }
                    }
                    if (flag)
                        break;
                }
                if (flag)
                {
                    way = (k + 1) / 2 + 1;
                    break;

                }
                k++;
            }

        }//Волновое прохождение
        public void ToDrawWawe( int[,] labirintx2)
        {
            int tmp = labirintx2[Xexit * 2, Yexit * 2];
            int x = Xexit * 2, y = Yexit * 2;
           // bool flag = false;
            labirintx2[Xexit * 2, Yexit * 2] = 1;
            int tmp2 = tmp;
            for (int i = 1; i < tmp2; i++)
            {
                try
                {
                    if (labirintx2[x + 1, y] < tmp && labirintx2[x + 1, y] > 1)
                    {
                        tmp = labirintx2[x + 1, y];
                        labirintx2[x + 1, y] = 1;
                        x = x + 1;
                    }
                }
                catch { }
                try
                {
                    if (labirintx2[x - 1, y] < tmp && labirintx2[x - 1, y] > 1)
                    {
                        tmp = labirintx2[x - 1, y];
                        labirintx2[x - 1, y] = 1;
                        x = x - 1;
                    }
                }
                catch { }
                try
                {
                    if (labirintx2[x, y + 1] < tmp && labirintx2[x, y + 1] > 1)
                    {
                        tmp = labirintx2[x, y + 1];
                        labirintx2[x, y + 1] = 1;
                        y = y + 1;
                    }
                }
                catch { }
                try
                {
                    if (labirintx2[x, y - 1] < tmp && labirintx2[x, y - 1] > 1)
                    {
                        tmp = labirintx2[x, y - 1];
                        labirintx2[x, y - 1] = 1;
                        y = y - 1;
                    }
                }
                catch { }

                if (x == Xentry * 2 && y == Yentry * 2)
                {
                    break;
                }
            }


        }//Подготовливает пройденный лабиринт к рисованию       
    }
}
        
    
