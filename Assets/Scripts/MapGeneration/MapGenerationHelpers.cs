using UnityEngine;
using System.Collections;

public static class MapGenerationHelpers
{
    public static MapGenerator.Direction Opposite(MapGenerator.Direction dir)
    {
        switch (dir)
        {
            case MapGenerator.Direction.Up:
                return MapGenerator.Direction.Down;
            case MapGenerator.Direction.Down:
                return MapGenerator.Direction.Up;
            case MapGenerator.Direction.Left:
                return MapGenerator.Direction.Right;
            case MapGenerator.Direction.Right:
                return MapGenerator.Direction.Left;
            default:
                Debug.Assert(false);
                break;
        }

        // if we get the same direction we've reached an error
        return dir;
    }
}
