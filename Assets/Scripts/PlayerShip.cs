using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header("Stats")]
    public float MaxSpeed = 10f;
    public float currentSpeed;
    public float Acceleration = 0.2f;
    public float TurnSpeed = 200f;

    [Header("Computation")]
    public float Decelerate = 0.5f;
    public float Accelerate = 5f;
    public int steerValue;


    void Update()
    {
        currentSpeed += Acceleration * Time.deltaTime;

        // Ensure currentSpeed doesn't exceed MaxSpeed
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, MaxSpeed);

        transform.Rotate(0f, steerValue * TurnSpeed * Time.deltaTime, 0f);

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider otherobject)
    {
        if (otherobject.CompareTag("Obstacle"))
        {
            currentSpeed *= Decelerate;
        }
        if (otherobject.CompareTag("Speedboost"))
        {
            currentSpeed += Accelerate;
        }
    }

    public void Steer(int value)
    {
        steerValue = value;
    }
}

/*
public class PlayerShip : MonoBehaviour
{
    [SerializeField] private float MaxSpeed=10f;
    [SerializeField] private float Acceleration =0.2f;
    [SerializeField] private float TurnSpeed= 200f;
    [SerializeField] private float Decelerate= 0.5f;
    [SerializeField] private float Accelerate= 5f;
   private int steerValue;
  
    void Update()
    {
        MaxSpeed+=Acceleration * Time.deltaTime;

        transform.Rotate(0f, steerValue* TurnSpeed * Time.deltaTime, 0f);

        transform.Translate(Vector3.forward *MaxSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider otherobject)
    {
        if (otherobject.CompareTag("Obstacle"))
        {
            MaxSpeed *=Decelerate;
        }

        if (otherobject.CompareTag("Speedboost"))
        {
            MaxSpeed +=Accelerate;
        }
    }

    public void Steer(int value)
    {
        steerValue=value; 
    }
}
*/