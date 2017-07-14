using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRelativePosition : MonoBehaviour {

    public static Vector3 PositionRelativeToJaw;

    void Update () {
        GameObject targetObject = GameObject.FindGameObjectWithTag("CImp");
        if(targetObject) {
            Transform targetTransform = targetObject.GetComponent<Transform>();
            PositionRelativeToJaw = this.transform.position - targetTransform.transform.position;

            //Debug.Log("PositonRealativeJAw: " + PositionRelativeToJaw.ToString());
        }
    }
}
