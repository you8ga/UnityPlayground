using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static GlobalEventUtility;
using System;
using Random = UnityEngine.Random;

public class TilemapController : SingletonBase<TilemapController>
{
    #region private fields
    private TilemapData m_tilemapStats;
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
    private float m_xMapLength;
    private float m_zMapLength;
    #endregion
    public bool isReady => m_tilemapStats != null;
    public Vector3 maxMapArea => isReady ? m_myGrid.CellToWorld(m_mapArea.max) : Vector3.zero;
    public event Action OnMapReady;
    [Space(10),Header("Editor Only:")]
    public Tile testTile;
    public TilemapData inspectorTilemapStats;
    public Color gizmosColor;
    protected override void Awake()
    {
        base.Awake();
        SceneDataManager.Instance.SceneIsReady += () =>InitSceneMap(SceneDataManager.Instance.currentTilemapStats);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitSceneMap(TilemapData loadStats)
    {
        m_tilemapStats = loadStats;
        m_myGrid.cellSize = testTile.sprite.bounds.size;
        m_mapArea = new BoundsInt(-m_tilemapStats.mapBoundSize / 2, m_tilemapStats.mapBoundSize);
        m_allTiles = new TileBase[m_mapArea.size.x * m_mapArea.size.y];
        for (int i = 0; i < m_allTiles.Length; i++)
        {
            m_allTiles[i] = testTile;
        }
        m_xMapLength = m_myGrid.cellSize.x * m_tilemapStats.mapBoundSize.x;
        m_zMapLength = m_myGrid.cellSize.y * m_tilemapStats.mapBoundSize.y;
        m_myTilemap.SetTilesBlock(m_mapArea, m_allTiles);
        CreateEdgeBlock();
        PlayerController.Instance.transform.position = GetMapCenter() + Vector3.up * 1f;
        OnMapReady?.Invoke();
    }

    private void CreateEdgeBlock()
    {
        float colOffset = 0.5f;
        Vector3 createPos = transform.position;
        Vector3 scale = Vector3.one;

        Vector3 xOffset = Mathf.Floor((m_mapArea.xMin + m_mapArea.xMax))/2 * Vector3.right * m_myGrid.cellSize.x;
        Vector3 zOffset = Mathf.Floor((m_mapArea.yMin + m_mapArea.yMax)) / 2 * Vector3.forward * m_myGrid.cellSize.y;
        Vector3 rootPos = transform.position;

        for (int zDir = -1; zDir < 2; zDir+=2)
        {
            var dir = m_mapArea.yMin;
            if (zDir > 0)
                dir = m_mapArea.yMax;

            createPos = rootPos + (m_myGrid.cellSize.y * dir + colOffset * zDir) * Vector3.forward + xOffset;
            scale = new Vector3(m_xMapLength, m_xMapLength, 1);
            CreateCube(LayerMask.NameToLayer("Block"), createPos, scale);
        }

        for (int xDir = -1; xDir < 2; xDir+=2)
        {
            var dir = m_mapArea.xMin;
            if (xDir > 0)
                dir = m_mapArea.xMax;

            createPos = rootPos + (m_myGrid.cellSize.x * dir + colOffset * xDir) * Vector3.right + zOffset;
            scale = new Vector3(1, m_zMapLength, m_zMapLength);
            CreateCube(LayerMask.NameToLayer("Block"), createPos, scale);
        }

        createPos = rootPos + xOffset + zOffset - Vector3.up * 0.5f;
        scale = new Vector3(m_xMapLength, 1, m_zMapLength);
        CreateCube(LayerMask.NameToLayer("Ground"), createPos, scale);

        void CreateCube(int layer, Vector3 pos,Vector3 scale)
        {
            GameObject groundCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            groundCube.layer = layer;
            groundCube.transform.SetParent(transform);
            groundCube.GetComponent<MeshRenderer>().enabled = false;
            groundCube.transform.position = pos;
            groundCube.transform.localScale = scale;
        }
    }
    public Vector3 RandomMapXZVector(float yValue = 0)
    {
        float xRange = Mathf.Round(Instance.maxMapArea.x);
        float zRange = Mathf.Round(Instance.maxMapArea.z);

        Vector3 randomPos = new Vector3(Random.Range(-xRange / 2, xRange / 2), yValue, Random.Range(-zRange / 2, zRange / 2));

        return randomPos;
    }

    public Vector3 GetMapCenter()
    {
        Vector3 xOffset = Mathf.Floor((m_mapArea.xMin + m_mapArea.xMax)) / 2 * Vector3.right * m_myGrid.cellSize.x;
        Vector3 zOffset = Mathf.Floor((m_mapArea.yMin + m_mapArea.yMax)) / 2 * Vector3.forward * m_myGrid.cellSize.y;
        Vector3 centerPos = transform.position + xOffset +zOffset;
        return centerPos;
    }

    public bool IsStartZone(Vector3 center, Vector3 point, float radius)
    {
        // 計算點到圓心的距離
        float distance = Vector3.Distance(point, center);

        // 判斷距離是否小於等於半徑
        return distance <= radius;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        OnMapReady = null;
    }

#if(UNITY_EDITOR)
    private void OnDrawGizmos()
    {
        if (inspectorTilemapStats == null || Application.isPlaying)
            return;

        Gizmos.color = gizmosColor;

        //m_mapArea = new BoundsInt(cellCenter, inspectorTilemapStats.mapBoundSize);
        Gizmos.DrawCube(transform.position, new Vector3(testTile.sprite.bounds.size.x* testTile.sprite.bounds.size.x/2, 0.1f, testTile.sprite.bounds.size.y * testTile.sprite.bounds.size.y / 2));
    }
#endif
}
