using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float forceAmount = -30f; // Negative to move left

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the object
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Apply a force to move the object from right to left
            GetComponent<Rigidbody>().AddForce(new Vector3(forceAmount, 0, 0), ForceMode.Impulse);
        }
    }
}
