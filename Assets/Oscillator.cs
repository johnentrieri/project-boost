using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    private Vector3 startingPos;
    [SerializeField] float oscillationSpeed;
    [SerializeField] Vector3 movementVector;

    // Start is called before the first frame update
    void Start() {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        transform.position = startingPos + ( Mathf.Sin(Time.time * oscillationSpeed) * movementVector );
    }
}
