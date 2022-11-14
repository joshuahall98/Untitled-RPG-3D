using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControls : MonoBehaviour
{
    public CinemachineVirtualCamera camera;

    GameObject sideCharacter;
    GameObject player;

    enum LookAt { Player, SideCharacter }
    LookAt lookAt;

    [SerializeField] int nextRotate; 

    private void Awake()
    {
        //this sets the clipping pain at the start of the scene so that the camera doesn't clip through object
        camera.m_Lens.NearClipPlane = -20f;

        player = GameObject.Find("Player");
        sideCharacter = GameObject.Find("SideCharacter");

        lookAt = LookAt.Player;

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 135, transform.eulerAngles.z);
        
    }

    //allows camera to switch between characters
    public void SwitchCharacter()
    {
        if (lookAt == LookAt.Player)
        {
            lookAt = LookAt.SideCharacter;
            camera.Follow = sideCharacter.transform;
        }
        else if (lookAt == LookAt.SideCharacter)
        {
            lookAt = LookAt.Player;
            camera.Follow = player.transform;
        }
    }

    private void Update()
    {
        //position to rotate to
        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, nextRotate, transform.eulerAngles.z);

        //execute rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);
    }

    public void RotateLeft()
    {
        nextRotate = nextRotate + 90;
    }

    public void RotateRight()
    {
        nextRotate = nextRotate - 90;
    }
}
