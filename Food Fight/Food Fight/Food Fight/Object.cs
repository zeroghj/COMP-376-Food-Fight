using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace FoodFight
{
    public class Object
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        public bool eaten=false;
        public int countdown = 0;

        public Object()
        {
            texture = null;
            position = new Vector2(0f, 0f);
            size = new Vector2(24f, 24f);
        }
        public Object(Texture2D newTexture, Random rng, int ScreensizeX, int ScreensizeY)
        {
            texture = newTexture;
            checkposition(rng,ScreensizeX,ScreensizeY);
            size = new Vector2(24f, 24f);
        }
        public void checkposition(Random rng, int ScreensizeX, int ScreensizeY){
            position = new Vector2(rng.Next(10, ScreensizeX - 48), rng.Next(30, ScreensizeY - 48));
            if ((position.X >= 650 && position.X >= 750 && position.Y <= 300 && position.Y >= 190)||
            (position.X >= 0 && position.X >= 64 && position.Y <= 360 && position.Y >= 240))
            {
                checkposition(rng, ScreensizeX, ScreensizeY);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!eaten)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
    }
}
