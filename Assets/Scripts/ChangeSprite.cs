using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour {

	public Sprite sprite1;
	public Sprite sprite2;

	private bool Lock;

	public void Change() {
		Lock = !Lock;
		if(!Lock) {
			GetComponent<Image>().sprite = sprite1;
		} else {
			GetComponent<Image>().sprite = sprite2;
		}
	}
}
