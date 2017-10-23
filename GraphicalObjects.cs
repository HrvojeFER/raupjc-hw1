using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{

    /// <summary >
    /// Game background representation
    /// </summary >
    public class Background : Sprite
    {
        public Background(int width, int height) : base(width, height)
        {
        }
    }

    public class Wall : IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Wall(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
    
    /// <summary >
    /// Game ball object representation
    /// </summary >
    public class Ball : Sprite
    {
        /// <summary >
        /// Defines current ball speed in time .
        /// </summary >
        public float Speed { get; set; }
        public float BumpSpeedIncreaseFactor { get; set; }

        public const float MaxSpeed = 1;
        public const float MinSpeed = 0.1f;
        public Vector2 Direction;

        /// <summary >
        /// Defines ball direction .
        /// Valid values ( -1 , -1) , (1 ,1) , (1 , -1) , ( -1 ,1).
        /// Using Vector2 to simplify game calculation . Potentially
        /// dangerous because vector 2 can swallow other values as well .
        /// OPTIONAL TODO : create your own , more suitable type
        /// </summary >

        public Ball(int size, float speed, float
            defaultBallBumpSpeedIncreaseFactor) : base(size, size)
        {
            Speed = speed;
            BumpSpeedIncreaseFactor = defaultBallBumpSpeedIncreaseFactor;
            // Initial direction
            Direction = new Vector2(1, 1);
        }
    }

    /// <summary >
    /// Represents player paddle .
    /// </summary >
    public class Paddle : Sprite
    {

        /// <summary >
        /// Current paddle speed in time
        /// </summary >
        public float Speed { get; set; }
        public Paddle(int width, int height, float initialSpeed) : base(width,
            height)
        {
            Speed = initialSpeed;
        }

        /// <inheritdoc />
        /// <summary>
        /// Overriding draw method . Masking paddle texture with black color .
        /// </summary>
        public override void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
                Width, Height), Color.GhostWhite);
        }
    }

}
