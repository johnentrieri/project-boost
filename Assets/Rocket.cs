﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource thrustSound, explosionSound, transcendSound;
    private Vector3 startPosition;
    private Quaternion startRotation;
    enum State { Alive, Transcending, Dying};
    private State state;
    private float upThrust, rotThrust;
    private bool collisionsDisabled;
    [SerializeField] ParticleSystem thrusterParticles, successParticles, deathParticles;

    // Start is called before the first frame update
    void Start() {

        startPosition = transform.position;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        thrustSound = GetComponents<AudioSource>()[0];
        explosionSound = GetComponents<AudioSource>()[1];
        transcendSound = GetComponents<AudioSource>()[2];

        state = State.Alive;
        collisionsDisabled = false;

        rotThrust = 120.0f;
        upThrust = 2.7f;
       
        #if UNITY_WEBGL
            upThrust = 20.0f;
        #endif

        #if UNITY_EDITOR
            upThrust = 2.7f;
        #endif      
    }

    // Update is called once per frame
    void Update() {
        if (state == State.Alive) {
            ThrustHandler();
            RotateHandler();
            if (Debug.isDebugBuild) { DebugHandler(); }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled) { return; }

        switch (collision.gameObject.tag) {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                state = State.Transcending;
                thrustSound.Stop();
                thrusterParticles.Stop();
                transcendSound.Play();
                successParticles.Play();
                Invoke("LoadNextLevel", 2.0f);
                break;
            default:
                state = State.Dying;
                thrustSound.Stop();
                thrusterParticles.Stop();
                explosionSound.Play();
                deathParticles.Play();
                Invoke("ResetPosition", 2.0f);
                break;
        }

    }

    private void DebugHandler() {
        if (Input.GetKeyDown(KeyCode.R)) ResetPosition();
        if (Input.GetKeyDown(KeyCode.C)) toggleCollisions();
        if (Input.GetKeyDown(KeyCode.Alpha1)) SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SceneManager.LoadScene(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SceneManager.LoadScene(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SceneManager.LoadScene(4);
    }

    private void LoadNextLevel() {
        transcendSound.Stop();
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);

    }

    private void RotateHandler() {

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

    private void ThrustHandler() {

        //Space Key Thrusts
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(upThrust * Vector3.up);
        }

        //Play/Pause Thrust Sound
        if (Input.GetKeyDown(KeyCode.Space)) { 
            thrustSound.Play(); 
            thrusterParticles.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space)) { 
            thrustSound.Pause(); 
            thrusterParticles.Stop();
        }
    }

    private void ResetPosition() {
        explosionSound.Stop();
        rigidBody.velocity = Vector3.zero;
        transform.SetPositionAndRotation(startPosition,startRotation);
        state = State.Alive;
    }

    private void toggleCollisions() {
        if (collisionsDisabled) { collisionsDisabled = false; }
        else { collisionsDisabled = true; }
    }
}