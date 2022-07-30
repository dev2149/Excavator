using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDigRightControl : MonoBehaviour {
	public Diging diging;
	public DigingShake digingShake;
	public DigingWarning digingWarning;
	public Transform widget;
	public float length = 100.0f;
	
	public bool play = false;
	private bool mouse = false;
	private int touch = -1;
	private float invLength;
	private float upperDelta;
	private float bucketDelta;

	public GameObject Down;
	public GameObject Up;
	public GameObject Bucket_push;
	public GameObject Bucket_pull;	
	public DigingWarning leversound;
	private Color color_lever = new Color(0.639f,0.01f,0.01f);
		
	public bool stop;
	public UILabel preMessage;

	// Use this for initialization
	void Start () 
	{                                                                                                                                                                                                 
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (false == Diging.play)
            return;

		Vector3 center = this.transform.localPosition;
		Vector3 position = GetPosition ();
		Vector3 v = position - center;
		
		invLength = 1.0f / length;
		v = v.normalized * Mathf.Clamp(v.magnitude, 0.0f, length);
		
		float _upperDelta = v.y * invLength;
		float _bucketDelta = v.x * invLength;

		float speed = diging.upper.speed;
		if (true == DigingBucket.shoveling)
            speed = diging.upper.speed * 0.7f;

		if(stop == false)
			diging.upper.angle += speed * Correct(_upperDelta) * Time.deltaTime;

		speed = diging.bucket.speed;
		if (true == DigingBucket.shoveling)
			speed = diging.bucket.speed * 0.7f;
		diging.bucket.angle -= speed * Correct(_bucketDelta) * Time.deltaTime;
		
		widget.localPosition = v;


		//////////////////
		if (widget.localPosition.y > 30) {
			Down.GetComponent<TweenColor> ().from = color_lever;
			Up.GetComponent<TweenColor> ().from = Color.white;
			Bucket_pull.GetComponent<TweenColor> ().from = Color.white;
			Bucket_push.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();
			preMessage.text = " "; 
			stop = false;

		} else if (widget.localPosition.y < -30) {
			Down.GetComponent<TweenColor> ().from = Color.white;
			Up.GetComponent<TweenColor> ().from = color_lever;
			Bucket_pull.GetComponent<TweenColor> ().from = Color.white;
			Bucket_push.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();
			preMessage.text = " "; 
		} 
		if (widget.localPosition.x > 30) {
			Bucket_pull.GetComponent<TweenColor> ().from = color_lever;
			Bucket_push.GetComponent<TweenColor> ().from = Color.white;
			Down.GetComponent<TweenColor> ().from = Color.white;
			Up.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();
		} else if (widget.localPosition.x < -30) {
			Bucket_pull.GetComponent<TweenColor> ().from = Color.white;
			Bucket_push.GetComponent<TweenColor> ().from = color_lever;
			Down.GetComponent<TweenColor> ().from = Color.white;
			Up.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();
		}
		
		if (widget.localPosition == new Vector3(0,0,0)) {
			Down.GetComponent<TweenColor> ().from = Color.white;
			Up.GetComponent<TweenColor> ().from = Color.white;
			Bucket_pull.GetComponent<TweenColor> ().from = Color.white;
			Bucket_push.GetComponent<TweenColor> ().from = Color.white;
		}

		
		if (0.8f <= Mathf.Abs (upperDelta - _upperDelta))
			digingShake.Run ();
		if (0.8f <= Mathf.Abs (bucketDelta - _bucketDelta))
			digingShake.Run ();
		
		if (true == play) 
		{
			if (diging.upper.angle >= diging.upper.max)
				digingWarning.On();
			if (diging.upper.angle <= diging.upper.min)
				digingWarning.On();
			
			if (diging.bucket.angle >= diging.bucket.max)
				digingWarning.On();
			if (diging.bucket.angle <= diging.bucket.min)
				digingWarning.On();
		}
		
		upperDelta = _upperDelta;
		bucketDelta = _bucketDelta;
	}
	
	public void OnPressButton(GameObject btn)
	{
		play = true;
		
		mouse = false;
		touch = -1;

        if (UICamera.GetMouse(0).pressed == btn)
        {
            mouse = true;
        }
        else
        {
            for (int i = 0; i < UICamera.activeTouches.Count; i++)
            {
                if (UICamera.activeTouches[i].pressed == btn)
                {
                    touch = i;
                    break;
                }
            }
        }
	}
	
	public void OnReleaseButton(GameObject btn)
	{
		play = false;
		widget.localPosition = Vector3.zero;
	}
	
	Vector3 GetPosition()
	{
		if (false == play)
			return this.transform.localPosition;
		
		Vector2 position = Vector2.zero;
		if (true == mouse) {
			position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		} 
		else if (-1 != touch)
		{
			UICamera.MouseOrTouch t = UICamera.GetTouch(touch, true);
			if (null != t)
				position = t.pos;
		}
		
		return new Vector3(UITools.GetWidth (position.x) - UITools.GetUIWidth(), UITools.GetHeight (position.y), 0.0f);
	}
	
	float Correct(float value)
	{
		float t = Interpolation.easeInQuad (0.0f, 1.0f, Mathf.Abs(value));
		if (0.0f <= value)
			return t;
		return -t;
	}
}
