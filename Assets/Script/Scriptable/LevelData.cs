using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelStats", menuName = "Stats/Level Stats")]
public class LevelData : ScriptableObject
{
    public int currentLevel;
    public int tilemapRatio;
    public int enemyRatio;
}
