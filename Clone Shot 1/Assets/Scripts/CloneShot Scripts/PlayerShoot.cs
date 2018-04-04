using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, 1);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {

        Debug.Log(_playerID + " has been shot!");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.TakeDamage(_damage);

    }
}
