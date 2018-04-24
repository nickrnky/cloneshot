using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScript : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        Character p = other.gameObject.GetComponent<Character>();
        if(p == null)
        {
            p = other.gameObject.GetComponentInParent<Character>();
        }

        if(p != null)
        {
            p.CmdDie();
        }
    }

}
