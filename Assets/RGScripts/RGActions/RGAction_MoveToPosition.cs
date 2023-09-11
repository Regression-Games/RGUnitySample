/*
* This file has been automatically generated. Do not modify.
*/

using System;
using System.Collections.Generic;
using RegressionGames;
using RegressionGames.RGBotConfigs;
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
            if (!input.ContainsKey("x"))
            {
                RGDebug.LogError("No parameter 'x' found");
                return;
            }

            string xInput = input["x"].ToString();
            float x = default;
            try
            {
                float.TryParse(xInput, out x);
            }
            catch (Exception ex)
            {
                RGDebug.LogError("Failed to parse 'x'");
                RGDebug.LogError(ex.Message);
            }
            finally
            {
            }

            if (!input.ContainsKey("y"))
            {
                RGDebug.LogError("No parameter 'y' found");
                return;
            }

            string yInput = input["y"].ToString();
            float y = default;
            try
            {
                float.TryParse(yInput, out y);
            }
            catch (Exception ex)
            {
                RGDebug.LogError("Failed to parse 'y'");
                RGDebug.LogError(ex.Message);
            }
            finally
            {
            }

            if (!input.ContainsKey("z"))
            {
                RGDebug.LogError("No parameter 'z' found");
                return;
            }

            string zInput = input["z"].ToString();
            float z = default;
            try
            {
                float.TryParse(zInput, out z);
            }
            catch (Exception ex)
            {
                RGDebug.LogError("Failed to parse 'z'");
                RGDebug.LogError(ex.Message);
            }
            finally
            {
            }

            Invoke("MoveToPosition", x, y, z);
        }
    }
}