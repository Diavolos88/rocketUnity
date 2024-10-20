using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MoveObject : MonoBehaviour
{
    [SerializeField] Vector3 movePosition;
    [SerializeField] [Range(0, 1)] float moveProgress;
    Vector3 startPosition;
    [SerializeField] float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveProgress = Mathf.PingPong(Time.time * moveSpeed / 100, 1);
        Vector3 offset = movePosition * moveProgress;
        transform.position = startPosition + offset;
    }
}
