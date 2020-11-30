using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    [SerializeField] float upThrust = 3.0f;
    [SerializeField] float rotThrust = 100.0f;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag) {
            case "Friendly":
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private void Rotate() {

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        float frameRotation = rotThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(frameRotation * Vector3.forward);
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(frameRotation * Vector3.back);
        }

        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    private void Thrust() {
        if (Input.GetKey(KeyCode.Space)) {
                rigidBody.AddRelativeForce(upThrust * Vector3.up);
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                audioSource.Play();
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                audioSource.Pause();
            }
    }
}