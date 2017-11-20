using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTransparency : MonoBehaviour {

    public float MaxAlpha = 1;
    public float MinAlpha;

	// Update is called once per frame
	void Update () {
        GameObject targetObject = GameObject.FindGameObjectWithTag("CImp");
        if (targetObject) {
            float distance = (targetObject.transform.position - this.transform.position).magnitude;
            float alpha = distance / 100;
            if (alpha >= MaxAlpha) alpha = MaxAlpha;
            if (alpha <= MinAlpha) alpha = MinAlpha;

            //GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);


            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            renderers[0].sharedMaterial.color = new Color(1, 1, 1, alpha);
            
            /*for (int i = 0; i < renderers.Length; i++) {
                renderers[i].sharedMaterial.color = new Color(1,1,1, alpha);
            }*/
        }
    }
}
