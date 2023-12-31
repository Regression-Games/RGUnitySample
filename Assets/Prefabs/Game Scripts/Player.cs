using System;
using System.Collections;
using System.Collections.Generic;
using RegressionGames;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3? targetPosition;
    [RGState]
    public float speed = 100f;
    [RGState("ResetActionRange")]
    public float range = 1f;
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        
        // If we are in range, reset the action
        if (targetPosition != null && Vector3.Distance((Vector3) targetPosition, transform.position) < range)
        {
            targetPosition = null;
        }
        
        // Set the target velocity
        if (targetPosition != null)
        {
            Vector3 direction = ((Vector3)targetPosition - transform.position).normalized;
            direction.y = 0;
            float force = speed * Time.deltaTime;
            _rigidbody.AddForce(direction * force);
        }
    }

    [RGAction]
    public void MoveToPosition(float x, float y, float z)
    {
        targetPosition = new Vector3(x, y, z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var powerUp = collision.gameObject.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            powerUp.Consume();
        }
    }
}
