using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Pathfinding;
using AI.States;

namespace AI
{
    public class PredatorAIController : MonoBehaviour
    {
        protected List<GameObject> patrolPoints;
        protected AIDestinationSetter target;
        protected DetectPrey detectPrey;
        protected Transform currentPatrolPoint;
        protected AIPath aiPath;

        protected Transform currentPrey;
        protected Transform currentWayPoint;

        protected bool requestTarget;
        protected bool preyDetected;
        protected predStates currentStates;


        public void Start()
        {
            target = GetComponent<AIDestinationSetter>();
            detectPrey = GetComponent<DetectPrey>();
            aiPath = GetComponent<AIPath>();
            patrolPoints = new List<GameObject>();
            requestTarget = true;

            currentStates = predStates.Patrol;

            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.name.Contains("Patrol"))
                {
                    patrolPoints.Add(obj);
                }
            }

        }


        public void Update()
        {
            detectPrey.ScanPrey();
            FindTarget();

            if (currentStates == predStates.Hunt)
            {
                target.target = currentPrey.position;
                aiPath.maxSpeed = 5;
            }
            else if (currentStates == predStates.Patrol)
            {
                target.target = currentWayPoint.position;
                aiPath.maxSpeed = 3;
            }

        }


        public void FindTarget()
        {
            currentStates = detectPrey.changeState();

            if (currentStates == predStates.Hunt)
            {
                currentPrey = detectPrey.prey.transform;

                if (Vector2.Distance(gameObject.transform.position, target.target) <= 1 && detectPrey.prey.GetComponent<UtilityStates>().hidden == false)
                {
                    detectPrey.prey.GetComponent<UtilityStates>().dead = true;
                }

                if (detectPrey.prey.GetComponent<UtilityStates>().dead == true || detectPrey.prey.GetComponent<UtilityStates>().hidden == true)
                {
                    preyDetected = false;
                    detectPrey.prey = null;
                }
            }

            if (currentStates == predStates.Patrol)
            {
                if (requestTarget)
                {
                    int i = 0;

                    i = Random.Range(0, patrolPoints.Count);

                    currentWayPoint = patrolPoints[i].transform;
                    requestTarget = false;
                }

                if (Vector2.Distance(gameObject.transform.position, target.target) <= 1)
                {
                    requestTarget = true;
                }
            }
        }
    }

}
