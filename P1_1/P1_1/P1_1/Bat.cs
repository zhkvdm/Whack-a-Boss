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
    public class Bat : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D sprTexture, sprTexture2;
        protected Rectangle sprRectangle, ScreenBounds;
        protected Vector2 sprPosition;
        MouseState mState;
        bool InBoards = false;

        public Bat(Game game, Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            sprTexture = newTexture;
            sprRectangle = newRectangle;
            sprPosition = newPosition;
            ScreenBounds = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();

            if (mState.X > 0 && mState.X < ScreenBounds.Width && mState.Y < ScreenBounds.Height - 150 && mState.Y > 0)
                InBoards = true;
            else
                InBoards = false;

            if (InBoards)
            {

                sprPosition.X = mState.X - 45;
                sprPosition.Y = mState.Y - 130;

                if (mState.X < ScreenBounds.Left)
                    sprPosition.X = ScreenBounds.Left;
                if (mState.X > ScreenBounds.Width)
                    sprPosition.X = ScreenBounds.Width;
                if (mState.Y < ScreenBounds.Top)
                    sprPosition.Y = ScreenBounds.Top;
                if (mState.Y > ScreenBounds.Height)
                    sprPosition.Y = ScreenBounds.Height;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sprBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sprBatch.Draw(sprTexture, sprPosition, sprRectangle, Color.White);

            base.Draw(gameTime);
        }

    }
}
