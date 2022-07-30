using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	
	// `Item` 프리팹이 연결될 객체
	public GameObject item;
	// Grid 에서 표시하고자 하는 이미지들이 저장될 Texture 배열
	public Texture[] images;
	
	/** 처음 객체가 로딩될 때, 초기화 함수 호출 */
	void Start () { 
		InitItem(); 
	}
	
	/** Grid 초기화 */
	void InitItem() {
		// 이미지의 수 만큼 반복합니다.
		for (int i = 0; i < images.Length; i++) {
			//일단 생성합니다. 무조건...
			GameObject obj = Instantiate(item, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
			
			//생성된 GameObject의 부모가 누구인지 명확히 알려줍니다. (내가 니 애비다!!)
			obj.transform.parent = this.transform;
			
			//NGUI는 자동이 너무많이 짜증나니 수동으로 Scale을 조정해줍니다.
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			
			// Item 하위의 자식 요소들에 대한 객체를 얻어냅니다.
			UITexture texture = GetChildObj (obj, "Texture").GetComponent<UITexture>(); 
			UILabel label = GetChildObj (obj, "Label").GetComponent<UILabel>(); 
			
			// 자식 객체들에게 이미지와 텍스트를 출력.
			texture.mainTexture = images[i];
			label.text = (i+1).ToString();
		}
		
		//Prefab을 생성한 이후에 Position이 모두 같아서 겹쳐지므로 Reposition시키도록 합니다.
		GetComponent<UIGrid>().Reposition();
	}
	
	/** 객체의 이름을 통하여 자식 요소를 찾아서 리턴하는 함수 */
	GameObject GetChildObj( GameObject source, string strName  ) { 
		Transform[] AllData = source.GetComponentsInChildren< Transform >(); 
		GameObject target = null;
		
		foreach( Transform Obj in AllData ) { 
			if( Obj.name == strName ) { 
				target = Obj.gameObject;
				break;
			} 
		}
		
		return target;
	}
}
