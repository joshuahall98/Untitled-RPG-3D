using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//Joshua

public class CameraControls : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    //public Camera camera;

    GameObject sideCharacter;
    GameObject player;

    enum LookAt { Player, SideCharacter }
    LookAt lookAt;

    [SerializeField] int nextRotate;

    float distance;
    GameObject distanceCheckerObj;

    private void Awake()
    {
        //this sets the clipping pain at the start of the scene so that the camera doesn't clip through object
        //camera.m_Lens.NearClipPlane = -20f;
        cinemachineCamera.m_Lens.NearClipPlane = -50f;

        /*player = GameObject.Find("Player");
        distanceCheckerObj = GameObject.Find("DistanceChecker");
        sideCharacter = GameObject.Find("SideCharacter");

        lookAt = LookAt.Player;*/

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 135, transform.eulerAngles.z);

    }

    //allows camera to switch between characters
    public void SwitchCharacter()
    {
        if (lookAt == LookAt.Player)
        {
            lookAt = LookAt.SideCharacter;
            cinemachineCamera.Follow = sideCharacter.transform;
        }
        else if (lookAt == LookAt.SideCharacter)
        {
            lookAt = LookAt.Player;
            cinemachineCamera.Follow = player.transform;
        }
    }

    private void Update()
    {
        Rotation();

        //AdjustableClippingPlane();
    }

    //this allows for far objects to appear small in the ortho camera based off distance from player to object
    /*void AdjustableClippingPlane()
    {
        distance = Vector3.Distance(distanceCheckerObj.transform.position, player.transform.position);

        distance -= 20;

        cinemachineCamera.m_Lens.NearClipPlane = distance;
        camera.farClipPlane = distance;
    }*/

    void Rotation()
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
