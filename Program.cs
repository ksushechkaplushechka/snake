using System.Timers;
using пракическая_работы_на_1;

System.Timers.Timer timer = new System.Timers.Timer(); //таймер

bool quit = false;
int vx;
int vy;
int headX;
int headY;
int[,] GameField;
int w = 80, h = 40;
int score = 0;
int lifes = 1;
int apples;
int level = 0;
chhhhhh zmey = new chhhhhh();
//Заставка
void SplashScreen() //заставка
{

    string[] ss = new string[10]; //создаю массив для 10 строчек
    ss[0] = "  sss   s    s   sss s   s   ssssss";
    ss[1] = " s   s  s    s  s  s s  s    s     ";
    ss[2] = "  s     ss   s s   s s s     s     ";
    ss[3] = "   s    s s  s sssss ss      ssssss";
    ss[4] = "    s   s  s s s   s s s     s     ";
    ss[5] = "     s  s   ss s   s s  s    s     ";
    ss[6] = "s    s  s    s s   s s   s   s     ";
    ss[7] = "  sss   s    s s   s s    s  ssssss";
    ss[8] = "                                   ";
    ss[9] = "Author: ZAA. All right reserved    ";

    Console.ForegroundColor = ConsoleColor.Green; //чтобы все буковки были красивыми
    for (int i = 0; i < ss.Length; i++)
        for (int j = 0; j < ss[i].Length; j++)
        {
            Console.SetCursorPosition(j + 25, i + 10);
            Console.Write(ss[i][j]);
            //System.Console.Beep(200, 1); //зввук с которым будет выводиться заставка
            //System.Threading.Thread.Sleep(5); //скорость с которой будет выводиться заставка
        }
    Console.SetCursorPosition(30, 25);
    Console.Write("Press any key to start");
    Console.ResetColor();

}

void Init() //для консоли
{
    Console.CursorVisible = false; //чтобы курсор не мешался
    Console.SetWindowSize(w + 1, h + 3); //устанавливаем размер окна (консоль)
    Console.SetBufferSize(w + 1, h + 3); //устанавливаем размер окна (консоль), если BufferSize больше WindowSize, то появятся ползунки 
}

void Load(int level = 1) //для загрузки игры
{
    vx = 0; //первоначальное направление змейки
    vy = 1; //первоначальное направление змейки
    headX = w / 2; //где будет находится голова змейки по x
    headY = h / 2; //где будет находится голова змейки по y
    GameField = new int[w + 1, h + 1]; //создаю игровое поле и добовляю 1, таким образом не придется считать с 0
    GameField[headX, headY] = 1; //прорисовка головы

    Random random = new Random();
    apples = 10;
    for (int i = 0; i < apples; i++)
        GameField[random.Next(1, w), random.Next(1, h)] = -1;



    //создаем бардюры
    for (int i = 0; i <= w; i++)
    {
        GameField[i, 0] = 10000;
        GameField[i, h] = 10000;
    }
    for (int i = 0; i < h; i++)
    {
        GameField[0, i] = 10000;
        GameField[w, i] = 10000;
    }
}

void Update()
{
    //GameField[headX, headY] = 0;
    //if (GameField[headX + vx, headY + vy] < 0) Next(headX + vx, headY + vy, 1);
    //else
    {
        headX += vx; //двигаем голову змеи
        headY += vy;
        if (Collision()) return; //проверка на столкновение

        if (GameField[headX, headY] < 0) //если отрицательное значение, то есть яблоко
        {
            score++; //увеличивается кол-во очков
            apples--; //"яблоко" удаляется 
            if (apples == 0)
            {
                quit = true;
                return;
            }
            GameField[headX, headY] = 1; //добовляем длину змеи
            Next(headX - vx, headY - vy, 1, 1); //и добовляем длину в размере 1шт
        }
        else
            Next(headX, headY, 1); //если не съели то змея такой же длины будет прорисовываться

    }
}

bool Collision() //столкновекия
{
    if (GameField[headX, headY] > 0) quit = true; //голова наехала сама на себя (число будет больше 0)
    if (headX < 1 || headX >= w || headY < 1 || headY >= h) quit = true; //врезались в забор
    return quit;
}
void Next(int tailX, int tailY, int n, int p = 0) //рекрсивный медод(когда в методе обращаемся к самому методу) для рисования змеи
{
    //чтобы понять где рисовать хвост
    GameField[tailX, tailY] = n + p; //для добавления хвоста, если "яблоко" не съедено, то не добавляем хвост

    if (GameField[tailX + 1, tailY] == n + p) Next(tailX + 1, tailY, n + 1, p); //смотрим есть ли "цифра" справа от змейки если есть то рисуем туда
    else
        if (GameField[tailX - 1, tailY] == n + p) Next(tailX - 1, tailY, n + 1, p);
    else
        if (GameField[tailX, tailY - 1] == n + p) Next(tailX, tailY - 1, n + 1, p);
    else
        if (GameField[tailX, tailY + 1] == n + p) Next(tailX, tailY + 1, n + 1, p);
    else
        if (p == 0) GameField[tailX, tailY] = 0;

}

void PrintGameField() //рисуем поле
{
    for (int y = 0; y <= h; y++) //обрабатывает строки
        for (int x = 0; x <= w; x++) //обрабатывает столбцы
        {
            Console.SetCursorPosition(x, y + 1); //устанавливаем позицию

            switch (GameField[x, y]) //в зависимости от того чему ровняется в массиве x и y
            {
                case 0: 
                    Console.WriteLine(' ');
                    break;
                case -1:
                    Console.WriteLine('&'); //яблочко
                    break;
                case 1:
                    Console.WriteLine('1'); //голова змейки
                    break;
                default:
                    //Console.WriteLine(GameField[x,y]);
                    Console.WriteLine('█'); //иначе забор
                    break;
            }
        }
    Console.SetCursorPosition(10, 0);
    Console.Write($"Score:{score} Apples:{apples}");
}


void KeyboardUpdate() //чтобы считывались стрелочки
{
    if (Console.KeyAvailable) //если любоя клавиша нажата
    {

        ConsoleKey key = Console.ReadKey().Key; //считываем нажатую клавишу
        Console.Title = DateTime.Now.ToLongTimeString();
        switch (key) //в зависимости от нажатой клавиши
        {

            case ConsoleKey.LeftArrow:

                vx = -1; //чтобы змейка поехала влево
                vy = 0;
                break;
            case ConsoleKey.RightArrow:

                vx = 1; //чтобы змейка поехала вправо
                vy = 0;
                break;
            case ConsoleKey.UpArrow:

                vx = 0; //чтобы змейка поехала вверх
                vy = -1;
                break;
            case ConsoleKey.DownArrow:

                vx = 0; //чтобы змейка поехала вниз
                vy = 1;
                break;
            case ConsoleKey.Escape: //если нажали Escape то выходим из программы
                timer.Stop(); //остановка таймера
                quit = true;
                Console.WriteLine("Bye-bye");
                break;

        }


    }

}

Init(); //чтобы сработали размеры консоли
SplashScreen();
Console.ReadKey();
while (lifes > 0)
{
    Load(++level);
    PrintGameField();
    Console.ReadKey();
    while (!quit)
    {
        KeyboardUpdate();
        Update();
        PrintGameField();
        //System.Threading.Thread.Sleep(5);
    };
    lifes--;
    quit = false;
};