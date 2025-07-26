using System.Collections.Generic;
using Project.Game.Shapes;

namespace Project.Game.Interfaces
{
    public interface ISortingSlotsProvider
    {
        List<ShapeData> GetAvailableShapeData();
    }
}