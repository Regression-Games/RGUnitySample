/*
* This file has been automatically generated. Do not modify.
*/

using System;
using System.Collections.Generic;
using RegressionGames;
using RegressionGames.RGBotConfigs;
using RegressionGames.StateActionTypes;
using UnityEngine;

namespace RegressionGames.RGBotConfigs
{
    public class RGStateEntity_Player : RGStateEntity<RGState_Player>
    {
        public float speed => (float)float.Parse(this.GetValueOrDefault("speed").ToString());
        public float range => (float)float.Parse(this.GetValueOrDefault("range").ToString());
    }

    public class RGState_Player : RGState
    {
        private Player myComponent;
        public void Start()
        {
            myComponent = this.GetComponent<Player>();
            if (myComponent == null)
            {
                RGDebug.LogError("Player not found");
            }
        }

        protected override Dictionary<string, object> GetState()
        {
            var state = new Dictionary<string, object>();
            state.Add("speed", myComponent.speed);
            state.Add("ResetActionRange", myComponent.range);
            return state;
        }
    }
}