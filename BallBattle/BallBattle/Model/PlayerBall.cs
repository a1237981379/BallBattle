﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BallBattle.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace BallBattle
{
    class PlayerBall:
        BaseBall
    {
        private static PlayerBall bb;

        public static PlayerBall getInstance() {
            return bb;
        }

        public static void init(Vector2 postion, int speed, Texture2D texture, Point frameSize, Point sheetSize, int val)
        {
            bb = new PlayerBall(postion, speed,texture, frameSize, sheetSize, val);
        
        }
        
        
        private PlayerBall(Vector2 postion, int speed, Texture2D texture, Point frameSize, Point sheetSize,int val) 
            :base(postion,speed,Vector2.Zero,texture,frameSize,sheetSize,val)
        {
            
        
        
        }

       

        public override Vector2 getDirection()
        {
            Vector2 v = new Vector2(0,0);
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Up))
            {
                v.Y = -1;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                v.Y = 1;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                v.X = 1;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                v.X = -1;
            }
            return v;
        
        }



        

    }
}
