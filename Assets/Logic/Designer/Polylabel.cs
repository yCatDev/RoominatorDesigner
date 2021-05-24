// modified for usage in Unity by grapefrukt, (removed support for polygons with holes as I do not need it)
// based C# port of Polylabel found here: https://github.com/mapbox/polylabel/issues/26#issue-211932869
// which in turn, is based on Polylabel from https://github.com/mapbox/polylabel

using System.Collections.Generic;
using UnityEngine;

public static class PolyLabel {

    public static Vector2 GetPolyLabel(Vector2[] polygon, float precision = .1f) {
        // calculates the in-center in fixed time for any triangles, it's faster and more accurate 
        if (polygon.Length == 3) return GetInCenter(polygon);
        
        //Find the bounding box of the outer ring
        float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;

        foreach (var point in polygon) {
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }

        var width = maxX - minX;
        var height = maxY - minY;
        var cellSize = Mathf.Max(width, height);
        var halfCell = cellSize / 2;

        //A priority queue of cells in order of their "potential" (max distance to polygon)
        var cellQueue = new CellPriorityQueue();

        if (FloatEquals(cellSize, 0)) return new Vector2(minX, minY);

        var firstCell = new Cell((minX + maxX) / 2, (minY + maxY) / 2, halfCell, polygon);
        cellQueue.Enqueue(firstCell);
        var bestCell = firstCell;

        while (cellQueue.HasItems) {
            //Pick the most promising cell from the queue
            var cell = cellQueue.Dequeue();
           
            //Update the best cell if we found a better one
            if (cell.D > bestCell.D) {
                bestCell = cell;
            }

            //Do not drill down further if there's no chance of a better solution
            if (cell.Max - bestCell.D <= precision)
                continue;

            //Split the cell into four cells
            halfCell = cell.H / 2;
            cellQueue.Enqueue(new Cell(cell.X - halfCell, cell.Y - halfCell, halfCell, polygon));
            cellQueue.Enqueue(new Cell(cell.X + halfCell, cell.Y - halfCell, halfCell, polygon));
            cellQueue.Enqueue(new Cell(cell.X - halfCell, cell.Y + halfCell, halfCell, polygon));
            cellQueue.Enqueue(new Cell(cell.X + halfCell, cell.Y + halfCell, halfCell, polygon));
        }

        // lastIterationCount = iterationCount;
        return new Vector2(bestCell.X, bestCell.Y);
    }

    static Vector2 GetInCenter(Vector2[] polygon) {
        // Formula to calculate in-center
        var a = Vector2.Distance(polygon[2], polygon[1]);
        var b = Vector2.Distance(polygon[0], polygon[2]);
        var c = Vector2.Distance(polygon[2], polygon[0]);
        var x = (a * polygon[0].x + b * polygon[1].x + c * polygon[2].x) / (a + b + c); 
        var y = (a * polygon[0].y + b * polygon[1].y + c * polygon[2].y) / (a + b + c);
        return new Vector2(x, y);
    }

    //Signed distance from point to polygon outline (negative if point is outside)
    public static float PointToPolygonDist(float x, float y, Vector2[] polygon) {
        var inside = false;
        var minDistSq = float.MaxValue;

        for (int i = 0, len = polygon.Length, j = len - 1; i < len; j = i++) {
            var a = polygon[i];
            var b = polygon[j];

            if (a.y > y != b.y > y && x < (b.x - a.x) * (y - a.y) / (b.y - a.y) + a.x)
                inside = !inside;

            minDistSq = Mathf.Min(minDistSq, GetSegDistSq(x, y, a, b));
        }

        return (inside ? 1 : -1) * Mathf.Sqrt(minDistSq);
    }

    //Get squared distance from a point to a segment
    static float GetSegDistSq(float px, float py, Vector2 a, Vector2 b) {
        var x = a.x;
        var y = a.y;
        var dx = b.x - x;
        var dy = b.y - y;

        if (!FloatEquals(dx, 0) || !FloatEquals(dy, 0)) {
            var t = ((px - x) * dx + (py - y) * dy) / (dx * dx + dy * dy);
            if (t > 1) {
                x = b.x;
                y = b.y;
            }
            else if (t > 0) {
                x += dx * t;
                y += dy * t;
            }
        }

        dx = px - x;
        dy = py - y;
        return dx * dx + dy * dy;
    }

    static bool FloatEquals(float a, float b) {
        return Mathf.Approximately(a, b);
    }
}

internal class Cell {
    public readonly float X;
    public readonly float Y;
    public readonly float H;
    public readonly float D;
    public readonly float Max;
    const float Sqrt2 = 1.41421356237f;

    public Cell(float x, float y, float h, Vector2[] polygon) {
        X = x;
        Y = y;
        H = h;
        D = PolyLabel.PointToPolygonDist(X, Y, polygon);
        Max = D + H * Sqrt2;
    }
}

internal class CellPriorityQueue {
    readonly SortedList<float, Cell> queue;

    public CellPriorityQueue() {
        queue = new SortedList<float, Cell>();
    }

    public void Enqueue(Cell cell) {
        if (!queue.ContainsKey(-cell.Max))
            queue.Add(-cell.Max, cell);
    }

    public bool HasItems => queue.Count > 0;

    public Cell Dequeue() {
        var c = queue.Values[0];
        queue.RemoveAt(0);
        return c;
    }
}