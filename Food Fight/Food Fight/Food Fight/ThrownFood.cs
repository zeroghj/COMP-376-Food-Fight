using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace FoodFight
{
    class ThrownFood : Object
    {
        public int foodtype;
        Vector2 velocity;
        public ThrownFood(Texture2D newtexture, Vector2 newposition, Vector2 target, int Foodtype, int difficulties)
        {
            texture = newtexture;
            position = newposition;
            size = new Vector2(32f, 32f);
            velocity = new Vector2(7, 0);
            difficulties = difficulties / 4 + 1;
            velocitysetter(target, difficulties);
            foodtype = Foodtype;
        }
        public void velocitysetter(Vector2 target, int difficulty)
        {
            Vector2 targetv = new Vector2(target.X - this.position.X, target.Y - this.position.Y);
            float root = (float)Math.Sqrt(Math.Pow(targetv.X, 2) + Math.Pow(targetv.Y, 2));
            Vector2 unittarget;
            unittarget.X = targetv.X / root;
            unittarget.Y = targetv.Y / root;
            velocity.X = unittarget.X * 2 * difficulty;
            velocity.Y = unittarget.Y * 2 * difficulty;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!eaten)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
        public void Update()
        {
            position += velocity;
        }
        public bool outofBound(int ScreensizeX, int ScreensizeY)
        {
            if ((this.position.X < 0) || (this.position.Y < 0) || (this.position.X > ScreensizeX) || (this.position.Y > ScreensizeY))
                return true;
            else
                return false;
        }

    }
}
