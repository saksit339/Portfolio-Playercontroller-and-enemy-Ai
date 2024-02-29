using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sampleBulet : MonoBehaviour
{

    [SerializeField]
    private float speed = 20;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed *Time.deltaTime);
        Destroy(gameObject,5);
    }
}
