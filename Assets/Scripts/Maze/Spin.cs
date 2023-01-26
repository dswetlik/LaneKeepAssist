using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    public float _spinSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, _spinSpeed) * Time.deltaTime);        
    }
}
