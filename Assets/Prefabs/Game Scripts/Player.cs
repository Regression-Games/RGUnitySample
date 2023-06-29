using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTowards(PowerUp powerUp)
    {
        if (powerUp)
        {
            var dir = (powerUp.transform.position - transform.position).normalized;
            _rigidbody.MovePosition(_rigidbody.transform.position + dir * 5.0f * Time.fixedDeltaTime);
        }
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
