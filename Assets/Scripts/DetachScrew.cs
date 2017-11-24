using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachScrew : MonoBehaviour {

	public void Detach() {
        if(Camera.main.GetComponentInChildren<Transformer>())
        {
            Camera.main.GetComponentInChildren<Transformer>().DetachScrew();
        } else
        {
            GetComponentInChildren<Transformer>().DetachScrew();
        }
	}
}
