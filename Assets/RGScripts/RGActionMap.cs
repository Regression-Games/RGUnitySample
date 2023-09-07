/*
* This file has been automatically generated. Do not modify.
*/

using System;

namespace RegressionGames
{
    using UnityEngine;

    public class RGActionMap : MonoBehaviour
    {
        private void Awake()
        {
            if (this.TryGetComponent<Player>(out var _))
            {
                gameObject.AddComponent<RGAction_MoveToPosition>();
            }
        }
    }
}