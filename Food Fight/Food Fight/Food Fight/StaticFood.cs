using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace FoodFight
{
    class StaticFood : Object
    {
        public int foodtype;

        public StaticFood(Texture2D newTexture, Random rng, int ScreensizeX, int ScreensizeY, int Foodtype)
        {
            texture = newTexture;
            checkposition(rng, ScreensizeX, ScreensizeY);
            size = new Vector2(24f, 24f);
            this.foodtype = Foodtype;
            eaten=false;
        }
    }
}
