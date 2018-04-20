using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectCollision : MonoBehaviour {

	void OnCollisionEnter (Collision col)
	{
		Debug.Log (col.gameObject.name);
	}
}
