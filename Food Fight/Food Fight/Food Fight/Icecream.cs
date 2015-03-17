using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
namespace FoodFight
{
    class Icecream : Object
    {


        public int meltingstage;

        public Icecream(Texture2D newTexture, Random rng, int ScreensizeX, int ScreensizeY)
        {
            texture = newTexture;
            position = new Vector2(700, 240); ;
            size = new Vector2(24f, 24f);
            meltingstage=0;
        }

        public void Update(GameTime gametime,double lastendtime)
        {

            float time = (float)gametime.TotalGameTime.Seconds - (float)lastendtime/1000;
            if (time < 10 && time > 5)
            {
                meltingstage = 2;
            }
            if (time < 15&&time >10)
            {
                meltingstage = 3;
            }
            if (time > 15)
            {
                meltingstage = 4;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!eaten && meltingstage != 4)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
    }
}
