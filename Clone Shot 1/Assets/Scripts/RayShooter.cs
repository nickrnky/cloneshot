using UnityEngine;
using System.Collections;

public class RayShooter : MonoBehaviour {
	private Camera _camera;

    [SerializeField]
    private GameObject bulletPrefab;

    public AudioSource FireSound;

	void Start()
    {
		_camera = GetComponent<Camera>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
    {
		if (Input.GetMouseButtonDown(0))
        {
            if(FireSound != null)
            {
                FireSound.Play();
            }

            GameObject _gunshot = Instantiate(bulletPrefab) as GameObject;
            _gunshot.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
            _gunshot.transform.rotation = transform.rotation;
        }
	}

	private IEnumerator SphereIndicator(Vector3 pos)
    {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = pos;

		yield return new WaitForSeconds(1);

		Destroy(sphere);
	}
}