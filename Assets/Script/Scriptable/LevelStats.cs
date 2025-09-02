using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelStats", menuName = "Stats/Level Stats")]
public class LevelStats : ScriptableObject
{
    public int currentLevel;
    public int tilemapSizeRatio;
    public int enemyRatio;
}
