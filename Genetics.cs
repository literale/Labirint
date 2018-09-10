using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirint
{
    class Genetics
    {
        public int counter = 0;
        public void Genetic(int way, List<Way> alive, Labirints labirint, List<Way> arcive, int size, bool onlyWall)
        {
            counter = 0;
            int par = 2500;//Колличество итераций
            GenericFirst(size,onlyWall, labirint, way, alive);
            SortAlive(alive);
            for (int i = 0; i < par; i++)//начинаем селекцию
            {
                if (alive[0].way == way)//если есть лабиринт с необходимым путем - заканчиваем
                {
                    counter = i + 1;
                    break;
                }
                Reproduse(alive, size, onlyWall, way, labirint, arcive);
                int tmp = alive.Count;
                for (int j = 0; j < tmp; j++)//копируем и Мутируем все живые лабиринты для большего разнобразия
                {
                    Way wayNew;
                    wayNew = Mutate(alive[j], onlyWall, labirint);
                    if (!Existed(wayNew,arcive, alive))//добавляем только если такой лабиринт не существовал
                    {

                        labirint.Copy();
                        wayNew.Wave(labirint.labirintCopy, labirint.width * 2, labirint.height * 2);
                        wayNew.ratio = (wayNew.way - way) * (wayNew.way - way);
                        alive.Add(wayNew);
                    }
                }
                SortAlive(alive);
                for (int j = alive.Count - 1; j > size; j--)//оставляем только первые size лабиринтов
                {
                    arcive.Add(alive[j]);
                    alive.RemoveAt(j);
                }
                counter = i + 1;
            }
        }//Генетика

        void GenericFirst(int size, bool onlyWall, Labirints labirint, int way, List<Way> alive)
        {
            Random r = new Random();
            for (int i = 0; i < size; i++)//Генерация первых родителей
            {
                int side = r.Next(0, 4);//0-верхб 1- правоб2- низб 3-лево
                int xentry = 0;
                int yentry = 0;
                int xexit = 0;
                int yexit = 0;


                if (onlyWall)
                {
                    switch (side)//Вход
                    {
                        case 0:
                            {
                                xentry = 0;
                                yentry = r.Next(0, labirint.width);
                                break;
                            }
                        case 1:
                            {
                                yentry = labirint.width - 1;
                                xentry = r.Next(0, labirint.height);
                                break;
                            }
                        case 2:
                            {
                                xentry = labirint.height - 1;
                                yentry = r.Next(0, labirint.width);
                                break;
                            }
                        case 3:
                            {
                                yentry = 0;
                                xentry = r.Next(0, labirint.height);
                                break;
                            }
                    }
                    side = r.Next(0, 4);
                    switch (side)//Выход
                    {
                        case 0:
                            {
                                xexit = 0;
                                yexit = r.Next(0, labirint.width);
                                break;
                            }
                        case 1:
                            {
                                yexit = labirint.width - 1;
                                xexit = r.Next(0, labirint.height);
                                break;
                            }
                        case 2:
                            {
                                xexit = labirint.height - 1;
                                yexit = r.Next(0, labirint.width);
                                break;
                            }
                        case 3:
                            {
                                yexit = 0;
                                xexit = r.Next(0, labirint.height);
                                break;
                            }
                    }
                }
                else
                {
                    xentry = r.Next(0, labirint.height);
                    xexit = r.Next(0, labirint.height);
                    yentry = r.Next(0, labirint.width);
                    yexit = r.Next(0, labirint.width);
                }
                Way way1 = new Way();
                way1.setE(xentry, yentry, xexit, yexit);
                labirint.Copy();
                way1.Wave(labirint.labirintCopy, labirint.width * 2, labirint.height * 2);
                way1.ratio = (way1.way - way) * (way1.way - way);
                alive.Add(way1);
            }
        }//генерация первых вариантов

        public void SortAlive(List<Way> alive)
        {
            Way tmp;
            for (int i = 1; i < alive.Count; i++)//буль-буль
            {
                for (int j = 1; j <= alive.Count - i; j++)
                {
                    if (alive[j].ratio < alive[j - 1].ratio)
                    {
                        tmp = alive[j];
                        alive.Remove(alive[j]);
                        alive.Insert(j - 1, tmp);
                    }
                }
            }
        }//Сортировка живых по их качеству

        public void Reproduse(List<Way> alive, int size, bool onlyWall, int way, Labirints labirint, List<Way> arcive)
        {
            Random r = new Random();
            for (int parOne = 0; parOne < size; parOne++)//перебираем ВСЕ пары. у каждой пары только 1 ребенок
            {
                for (int parTwo = parOne + 1; parTwo < size - 1; parTwo++)
                {
                    Way sonWay = new Way();

                    int type = 0;
                    //вот тут причина входа и выхода в середине!! А вообще это тип скрещивания
                    if (onlyWall)
                    {
                        type = r.Next(0, 2);
                    }
                    else
                    {
                        type = r.Next(0, 6);
                    }
                    switch (type)
                    {
                        case 0:
                            {
                                int xentry = alive[parOne].Xentry;
                                int yentry = alive[parOne].Yentry;
                                int xexit = alive[parTwo].Xexit;
                                int yexit = alive[parTwo].Yexit;
                                sonWay.setE(xentry, yentry, xexit, yexit);
                                break;
                            }
                        case 1:
                            {
                                int xentry = alive[parTwo].Xentry;
                                int yentry = alive[parTwo].Yentry;
                                int xexit = alive[parOne].Xexit;
                                int yexit = alive[parOne].Yexit;

                                sonWay.setE(xentry, yentry, xexit, yexit);
                                break;
                            }
                        case 2:
                            {
                                int xentry = alive[parTwo].Xentry;
                                int yentry = alive[parOne].Yentry;
                                int xexit = alive[parOne].Xexit;
                                int yexit = alive[parTwo].Yexit;
                                sonWay.setE(xentry, yentry, xexit, yexit);
                                break;
                            }
                        case 3:
                            {
                                int xentry = alive[parTwo].Xentry;
                                int yentry = alive[parOne].Yentry;
                                int xexit = alive[parTwo].Xexit;
                                int yexit = alive[parOne].Yexit;
                                sonWay.setE(xentry, yentry, xexit, yexit);
                                break;
                            }
                        case 4:
                            {
                                int xentry = alive[parOne].Xentry;
                                int yentry = alive[parTwo].Yentry;
                                int xexit = alive[parOne].Xexit;
                                int yexit = alive[parTwo].Yexit;
                                sonWay.setE(xentry, yentry, xexit, yexit);
                                break;
                            }
                        case 5:
                            {
                                int xentry = alive[parOne].Xentry;
                                int yentry = alive[parTwo].Yentry;
                                int xexit = alive[parTwo].Xexit;
                                int yexit = alive[parOne].Yexit;
                                sonWay.setE(xentry, yentry, xexit, yexit);
                                break;
                            }
                    }
                    if (!Existed(sonWay, arcive, alive))//Ну понятно, добавляет только если это новый вариант
                    {

                        labirint.Copy();
                        sonWay.Wave(labirint.labirintCopy, labirint.width * 2, labirint.height * 2);
                        sonWay.ratio = (sonWay.way - way) * (sonWay.way - way);
                        alive.Add(sonWay);
                    }
                    sonWay = Mutate(sonWay, onlyWall, labirint);//Обязательно мутируем для большего разнообразия. Бооольше мутаций
                    if (!Existed(sonWay, arcive, alive))
                    {
                        labirint.Copy();
                        sonWay.Wave(labirint.labirintCopy, labirint.width * 2, labirint.height * 2);
                        sonWay.ratio = (sonWay.way - way) * (sonWay.way - way);
                        alive.Add(sonWay);
                    }
                }
            }
        }//Размножение

        bool Existed(Way way, List<Way> arcive, List<Way> alive)
        {
            bool flag = false;
            for (int i = 0; i < arcive.Count; i++)
            {
                if (way.Xentry == arcive[i].Xentry && way.Yentry == arcive[i].Yentry && way.Xexit == arcive[i].Xexit && way.Yexit == arcive[i].Yexit)
                    flag = true;
                if (way.Xentry == arcive[i].Xexit && way.Xexit == arcive[i].Xentry && way.Yentry == arcive[i].Yexit && way.Yexit == arcive[i].Yentry)
                    flag = true;
            }
            for (int i = 0; i < alive.Count; i++)
            {
                if (way.Xentry == alive[i].Xentry && way.Yentry == alive[i].Yentry && way.Xexit == alive[i].Xexit && way.Yexit == alive[i].Yexit)
                    flag = true;
                if (way.Xentry == alive[i].Xexit && way.Xexit == alive[i].Xentry && way.Yentry == alive[i].Yexit && way.Yexit == alive[i].Yentry)
                    flag = true;
            }
            return flag;
        }//Существования такого лабиринта в прошлом
        Way Mutate(Way way, bool onlyWall, Labirints labirint)
        {
            Random r = new Random();
            Way way2 = new Way();
            way2.setE(way.Xentry, way.Yentry, way.Xexit, way.Yexit);
            if (onlyWall)
            {
                int type = r.Next(0, 4);//сдвиг 0-вход по часовой, 1-против часовой, 2 - выход по часовой, 3 - против часовой
                // //for (int i = 0; i<(height+width); i++)
                // //{

                switch (type)
                {
                    case 0:
                        {
                            if (way.Xentry == 0 && way.Yentry < labirint.width - 1)
                            {
                                way2.Yentry++;
                            }
                            else
                            {
                                if (way.Xentry < labirint.height - 1 && way.Yentry == labirint.width - 1)
                                {
                                    way2.Xentry++;
                                }
                                else
                                {
                                    if (way.Xentry == labirint.height - 1 && way.Yentry > 0)
                                        way2.Yentry--;
                                    else
                                    {
                                        if (way.Yentry == 0)
                                            way2.Xentry--;
                                    }
                                }
                            }
                            break;
                        }
                    case 1:
                        {
                            if (way.Xentry == 0 && way.Yentry > 0)
                            {
                                way2.Yentry--;
                            }
                            else
                            {
                                if (way.Xentry > 0 && way.Yentry == labirint.width - 1)
                                {
                                    way2.Xentry--;
                                }
                                else
                                {
                                    if (way.Xentry == labirint.height - 1 && way.Yentry < labirint.width - 1)
                                        way2.Yentry++;
                                    else
                                    {
                                        if (way.Yentry == 0)
                                            way2.Xentry++;
                                    }
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            if (way.Xexit == 0 && way.Yexit < labirint.width - 1)
                            {
                                way2.Yexit++;
                            }
                            else
                            {
                                if (way.Xexit < labirint.height - 1 && way.Yexit == labirint.width - 1)
                                {
                                    way2.Xexit++;
                                }
                                else
                                {
                                    if (way.Xexit == labirint.height - 1 && way.Yexit > 0)
                                        way2.Yexit--;
                                    else
                                    {
                                        if (way.Yexit == 0)
                                            way2.Xexit--;
                                    }
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            if (way.Xexit == 0 && way.Yexit > 0)
                            {
                                way2.Yexit--;
                            }
                            else
                            {
                                if (way.Xexit > 0 && way.Yexit == labirint.width - 1)
                                {
                                    way2.Xexit--;
                                }
                                else
                                {
                                    if (way.Xexit == labirint.height - 1 && way.Yexit < labirint.width - 1)
                                        way2.Yexit++;
                                    else
                                    {
                                        if (way.Yexit == 0)
                                            way2.Xexit++;
                                    }
                                }
                            }
                            break;
                        }
                }
            }

            else
            {
                int type = r.Next(0, 4);//вверх, вправо, вниз, влево
                int EorE = r.Next(0, 2);//вход или выход двигаем

                if (EorE == 0)
                {
                    switch (type)
                    {
                        case 0:
                            {
                                if (way.Xentry > 0)
                                    way2.Xentry--;
                                break;
                            }
                        case 1:
                            {
                                if (way.Yentry < labirint.width - 1)
                                    way2.Yentry++;

                                break;
                            }
                        case 2:
                            {
                                if (way.Xentry < labirint.height - 1)
                                    way2.Xentry++;
                                break;
                            }
                        case 3:
                            {
                                if (way.Yentry > 0)
                                    way2.Yentry--;
                                break;
                            }
                    }
                }
                else
                {
                    switch (type)
                    {
                        case 0:
                            {
                                if (way.Xexit > 0)
                                    way2.Xexit--;
                                break;
                            }
                        case 1:
                            {
                                if (way.Yexit < labirint.width - 1)
                                    way2.Yexit++;

                                break;
                            }
                        case 2:
                            {
                                if (way.Xexit < labirint.height - 1)
                                    way2.Xexit++;
                                break;
                            }
                        case 3:
                            {
                                if (way.Yexit > 0)
                                    way2.Yexit--;
                                break;
                            }
                    }

                }
            }

            return way2;
        }//Мутация
    }
}
