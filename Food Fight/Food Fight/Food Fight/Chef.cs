using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
namespace FoodFight
{
    class Chef : Object
    {
        public int direction;
        public int pit;
        public Chef(Texture2D newTexture, Pit[] pit, Random rng)
        {
            texture = newTexture;
            position = pit[getPit(pit, rng)].position;
            position.Y -= 50;
            size = new Vector2(48f, 48f);
        }
        public int getPit(Pit[] pit, Random rng)
        {
            int random = rng.Next(0, 6);
            if (pit[random].Occupied)
            {
                return getPit(pit, rng);
            }
            else
            {
                pit[random].Occupied = true;
                this.pit = random;
                return random;
            }
        }
        public void Update(GameTime gametime, Vector2 playerPosition, Random rng)
        {
            this.Thrown(rng);
        }

        public bool Thrown(Random rng)
        {
            int number = rng.Next(0, 1000);
            if (number > 990)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Foodtype(Random rng)
        {
            int number = rng.Next(0, 100);
            if (number > 90)
            {
                return 2;
            }
            else if (number < 50)
            {
                return 0;
            }
            else
                return 1;
        }
        public ThrownFood throwing(Vector2 playerPosition, ContentManager content, ThrownFood self, Random rng, int difficulty)
        {
            if (this.Thrown(rng))
            {
                int food = this.Foodtype(rng);
                string texture = "";
                switch (food)
                {
                    case 0:
                        texture = "Orange";
                        break;
                    case 1:
                        texture = "banana";
                        break;
                    case 2:
                        texture = "Pizza";
                        break;
                }
                return (new ThrownFood(content.Load<Texture2D>(texture), this.position, playerPosition, food, difficulty));
            }
            else
                return self;
        }
        public void orientation(Vector2 playerPosition)
        {

            Vector2 difference = playerPosition - this.position;
            if ((difference.X>0)&&(Math.Abs(difference.Y)<(Math.Abs(difference.X))))
            {
                direction = 0;
            }
            else if ((difference.Y < 0) && (Math.Abs(difference.Y) >= Math.Abs(difference.X)))
            {
                direction = 1;
            }
            else if ((difference.X <= 0) && (Math.Abs(difference.Y) < Math.Abs(difference.X)))
            {
                direction = 2;
            }
            else if ((difference.Y >= 0) && (Math.Abs(difference.Y) > Math.Abs(difference.X)))
            {
                direction = 3;
            }
           

        }
    }
}
