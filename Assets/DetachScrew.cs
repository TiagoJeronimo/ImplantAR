using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachScrew : MonoBehaviour {

	public void Detach() {
		GetComponentInChildren<Transformer> ().DetachScrew ();
	}
}
