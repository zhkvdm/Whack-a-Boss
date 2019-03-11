using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace P1_1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //Переменные текстур
        private Texture2D BossTexture, BackTexture, DeskTextureL1, DeskTextureL2, BatTexture, CoinTexture, ClockTexture, WhackTexture, WhompTexture, SpeakerTexture, PlusTexture;
        //Переменные для позиций некоторых текстур
        private Vector2 ScoreStringPosition, GameTimerStringPosition, GameAimStringPosition;
        //Переменная для работы с клавиатурой
        private KeyboardState keyboardState, PastKey;
        protected Rectangle ScreenBounds;
        //Переменная для шрифта
        private SpriteFont Font1;

        //Переменные для управления меню
        private Menu menu = new Menu();
        private bool
            menuState = true,
            gameProcess = false,
            scoreState = false,
            ScoreBoardList = false;
        //Текущий выбор в меню
        private int buttonState = 0;

        Song MainMusic;

        private int GameAim = 10, Score = 0, GameTimer = 20;//
        public int NumOfCollides = 0;
        //Секундный таймер
        private float SecondsTimer;

        //Объявление класса AnimatedBat (анимированная бита)
        private AnimatedBat Bat1;
        //Начальное положение биты
        private Vector2 BatPosition = new Vector2(-100, -100);

        //Список объектов класса AnimatedBoss (анимированный босс)
        List <AnimatedBoss> Boss = new List<AnimatedBoss>();
        List<int> BossPositions = new List<int>();

        //Список объектов класса AnimatedBoss (анимированный босс)
        //List<AnimatedBoss> BossRed = new List<AnimatedBoss>();
        //List<int> BossRedPositions = new List<int>();

        //Файл, хранящий таблицу рекордов
        List<string> ResultList = new List<string>();

        string PlayerName = "";

        int Level = 1;

        //Методы get set для счета игры
        public int ScoreValue
        {
            get { return Score; }
            set { Score = value; }
        }
        //Методы get set для таймера игры
        public int TimeValue
        {
            get { return GameTimer; }
            set { GameTimer = value; }
        }

        public void SaveResult(String name, int value)
        {
            List<String> ResList = new List<String>();
            ResList.AddRange(GetResult());
            int i;
            for (i = 0; i < ResList.Count; i++)
            {
                String s;
                s = ResList[i].Remove(0, ResList[i].IndexOf(' ') + 1);
                if (Convert.ToInt32(s) < value || i == 5)
                    break;
            }
            ResList.Insert(i, name + " " + value);
            while (ResList.Count > 6)
                ResList.RemoveAt(ResList.Count - 1);
            try
            {
                File.WriteAllLines("Content\\Results.txt", ResList);
            }
            catch (Exception) { }
        }

        public List<String> GetResult()
        {
            bool IsFileOk = false;
            List<String> ResList = new List<String>();
            try
            {
                IsFileOk = File.Exists("Content\\Results.txt");
            }
            catch (Exception) { }
            if (IsFileOk)
                ResList.AddRange(File.ReadAllLines("Content\\Results.txt"));
            else
            {
                try
                {
                    File.Create("Content\\Results.txt");
                }
                catch (Exception) { }
            }

            return ResList;
        }


        //Конструктор класса Game1
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Настройка высоты окна
            graphics.PreferredBackBufferHeight = 600;
            //Получение размера окна в переменную ScreenBounds
            ScreenBounds = new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            //Загрузка шрифта
            Font1 = Content.Load<SpriteFont>("Gill Sans Ultra Bold");
            //Определение позиций некоторых текстур
            ScoreStringPosition = new Vector2(100, 30);
            GameTimerStringPosition = new Vector2(100, 120);
            GameAimStringPosition = new Vector2(320, 30);
            //Загрузка текстур
            BackTexture = Content.Load<Texture2D>("GameBackground");
            WhackTexture = Content.Load<Texture2D>("WhackTexture");
            WhompTexture = Content.Load<Texture2D>("WhompTexture");
            DeskTextureL1 = Content.Load<Texture2D>("DeskTextureL1");
            DeskTextureL2 = Content.Load<Texture2D>("DeskTextureL2");
            BossTexture = Content.Load<Texture2D>("BossTexture");
            BatTexture = Content.Load<Texture2D>("BatTexture");
            CoinTexture = Content.Load<Texture2D>("CoinTexture");
            ClockTexture = Content.Load<Texture2D>("ClockTexture");
            SpeakerTexture = Content.Load<Texture2D>("SpeakerTexture");
            PlusTexture = Content.Load<Texture2D>("PlusTexture");
            
            // Загрузка ресурсов для меню
            menu.Load(this.Content);

            MediaPlayer.IsRepeating = true;
            MainMusic = Content.Load<Song>("MainMusic");
            MediaPlayer.Play(MainMusic);

            CreateNewObject();
        }

        protected void CreateNewObject()
        {
            //Создание объектов класса AnimatedBoss (анимированный босс)
            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(234, 150)));
            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(362, 146)));
            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(492, 145)));

            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(180, 225)));
            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(355, 222)));
            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(530, 218)));

            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(110, 318)));
            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(355, 315)));
            Boss.Add(new AnimatedBoss(this, BossTexture, 12, new Rectangle(0, 0, 87, 140), new Vector2(590, 312)));


            for (int i = 0; i < Boss.Count; i++)
            {
                Components.Add(Boss[i]);
                BossPositions.Add(0);
                Boss[i].Load(this.Content);
            }
            
            //Создание объекта класса AnimatedBat (анимированная бита)
            Bat1 = new AnimatedBat(this, BatTexture, 4, new Rectangle(0, 0, 190, 170), new Vector2(-1000, -1000));
            Bat1.Load(this.Content);
            Components.Add(Bat1);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Выход из игры...
            //Читать буфер клавиатуры 
            keyboardState = Keyboard.GetState();
    
            if (keyboardState.IsKeyDown(Keys.OemPlus) && PastKey.IsKeyUp(Keys.OemPlus))
                if (MediaPlayer.State != MediaState.Playing)
                {
                    //Если проигрывание "на паузе"
                    if (MediaPlayer.State == MediaState.Paused)
                        //Возобновим проигрывание
                        MediaPlayer.Resume();
                    //Если проигрывание остановлено
                    if (MediaPlayer.State == MediaState.Stopped)
                        //Начнём проигрывание с начала композиции
                        MediaPlayer.Play(MainMusic);
                }
                else
                    MediaPlayer.Pause();

            //Если menuState == true, то отображаем меню,
            //Если menuState == false, отображаем игровой процесс
            if (menuState)
            {
                //Отслеживаем управление клавиатурой:
                //Обработчик нажатия клавиши Вверх
                if (keyboardState.IsKeyDown(Keys.Up) && PastKey.IsKeyUp(Keys.Up))
                {
                    //Проигрывание звука переключения кнопки меню
                    menu.soundEffect.Play();
                    //Смена текущего элемента меню
                    buttonState = (buttonState + 2) % 3;
                }

                //Обработчик нажатия клавиши Вниз
                if (keyboardState.IsKeyDown(Keys.Down) && PastKey.IsKeyUp(Keys.Down))
                {
                    //Проигрывание звука переключения кнопки меню
                    menu.soundEffect.Play();
                    //Смена текущего элемента меню
                    buttonState = (buttonState + 1) % 3;
                }

                //Обработчик нажатия клавиши Enter на пункте меню "Игра"
                if (buttonState == 0 && keyboardState.IsKeyDown(Keys.Enter) && PastKey.IsKeyUp(Keys.Enter))
                {
                    if (Level == 1)
                    {
                        Boss[1].Visible = false;
                        Boss[3].Visible = false;
                        Boss[5].Visible = false;
                        Boss[7].Visible = false;
                    }
                    else if (Level == 2)
                    {
                        Boss[1].Visible = true;
                        Boss[3].Visible = true;
                        Boss[5].Visible = true;
                        Boss[7].Visible = true;
                    }

                    //Если нужно начать новую игру, то обнуляем счет, время и тд.
                    if (menu.IsNewGame && Level == 1)
                    { 
                        menu.IsNewGame = false;

                        for (int i = 0; i < Boss.Count; i++)
                            Boss[i].numberOfFrame = 11;
                        Score = 0;
                        GameTimer = 15;
                        GameAim = 15;
                    }

                    if (menu.IsNewGame && Level == 2)
                    {
                        menu.IsNewGame = false;

                        for (int i = 0; i < Boss.Count; i++)
                            Boss[i].numberOfFrame = 11;

                        Score = 0;
                        GameTimer = 10;
                        GameAim = 30;
                    }
                    //Если menuState == true, то отображаем меню,
                    //Если menuState == false, отображаем игровой процесс
                    menuState = false;
                    gameProcess = true;
                }
                //Обработчик нажатия клавиши Enter на пункте меню "Счет"
                if (buttonState == 1 && keyboardState.IsKeyDown(Keys.Enter) && PastKey.IsKeyUp(Keys.Enter))
                {
                    ScoreBoardList = true;
                }
                //Обработчик нажатия клавиши Enter на пункте меню "Выход"
                if (buttonState == 2 && keyboardState.IsKeyDown(Keys.Enter) && PastKey.IsKeyUp(Keys.Enter))
                {
                    //Выход
                    this.Exit();
                }

                if (keyboardState.IsKeyDown(Keys.Escape) && PastKey.IsKeyUp(Keys.Escape) && !menu.IsNewGame)
                {
                    //Если menuState == true, то отображаем меню,
                    //Если menuState == false, отображаем игровой процесс
                    menuState = false;
                    gameProcess = true;
                }
            }

            if (ScoreBoardList)
            {
                buttonState = 4;

                ResultList = GetResult();
                menu.ScoreBoardString = "";
                for (int i = 0; i < ResultList.Count && i < 6; i++)
                    menu.ScoreBoardString += ResultList[i] + "\n";

                if (keyboardState.IsKeyDown(Keys.Escape) && PastKey.IsKeyUp(Keys.Escape))
                {
                    buttonState = 0;
                    ScoreBoardList = false;
                    menuState = true;
                }
            }

            if (scoreState)
            {
                buttonState = 3;
                if (keyboardState.IsKeyDown(Keys.Enter) && PastKey.IsKeyUp(Keys.Enter))
                {
                    if (PlayerName != "")
                        SaveResult(PlayerName, Score);

                    buttonState = 0;
                    scoreState = false;

                }
                if (keyboardState.IsKeyDown(Keys.Escape) && PastKey.IsKeyUp(Keys.Escape))
                {
                    buttonState = 0;
                    scoreState = false;

                }
                if (keyboardState.IsKeyDown(Keys.Back) && PastKey.IsKeyUp(Keys.Back))
                {
                    if (PlayerName.Length != 0)
                        PlayerName = PlayerName.Remove(PlayerName.Length - 1);
                }

                if (PlayerName.Length < 9)
                {
                    if (keyboardState.IsKeyDown(Keys.A) && PastKey.IsKeyUp(Keys.A))
                        PlayerName = PlayerName + "Ф";
                    if (keyboardState.IsKeyDown(Keys.B) && PastKey.IsKeyUp(Keys.B))
                        PlayerName = PlayerName + "И";
                    if (keyboardState.IsKeyDown(Keys.C) && PastKey.IsKeyUp(Keys.C))
                        PlayerName = PlayerName + "С";
                    if (keyboardState.IsKeyDown(Keys.D) && PastKey.IsKeyUp(Keys.D))
                        PlayerName = PlayerName + "В";
                    if (keyboardState.IsKeyDown(Keys.E) && PastKey.IsKeyUp(Keys.E))
                        PlayerName = PlayerName + "У";
                    if (keyboardState.IsKeyDown(Keys.F) && PastKey.IsKeyUp(Keys.F))
                        PlayerName = PlayerName + "А";
                    if (keyboardState.IsKeyDown(Keys.G) && PastKey.IsKeyUp(Keys.G))
                        PlayerName = PlayerName + "П";
                    if (keyboardState.IsKeyDown(Keys.H) && PastKey.IsKeyUp(Keys.H))
                        PlayerName = PlayerName + "Р";
                    if (keyboardState.IsKeyDown(Keys.I) && PastKey.IsKeyUp(Keys.I))
                        PlayerName = PlayerName + "Ш";
                    if (keyboardState.IsKeyDown(Keys.J) && PastKey.IsKeyUp(Keys.J))
                        PlayerName = PlayerName + "О";
                    if (keyboardState.IsKeyDown(Keys.K) && PastKey.IsKeyUp(Keys.K))
                        PlayerName = PlayerName + "Л";
                    if (keyboardState.IsKeyDown(Keys.L) && PastKey.IsKeyUp(Keys.L))
                        PlayerName = PlayerName + "Д";
                    if (keyboardState.IsKeyDown(Keys.M) && PastKey.IsKeyUp(Keys.M))
                        PlayerName = PlayerName + "Ь";
                    if (keyboardState.IsKeyDown(Keys.N) && PastKey.IsKeyUp(Keys.N))
                        PlayerName = PlayerName + "Т";
                    if (keyboardState.IsKeyDown(Keys.O) && PastKey.IsKeyUp(Keys.O))
                        PlayerName = PlayerName + "Щ";
                    if (keyboardState.IsKeyDown(Keys.P) && PastKey.IsKeyUp(Keys.P))
                        PlayerName = PlayerName + "З";
                    if (keyboardState.IsKeyDown(Keys.Q) && PastKey.IsKeyUp(Keys.Q))
                        PlayerName = PlayerName + "Й";
                    if (keyboardState.IsKeyDown(Keys.R) && PastKey.IsKeyUp(Keys.R))
                        PlayerName = PlayerName + "К";
                    if (keyboardState.IsKeyDown(Keys.S) && PastKey.IsKeyUp(Keys.S))
                        PlayerName = PlayerName + "Ы";
                    if (keyboardState.IsKeyDown(Keys.T) && PastKey.IsKeyUp(Keys.T))
                        PlayerName = PlayerName + "Е";
                    if (keyboardState.IsKeyDown(Keys.U) && PastKey.IsKeyUp(Keys.U))
                        PlayerName = PlayerName + "Г";
                    if (keyboardState.IsKeyDown(Keys.V) && PastKey.IsKeyUp(Keys.V))
                        PlayerName = PlayerName + "М";
                    if (keyboardState.IsKeyDown(Keys.W) && PastKey.IsKeyUp(Keys.W))
                        PlayerName = PlayerName + "Ц";
                    if (keyboardState.IsKeyDown(Keys.X) && PastKey.IsKeyUp(Keys.X))
                        PlayerName = PlayerName + "Ч";
                    if (keyboardState.IsKeyDown(Keys.Y) && PastKey.IsKeyUp(Keys.Y))
                        PlayerName = PlayerName + "Н";
                    if (keyboardState.IsKeyDown(Keys.Z) && PastKey.IsKeyUp(Keys.Z))
                        PlayerName = PlayerName + "Я";
                    if (keyboardState.IsKeyDown(Keys.OemSemicolon) && PastKey.IsKeyUp(Keys.OemSemicolon))
                        PlayerName = PlayerName + "Ж";
                    if (keyboardState.IsKeyDown(Keys.OemOpenBrackets) && PastKey.IsKeyUp(Keys.OemOpenBrackets))
                        PlayerName = PlayerName + "Х";
                    if (keyboardState.IsKeyDown(Keys.OemPeriod) && PastKey.IsKeyUp(Keys.OemPeriod))
                        PlayerName = PlayerName + "Ю";
                    if (keyboardState.IsKeyDown(Keys.OemCloseBrackets) && PastKey.IsKeyUp(Keys.OemCloseBrackets))
                        PlayerName = PlayerName + "Ъ";
                    if (keyboardState.IsKeyDown(Keys.OemQuotes) && PastKey.IsKeyUp(Keys.OemQuotes))
                        PlayerName = PlayerName + "Э";
                    if (keyboardState.IsKeyDown(Keys.OemComma) && PastKey.IsKeyUp(Keys.OemComma))
                        PlayerName = PlayerName + "Б";
                }

                menu.PlayerName = PlayerName;

            }

            //Отображение игрового процесса, если переменная menuState == false
            if(gameProcess)
            {
                //Выход в меню из игрового процесса
                //Обработчик нажатия клавиши Escape
                if (keyboardState.IsKeyDown(Keys.Escape) && PastKey.IsKeyUp(Keys.Escape))
                {
                    buttonState = 0;
                    gameProcess = false;
                    menuState = true;
                }

                //Получение времени для работа анимаций
                float Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Вызов функции, управляющей процессом игры
                ControlAnimation(Time);
                //Переменная, показывающая что игра не закончена (используется в меню)
                menu.IsGameOver = false;

                //Секундный таймер
                SecondsTimer += Time;
                //Если секундный таймер отсчитал одну секунду
                if (SecondsTimer >= 1)
                {
                    //Уменьшение времени инры на одну секунду
                    GameTimer--;
                    //Обнуление секундного таймера
                    SecondsTimer = 0;
                }
                //Если игровой таймер (отсчитавает оставшееся время игры) равен нулю
                if (GameTimer <= 0)
                {
                    gameProcess = false;

                    //Если счет больше или равен нужному, то победа
                    if (Score >= GameAim)
                        menu.GameResult = true;
                    //Иначе поражение
                    else
                        menu.GameResult = false;

                    if (menu.GameResult)
                    {
                        if (Level == 1)
                            Level = 2;
                        else if (Level == 2)
                        {
                            Level = 1;
                            scoreState = true;
                        }
                    }
                    else
                    {
                        Level = 1;
                        scoreState = true;
                    }

                    //Переменные, показывающие, что игра закончена
                    menu.IsGameOver = true;
                    menu.IsNewGame = true;

                    menuState = true;
                    
                    //Запись текущего счета игры в переменную класса Menu, показывающую этот счет в меню
                    menu.Score = Score;

                }
            }

            PastKey = keyboardState;
            
            base.Update(gameTime);
        }

        //Функция, управляющая процессом игры
        protected void ControlAnimation(float Time)
        {
            int NotNullBoss = 0;

            for (int i = 0; i < Boss.Count; i++)
                if (Boss[i].numberOfFrame != 0)
                {
                    NotNullBoss++;
                    Boss[i].ChangeFrame(Time);
                }


            if (Level == 1)
            {
                if (NotNullBoss < 2)
                    while (true)
                    {
                        Random RandomBossPosition = new Random();
                        int j = RandomBossPosition.Next(8);

                        if (Level == 1 && (j == 1 || j == 3 || j == 5 || j == 7))
                            j++;

                        if (Boss[j].numberOfFrame == 0)
                        {
                            Boss[j].IsBossActive = true;
                            Boss[j].numberOfFrame++;
                            break;
                        }
                    }
            }
            else if (Level == 2)
            {
                for (int i = 0; i < 8; i++)
                    if (Boss[i].IsBossActive == false)
                        Boss[i].Red = false;

                if (NotNullBoss < 3)
                    while (true)
                    {
                        Random RandomBossPosition = new Random();
                        int j = RandomBossPosition.Next(8);

                        int e = RandomBossPosition.Next(8);
                        Boss[e].Red = true;

                        if (Boss[j].numberOfFrame == 0)
                        {
                            Boss[j].IsBossActive = true;
                            Boss[j].numberOfFrame++;
                            break;
                        }
                    }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Если menuState == true, то происходит отрисовка текстур для меню
            if (menuState)
            {
                spriteBatch.Begin();
                //Рисует меню
                menu.DrawMenu(spriteBatch, buttonState);
                spriteBatch.End();
                
            }
            //Иначе происходит отрисовка текстур для игры
            else
            {
                spriteBatch.Begin();
                
                //Отрисовка текстур
                spriteBatch.Draw(BackTexture, new Rectangle(0, 0, 800, 600), Color.White);
                spriteBatch.Draw(WhackTexture, new Rectangle(450, 10, 350, 250), Color.White);
                spriteBatch.Draw(WhompTexture, new Rectangle(280, 140, 250, 150), Color.White);
                if(Level == 1)
                    spriteBatch.Draw(DeskTextureL1, new Rectangle(20, 250, 780, 420), Color.White);
                else if(Level == 2)
                    spriteBatch.Draw(DeskTextureL2, new Rectangle(20, 250, 780, 420), Color.White);
                spriteBatch.Draw(CoinTexture, new Rectangle(30, 25, 60, 60), Color.White);
                spriteBatch.Draw(ClockTexture, new Rectangle(30, 90, 60, 85), Color.White);
                spriteBatch.Draw(menu.MenuItems, new Vector2(180, 20), new Rectangle(370, 200, 140, 65), Color.White);

                //Вывод строк
                spriteBatch.DrawString(Font1, Score.ToString(), ScoreStringPosition, Color.Yellow);
                spriteBatch.DrawString(Font1, GameAim.ToString(), GameAimStringPosition, Color.Yellow);
                spriteBatch.DrawString(Font1, GameTimer.ToString(), GameTimerStringPosition, Color.Yellow);

                base.Draw(gameTime);
                spriteBatch.End();
            }
            spriteBatch.Begin();
            spriteBatch.Draw(SpeakerTexture, new Rectangle(45, 520, 60, 60), Color.White);
            spriteBatch.Draw(PlusTexture, new Rectangle(110, 520, 60, 60), Color.White);
            spriteBatch.End();
        }
    }
}
