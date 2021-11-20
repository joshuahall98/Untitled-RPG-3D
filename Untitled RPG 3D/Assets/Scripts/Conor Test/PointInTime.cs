using UnityEngine;
using UnityEngine.UI;

public class PointInTime 
{
    public Vector3 position;
    public Quaternion rotation;
    public float hp;
    public Image hb;



    public PointInTime(Vector3 _position, Quaternion _rotation, float _hp)
    {
       // hb.fillAmount = _hb;
        hp = _hp;
        position = _position;
        rotation = _rotation;
    }

  
}
