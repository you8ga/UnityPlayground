using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    #region private fields
    private TilemapStats m_tilemapStats;
    private BoundsInt m_mapArea;
    private TileBase[] m_allTiles;
    private Tilemap m_myTilemap
    {
        get
        {
            _myTilemap ??= GetComponent<Tilemap>();
            return _myTilemap;
        }
    }
    private Tilemap _myTilemap;

    private Grid m_myGrid 
    {
        get
        {
            _myGrid ??= GetComponentInParent<Grid>();
            return _myGrid;
        }
    }
    private Grid _myGrid;
    #endregion
    public bool isReady => m_tilemapStats != null;
    public Vector3Int cellCenter => m_myGrid.WorldToCell(transform.position);

    [Space(10),Header("Editor Only:")]
    public Tile testTile;
    public TilemapStats inspectorTilemapStats;
    public Color gizmosColor;
    // Start is called before the first frame update
    async void Start()
    {
        m_tilemapStats = await AddressableManager.Instance.LoadAssetAsync<TilemapStats>(AddressableKey.TilemapStats);
        Vector3Int cellCenter = m_myGrid.WorldToCell(transform.position);
        Vector3Int cellStartPos = cellCenter - (m_tilemapStats.mapBoundSize / 2);
        m_myGrid.cellSize = testTile.sprite.bounds.size;
        m_mapArea = new BoundsInt(cellStartPos, m_tilemapStats.mapBoundSize);
        m_allTiles = new TileBase[m_mapArea.size.x * m_mapArea.size.y];

        for (int i = 0; i < m_allTiles.Length; i++)
        {
            m_allTiles[i] = testTile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReady)
            return;
        m_myTilemap.SetTilesBlock(m_mapArea, m_allTiles);
    }

#if(UNITY_EDITOR)
    private void OnDrawGizmos()
    {
        if (inspectorTilemapStats == null)
            return;

        Gizmos.color = gizmosColor;

        m_mapArea = new BoundsInt(cellCenter, inspectorTilemapStats.mapBoundSize);

        Gizmos.DrawCube(cellCenter, new Vector3(testTile.sprite.bounds.size.x, 0.1f, testTile.sprite.bounds.size.y));
    }
#endif
}
