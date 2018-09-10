using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirint
{
    class Labirints
    {
       public int width = 0;
       public int height = 0;
       public int[,] labirint;
       public int[,] labirintCopy;
        public int[,] labirintx2;
       Random r = new Random();
       int[] layer;
        public void DoLabirint()//Алгоритм генерации лабиринта
        {
            layer = new int[width];
            int counter = 1;
            for (counter = 1; counter <= width; counter++)
            {
                layer[counter - 1] = counter;
            } //0- нет стен, 1 - стена справа, 2 - стена снизу, 3 - снизу и справа 
            for (int g = 0; g < height - 1; g++)
            {
                labirint[g, width - 1] = 1; //ставим последнюю правую стену 
                for (int i = 1; i < width; i++)
                {
                    if (layer[i] == layer[i - 1]) //если клетки из одного множества 
                    {
                        labirint[g, i - 1] = 1; // разделяем их стеной 
                    }
                    else //а если нет, то ставим стену (возможно) и объединяем два множества в одно 
                    {
                        int rwall = r.Next(0, 2);
                        if (rwall == 0)//если нет стены - проходим по всему слою и заменяем старое множество новым (соединяем клетки между стенами в одно множество и клетки, относящиеся к этим множествам) 
                        {
                            int target = layer[i - 1];
                            int prey = layer[i];
                            for (int j = 0; j < width; j++)
                                if (layer[j] == prey) layer[j] = target;
                        }

                        labirint[g, i - 1] = rwall; // добавяем в лабиринт стену (возможно, если рволл = 1) 
                    }
                }
                labirint[g, 0] += r.Next(0, 2) * 2; // ставим первую нижнюю стену (возможно) 
                bool flag = false;
                int start = 0;
                if (labirint[g, 0] < 2) // проверяем, есть ли проход вниз 
                    flag = true;
                for (int i = 1; i < width; i++) //цикл нижних стен 
                {
                    if ((layer[i] != layer[i - 1])) //делаем проверку на "переход" (между стенами) 
                    {
                        if (!flag)
                        {
                            int place = r.Next(start, i);
                            labirint[g, place] -= 2;//если есть переход и нет прохода вниз - убираем предыдущую стену вниз 
                        }
                        flag = false; start = i;
                    }
                    labirint[g, i] += r.Next(0, 2) * 2; //ставим нижнюю стену (возможно) 
                    if (labirint[g, i] < 2)
                    {
                        flag = true;
                    }
                }
                if (!flag) //проверяем последнюю клетку 
                    labirint[g, width - 1] -= 2; //убираем нижнюю стену вниз, если в последнем множестве не было прохода 
                for (int i = 0; i < width; i++)//готовим следующий слой 
                {
                    if (labirint[g, i] > 1)//если есть нижняя стена - заменяем старое множество новым (т.к теперь все клетки с нижними принадлежат разным множествам - нижнии стены автоматически удаляются на следующей проверке) 
                    { layer[i] = counter; counter++; }
                }
            } //конец g-цикла

            for (int i = 1; i < width; i++)//обрабатываем последний ряд, что бы избежать петель и замкнутых множеств (иначе выход за границы массива) 
            {
                if (layer[i - 1] == layer[i])// если клетка справа и слева из одного множества 
                {
                    labirint[height - 1, i - 1] = 1;//ставим стену 
                }
                else //объединяем клетки справа и слева в одно множество 
                {
                    int target = layer[i - 1];
                    int prey = layer[i];
                    for (int j = 0; j < width; j++)
                        if (layer[j] == prey) layer[j] = target;
                }
            }
            labirint[height - 1, width - 1] += 1;
            for (int i = 0; i < width; i++)
                labirint[height - 1, i] += 2;

        }
        public void Make2xLabirint()
        {
            labirintx2 = new int[height * 2, width * 2];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    switch (labirint[i, j])
                    {
                        case 0:
                            {
                                labirintx2[i * 2, j * 2] = 0;
                                labirintx2[i * 2, (j * 2) + 1] = 0;
                                labirintx2[(i * 2) + 1, j * 2] = 0;
                                labirintx2[(i * 2) + 1, (j * 2) + 1] = -1;
                                break;
                            }
                        case 1:
                            {
                                labirintx2[i * 2, j * 2] = 0;
                                labirintx2[i * 2, (j * 2) + 1] = -1;
                                labirintx2[(i * 2) + 1, j * 2] = 0;
                                labirintx2[(i * 2) + 1, (j * 2) + 1] = -1;
                                break;
                            }
                        case 2:
                            {
                                labirintx2[i * 2, j * 2] = 0;
                                labirintx2[i * 2, (j * 2) + 1] = 0;
                                labirintx2[(i * 2) + 1, j * 2] = -1;
                                labirintx2[(i * 2) + 1, (j * 2) + 1] = -1;
                                break;
                            }
                        case 3:
                            {
                                labirintx2[i * 2, j * 2] = 0;
                                labirintx2[i * 2, (j * 2) + 1] = -1;
                                labirintx2[(i * 2) + 1, j * 2] = -1;
                                labirintx2[(i * 2) + 1, (j * 2) + 1] = -1;
                                break;
                            }
                    }

                }
            }
        }//превращаем лабиринтв в последоватьльность нулей и единиц размером х2 от начальног лабиринта. 0-проход, 1- стена
        public void Copy()
        {
            labirintCopy = new int[height*2, width*2];
            for (int i = 0; i < height * 2; i++)
            {
                for (int j = 0; j <width * 2; j++)
                {
                    labirintCopy[i, j] = labirintx2[i, j];
                }
            }

        }//Koпия лабиринта
        
    }
}
