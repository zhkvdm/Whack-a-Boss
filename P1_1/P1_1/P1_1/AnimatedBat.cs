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
    class AnimatedBat : Bat
    {
        MouseState mState;
        Game1 GameExemp;

        protected const int framesPerSec = 1;
        protected int
            numberOfFrame = 0,
            widthFrame,
            startFrame = 0,
            countFrame;

        protected float
            timePerFrame,
            totalElapsed;

        protected bool
            update = false,
            stop1frame = false;

        bool BtnPressed = false;

        SoundEffect soundEffect;

        public AnimatedBat(Game1 game, Texture2D st, int cFrames, Rectangle newRec, Vector2 newPos)//, int width, int sF
            : base(game, st, newRec, newPos)
        {
            GameExemp = game;
            sprRectangle = newRec;
            sprPosition = newPos;
            countFrame = cFrames;
            widthFrame = 195;
        }

        public void Load(ContentManager Content)
        {
            soundEffect = Content.Load<SoundEffect>("KickSound");
        }

        public override void Initialize()
        {
            timePerFrame = (float)1 / framesPerSec;

            base.Initialize();
        }

        protected virtual void ChangeFrame(float elpT)
        {
            totalElapsed += elpT;
            if (totalElapsed > 0.04)
            {
                if (numberOfFrame == countFrame - 1)
                    numberOfFrame = startFrame;
                else
                    numberOfFrame++;

                sprRectangle = new Rectangle((int)widthFrame * numberOfFrame, sprRectangle.Top, sprRectangle.Width, sprRectangle.Height);
                totalElapsed = 0;
            }

        }


        public override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed)
            {
                ChangeFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
                if (!BtnPressed)
                {
                    soundEffect.Play();
                    BtnPressed = true;
                }
            }
            else
            {
                sprRectangle.X = 0;
                BtnPressed = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
