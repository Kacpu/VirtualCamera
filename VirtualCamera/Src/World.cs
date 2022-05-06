using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Src
{
    public class World
    {
        private readonly List<WorldObject> worldObjects;

        public World()
        {
            Cuboid cuboid1 = new Cuboid(50f, -100f);
            Cuboid cuboid2 = new Cuboid(50f, -200f);
            Cuboid cuboid3 = new Cuboid(-90f, -100f);
            Cuboid cuboid4 = new Cuboid(-90f, -200f);

            worldObjects = new List<WorldObject>()
            {
                cuboid1, cuboid2, cuboid3, cuboid4
            };
        }

        public void Observe(Matrix? worldTransformationMatrix, Matrix perspectiveTransformationMatrix)
        {
            foreach (var obj in worldObjects)
            {
                obj.Observe(worldTransformationMatrix, perspectiveTransformationMatrix);
            }
        }

        public void Draw()
        {
            foreach(var obj in worldObjects)
            {
                obj.Draw();
            }
        }
    }
}
