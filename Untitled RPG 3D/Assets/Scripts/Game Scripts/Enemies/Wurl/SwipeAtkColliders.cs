using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAtkColliders : MonoBehaviour
{
    public GameObject[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].gameObject.SetActive(false);
        } 
    }

    public void SwipeCollider1()
    {
        colliders[0].gameObject.SetActive(true);
    }
    public void SwipeCollider2()
    {
        colliders[0].gameObject.SetActive(false);
        colliders[1].gameObject.SetActive(true);
    }
    public void SwipeCollider3()
    {
        colliders[1].gameObject.SetActive(false);
        colliders[2].gameObject.SetActive(true);
        colliders[3].gameObject.SetActive(true);
    }
    public void SwipeCollider4()
    {
        colliders[2].gameObject.SetActive(false);
        colliders[3].gameObject.SetActive(false);
        colliders[4].gameObject.SetActive(true);
        colliders[5].gameObject.SetActive(true);
        colliders[6].gameObject.SetActive(true);

    }
    public void SwipeCollider5()
    {
        colliders[4].gameObject.SetActive(false);
        colliders[5].gameObject.SetActive(false);
        colliders[6].gameObject.SetActive(false);
        colliders[7].gameObject.SetActive(true);

    }

    public void SwipeCollider6()
    {
        colliders[7].gameObject.SetActive(false);
        colliders[8].gameObject.SetActive(true);

    }
    public void SwipeCollider7()
    {
        colliders[8].gameObject.SetActive(false);
        colliders[9].gameObject.SetActive(true);

    }
    public void SwipeCollider8()
    {
        colliders[9].gameObject.SetActive(false);

    }
    public void SwipeCollider9()
    {
        colliders[10].gameObject.SetActive(true);

    }
    public void SwipeCollider10()
    {
        colliders[10].gameObject.SetActive(false);

    }

}
