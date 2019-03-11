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
    class Menu
    {
        //Закрытые текстуры
        private Texture2D
            menuTexture,
            WhackTexture,
            WhompTexture;
        //Открытые текстуры
        public Texture2D MenuItems;
        //Размер текстур
        private Rectangle buttonNewGameRectangle, buttonGameRectangle, buttonScoresRectangle, buttonExitRectangle;
        //Закрытые поля-ссылки на координаты
        private Vector2 menuPosition, buttonGamePosition, buttonScoresPosition, buttonExitPosition;

        public SoundEffect soundEffect;
        private SpriteFont Font1;
        //Счет игры
        public int Score = 0;
        public string PlayerName = "", ScoreBoardString;

        public bool
            IsNewGame = true,
            IsGameOver = false,
            GameResult = false,
            ScoreBoard = false;

        // Конструктор класса Menu
        public Menu()
        {
            //Определение координат объектов
            menuPosition = new Vector2(0, 0);
            buttonGamePosition = new Vector2(30, 100);
            buttonScoresPosition = new Vector2(30, 200);
            buttonExitPosition = new Vector2(30, 300);

            buttonNewGameRectangle = new Rectangle(0, 105, 620, 100);
            buttonGameRectangle = new Rectangle(0, 0, 215, 100);
            buttonScoresRectangle = new Rectangle(210, 0, 210, 100);
            buttonExitRectangle = new Rectangle(430, 0, 295, 105);
        }

        // Загрузка рисунков
        public void Load(ContentManager Content)
        {
            menuTexture = Content.Load<Texture2D>("PauseBackground");
            MenuItems = Content.Load<Texture2D>("MenuItems");
            WhackTexture = Content.Load<Texture2D>("WhackTexture");
            WhompTexture = Content.Load<Texture2D>("WhompTexture");

            soundEffect = Content.Load<SoundEffect>("TriggerSound");

            Font1 = Content.Load<SpriteFont>("Gill Sans Ultra Bold");
        }

        // Вывод на экран
        public void DrawMenu(SpriteBatch spriteBatch, int buttonState)
        {
            spriteBatch.Draw(menuTexture, menuPosition, Color.White);
            spriteBatch.Draw(WhackTexture, new Rectangle(450, 10, 350, 250), Color.White);

            if (IsGameOver)
            {
                if (GameResult)
                    spriteBatch.Draw(MenuItems, new Vector2(420, 270), new Rectangle(0, 205, 368, 105), Color.White);
                else
                    spriteBatch.Draw(MenuItems, new Vector2(420, 270), new Rectangle(0, 315, 320, 125), Color.White);

                spriteBatch.Draw(MenuItems, new Vector2(440, 425), new Rectangle(510, 205, 250, 55), Color.White);
                spriteBatch.DrawString(Font1, Score.ToString(), new Vector2(690, 430), Color.Yellow);
            }

            //Переключение между объектами меню
            switch (buttonState)
            {
                case 0:
                    spriteBatch.Draw(MenuItems, buttonGamePosition, buttonGameRectangle, Color.White);
                    spriteBatch.Draw(MenuItems, buttonScoresPosition, buttonScoresRectangle, Color.Gray);
                    spriteBatch.Draw(MenuItems, buttonExitPosition, buttonExitRectangle, Color.Gray);
                    break;
                case 1:
                    spriteBatch.Draw(MenuItems, buttonGamePosition, buttonGameRectangle, Color.Gray);
                    spriteBatch.Draw(MenuItems, buttonScoresPosition, buttonScoresRectangle, Color.White);
                    spriteBatch.Draw(MenuItems, buttonExitPosition, buttonExitRectangle, Color.Gray);
                    break;
                case 2:
                    spriteBatch.Draw(MenuItems, buttonGamePosition, buttonGameRectangle, Color.Gray);
                    spriteBatch.Draw(MenuItems, buttonScoresPosition, buttonScoresRectangle, Color.Gray);
                    spriteBatch.Draw(MenuItems, buttonExitPosition, buttonExitRectangle, Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(MenuItems, new Vector2(40, 50), new Rectangle(340, 360, 470, 95), Color.White);
                    spriteBatch.DrawString(Font1, PlayerName, new Vector2(40, 150), Color.Yellow);
                    break;
                case 4:
                    spriteBatch.Draw(MenuItems, new Vector2(40, 50), new Rectangle(355, 265, 280, 90), Color.White);
                    spriteBatch.DrawString(Font1, ScoreBoardString, new Vector2(40, 150), Color.Yellow);
                    break;
            }
        }

    }
}
