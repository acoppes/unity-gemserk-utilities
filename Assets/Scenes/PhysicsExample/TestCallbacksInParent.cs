using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCallbacksInParent : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"collision: {other.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"trigger: {other.gameObject.name}");
    }
}
