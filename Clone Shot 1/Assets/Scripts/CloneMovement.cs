using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMovement : MonoBehaviour
{
    Vector3 offset;
    private Transform master;
    private Transform myTransform;

    void Start()
    { 
        master = GameObject.Find("Player").transform;
        myTransform = transform;
        offset = master.position - myTransform.position;
    }

    void Update()
    { 
        myTransform.position = master.position - offset;
        myTransform.rotation = master.rotation;
    }

}
