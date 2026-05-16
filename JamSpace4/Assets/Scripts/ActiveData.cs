using UnityEngine;

[System.Serializable]
public struct ActiveData
{
    public Vector3 position;
    public bool isFacingRight;

    public ActiveData(Vector3 pos, bool facing)
    {
        this.position = pos;
        this.isFacingRight = facing;
    }
}