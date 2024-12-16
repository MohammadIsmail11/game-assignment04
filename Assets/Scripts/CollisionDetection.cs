using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // This method is called when two colliders touch each other if "Is Trigger" is not checked.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }

    // This method is called when one of the colliders has "Is Trigger" checked.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger collision detected with " + other.gameObject.name);
    }
}