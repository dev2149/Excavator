
#pragma strict

var target : Transform;
var smoothTime = 0.5;
private var thisTransform : Transform;
private var velocity : Vector3;
private var rotate_ : Vector3;

function Start()
{
	thisTransform = transform;
}

function Update() 
{
	thisTransform.position.x = Mathf.SmoothDamp( thisTransform.position.x, 
		target.position.x, velocity.x, smoothTime);
	thisTransform.position.y = Mathf.SmoothDamp( thisTransform.position.y, 
		target.position.y, velocity.y, smoothTime);
	thisTransform.position.z = Mathf.SmoothDamp( thisTransform.position.z, 
		target.position.z, velocity.z, smoothTime);
		
//	thisTransform.rotation.x = Mathf.SmoothDamp( thisTransform.rotation.x, 
//		target.rotation.x, velocity.x, smoothTime);
//	thisTransform.rotation.y = Mathf.SmoothDamp( thisTransform.rotation.y, 
//		target.rotation.y, velocity.y, smoothTime);
//	thisTransform.rotation.z = Mathf.SmoothDamp( thisTransform.rotation.z, 
//		target.rotation.z, velocity.z, smoothTime);
}