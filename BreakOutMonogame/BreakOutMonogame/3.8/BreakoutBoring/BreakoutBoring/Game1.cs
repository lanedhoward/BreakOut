﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;

namespace BreakoutBoring
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public enum GameState
    {
        Title,
        Playing,
        Summary
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //Services
        InputHandler input;
        GameConsole console;

        //Components
        BlockManager bm;
        Paddle paddle;
        Ball ball;

        ScoreManager score;

        public GameState state;
        SpriteFont font;

        Vector2 titleLoc;
        Vector2 summaryLoc;
        Vector2 buttonPromptLoc;

        public Game1()
            : base()
        {
            Window.Title = "LANE'S BREAKOUT <3";
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Services
            input = new InputHandler(this);
            console = new GameConsole(this);
            this.Components.Add(console);
#if RELEASE
            console.ToggleConsole(); //close the console
#endif
            this.Components.Add(input);

            score = new ScoreManager(this);
            this.Components.Add(score);

            //GameComponents
            ball = new Ball(this); //Ball first paddle and block manager depend on ball
            this.Components.Add(ball);
            paddle = new Paddle(this, ball);
            this.Components.Add(paddle);
            
            bm = new BlockManager(this, ball);
            this.Components.Add(bm);

            state = GameState.Title;
            titleLoc = new Vector2(300, 200);
            summaryLoc = new Vector2(300, 200);
            buttonPromptLoc = new Vector2(300, 300);
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

            DisableAllSprites();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if (input.WasKeyPressed(Keys.Space))
            {
                if (state == GameState.Title || state == GameState.Summary)
                {
                    StartGame();
                    state = GameState.Playing;
                }
            }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGoldenrod);

            spriteBatch.Begin();

            switch (state)
            {
                case GameState.Title:
                    spriteBatch.DrawString(font, "Welcome to Lane's Brick Breaker <3", titleLoc, Color.White);
                    spriteBatch.DrawString(font, "Press Space", buttonPromptLoc, Color.White);
                    break;
                case GameState.Playing:

                    break;
                case GameState.Summary:
                    spriteBatch.DrawString(font, "Game over <3 You scored " + ScoreManager.Score, summaryLoc, Color.White);
                    spriteBatch.DrawString(font, "Press Space", buttonPromptLoc, Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DisableAllSprites()
        {
            DisableDrawableGameComponent(ball);
            DisableDrawableGameComponent(paddle);
            DisableDrawableGameComponent(bm);
        }

        public void EnableAllSprites()
        {
            EnableDrawableGameComponent(ball);
            EnableDrawableGameComponent(paddle);
            EnableDrawableGameComponent(bm);
        }

        public void DisableDrawableGameComponent(DrawableGameComponent dgc)
        {
            dgc.Enabled = false;
            dgc.Visible = false;
        }
        public void EnableDrawableGameComponent(DrawableGameComponent dgc)
        {
            dgc.Enabled = true;
            dgc.Visible = true;
        }

        public void StartGame()
        {
            EnableAllSprites();
            ScoreManager.SetupNewGame();
            ball.resetBall();
            bm.ResetBlocks();
            bm.LoadLevel();
        }

        public void EndGame()
        {
            DisableAllSprites();
            state = GameState.Summary;
        }
    }
}
