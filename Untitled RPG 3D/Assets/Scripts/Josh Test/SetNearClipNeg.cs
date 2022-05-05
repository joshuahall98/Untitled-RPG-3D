using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetNearClipNeg : MonoBehaviour
{

    public CinemachineVirtualCamera camera;

    private void Awake()
    {

        camera.m_Lens.NearClipPlane = -20f;
    }

}
