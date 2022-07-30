using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour {

	public Color color_;

	void Start()
	{
		float RGB = Random.Range (1f,0.56f); // RGB다른 느낌으로
		this.gameObject.GetComponent<Renderer>().material.color = new Color(RGB,RGB,RGB); //색상다르게 

		int rockTexture = Random.Range (0,5);
		this.GetComponent<Renderer>().material.SetTexture("_MainTex",(Texture)Resources.Load("rocktexture"+rockTexture));	// 텍스쳐 랜덤으로 	
		float random = Random.Range(0.5f,2.5f);// 랜덤한 크기,저항력, 텍스쳐
		this.gameObject.transform.localScale = new Vector3 (random, random, random); // 크기 다르게

		Physics.gravity = new Vector3(0, -15f, 0); // 전체중력.
		this.GetComponent<Rigidbody>().drag = random / 15f ; // 저항력을 다르게 한다.

	}

}
