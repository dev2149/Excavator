using UnityEngine;
using System.Collections;

public class ItemClick : MonoBehaviour {
	// Item 객체가 스스로 인식하게 될 index 값
	int index;
	
	void Start() {
		// 일단 0으로 초기화
		index = 0;
		
		// Item 객체의 부모(=Grid) 하위의 모든 자식요소(=Item들)을 스캔한다.
		foreach (Transform child in this.transform.parent) {
			// 특정 자식 요소와 나 자신이 동일하다면 반복을 멈춘다.
			if (child == transform) {
				// 여기서 멈추게 되면, 현재의 index값이 나 스스로의 번호가 된다.
				break;
			}
			// 인덱스 값을 1씩 증가한다.
			index++;
		}
		
	}
	
	// 클릭했을 때 실행될 이벤트 함수
	void OnClick() {
	//	Debug.Log (index + "번 째 아이템 클릭됨");
	}
}
