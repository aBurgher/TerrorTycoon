using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathFinding : MonoBehaviour {
    //Used for storing pathfinding values for squares
    
    List<Square> open, closed;
    
    Vector2 gridDimensions, endPos;
    Square[,] grid;
    public List<Vector2> Path(Vector2 StartPos, Vector2 EndPos, Vector2 GridDimensions, bool[,] Grid)
    {
        open = new List<Square>();
        closed = new List<Square>();
        endPos = EndPos;
        gridDimensions = GridDimensions;
        grid = new Square[(int)gridDimensions.x, (int)gridDimensions.y];
        for (int i = 0; i < gridDimensions.x; i++)
        {
            for (int j = 0; j < gridDimensions.y; j++)
            {
                grid[i, j] = new Square(new Vector2(i, j), Grid[i, j]);
            }
        }
        Square startSquare = grid[(int)StartPos.x, (int)StartPos.y];
        startSquare.G = 0;
        open.Add(startSquare);
        search(startSquare);
        List<Vector2> path = new List<Vector2>();
        Square s = closed[closed.Count-1];
        while (s != startSquare)
        {
            path.Add(s.Pos);
            s = s.Parent;
        }
        path.Add(s.Pos);
        path.Reverse();
        return path;
    }

    public void search(Square current)
    {
        if (closed.Contains(grid[(int)endPos.x-1, (int)endPos.y-1]))
            return;
        
        
        for (int i = (int)current.Pos.x - 1; i <= (int)current.Pos.x +1; i++)
        {
            for (int j = (int)current.Pos.y - 1; j <= (int)current.Pos.y + 1; j++)
            {
                
                if (i < 0 || j < 0 || i > gridDimensions.x-1 || j > gridDimensions.y-1)
                    continue;
                if (closed.Contains(grid[i, j]) || !grid[i, j].canWalk)
                {
                    continue;
                }
                if (open.Contains(grid[i, j]))
                {
                    if (grid[i, j].G < current.G)
                    {
                        grid[i, j].Parent = current;
                        grid[i, j].G = Vector2.Distance(grid[i, j].Pos, current.Pos)*10 + current.G;

                    }
                    else
                        continue;
                }
                grid[i, j].H = Mathf.Abs(grid[i, j].Pos.x - endPos.x) * 10 + Mathf.Abs(grid[i, j].Pos.y - endPos.y)*10;
               
                grid[i, j].G = Vector2.Distance(grid[i, j].Pos, current.Pos) *10 + current.G;
                grid[i, j].F = grid[i, j].G + grid[i, j].H;
                grid[i, j].Parent = current;
                open.Add(grid[i, j]);
            }
        }
        open.Remove(current);
        closed.Add(current);
        current = getLowestF();
        search(current);
        
    }
    Square getLowestF()
    {
        float f = 10000;
        Square sq = null;
        foreach (Square s in open)
        {
            if (s.F < f)
            {
                f = s.F;
                sq = s;
            }
        }
        return sq;
    }
    public class Square
    {
        public int open { get; set; }
        public bool canWalk;
        public float G {get; set;}
        public float H { get; set; }
        public float F { get; set; }
        public Vector2 Pos;
        public Square Parent { get; set; }
        public Square(Vector2 pos, bool canwalk)
        {
            canWalk = canwalk;
            Pos = pos;
            open = -1;
        }
    }
    
}
