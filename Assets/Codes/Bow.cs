using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject arrowPrefab;

    public void Attack()
    {
        GameObject arrowObject = Instantiate (arrowPrefab);
        arrowObject.transform.position = transform.position + transform.forward;
        arrowObject.transform.forward = transform.forward;

    }
}
