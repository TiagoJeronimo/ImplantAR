using UnityEngine;
using System.Collections;
 
public class LookAtMouse : MonoBehaviour
{

    public GameObject DBase;

    private void Update()
    {
        transform.up = (transform.position - DBase.transform.position);
    }
}