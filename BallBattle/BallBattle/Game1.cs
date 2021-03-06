using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BallBattle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        StartComponent startComponent;//游戏开始
        MyGameComponent gameComponent;//游戏
        GameComponent endComponent;//游戏结束

        SoundEffectInstance bgm;

        public static int gameState = 0;//记录游戏状态,0是开始界面,1是游戏进行时候,2是游戏结束画面(失败) 3是通关画面

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;    // 设置分辨率
            graphics.PreferredBackBufferHeight = 600;
            //  graphics.IsFullScreen = true;
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

            startComponent = new StartComponent(this);
            gameComponent = new MyGameComponent(this);
            endComponent = new EndComponent(this);
            Components.Add(startComponent);

            Resourse.init(Content);     //初始化全局纹理类 ,应该放在LoadContent里,但测试的时候发现会空指针,应该是先执行了  Components的LoadContent,才会这样

            this.IsMouseVisible = true;
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


            ScoreBoard.init(Resourse.getInstance().scoreFont, Resourse.getInstance().bigFont);
            bgm = Resourse.getInstance().bgm.CreateInstance();
            
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
           
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            KeyboardState keystate = Keyboard.GetState();

            switch (gameState)
            {
                case 0:
                    if(StartComponent.start){
                        playGame();
                    }
                    break;
                case 1:
                    break;
                case 2:
                case 3:
                         bgm.Stop();
                        Components.RemoveAt(0);
                        Components.Add(endComponent);
                    if(keystate.IsKeyDown(Keys.Enter)){
                        gameState = 0;
                        playGame();
                    }
                        
                    break;
            }





            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        public void playGame()
        {
            gameState = 1;//进入游戏状态
            Components.RemoveAt(0);
            Components.Add(gameComponent);
            Chapters.getInstance().init();
            ScoreBoard.getInstance().init();
            gameComponent.clear();
            PlayerBall.init(new Vector2(100, 100), 6, Resourse.getInstance().playerBallTexture, 50);
            bgm.Play();

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //积分板
            //  spriteBatch.Begin();
            //ScoreBoard.getInstance().onDraw(spriteBatch);
            // spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
