using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {


	void FixedUpdate() {

		transform.Rotate(new Vector3(1,0,0));
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Body") 
		{
			Destroy(this.gameObject);
		}
	}
}
