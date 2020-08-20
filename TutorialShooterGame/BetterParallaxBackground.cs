using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialShooterGame
{
    class BetterParallaxBackground
    {
        // This is a drop-in replacement for the ParallaxBackground
        // I found on a tutorial but with way better performance.
        Vector2 position;

        private Texture2D texture;
        // TODO: By design, this class doesn't handle Y.
        private float speed;

        private int bgHeight;
        private int bgWidth;


        public BetterParallaxBackground(Texture2D texture)
        {
            this.texture = texture;
            this.position = new Vector2();

        }
        public void Initialize(int screenWidth, int screenHeight, float speed)
        {

            bgHeight = screenHeight;

            bgWidth = screenWidth;

            // Set the speed of the background
            this.speed = speed;

            // TODO: Limits + original offset.
        }

        public void Update(float updateX, float _updateY)
        {
            // The background moves in reverse to the player:
            position.X += speed * updateX;

            // Keep the offset within reasonable bounds.
            if (position.X < 0)
            {
                position.X += bgWidth;
            } else if (position.X > bgWidth)
            {
                position.X -= bgWidth;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw from the offset.
            Rectangle sourceRect = new Rectangle((int)position.X, 0, (int)(texture.Width - position.X), (int)(texture.Height));
            Rectangle destinationRect = new Rectangle(0, 0, sourceRect.Width, sourceRect.Height);
            spriteBatch.Draw(texture, destinationRect, sourceRect, Color.White);

            // Draw the remaining part.
            Rectangle secondSourceRect = new Rectangle(0, 0, (int)position.X, bgHeight);
            Rectangle secondDestinationRect = new Rectangle((int)(bgWidth - position.X), 0, (int)position.X, bgHeight);
            spriteBatch.Draw(texture, secondDestinationRect, secondSourceRect, Color.White);
        }
    }
}
