using Project.Game.Pools;
using Project.Game.Shapes;
using UnityEngine;

namespace Project.Game.Interfaces
{
    public interface IShapeFactory
    {
        ShapePool CreateShape(ShapeData shapeData, Vector3 position);
    }
}