using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    private float upThrust = 1.5f;
    private float rotThrust = 0.3f;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    private void ProcessInput() {
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(upThrust * Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            rigidBody.AddRelativeForce(upThrust * Vector3.up);
            audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            rigidBody.AddRelativeForce(upThrust * Vector3.up);
            audioSource.Pause();
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(rotThrust * Vector3.forward);
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(rotThrust * Vector3.back);
        }
    }
}
