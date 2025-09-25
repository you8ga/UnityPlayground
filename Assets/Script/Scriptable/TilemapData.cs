using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileMapStats", menuName = "Stats/TileMap Stats")]
public class TilemapData : ScriptableObject
{
    public Vector3Int mapBoundSize;
}
