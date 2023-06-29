using System;
using System.Collections;
using System.Collections.Generic;
using RegressionGames.RGBotConfigs;
using UnityEngine;

public class RGPlayerMoveAction : RGAction
{
    
    private Vector3? targetPosition;
    private Rigidbody rigidbody;
    public float speed = 5000f;
    public float range = 1f;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public override string GetActionName()
    {
        return "MoveToPosition";
    }

    public override void StartAction(Dictionary<string, object> input)
    {
        var targetX = float.Parse(input["x"].ToString());
        var targetY = float.Parse(input["y"].ToString());
        var targetZ = float.Parse(input["z"].ToString());
        targetPosition = new Vector3(targetX, targetY, targetZ);
    }

    public void Update()
    {
        
        // If we are in range, reset the action
        if (targetPosition != null && Vector3.Distance((Vector3) targetPosition, transform.position) < range)
        {
            targetPosition = null;
        }
        
        // Set the target velocity
        if (targetPosition != null)
        {
            rigidbody.velocity = ((Vector3) targetPosition - transform.position).normalized * speed * Time.deltaTime;
        }
    }
}
