using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;

      [SerializeField] float rcsThrust = 100f; // serializefield makes it public for the editor so its pretty handy
      [SerializeField] float mainThrust = 100f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

   
    void Update()
    {
        Thrust();
        Rotate();

    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("OK");
                break;
            case "Fuel":
                print("FUEL");
                break;
            default:
                print("Dead");
                break;


        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
           
            rb.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
    private void Rotate()
    {
        rb.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);

        }
        rb.freezeRotation = false;
    }
}
