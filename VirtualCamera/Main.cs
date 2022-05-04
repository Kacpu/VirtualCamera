using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using VirtualCamera.Src;

namespace VirtualCamera
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        private Src.Viewport _viewport;
        private PerspectiveProjection _perspectiveProjection;
        private Cuboid cuboid1;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = GraphicsManager.ScreenWidth;
            _graphics.PreferredBackBufferHeight = GraphicsManager.ScreenHeight;

            _graphics.ApplyChanges();

            _viewport = new Src.Viewport(1080f, 720f, -240f);
            _perspectiveProjection = new PerspectiveProjection(_viewport);
            
            cuboid1 = new Cuboid(30f, -60f);
            cuboid1.Project(_perspectiveProjection.ptm);
            cuboid1.TransformToPixels();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GraphicsManager.spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            GraphicsManager.spriteBatch.Begin();

            cuboid1.Draw();

            GraphicsManager.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
