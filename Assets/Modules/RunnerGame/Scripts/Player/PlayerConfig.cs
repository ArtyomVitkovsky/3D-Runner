using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Config", fileName = "PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float resetSpeedDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private int startHealthPoints;

    public float Speed => speed;

    public float JumpForce => jumpForce;

    public int StartHealthPoints => startHealthPoints;

    public float ResetSpeedDuration => resetSpeedDuration;
}