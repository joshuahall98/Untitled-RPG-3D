using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAHMousePosition : MonoBehaviour
{
    public Transform customMouse;

    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, ground))
        {
            customMouse.transform.position = hit.point;
        }

        //GetComponent<AttackAim>().Aim();
    }
}
