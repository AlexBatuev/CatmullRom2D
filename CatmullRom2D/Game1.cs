using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace CatmullRom2D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouse;
        MouseState prevMouse;
        List<Vector2> points = new List<Vector2>();
        List<Vector2> pointsAndInterpolatedPoints = new List<Vector2>();
        List<Rectangle> rectangles = new List<Rectangle>();
        int interpolationCount = 200;
        Texture2D pixel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            mouse = Mouse.GetState();
            prevMouse = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colorData = { Color.White };
            pixel.SetData<Color>(colorData);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouse = mouse;
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                points.Add(new Vector2(mouse.X, mouse.Y));

                pointsAndInterpolatedPoints.Clear();

                for (int i = 0; i < points.Count - 1; i++)
                {
                    int a = MathHelper.Max(0, i - 1);
                    int b = i;
                    int c = i+1;
                    int d = MathHelper.Min(points.Count-1, i+2);

                    Vector2 pointA = points[a];
                    Vector2 pointB = points[b];
                    Vector2 pointC = points[c];
                    Vector2 pointD = points[d];

                    pointsAndInterpolatedPoints.AddRange(GetInterpolatedPoints(pointA, pointB, pointC, pointD, interpolationCount));
                }

                pointsAndInterpolatedPoints.Add(points[points.Count-1]);

                rectangles.Clear();

                foreach (Vector2 v in pointsAndInterpolatedPoints)
                {
                    Rectangle r = new Rectangle(v.ToPoint(), new Point(2));
                    rectangles.Add(r);
                }
            }

            base.Update(gameTime);
        }

        private List<Vector2> GetInterpolatedPoints(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD, int interpolationCount)
        {
            List<Vector2> bla = new List<Vector2>();

            for (int i = 0; i < interpolationCount; i++)
            {
                float step = (float)i / interpolationCount;
                Vector2 interpolatedVec = Vector2.CatmullRom(pointA, pointB, pointC, pointD, step);
                bla.Add(interpolatedVec);
            }

            return bla;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach (Rectangle r in rectangles)
            {
                spriteBatch.Draw(pixel, r, Color.Purple);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
