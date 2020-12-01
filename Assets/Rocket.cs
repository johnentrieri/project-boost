using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private Vector3 startPosition;
    private Quaternion startRotation;
    enum State { Alive, Transcending, Dying};
    private State state;
    
    [SerializeField] float upThrust = 2.7f;
    [SerializeField] float rotThrust = 120.0f;

    // Start is called before the first frame update
    void Start() {

        startPosition = transform.position;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        audioSource = GetComponent<AudioSource>();

        state = State.Alive;
    }

    // Update is called once per frame
    void Update() {
        if (state == State.Alive) {
            Thrust();
            Rotate();
            if (Input.GetKey(KeyCode.R)) {
                ResetPosition();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        
        switch (collision.gameObject.tag) {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                Invoke("LoadNextLevel", 2.0f);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                Invoke("ResetPosition", 2.0f);
                break;
        }

    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    private void ResetPosition() {
        rigidBody.velocity = Vector3.zero;
        transform.SetPositionAndRotation(startPosition,startRotation);
        state = State.Alive;
    }
}