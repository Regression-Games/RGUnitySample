/*
* This file has been automatically generated. Do not modify.
*/

using System;
using System.Collections.Generic;
using RegressionGames;
using RegressionGames.RGBotConfigs;
using RegressionGames.StateActionTypes;
using UnityEngine;

namespace RegressionGames
{
    public class RGAction_MoveToPosition : RGAction
    {
        public void Start()
        {
            AddMethod("MoveToPosition", new Action<float, float, float>(GetComponent<Player>().MoveToPosition));
        }

        public override string GetActionName()
        {
            return "MoveToPosition";
        }

        public override void StartAction(Dictionary<string, object> input)
        {
            float x = default;
            if (input.TryGetValue("x", out var xInput))
            {
                try
                {
                    float.TryParse(xInput.ToString(), out x);
                }
                catch (Exception ex)
                {
                    RGDebug.LogError($"Failed to parse 'x' - {ex}");
                }
            }
            else
            {
                RGDebug.LogError("No parameter 'x' found");
                return;
            }

            float y = default;
            if (input.TryGetValue("y", out var yInput))
            {
                try
                {
                    float.TryParse(yInput.ToString(), out y);
                }
                catch (Exception ex)
                {
                    RGDebug.LogError($"Failed to parse 'y' - {ex}");
                }
            }
            else
            {
                RGDebug.LogError("No parameter 'y' found");
                return;
            }

            float z = default;
            if (input.TryGetValue("z", out var zInput))
            {
                try
                {
                    float.TryParse(zInput.ToString(), out z);
                }
                catch (Exception ex)
                {
                    RGDebug.LogError($"Failed to parse 'z' - {ex}");
                }
            }
            else
            {
                RGDebug.LogError("No parameter 'z' found");
                return;
            }

            Invoke("MoveToPosition", x, y, z);
        }
    }

    public class RGActionRequest_MoveToPosition : RGActionRequest
    {
        public RGActionRequest_MoveToPosition(float x, float y, float z)
        {
            action = "MoveToPosition";
            Input = new ()
            {{"x", x}, {"y", y}, {"z", z}, };
        }
    }
}