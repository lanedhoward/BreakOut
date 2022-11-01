﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using Microsoft.Xna.Framework.Input;

namespace BreakoutBoring
{
    class PaddleController
    {
        InputHandler input;
        Ball ball; //may should delgate to parent
        public Vector2 Direction { get; private set; }
        public bool dashPressed;
        public bool autoPressed;

        public PaddleController(Game game, Ball ball)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            this.Direction = Vector2.Zero;
            this.dashPressed = false;
            this.ball = ball;   //need refernce to ball to be able to lanch ball could possibly use delegate here
        }

        public void HandleInput(GameTime gametime)
        {
            this.Direction = Vector2.Zero;  //Start with no direction on each new upafet
            this.dashPressed = false;
            this.autoPressed = false;

            //No need to sum input only uses left and right
            if (input.KeyboardState.IsKeyDown(Keys.Left))
            {
                this.Direction = new Vector2(-1, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                this.Direction = new Vector2(1, 0);
            }
            //TODO add mouse controll?

            if (input.WasKeyPressed(Keys.Space))
            {
                dashPressed = true;
            }
            if (input.WasKeyPressed(Keys.A))
            {
                autoPressed = true;
            }

            //Up launches ball
            if (input.KeyboardState.WasKeyPressed(Keys.Up))
            {
                if (ball.State == BallState.OnPaddleStart) //Only Launch Ball is it's on paddle
                    this.ball.LaunchBall(gametime);
            }
        }
    }
}

