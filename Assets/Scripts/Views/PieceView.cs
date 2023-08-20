using UnityEngine;

public class PieceView : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    public Player Player => _player;


    public Vector3 WorldPosition => transform.position;

    internal void MoveTo(Vector3 WorldPosition)
    {
        transform.position = WorldPosition;
    }

    internal void Taken()
    {
        gameObject.SetActive(false);
    }

    internal void Placed(Vector3 WorldPosition)
    {
        transform.position = WorldPosition;
        gameObject.SetActive(true);
    }
}
