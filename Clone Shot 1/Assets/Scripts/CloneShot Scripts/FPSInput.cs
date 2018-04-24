using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// basic WASD-style movement control
// commented out line demonstrates that transform.Translate instead of charController.Move doesn't have collision detection

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour {
	public float speed = 6.0f;
	public float gravity = -9.8f;
    public float jump_speed = 5.0f;

    public bool AllowMovement = true;

	private CharacterController _charController;
    private Animator _animator;

    private float falling_speed;
	
	void Start() {
        _animator = GetComponent<Animator>();
		_charController = GetComponent<CharacterController>();
        falling_speed = 0;
	}
	
	void Update()
    {
        if (AllowMovement)
        {
            
            //transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);
            float deltaX = Input.GetAxis("Horizontal") * speed;
            float deltaZ = Input.GetAxis("Vertical") * speed;
            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement = Vector3.ClampMagnitude(movement, speed);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                falling_speed = jump_speed;
            }
            
            falling_speed = gravity * Time.deltaTime + falling_speed;
            movement.y = falling_speed;

            if (_charController.isGrounded)
            {
                CmdAnimateRunning(movement);
            }

            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);
            _charController.Move(movement);
        }


        if (_charController.isGrounded)
        {
            falling_speed = 0;
        }
	}
    
    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    [Command]
    private void CmdAnimateRunning(Vector3 Movement)
    {
        RpcAnimateRunning(Movement);
    }

    [ClientRpc]
    private void RpcAnimateRunning(Vector3 Movement)
    {
        _animator.SetFloat("Speed", Movement.sqrMagnitude);
    }
}
