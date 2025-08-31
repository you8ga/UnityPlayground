using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileMapStats", menuName = "Stats/TileMap Stats")]
public class TilemapStats : ScriptableObject
{
    public Vector3Int mapBoundSize;
}
