using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FoodFight
{

    class Pit : Object
    {
        public bool Occupied = false;
        public Pit(Texture2D newTexture, Random rng, int ScreensizeX, int ScreensizeY)
        {
            texture = newTexture;
            checkposition(rng, ScreensizeX, ScreensizeY);
            size = new Vector2(30f, 10f);
        }
    }
}
