using System.Collections;
using System.Collections.Generic;
using AI.Controller;
using UnityEngine;
using AI.States;


namespace AI.Spawn
{
    public class NestSpawn : MonoBehaviour
    {
        protected GameObject nestOwner;
        protected UtilityStates nestOwnerStates;
        protected GameObject prefab;
        protected bool ownerAlive;
        protected bool respawning;

        public float respawnTime;
        protected float internalClock;

        public AnimationCurve HungerRate;
        public AnimationCurve ThirstRate;
        public AnimationCurve TirednessRate;


        void Start()
        {
            AIController.Death += Nest_OwnerDeath;
            prefab = Resources.Load<GameObject>("Prey AI");
            nestOwner = Instantiate(prefab, gameObject.transform);
            nestOwnerStates = nestOwner.GetComponent<UtilityStates>();
            nestOwnerStates.HungerRate = HungerRate;
            nestOwnerStates.ThirstRate = ThirstRate;
            nestOwnerStates.TirednessRate = TirednessRate;
            ownerAlive = true;
            internalClock = respawnTime;
            respawning = false;


        }

        void Update()
        {
            if (respawning)
            {
                internalClock -= Time.deltaTime;
            }

            if (!ownerAlive && internalClock <= 0)
            {
                nestOwner = Instantiate(prefab, gameObject.transform);
                nestOwnerStates = nestOwner.GetComponent<UtilityStates>();
                nestOwnerStates.HungerRate = HungerRate;
                nestOwnerStates.ThirstRate = ThirstRate;
                nestOwnerStates.TirednessRate = TirednessRate;
                ownerAlive = true;
                respawning = false;
                internalClock = respawnTime;
            }



        }


        protected void Nest_OwnerDeath(GameObject sender)
        {

            if (nestOwner == sender as GameObject)
            {
                ownerAlive = false;
                respawning = true;
            }
         
        }
       

    }
}

