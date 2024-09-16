using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMotion : MonoBehaviour
{
    public float speed = 2f;         // Speed of the motion
    public float height = 2f;        // Height of the up and down motion

    private Vector3 startPosition;   // Starting position of the rock

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveRockUpDown());
    }

    IEnumerator MoveRockUpDown()
    {
        while (true)
        {
            // Move the rock up and down over time using a sine wave
            float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            // Wait for the next frame
            yield return null;
        }
    }
}

