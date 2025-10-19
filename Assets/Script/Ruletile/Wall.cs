using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Wall Ruletile", menuName = "CookieMan/Tilemap/Wall Ruletile")]
public class Wall : RuleTile<Wall.Neighbor>
{
    public List<TileBase> walkableTiles = new();

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int PATH = 3;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.PATH:
                return walkableTiles.Contains(tile);
        }
        
        return base.RuleMatch(neighbor, tile);
    }
}
