using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.VectorDraw;
using System.Diagnostics;
using VirtualCamera.Src;

namespace VirtualCamera
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        private World world;
        private Camera camera;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GraphicsManager.spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            SetCamera();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            var action = camera.TakeAction();

            if(action == Camera.Action.Reset)
            {
                SetCamera();
            }
            else if(action != Camera.Action.None)
            {
                camera.Observe(world, action);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(247, 244, 242));

            // TODO: Add your drawing code here

            GraphicsManager.spriteBatch.Begin();

            world.Draw();

            GraphicsManager.spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetCamera()
        {
            camera = new Camera(GraphicsManager.ScreenWidth, GraphicsManager.ScreenHeight, -1000f);
            world = new World();
            camera.Observe(world, Camera.Action.None);
        }
    }
}
