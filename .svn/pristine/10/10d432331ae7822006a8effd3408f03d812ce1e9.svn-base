using UnityEngine;
using System.Collections;

public class UITools : MonoBehaviour 
{
	public static UITools instance = null;
	
	public UIRoot root;
	
	[HideInInspector]
	public Vector2 screen = Vector2.zero;
	
	void Awake()
	{
		instance = this;
		//Refresh();
	}
	
	void Refresh()
	{
		if (Vector2.zero == screen)
		{
			float aspect = (float)Screen.width / (float)Screen.height;
			screen.y = root.activeHeight;
			screen.x = screen.y * aspect;
			
		//	Debug.Log("Screen : " + Screen.width.ToString() + " x " + Screen.height.ToString() + " : " + aspect.ToString());
		//	Debug.Log("UI : " + screen.x.ToString() + " x " + screen.y);
		}
	}
	
	public static float GetUIWidth()
	{
		UITools.instance.Refresh();
		return UITools.instance.screen.x;
	}
	
	public static float GetUIHeight()
	{
		UITools.instance.Refresh();
		return UITools.instance.screen.y;
	}
	
	public static float GetWidth(float value)
	{
		UITools.instance.Refresh();
		//return value;
		return (value * UITools.instance.screen.x) / Screen.width;
	}
	
	public static float GetHeight(float value)
	{
		UITools.instance.Refresh();
		return (value * UITools.instance.screen.y) / Screen.height;
	}
	
	public static void SetText(Transform item, string text)
	{
		SetText(item.gameObject, text);
	}
	
	public static void SetText(GameObject item, string text)
	{
		item.GetComponent<UILabel>().text = text;
	}
}
