using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Stats/Player Stats")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed;
    public float jumpForce;
    public float bounceForce;
    public LayerMask groundLayer;
}