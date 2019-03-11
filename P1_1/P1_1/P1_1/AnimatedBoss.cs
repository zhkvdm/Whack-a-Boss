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

namespace P1_1
{
    class AnimatedBoss : Boss
    {
        protected int countFrame;

        protected const int framesPerSec = 1;
        public int numberOfFrame = 0;
        protected float timePerFrame;
        public float totalElapsed=0, timeOnPick=0;

        Game1 GameInstance;

        protected int
            widthFrame,
            startFrame = 0;

        protected bool
            update = false,
            stop1frame = false;

        public bool IsBossActive = false, Red = false;
        //Переменная для работы с мышью
        MouseState mState;

        SoundEffect soundEffect;
        //Конструктор класса AnimatedBoss
        public AnimatedBoss(Game1 game, Texture2D st, int cFrames, Rectangle newRec, Vector2 newPos)
            : base(game, st, newRec, newPos)//Наследование класса Boss
        {
            GameInstance = game;
            sprRectangle = newRec;
            sprPosition = newPos;
            countFrame = cFrames;
            widthFrame = 89;
        }

        public void Load(ContentManager Content)
        {
            //Загрузка звука
            soundEffect = Content.Load<SoundEffect>("ScreamSound");
        }

        public override void Initialize()
        {
            timePerFrame = (float)1 / framesPerSec;

            base.Initialize();
        }

        //Функция анимации босса
        public virtual void ChangeFrame(float elpT)
        {
            totalElapsed += elpT;
            //Если достигнут шестой кадр анимации, то задержать его на timeOnPick = 1 (1 секунда)
            if (numberOfFrame == 6)
            {
                //Отсчет одной секунды
                timeOnPick+=elpT;
                //Если прошла одна секунда, то скрыть босса
                if (timeOnPick > 1)
                {
                    IsBossActive = false;
                    numberOfFrame++;
                    timeOnPick = 0;
                }
            }
            //Иначе если не достигнут или уже пройден шестой кадр анимации
            else if (totalElapsed > 0.06)
            {
                //Проверка достижения конечного кадра анимации
                if (numberOfFrame == countFrame - 1)
                    numberOfFrame = startFrame;
                else
                    //Смена кадра через 0,06 секунды
                    numberOfFrame++;

                totalElapsed = 0;

                int Top1 = 0, Height1 = 140, Top2 = 140, Height2 = 280;

                if (Red)
                    sprRectangle = new Rectangle((int)widthFrame * numberOfFrame, Top2, sprRectangle.Width, Height2);
                else
                    sprRectangle = new Rectangle((int)widthFrame * numberOfFrame, Top1, sprRectangle.Width, Height1);
            }

        }

        //Фуекция определения нажетия мыши в пределах объекта
        public bool MouseCollide()
        {
            Rectangle R = sprRectangle;
            R.X = (int)sprPosition.X;
            R.Y = (int)sprPosition.Y;
            return R.Contains(new Point(mState.X, mState.Y));
        }

        public override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();
            //Обработчик нажатия левой кнопки мыши
            if (mState.LeftButton == ButtonState.Pressed)
            {
                //Если при щелчке мышью указатель находился в пределах          
                //текущего объекта уничтожаем объект (босса)
                if (MouseCollide() && numberOfFrame != 0)
                {
                    if (IsBossActive)
                    {
                        if (Red)
                            GameInstance.TimeValue++;
                        GameInstance.ScoreValue++;
                        IsBossActive = false;
                        numberOfFrame = 7;
                        soundEffect.Play();
                    }
                }
            }   
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
