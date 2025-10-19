using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Path Ruletile", menuName = "CookieMan/Tilemap/Path Ruletile")]
public class Path : RuleTile<Path.Neighbor>
{
    public List<TileBase> wallTiles = new();

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int WALL = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.WALL:
                return wallTiles.Contains(tile);
        }
        
        return base.RuleMatch(neighbor, tile);
    }
}
