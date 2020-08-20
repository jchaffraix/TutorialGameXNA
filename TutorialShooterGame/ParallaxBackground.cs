using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialShooterGame
{
    class ParallaxBackground
    {
        private Texture2D texture;
        // TODO: By design, this class doesn't handle Y.
        // It seems to be designed for a platformer though it wasn't the game I looked at...
        private Vector2[] positions;
        private float speed;

        private int bgHeight;
        private int bgWidth;


        public ParallaxBackground(Texture2D texture)
        {
            this.texture = texture;

        }

        public void Initialize(int screenWidth, int screenHeight, float speed)
        {

            bgHeight = screenHeight;

            bgWidth = screenWidth;

            // Set the speed of the background

            this.speed = speed;

            // If we divide the screen with the texture width then we can determine the number of tiles need.

            // We add 1 to it so that we won't have a gap in the tiling

            positions = new Vector2[screenWidth / texture.Width + 1];

            // Set the initial positions of the parallaxing background

            for (int i = 0; i < positions.Length; i++)
            {

                // We need the tiles to be side by side to create a tiling effect

                positions[i] = new Vector2(i * texture.Width, 0);

            }
        }

        public void Update(float updateX, float _updateY)
        {
            // TODO: I am pretty sure the design is hurting us
            // here as the update is now O(N) where it could be O(1).
            // I **think** we could get away with keeping the offset into
            // the texture and do several draws based on our offset viewport.
            //
            // Update the positions of the background
            for (int i = 0; i < positions.Length; i++)

            {

                // Update the position of the screen by adding the speed

                positions[i].X += updateX * speed;

                // If the speed has the background moving to the left

                if (speed <= 0)

                {

                    // Check the texture is out of view then put that texture at the end of the screen

                    if (positions[i].X <= -texture.Width)

                    {

                        WrapTextureToLeft(i);

                    }

                }

                // If the speed has the background moving to the right

                else

                {

                    // Check if the texture is out of view then position it to the start of the screen

                    if (positions[i].X >= texture.Width * (positions.Length - 1))

                    {

                        WrapTextureToRight(i);

                    }

                }

            }

        }

        private void WrapTextureToLeft(int index)
        {
            // If the textures are scrolling to the left, when the tile wraps, it should be put at the
            // one pixel to the right of the tile before it.
            int prevTexture = index - 1;
            if (prevTexture < 0)
                prevTexture = positions.Length - 1;

            positions[index].X = positions[prevTexture].X + texture.Width;
        }

        private void WrapTextureToRight(int index)
        {
            // If the textures are scrolling to the right, when the tile wraps, it should be
            //placed to the left of the tile that comes after it.

            int nextTexture = index + 1;
            if (nextTexture == positions.Length)
                nextTexture = 0;

            positions[index].X = positions[nextTexture].X - texture.Width;
        }
        public void Draw(SpriteBatch spriteBatch)

        {
            for (int i = 0; i < positions.Length; i++)

            {

                Rectangle rectBg = new Rectangle((int)positions[i].X, (int)positions[i].Y, bgWidth, bgHeight);

                spriteBatch.Draw(texture, rectBg, Color.White);

            }

        }
    }
}
