using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GenericListEnumerator;

namespace Pong
{

    public class GameConstants
    {
        public const float PaddleDefaulSpeed = 0.9f;
        public const int PaddleDefaultWidth = 200;
        public const int PaddleDefaulHeight = 20;
        public const float DefaultInitialBallSpeed = 0.4f;
        public const float DefaultBallBumpSpeedIncreaseFactor = 1.05f;
        public const int DefaultBallSize = 40;
        public const int WallDefaultSize = 1;
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle _screenBounds;
        
        public Game()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };
            Content.RootDirectory = "Content";
        }

        /// <summary >
        /// Bottom paddle object
        /// </summary >
        public Paddle PaddleBottom { get; private set; }

        /// <summary >
        /// Top paddle object
        /// </summary >
        public Paddle PaddleTop { get; private set; }

        /// <summary >
        /// Ball object
        /// </summary >
        public Ball Ball { get; private set; }

        /// <summary >
        /// Background image
        /// </summary >
        public Background Background { get; private set; }

        /// <summary >
        /// Sound when ball hits an obstacle .
        /// SoundEffect is a type defined in Monogame framework
        /// </summary >
        public SoundEffect HitSound { get; private set; }

        /// <summary >
        /// Background music . Song is a type defined in Monogame framework
        /// </summary >
        public Song Music { get; private set; }

        /// <summary >
        /// Generic list that holds Sprites that should be drawn on screen
        /// </summary >
        private readonly IGenericList<Sprite> _spritesForDrawList = new GenericList<Sprite>();

        public GenericList<Wall> Walls { get; set; }
        public GenericList<Wall> Goals { get; set; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Screen bounds details . Use this information to set up game objects
            // positions.
            _screenBounds = GraphicsDevice.Viewport.Bounds;

            PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth,
                GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleBottom.X = _screenBounds.Width / 2f - PaddleBottom.Width / 2f;
            PaddleBottom.Y = _screenBounds.Bottom - PaddleBottom.Height;

            PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth,
                GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleTop.X = _screenBounds.Width / 2f - PaddleTop.Width / 2f;
            PaddleTop.Y = _screenBounds.Top;
            
            Ball = new Ball(GameConstants.DefaultBallSize, GameConstants.DefaultInitialBallSpeed,
                GameConstants.DefaultBallBumpSpeedIncreaseFactor)
            {
                X = _screenBounds.Width / 2f - GameConstants.DefaultBallSize / 2f,
                Y = _screenBounds.Bottom / 2f - GameConstants.DefaultBallSize / 2f
            };
            
            Background = new Background(_screenBounds.Width, _screenBounds.Height);

            // Add our game objects to the sprites that should be drawn collection .
            _spritesForDrawList.Add(Background);
            _spritesForDrawList.Add(PaddleBottom);
            _spritesForDrawList.Add(PaddleTop);
            _spritesForDrawList.Add(Ball);

            Walls = new GenericList<Wall>()
            {
                new Wall (_screenBounds.Left , _screenBounds.Top,
                    GameConstants.WallDefaultSize , _screenBounds.Height ),
                new Wall ( _screenBounds.Right - GameConstants.WallDefaultSize , _screenBounds.Top, 
                    GameConstants.WallDefaultSize, _screenBounds.Height),
            };
            Goals = new GenericList<Wall>()
            {
                new Wall (_screenBounds.Left, _screenBounds.Top, 
                    _screenBounds.Width, GameConstants.WallDefaultSize ),
                new Wall ( _screenBounds.Left , _screenBounds.Bottom - GameConstants.WallDefaultSize,
                    _screenBounds.Width, GameConstants.WallDefaultSize ),
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Set textures
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            PaddleBottom.Texture = paddleTexture;
            PaddleTop.Texture = paddleTexture;
            Ball.Texture = Content.Load<Texture2D>("ball");
            Background.Texture = Content.Load<Texture2D>("background");

            // Load sounds
            // Start background music
            HitSound = Content.Load<SoundEffect>("hit");
            //Music = Content.Load<Song>("music");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Music);
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

            PaddleBottom.X = MathHelper.Clamp(PaddleBottom.X, _screenBounds.Left, 
                _screenBounds.Right - PaddleBottom.Width);
            PaddleTop.X = MathHelper.Clamp(PaddleTop.X, _screenBounds.Left, 
                _screenBounds.Right - PaddleTop.Width);

            var touchState = Keyboard.GetState();
            if (touchState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.X = PaddleBottom.X - (float)(PaddleBottom.Speed *
                    gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.Right))
            {
                PaddleBottom.X = PaddleBottom.X + (float)(PaddleBottom.Speed *
                    gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.A))
            {
                PaddleTop.X = PaddleTop.X - (float)(PaddleTop.Speed *
                    gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.D))
            {
                PaddleTop.X = PaddleTop.X + (float)(PaddleTop.Speed *
                    gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            Ball.Speed = MathHelper.Clamp(Ball.Speed, Ball.MinSpeed, Ball.MaxSpeed);

            var ballPositionChange = Ball.Direction *
                (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
            Ball.X += ballPositionChange.X;
            Ball.Y += ballPositionChange.Y;

            if (CollisionDetector.Overlaps(Ball, Walls.GetElement(0)) ||
                CollisionDetector.Overlaps(Ball, Walls.GetElement(1)))
            {
                Ball.Direction.X = -Ball.Direction.X;
                Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
            }

            else if (CollisionDetector.Overlaps(Ball, Goals.GetElement(0)) ||
                     CollisionDetector.Overlaps(Ball, Goals.GetElement(1)))
            {
                Ball.X = _screenBounds.Width / 2f - Ball.Width / 2f;
                Ball.Y = _screenBounds.Bottom / 2f - Ball.Height / 2f;
                Ball.Speed = GameConstants.DefaultInitialBallSpeed;
                //HitSound.Play();
            }

            else if (CollisionDetector.Overlaps(Ball, PaddleTop) ||
                     CollisionDetector.Overlaps(Ball, PaddleBottom))
            {
                Ball.Direction.Y = -Ball.Direction.Y;
                Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Start drawing .
            _spriteBatch.Begin();
            for (var i = 0; i < _spritesForDrawList.Count; i++)
            {
                _spritesForDrawList.GetElement(i).DrawSpriteOnScreen(_spriteBatch);
            }

            // End drawing .
            // Send all gathered details to the graphic card in one batch .
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
