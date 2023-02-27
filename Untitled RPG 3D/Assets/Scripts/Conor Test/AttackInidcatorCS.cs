using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using UnityEngine;

public class AttackInidcatorCS : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            transform.position = raycastHit.point;
            Debug.Log(raycastHit.point);
        }
    }

}


