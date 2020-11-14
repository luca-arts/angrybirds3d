using System;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject Bird;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        if (Bird != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Math.Max(Bird.transform.position.z - 10, _startPosition.z));
        }
    }
}
