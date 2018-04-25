using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkAnimator))]
public class NetworkAnimationSync : NetworkBehaviour
{

    // Use this for initialization
    void Start()
    {
        NetworkAnimator anim = GetComponent<NetworkAnimator>();
        anim.SetParameterAutoSend(0, true);
        anim.SetParameterAutoSend(1, true);
    }

    public override void PreStartClient()
    {
        NetworkAnimator anim = GetComponent<NetworkAnimator>();
        anim.SetParameterAutoSend(0, true);
        anim.SetParameterAutoSend(1, true);
    }
}
