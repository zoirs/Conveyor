using UnityEngine;
using Zenject;

public class MapService : IInitializable{
    
    int[,] map = new int[,] {
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, 0, 0, 5, 5, 5, 1, 1, 1, 1, 1, _, _, _, _, _},
        {_, _, _, _, _, 0, 0, 0, 5, 5, 5, 1, 1, 1, 1, _, _, _, _, _},
        {_, _, _, _, _, 0, 0, 0, 0, 5, 5, 1, 1, 1, 1, _, _, _, _, _},
        {_, _, _, _, _, 0, 0, 0, 0, 0, 0, 1, 6, 6, 6, _, _, _, _, _},
        {_, _, _, _, _, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, _, _, _, _, _},
        {_, _, _, _, _, 2, 2, 0, 0, 0, 6, 6, 4, 4, 6, _, _, _, _, _},
        {_, _, _, _, _, 2, 2, 2, 3, 3, 3, 4, 4, 4, 4, _, _, _, _, _},
        {_, _, _, _, _, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, _, _, _, _, _},
        {_, _, _, _, _, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, _, _, _, _, _},
        {_, _, _, _, _, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
        {_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _},
    };

    private Vector3 originPosition = new Vector3(0, 0);
    private int cellSize = 1;
    public static int _ = 11;
    public int currentLevel = 1;

    public int[,] Map => map;
    
    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public int GetAreaValue(Vector3 worldPosition) {
        Vector2Int point = GetXY(worldPosition);
        return map[point.x, point.y];
    }

    public Vector2Int GetXY(Vector3 worldPosition) {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        return new Vector2Int(x, y);
    }

    public void Initialize() {
        
    }
}