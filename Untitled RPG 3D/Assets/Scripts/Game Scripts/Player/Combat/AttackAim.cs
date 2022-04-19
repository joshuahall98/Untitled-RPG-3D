using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAim : MonoBehaviour
{
    //THIS ROTATES YOUR CHARACTER WHEN ACTIVATED TO FACE TOWARDS MOUSE LOCATION

    private Camera mainCamera;
    [SerializeField] private LayerMask groundMask;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;

        }

    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);

        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }


    }

}
