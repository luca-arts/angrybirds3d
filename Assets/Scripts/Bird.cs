﻿using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Rigidbody Rb;
    public GameObject Feathers;
    public GameObject FeatherExplosion;
    public float ReleaseTime = 0.5f;
    private bool _isPressed;
    private bool _isFired;

    void FixedUpdate()
    {
        if (_isPressed && !_isFired)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 6.5f));
            if (worldPosition.y >= 0.2f && worldPosition.y <= 8f)
            {
                Rb.position = worldPosition;
            }
        }
    }

    void OnMouseDown()
    {
        if (_isFired)
        {
            return;
        }

        _isPressed = true;
        Rb.isKinematic = true;
    }

    void OnMouseUp()
    {
        if (_isFired)
        {
            return;
        }

        _isPressed = false;
        Rb.isKinematic = false;

        GetComponent<TrailRenderer>().enabled = true;
        _isFired = true;
        StartCoroutine(Release());
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<TrailRenderer>().enabled = false;
        if (!collision.collider.CompareTag("Ground"))
        {
            GameObject feathers = Instantiate(Feathers, transform.position, Quaternion.identity);
            Destroy(feathers, 2);
        }
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(ReleaseTime);

        Destroy(GetComponent<SpringJoint>());
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(5);

        Instantiate(FeatherExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
