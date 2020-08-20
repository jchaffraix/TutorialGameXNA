using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace TutorialShooterGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Represents the player
        Player player;

        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        //Mouse states used to track Mouse button press
        MouseState currentMouseState;
        MouseState previousMouseState;

        // A movement speed for the player
        float playerMoveSpeed;

        // Animated Spite (other part of the tutorial).
        AnimatedSprite animatedSprite;
        private BetterParallaxBackground background;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            player = new Player();

            // Set a constant player move speed

            playerMoveSpeed = 8.0f;

            //Enable the FreeDrag gesture.
            // No idea why? TODO: Figure it out.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

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

            // Load the player resources

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition);

            // Better to do this in init or here?
            animatedSprite = new AnimatedSprite(Content.Load<Texture2D>("Graphics\\SmileyWalk"), 4, 4);
            background = new BetterParallaxBackground(Content.Load<Texture2D>("Graphics\\stars"));
            background.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, .2f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            // Save the previous state of the keyboard and game pad so we can determine single key/button presses

            previousGamePadState = currentGamePadState;

            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it

            currentKeyboardState = Keyboard.GetState();

            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            animatedSprite.Update(gameTime);

            //Update the player

            UpdatePlayer(gameTime);

            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)

        {
            // Windows 8 Touch Gestures for MonoGame

            while (TouchPanel.IsGestureAvailable)

            {

                GestureSample gesture = TouchPanel.ReadGesture();



                if (gesture.GestureType == GestureType.FreeDrag)

                {

                    player.Position += gesture.Delta;



                }

            }
            // Get Thumbstick Controls

            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;

            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;



            // Use the Keyboard / Dpad

            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed)

            {

                //player.Position.X -= playerMoveSpeed;
                player.angle -= 5 * 3.14156f / 180;

            }



            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed)

            {

                //player.Position.X += playerMoveSpeed;
                player.angle += 5 * 3.14156f / 180;

            }



            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed)

            {
                float updateX = playerMoveSpeed * (float)Math.Sin(player.angle);
                float updateY = -playerMoveSpeed * (float)Math.Cos(player.angle);
                player.Position.Y += updateY;
                player.Position.X += updateX;
                background.Update(updateX, updateY);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed)

            {
                float updateX = -playerMoveSpeed * (float)Math.Sin(player.angle);
                float updateY = playerMoveSpeed * (float)Math.Cos(player.angle);
                player.Position.Y += updateY;
                player.Position.X += updateX;
                background.Update(updateX, updateY);
            }

            // Mouse.
            //Get Mouse State then Capture the Button type and Respond Button Press
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            if (currentMouseState.LeftButton == ButtonState.Pressed)

            {

                Vector2 posDelta = mousePosition - player.Position;

                posDelta.Normalize();

                posDelta = posDelta * playerMoveSpeed;

                player.Position = player.Position + posDelta;

            }

            // Make sure that the player does not go out of bounds

            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);

            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
        }


    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            spriteBatch.Begin();

            background.Draw(spriteBatch);

            // Draw the Player
            player.Draw(spriteBatch);

            animatedSprite.Draw(spriteBatch, new Vector2(100, 100));
            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
