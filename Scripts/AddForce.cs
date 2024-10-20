using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addForce : MonoBehaviour
{
    [SerializeField] float force;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
