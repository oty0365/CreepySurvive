using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapShadow : Shadow,IShadow
{
    public void SetRenderer(GameObject go)
    {
        var tm = gameObject.GetComponent<Tilemap>();
        var tmr = gameObject.GetComponent<TilemapRenderer>();
        var gord = go.GetComponent<ShadowRenderer>();
        var gotm = go.GetComponent<Tilemap>();
        BoundsInt bounds = gotm.cellBounds;
        TileBase[] tiles = gotm.GetTilesBlock(bounds);
        tm.ClearAllTiles();
        tm.SetTilesBlock(bounds, tiles);
        tm.color = gord.shadowColor;
        tmr.sortingOrder = gord.order;
        _renderLength = gord.rendForce;
        gameObject.transform.SetParent(parent.transform.parent, false);
    }
    
}
