using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceImplant : MonoBehaviour {

    public LayerMask Mandible;

    public Camera Camera;
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 200.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.transform.CompareTag("Trans"))
            {
                if (Input.GetMouseButtonDown(2))
                {
                    transform.position = hit.point;
                }
            }
        }
	}
}