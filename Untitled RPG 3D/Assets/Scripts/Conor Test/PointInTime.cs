using UnityEngine;

public class PointInTime 
{
    public Vector3 position;
    public Quaternion rotation;
    public float hp;



    public PointInTime (Vector3 _position, Quaternion _rotation, float _hp)
    {
        hp = _hp;
        position = _position;
        rotation = _rotation;
    }

  
}
