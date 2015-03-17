#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion

namespace FoodFight
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        //  Sprite objects
        Player Kathy;
        Icecream icecream;
        public Song Soundtrack;
        int level = 1;
        int timer=0;
        int[] respawntimer= new int[4];
        bool Anim;
        int strength=75;
        StaticFood pizza;
        StaticFood Banana;
        StaticFood Orange;
        SpriteFont font;
        double endleveltime;
        Pit[] pits = new Pit[9];
        Chef[] chef = new Chef[4];
        ThrownFood[] projectile = new ThrownFood[4];
        public Random rng = new Random();
        private KeyboardState keyboardState;

        //  SpriteBatch which will draw (render) the sprite
        SpriteBatch spriteBatch;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            //  changing the back buffer size changes the window size (when in windowed mode)
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Soundtrack = Content.Load<Song>("Soundtrack");
            MediaPlayer.Volume = 1.0f;
            if(level==1)
            MediaPlayer.Play(Soundtrack);
            Kathy = new Player(Content.Load<Texture2D>("StandS"), new Vector2(0f, 300f), new Vector2(48f, 48f),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Kathy.Strength = strength;
            icecream = new Icecream(Content.Load<Texture2D>("icecream0"), rng,
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pizza = new StaticFood(Content.Load<Texture2D>("pizza"), rng,
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 2);
            pizza.eaten = false;
            Banana = new StaticFood(Content.Load<Texture2D>("banana"), rng,
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight,1);
            Orange = new StaticFood(Content.Load<Texture2D>("Orange"), rng,
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight,0);
            Banana.eaten = false;
            Orange.eaten = false;
            icecream.meltingstage = 0;
            for (int i = 0; i < pits.Length; i++)
            {
                pits[i] = new Pit(Content.Load<Texture2D>("pit"), rng,
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            }
            respawntimer[0] = 0;
            respawntimer[1] = 0;
            respawntimer[2] = 0;
            respawntimer[3] = 0;
            projectile[0] = null;
            projectile[1] = null;
            projectile[2] = null;
            projectile[3] = null;
            for (int i = 0; i < chef.Length; i++)
            {
                chef[i] = null;
            }
            // Create a SpriteBatch to render the sprite 
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void disposesomething(Object something)
        {
            if (something != null)
            {
                something.texture.Dispose();
            }

        }
        protected override void UnloadContent()
        {
            //  Free the previously alocated resources
            Kathy.texture.Dispose();
            icecream.texture.Dispose();
            disposesomething(pizza);
            disposesomething(Banana);
            disposesomething(Orange);
            for (int i = 0; i < pits.Length; i++)
            {
                pits[i].texture.Dispose();
            }
            disposesomething(chef[0]);
            disposesomething(chef[1]);
            disposesomething(chef[2]);
            disposesomething(chef[3]);
            if (projectile[0] != null)
                projectile[0].texture.Dispose();
            if (projectile[1] != null)
                projectile[1].texture.Dispose();
            if (projectile[2] != null)
                projectile[2].texture.Dispose();
            if (projectile[3] != null)
                projectile[3].texture.Dispose();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>


        public void DrawHud()
        {


            SpriteFont font = Content.Load<SpriteFont>("Font");
            spriteBatch.DrawString(font, ("Strength: " + Kathy.Strength.ToString()), (new Vector2(300f, 10f)), Color.White);
            spriteBatch.DrawString(font, ("Level: " + this.level), (new Vector2(10f, 10f)), Color.White);
            spriteBatch.DrawString(font, ("FOOD FIGHT !"), (new Vector2(500f, 10f)), Color.White);

            if (Kathy.Strength == 0)
            {
                spriteBatch.DrawString(font, ("Game Over -> Enter to Restart |-| Escape to Exit"), (new Vector2(200f, 220f)), Color.White);
            }
         }
        protected override void Update(GameTime gameTime)
        {

            //static food block
            if (gameTime.TotalGameTime.Milliseconds%100==0) { timer +=1; }
            if (timer%2==0)
            {
                Anim = true;
            }
            else
            {
                Anim = false;
            }
            if (pizza!=null&&pizza.eaten)
            {
                pizza = null;
            }
            if (Banana!=null&&Banana.eaten)
            {
                Banana = null;
            }
            if (Orange != null && Orange.eaten)
            {
                Orange = null;
            }
            if (Kathy.Strength == 0)
            {
                Kathy.stunned = 999999999;
                    keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    if (keyboardState.IsKeyDown(Keys.Enter))
                    {
                        this.level = 1;
                        endleveltime = gameTime.TotalGameTime.TotalMilliseconds;
                        strength = 75;
                        LoadContent();
                    }

            }
            switch (icecream.meltingstage)
            {
                case 1:
                icecream.texture = Content.Load<Texture2D>("icecream0");
                break;
                case 2:
                icecream.texture = Content.Load<Texture2D>("icecream1");
                break;
                case 3:
                icecream.texture = Content.Load<Texture2D>("icecream2");
                break;
                case 4:
                icecream.texture = Content.Load<Texture2D>("icecream3");
                break;
            }
            for (int i = 0; i < chef.Length; i++)
            {
                if (chef[i] != null)
                {
                    chef[i].orientation(Kathy.position);
                    switch (chef[i].direction)
                    {
                        case 0:
                            chef[i].texture = Content.Load<Texture2D>("ChefE");
                            break;
                        case 1:
                            chef[i].texture = Content.Load<Texture2D>("ChefN");
                            break;
                        case 2:
                            chef[i].texture = Content.Load<Texture2D>("ChefW");
                            break;
                        case 3:
                            chef[i].texture = Content.Load<Texture2D>("ChefS");
                            break;

                    }
                }
            }
            Kathy.Update(keyboardState, timer);
            icecream.Update(gameTime, endleveltime);
            if (!Kathy.moving)
            {
                if (Kathy.stunned != 0)
                {
                    if (Anim)
                    {
                        Kathy.texture = Content.Load<Texture2D>("stunned1");
                    }
                    else
                    {
                        Kathy.texture = Content.Load<Texture2D>("stunned2");
                    }
                }
                else
                {
                    switch (Kathy.direction)
                    {
                        case 0:
                            Kathy.texture = Content.Load<Texture2D>("StandE");
                            break;
                        case 1:
                            Kathy.texture = Content.Load<Texture2D>("StandN");
                            break;
                        case 2:
                            Kathy.texture = Content.Load<Texture2D>("standW");
                            break;
                        case 3:
                            Kathy.texture = Content.Load<Texture2D>("StandS");
                            break;

                    }
                }
            }
            else
            {
                if (Anim)
                {
                    switch (Kathy.direction)
                    {
                        case 0:
                            Kathy.texture = Content.Load<Texture2D>("WR1");
                            break;
                        case 1:
                            Kathy.texture = Content.Load<Texture2D>("WN1");
                            break;
                        case 2:
                            Kathy.texture = Content.Load<Texture2D>("WL1");
                            break;
                        case 3:
                            Kathy.texture = Content.Load<Texture2D>("WS1");
                            break;

                    }
                }
                else
                {
                    switch (Kathy.direction)
                    {
                        case 0:
                            Kathy.texture = Content.Load<Texture2D>("WalkR2");
                            break;
                        case 1:
                            Kathy.texture = Content.Load<Texture2D>("WN2");
                            break;
                        case 2:
                            Kathy.texture = Content.Load<Texture2D>("WalkL2");
                            break;
                        case 3:
                            Kathy.texture = Content.Load<Texture2D>("WS2");
                            break;

                    }
                }

            }
            //chef updating block
            if (chef[0] != null)
            chef[0].Update(gameTime, Kathy.position, rng);
            if (chef[1] != null)
            chef[1].Update(gameTime, Kathy.position, rng);
            if (chef[2] != null)
            chef[2].Update(gameTime, Kathy.position, rng);
            if (chef[3] != null)
            chef[3].Update(gameTime, Kathy.position, rng);
            if (projectile[0] == null && chef[0] != null && gameTime.TotalGameTime.TotalMilliseconds > (endleveltime + 500))
                projectile[0] = chef[0].throwing(Kathy.position, Content, projectile[0], rng, level);
            if (projectile[1] == null && chef[1] != null && gameTime.TotalGameTime.TotalMilliseconds > (endleveltime +500))
                projectile[1] = chef[1].throwing(Kathy.position, Content, projectile[1], rng, level);
            if (projectile[2] == null && chef[2] != null && gameTime.TotalGameTime.TotalMilliseconds > (endleveltime + 500))
                projectile[2] = chef[2].throwing(Kathy.position, Content, projectile[2], rng, level);
            if (projectile[3] == null && chef[3] != null && gameTime.TotalGameTime.TotalMilliseconds > (endleveltime + 500))
                projectile[3] = chef[3].throwing(Kathy.position, Content, projectile[3], rng, level);

            //projectile update block
            if (projectile[0] != null)
            {
                projectile[0].Update();
                if (projectile[0].outofBound(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight))
                {
                    projectile[0] = null;
                }
            }
            if (projectile[1] != null)
            {
                projectile[1].Update();
                if (projectile[1].outofBound(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight))
                {
                    projectile[1] = null;
                }
            }
            if (projectile[2] != null)
            {
                projectile[2].Update();
                if (projectile[2].outofBound(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight))
                {
                    projectile[2] = null;
                }

            }
            if (projectile[3] != null)
            {
                projectile[3].Update();
                if (projectile[3].outofBound(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight))
                {
                    projectile[3] = null;
                }

            }
            if (Kathy.collision(icecream) && icecream.meltingstage != 4)
            {
                level += 1;
                endleveltime = gameTime.TotalGameTime.TotalMilliseconds;
                strength = Kathy.Strength;
                LoadContent();
            }
            for (int i = 0; i < respawntimer.Length; i++)
            {
                if (respawntimer[i] <= gameTime.TotalGameTime.Seconds-5)
                {
                    respawntimer[i] = 0;
                }
            }
            for (int i = 0; i < chef.Length; i++)
            {
                if (Kathy.collision(chef[i]))
                {
                    pits[chef[i].pit].Occupied=false;
                    chef[i]= null;
                    respawntimer[i] = gameTime.TotalGameTime.Seconds;
                    Kathy.Strength -= 20;
                }
            }
            foreach (Pit p1t in pits)
            {
                if (Kathy.collision(p1t))
                {
                    Kathy.Strength -= 50;
                    Kathy.position.Y += 60;
                    Kathy.prevposition = Kathy.position;
                    Kathy.stunned = 10;
                }
            }
            for (int i = 0; i < chef.Length; i++)
            {
                if (chef[i] == null&&respawntimer[i]==0)
                {
                    chef[i] = new Chef(Content.Load<Texture2D>("ChefS"), pits, rng); 
                }
            }
            for (int i=0; i<projectile.Length;i++)
            {
                if (Kathy.collision(projectile[i]))
                {
                    switch (projectile[i].foodtype)
                    {
                        case 0:
                            Kathy.Strength -= 5;
                            projectile[i] = null;
                            break;
                        case 1:
                            Kathy.Strength -= 7;
                            projectile[i] = null;
                            break;
                        case 2:
                            Kathy.Strength -= 10;
                            projectile[i] = null;
                            break;
                    }
                }
            }
            if (Kathy.collision(pizza))
            {
                pizza.eaten = true;
                Kathy.Strength += 10;
            }
            if (Kathy.collision(Banana))
            {
                Banana.eaten = true;
                Kathy.Strength += 7;
            }
            if (Kathy.collision(Orange))
            {
                Orange.eaten = true;
                Kathy.Strength += 5;
            }

            base.Update(gameTime);
        }
        public void drawsomething(Object something)
        {
            if (something != null)
            {
                something.Draw(spriteBatch);
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            
            spriteBatch.Begin();
            DrawHud();
            Kathy.Draw(spriteBatch);
            drawsomething(pizza);
            drawsomething(Banana);
            drawsomething(Orange);
            icecream.Draw(spriteBatch);
            for (int i = 0; i < pits.Length; i++)
            {
                pits[i].Draw(spriteBatch);
            }
            drawsomething(chef[0]);
            drawsomething(chef[1]);
            drawsomething(chef[2]);
            drawsomething(chef[3]);
            if (projectile[0] != null)
                projectile[0].Draw(spriteBatch);
            if (projectile[1] != null)
                projectile[1].Draw(spriteBatch);
            if (projectile[2] != null)
                projectile[2].Draw(spriteBatch);
            if (projectile[3] != null)
                projectile[3].Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
