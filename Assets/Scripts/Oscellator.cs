﻿using UnityEngine;

[DisallowMultipleComponent]
public class Oscellator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;


    [Range(0,1)] [SerializeField] float movementFactor;
    Vector3 startingPos;
    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        if( period <= Mathf.Epsilon) { return; } //protection agains dividing by 0
        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;

    }
}
