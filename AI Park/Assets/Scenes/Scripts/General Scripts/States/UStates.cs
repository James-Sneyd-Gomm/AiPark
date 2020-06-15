using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.States
{
    public class UStates 
    {
        protected float _utilityValue;
        public float utilityValue { get { return _utilityValue; } set { _utilityValue = value; } }

        protected states _currentState;
        public states currentState { get { return _currentState; } set { _currentState = value; } }

    }
    public enum states { Hungry, Thirsty, Tried, Idle, Scared };
    public enum predStates { Hunt, Patrol };

}
