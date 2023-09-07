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
            string xInput = input["x"].ToString();
            float x = default;
            try
            {
                float.TryParse(xInput, out x);
            }
            catch (Exception)
            {
                RGDebug.LogError("Failed to parse 'x'");
            }
            finally
            {
            }

            string yInput = input["y"].ToString();
            float y = default;
            try
            {
                float.TryParse(yInput, out y);
            }
            catch (Exception)
            {
                RGDebug.LogError("Failed to parse 'y'");
            }
            finally
            {
            }

            string zInput = input["z"].ToString();
            float z = default;
            try
            {
                float.TryParse(zInput, out z);
            }
            catch (Exception)
            {
                RGDebug.LogError("Failed to parse 'z'");
            }
            finally
            {
            }

            Invoke("MoveToPosition", x, y, z);
        }
    }
}