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
    class Player
    {
        public Vector2 position;
        public Vector2 prevposition = new Vector2(500,500);
        public Texture2D texture { get; set; }
        public Vector2 size { get; set; }     
        public Vector2 velocity;
        public bool moving;
        private Vector2 screenSize { get; set; } //  screen size
        public int stunned;
        public int direction;
        public int Strength;



        public Player(Texture2D newTexture, Vector2 newPosition, Vector2 newSize, int ScreenWidth, int ScreenHeight)
        {
            texture = newTexture;
            position = newPosition;
            size = newSize;
            screenSize = new Vector2(ScreenWidth, ScreenHeight);
            Strength = 200;
            stunned = 0;
            direction = 3;
        }
        public void Move()
        {
            //  if we´ll move out of the screen, invert velocity
            //  checking right boundary
            if (position.X + size.X + velocity.X > screenSize.X)
                position = new Vector2(position.X - 3.0f, position.Y);
            //  checking bottom boundary
            if (position.Y + size.Y + velocity.Y > screenSize.Y)
                position = new Vector2(position.X, position.Y - 3.0f);
            //  checking left boundary
            if (position.X + velocity.X < 0)
                position = new Vector2(position.X+ 3.0f, position.Y);
            //  checking bottom boundary
            if (position.Y + velocity.Y < 0)
                position = new Vector2(position.X,position.Y + 3.0f);
            //  since we adjusted the velocity, just add it to the current position
        }
        public bool collision(Object otherSprite)
        {
            // check if two sprites intersect
            if (otherSprite != null)
            {
                return (this.position.X + this.size.X > otherSprite.position.X &&
                        this.position.X < otherSprite.position.X + otherSprite.size.X &&
                        this.position.Y + this.size.Y > otherSprite.position.Y &&
                        this.position.Y < otherSprite.position.Y + otherSprite.size.Y);
            }
            return false;
        }
        public void Update(
            KeyboardState keyboardState, int timer)
        {
            GetInput(keyboardState);
            orientation();
            if (Strength < 0)
            {
                Strength = 0;
            }
            if (stunned < 0)
            {
                stunned = 0;
            }
            this.Move();
            if (stunned > 0 && timer % 10 == 0)
            { stunned -= 1; }
            velocity.X = 0.0f;
            velocity.Y = 0.0f;
        }

        public void orientation()
        {
            moving = false;
            Vector2 difference = position - prevposition;
            if ((difference.X > 0) && (Math.Abs(difference.Y) < (Math.Abs(difference.X))))
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
            prevposition = position;
            if (!(difference.X == 0 && difference.Y == 0))
            {
                moving = true;
            }
        }
        private void GetInput(
            KeyboardState keyboardState)
        {


            // If any digital horizontal movement input is found, override the analog movement.
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)){
                if (stunned == 0)
                this.position += new Vector2(0, -3);
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)){
                if (stunned == 0)
                this.position += new Vector2(0, 3);
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)){
                if (stunned == 0)
                this.position += new Vector2(-3, 0);
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                if (stunned == 0)
                this.position += new Vector2(3, 0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
