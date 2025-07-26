using Project.Game.Interfaces;
using Project.Game.Pools;
using Project.Game.Shapes;
using UnityEngine;

namespace Project.Game.Factories
{
    public class ShapeFactory : IShapeFactory
    {
        private readonly ShapePool.Pool _shapePool;

        public ShapeFactory(ShapePool.Pool shapePool)
        {
            _shapePool = shapePool;
        }

        public ShapePool CreateShape(ShapeData shapeData, Vector3 position)
        {
            return _shapePool.Spawn(shapeData, position);
        }
    }
}