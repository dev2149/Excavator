using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour{
	
	public GameObject addPlayer;
	public GameObject Player;
	public float smoothTime = 0.5f;
	public float offsetX;
	public float offsetY;
	public float offsetZ;
	
	private float positionX ;
	private float positionY;
	private float positionZ;
	
	private Transform thisTransform;
	private Vector3 velocity;

	public CameraTurn turn;
	public ModeOption option;
	
	void Start(){
		thisTransform = transform;

	}
	
	void Update(){
		if (!Diging.tutorial)
			return;

		if (turn.turn ) 
		{

			if(!option.zoom_on)
				return;

			positionX = Mathf.SmoothDamp (thisTransform.position.x, addPlayer.transform.position.x + offsetX, ref velocity.x, smoothTime);
			positionY = Mathf.SmoothDamp (thisTransform.position.y, addPlayer.transform.position.y + offsetY, ref velocity.y, smoothTime);

			positionZ = Mathf.SmoothDamp (thisTransform.position.z, addPlayer.transform.position.z + offsetZ, ref velocity.z, smoothTime);
			thisTransform.position = new Vector3 (positionX, positionY, positionZ);

		}
		if (!turn.turn) {
			positionX = Mathf.SmoothDamp (thisTransform.position.x, Player.transform.position.x + offsetX, ref velocity.x, smoothTime);
			positionY = Mathf.SmoothDamp (thisTransform.position.y, Player.transform.position.y + offsetY, ref velocity.y, smoothTime);
			
			positionZ = Mathf.SmoothDamp (thisTransform.position.z, Player.transform.position.z + offsetZ, ref velocity.z, smoothTime);

			thisTransform.position = new Vector3 (positionX, positionY, positionZ);	

		}


	}



}