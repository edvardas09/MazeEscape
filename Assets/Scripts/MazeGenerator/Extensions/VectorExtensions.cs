using UnityEngine;

namespace MazeEscape.MazeGenerator
{
    public static class VectorExtensions
    {
        public static Vector3 CellToWorldSpace(this Vector2Int coords, float depth = 0)
        {
            return new Vector3(coords.x * 10, depth, coords.y * 10);
        }

        public static Vector2Int WorldToCellSpace(this Vector3 position)
        {
            return new Vector2Int(Mathf.RoundToInt(position.x / 10), Mathf.RoundToInt(position.z / 10));
        }

        public static float NormalizeAngle(this float angle)
        {
            return angle - Mathf.CeilToInt(angle / 360f) * 360f;
        }
    }
}