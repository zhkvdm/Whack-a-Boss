using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace P1_1
{
    class SpriteClass
    {
        public Texture2D spTexture;
        public Vector2 spPosition;

        public SpriteClass(Texture2D newSpTexture, Vector2 newSpPosition)
        {
            spTexture = newSpTexture;
            spPosition = newSpPosition;
        }
    }
}
