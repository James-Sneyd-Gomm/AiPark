using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.States;
using AI.Spawn;


namespace AI.States
{
    public class UtilityStates : MonoBehaviour, IUtilityStates
    {
        [SerializeField]
        protected float hunger;
        [SerializeField]
        protected float thirst;
        [SerializeField]
        protected float energy;

        [SerializeField]
        protected float HungerLoss;
        [SerializeField]
        protected float WaterLoss;
        [SerializeField]
        protected float EnergyLoss;

        protected float sightRange;

        public AnimationCurve HungerRate;
        public AnimationCurve ThirstRate;
        public AnimationCurve TirednessRate;

        private float uValueHunger;
        private float uValueThirst;
        private float uValueEnergy;

        protected UStates tempUState;

        public float timePerTick;
        [SerializeField]
        protected bool _NodeOccupied;
        public bool NodeOccupied { set { _NodeOccupied = value; } get { return _NodeOccupied; } }

        protected bool _dead;
        public bool dead { set { _dead = value; } get { return _dead; } }

        protected bool _hidden;
        public bool hidden { set { _hidden = value; } get { return _hidden; } }

        protected bool _oblivious;
        public bool oblivious { set { _oblivious = value; } get { return _oblivious; } }




        // current timer via events casusing problems removing for now
        //protected TimerNew timer;

        List<UStates> stateValue = new List<UStates>();

        protected states _currentActiveState;
        public states currentActiveState { get { return _currentActiveState;} }
      

        void Start()
        {
            _NodeOccupied = false;
         
            // Base Stats
            hunger = 100;
            thirst = 100;
            energy = 100;

            // Timer
            timePerTick = 5;

        //timer = GetComponent<TimerNew>();
        //timer.tick.AddListener(updateStats);

            tempUState = new UStates();
            stateListSetup();
           
        }


        void Update()
        {
            tickRate();
            FindState();

            if (hunger <= 0 || thirst <= 0 || energy <= 0)
            {
               _dead = true;
            }
        }

        public void updateStats()
        {
            hunger -= HungerLoss;
            thirst -= WaterLoss;
            energy -= EnergyLoss;
        }


        private void stateListSetup()
        {
            float tempUVal;
            // change state values into percentages to be used with the public curves

            uValueHunger = hunger / 100;
            uValueThirst = thirst / 100;
            uValueEnergy = energy / 100;

            // find the value of the state value as set of the curve

            // Hunger
            tempUVal = HungerRate.Evaluate(uValueHunger);
            tempUVal = Mathf.Clamp(tempUVal, 0.0f, 1.0f);
            tempUState.currentState = states.Hungry;
            tempUState.utilityValue = (1 * (tempUVal * 10));
            stateValue.Add(tempUState);

            tempUState = new UStates();

            // Thirst
            tempUVal = ThirstRate.Evaluate(uValueThirst);
            tempUVal = Mathf.Clamp(tempUVal, 0.0f, 1.0f);
            tempUState.currentState = states.Thirsty;
            tempUState.utilityValue = (1 * (tempUVal * 10));
            stateValue.Add(tempUState);
            tempUState = new UStates();

            // Energy
            tempUVal = TirednessRate.Evaluate(uValueEnergy);
            tempUVal = Mathf.Clamp(tempUVal, 0.0f, 1.0f);
            tempUState.currentState = states.Tried;
            tempUState.utilityValue = (1 * (tempUVal * 10));
            stateValue.Add(tempUState);

            tempUState = new UStates();
        }



        private void FindState()
        {
            float tempUVal;
            // change state values into percentages to be used with the public curves

            uValueHunger = hunger / 100;
            uValueThirst = thirst / 100;
            uValueEnergy = energy / 100;

            // find the value of the state value as set of the curve

            // Hunger
            tempUVal = HungerRate.Evaluate(uValueHunger);
            tempUState.utilityValue = (1 * (tempUVal * 10));
            stateValue[0].utilityValue = tempUState.utilityValue;

            // Thirst
            tempUVal = ThirstRate.Evaluate(uValueThirst);
            tempUState.utilityValue = (1 * (tempUVal * 10));
            stateValue[1].utilityValue = tempUState.utilityValue;

            // Energy
            tempUVal = TirednessRate.Evaluate(uValueEnergy);
            tempUState.utilityValue = (1 * (tempUVal * 10));
            stateValue[2].utilityValue = tempUState.utilityValue;


            // Find highest Utility Value
            float tempFloat = 0;
            int i;

            for (i = 0; i < stateValue.Count; i++)
            {
               if (stateValue[i].utilityValue > tempFloat )
                {
                    _currentActiveState = stateValue[i].currentState;
                    tempFloat = stateValue[i].utilityValue;
                }
            }

            FindPredator();

            Debug.Log(_currentActiveState);
        }




        protected void tickRate()
        {

            timePerTick -= Time.deltaTime;

            if (timePerTick <= 0)
            {
                Debug.Log("Timer Activated");
                timePerTick = 5;
                updateStats();
            }
        }


        public void healHunger(int ammount)
        {
            hunger = Mathf.Clamp(hunger + ammount, 0, 100); 
        }

        public void healThirst(int ammount)
        {
            thirst = Mathf.Clamp(thirst + ammount, 0, 100);  
        }

        public void healEnergy(int ammount)
        {
            energy = Mathf.Clamp(energy + ammount, 0, 100); 
        }



        public void FindPredator()
        {
            if (oblivious)
            {
                sightRange = 3.0f;
            }
            else
            {
                sightRange = 8.0f;
            }

            foreach (Collider2D predator in Physics2D.OverlapCircleAll(gameObject.transform.position, sightRange))
            {
                if (predator.gameObject.tag.Contains("Predator"))
                {
                    _currentActiveState = states.Scared;
                }
            }
        }

    }

}
