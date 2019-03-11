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
    public class Boss : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D sprTexture;
        public Rectangle sprRectangle, ScreenBounds;
        protected Vector2 sprPosition;
        MouseState mState;

        public Boss(Game game, Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition):base(game)
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
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sprBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            //Отрисовка статического босса
            sprBatch.Draw(sprTexture, sprPosition, sprRectangle, Color.White);

            base.Draw(gameTime);
        }
    }
}
