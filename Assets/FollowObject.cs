using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    private GameObject Target;
	
	// Update is called once per frame
	void FixedUpdate () {
        if(Target == null)
            Target = GameObject.FindGameObjectWithTag("CImp");
        
        if(Target != null)
            transform.position = Target.transform.position;
	}
}
