using UnityEngine;
using UnityEngine.UI;

//Joshua

public class PointInTime 
{
    public Vector3 position;
    public Quaternion rotation;
    public int hp;
 
    public PointInTime(Vector3 _position, Quaternion _rotation, int _hp)
    {
        hp = _hp;
        position = _position;
        rotation = _rotation;
    }



}
