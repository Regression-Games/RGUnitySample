/*
* This file has been automatically generated. Do not modify.
*/

using System;
using System.Collections.Generic;

namespace RegressionGames.RGBotConfigs
{
    public class Player_RGState : RGState
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