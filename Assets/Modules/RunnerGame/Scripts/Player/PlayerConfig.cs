using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Config", fileName = "PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public float Speed => speed;

    public float JumpForce => jumpForce;
}