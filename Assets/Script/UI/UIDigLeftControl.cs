using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDigLeftControl : MonoBehaviour {
	public Diging diging;
	public digMap map;
	public DigingShake digingShake;
	public DigingWarning digingWarning;
	public Transform widget;
	public float length = 100.0f;
	
	public bool play = false;
	private bool mouse = false;
	private int touch = -1;
	private float invLength;
	private float bodyDelta;
	private float foreDelta;
	
	public GameObject full;
	public GameObject push;
	public GameObject L_turn;
	public GameObject R_turn;	
	public GameObject Camera2;
	public DigingWarning leversound;
	public DigingWarning caution;
	public DigingBucket bucket;

	public UILabel preMessage;

	//	private Color color_lever = new Color(0.1f, 0.58f, 0.7f);
	private Color color_lever = new Color(0.639f,0.01f,0.01f);

	int index = 0;
	void Update () 
	{
		if (false == Diging.play)
			return;

		Vector3 center = this.transform.localPosition;
		Vector3 position = GetPosition();
		Vector3 v = position - center;
		
		invLength = 1.0f / length;
		v = v.normalized * Mathf.Clamp(v.magnitude, 0.0f, length);
		
		float _bodyDelta = v.x * invLength;
		float _foreDelta = v.y * invLength;
		
		float speed = diging.body.speed;
		if (true == DigingBucket.shoveling)
			speed = 1.0f;	//들어가면좌우 로움직이지않음
		
		if (!bucket.stall)
			diging.body.angle += speed * Correct (_bodyDelta) * Time.deltaTime;

		
		speed = diging.fore.speed;
		if (true == DigingBucket.shoveling) 
			speed = diging.fore.speed * 0.5f;
		
		if (!bucket.stall)
			diging.fore.angle -= speed * Correct (_foreDelta) * Time.deltaTime;
		
		
		widget.localPosition = v;

		if (widget.localPosition.y > 30) {
			full.GetComponent<TweenColor> ().from = color_lever;
			push.GetComponent<TweenColor> ().from = Color.white;
			L_turn.GetComponent<TweenColor> ().from = Color.white;
			R_turn.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();


		} else if (widget.localPosition.y < -30) {
			full.GetComponent<TweenColor> ().from = Color.white;
			push.GetComponent<TweenColor> ().from = color_lever;
			L_turn.GetComponent<TweenColor> ().from = Color.white;
			R_turn.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();
		} 
		else if (widget.localPosition.x > 30) {
			R_turn.GetComponent<TweenColor> ().from = color_lever;
			L_turn.GetComponent<TweenColor> ().from = Color.white;
			push.GetComponent<TweenColor> ().from = Color.white;
			full.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();
			Camera2.SetActive(false);
			if (true == DigingBucket.shoveling)
			{
				caution.On();
                preMessage.text = "버킷이 땅속에 있습니다. 케빈을 움직이는것은 위험합니다.";
			}

		} else if (widget.localPosition.x < -30) {
			R_turn.GetComponent<TweenColor> ().from = Color.white;
			L_turn.GetComponent<TweenColor> ().from = color_lever;
			push.GetComponent<TweenColor> ().from = Color.white;
			full.GetComponent<TweenColor> ().from = Color.white;
			leversound.On();
			Camera2.SetActive(false);
			if (true == DigingBucket.shoveling)
			{
				caution.On();
                preMessage.text = "버킷이 땅속에 있습니다. 케빈을 움직이는것은 위험합니다.";
			}
		}
		
		if (widget.localPosition == new Vector3(0,0,0))
        {
			full.GetComponent<TweenColor> ().from = Color.white;
			push.GetComponent<TweenColor> ().from = Color.white;
			R_turn.GetComponent<TweenColor> ().from = Color.white;
			L_turn.GetComponent<TweenColor> ().from = Color.white;
			preMessage.text = " " ;

			if(index == 20)
			{
				index = 0;
				map.SendMessage("OnFailExit");
			}
				
		}

        if (0.8f <= Mathf.Abs(bodyDelta - _bodyDelta))
			digingShake.Run ();
        if (0.8f <= Mathf.Abs(foreDelta - _foreDelta))
			digingShake.Run ();
		
		if (true == play) 
		{
			if (diging.fore.angle >= diging.fore.max)
				digingWarning.On();
			if (diging.fore.angle <= diging.fore.min)
				digingWarning.On();
		}
		
		bodyDelta = _bodyDelta;
		foreDelta = _foreDelta;
	}
	
	public void OnPressButton(GameObject btn)
	{
		play = true;
		
		mouse = false;
		touch = -1;
		
		if (UICamera.GetMouse(0).pressed == btn) {
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
		
		return new Vector3(UITools.GetWidth (position.x), UITools.GetHeight (position.y), 0.0f);
	}
	
	float Correct(float value)
	{
		float t = Interpolation.easeInQuad (0.0f, 1.0f, Mathf.Abs(value));
		if (0.0f <= value)
			return t;
		return -t;
	}
}
