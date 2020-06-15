using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Pathfinding;
using AI.States;



namespace AI.Controller
{
    public class AIController : MonoBehaviour
    {

        protected List<Vector2> waterLoc;
        protected List<Vector2> foodLoc;
        protected List<Vector2> restLoc;

        public delegate void States(GameObject sender);
        public static event States Death;


        AIDestinationSetter target;
        UtilityStates utilityStates;

        public List<Keywords> Keyword;
        protected Keywords FoodKeyword;

        void Start()
        {
            SetUpKeywords();
            FindBestFoodKeyword();
            waterLoc = new List<Vector2>();
            foodLoc = new List<Vector2>();
            restLoc = new List<Vector2>();
            target = GetComponent<AIDestinationSetter>();
            utilityStates = GetComponent<UtilityStates>();
            AddWater();
            AddFood();
            AddRest();
        }

        void Update()
        {
            GetCurrentAIState();

            if (utilityStates.dead)
            {
                Call_Death(gameObject);
                gameObject.SetActive(false);
            }
        }


        #region locations

        void AddWater()
        {

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Water"))
            {
                if (obj.name == "Water Source")
                {
                    waterLoc.Add(obj.transform.position);
                }
            }

        }

        void AddFood()
        {
            List<GameObject> foodObjects = new List<GameObject>();

            switch(FoodKeyword)
            {
                case Keywords.None:
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
                    {
                        if (obj.name == "Food")
                        {
                            foodObjects.Add(obj);
                        }
                    }
                
                foreach (GameObject food in foodObjects)
                    {
                        foodLoc.Add(food.transform.position);
                    }

                    break;

                case Keywords.HardClaws:

                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
                    {
                        if (obj.name == "Dig Food")
                        {
                            foodObjects.Add(obj);
                        }
                    }

                    foreach (GameObject food in foodObjects)
                    {
                        foodLoc.Add(food.transform.position);
                    }

                    break;

                case Keywords.Toxic:
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
                    {
                        if (obj.name == "Poison Food")
                        {
                            foodObjects.Add(obj);
                        }
                    }

                    foreach (GameObject food in foodObjects)
                    {
                        foodLoc.Add(food.transform.position);
                    }
                    break;

                case Keywords.Aquatic:
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
                    {
                        if (obj.name == "Water Food")
                        {
                            foodObjects.Add(obj);
                        }
                    }

                    foreach (GameObject food in foodObjects)
                    {
                        foodLoc.Add(food.transform.position);
                    }
                    break;

                case Keywords.HardHide:
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
                    {
                        if (obj.name == "Spikey Food")
                        {
                            foodObjects.Add(obj);
                        }
                    }

                    foreach (GameObject food in foodObjects)
                    {
                        foodLoc.Add(food.transform.position);
                    }
                    break;

            }
        }

        void AddRest()
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Nest"))
            {
                if (obj.name == "Nest")
                {
                    restLoc.Add(obj.transform.position);
                }
            }
        }

        #endregion

        #region FindLocation

        public Vector3 findCloseWater()
        {
            Transform currentLoc;
            Vector3 tempLoc;
            Vector3 bestLoc = new Vector3(5000.0f, 5000.0f, 0.0f);
            Vector3 secondBestLoc = new Vector3(5000.0f, 5000.0f, 0.0f);
            Vector3 FinalLoc = new Vector3(0.0f, 0.0f, 0.0f);
            float disOne, disTwo;
            int i;

            currentLoc = gameObject.transform;

            for (i = 0; i < waterLoc.Count; i++)
            {
                tempLoc = waterLoc[i];

                disOne = Vector3.Distance(currentLoc.position, tempLoc);
                disTwo = Vector3.Distance(currentLoc.position, bestLoc);

                if (disOne < disTwo)
                {
                    bestLoc = tempLoc;
                }
                else
                {
                    disOne = Vector3.Distance(currentLoc.position, tempLoc);
                    disTwo = Vector3.Distance(currentLoc.position, secondBestLoc);

                    if (disOne < disTwo)
                    {
                        secondBestLoc = tempLoc;
                    }
                }

            }

            if (utilityStates.NodeOccupied == false)
            {
                FinalLoc = bestLoc;
            }
            else if (utilityStates.NodeOccupied == true)
            {
                FinalLoc = secondBestLoc;
            }

            return FinalLoc;

        }

        public Vector3 findCloseFood()
        {
            Transform currentLoc;
            Vector3 tempLoc;
            Vector3 bestLoc = new Vector3(5000.0f, 5000.0f, 0.0f);
            Vector3 secondBestLoc = new Vector3(5000.0f, 5000.0f, 0.0f);
            Vector3 FinalLoc = new Vector3(0.0f, 0.0f, 0.0f);
            float disOne, disTwo;
            int i;

            currentLoc = gameObject.transform;

            for (i = 0; i < foodLoc.Count; i++)
            {
                tempLoc = foodLoc[i];

                disOne = Vector3.Distance(currentLoc.position, tempLoc);
                disTwo = Vector3.Distance(currentLoc.position, bestLoc);

                if (disOne < disTwo)
                {
                    bestLoc = tempLoc;
                }
                else
                {
                    disOne = Vector3.Distance(currentLoc.position, tempLoc);
                    disTwo = Vector3.Distance(currentLoc.position, secondBestLoc);

                    if (disOne < disTwo)
                    {
                        secondBestLoc = tempLoc;
                    }
                }

            }

            if (utilityStates.NodeOccupied == false)
            {
                FinalLoc = bestLoc;
            }
            else if (utilityStates.NodeOccupied == true)
            {
                FinalLoc = secondBestLoc;
            }

            return FinalLoc;
        }

        public Vector3 findCloseRest()
        {
            Transform currentLoc;
            Vector3 tempLoc;
            Vector3 bestLoc = new Vector3(5000.0f, 5000.0f, 0.0f);
            Vector3 secondBestLoc = new Vector3(5000.0f, 5000.0f, 0.0f);
            Vector3 FinalLoc = new Vector3(0.0f, 0.0f, 0.0f);
            float disOne, disTwo;
            int i;

            currentLoc = gameObject.transform;

            for (i = 0; i < restLoc.Count; i++)
            {
                tempLoc = restLoc[i];

                disOne = Vector3.Distance(currentLoc.position, tempLoc);
                disTwo = Vector3.Distance(currentLoc.position, bestLoc);

                disOne = Vector3.Distance(currentLoc.position, tempLoc);
                disTwo = Vector3.Distance(currentLoc.position, bestLoc);

                if (disOne < disTwo)
                {
                    bestLoc = tempLoc;
                }
                else
                {
                    disOne = Vector3.Distance(currentLoc.position, tempLoc);
                    disTwo = Vector3.Distance(currentLoc.position, secondBestLoc);

                    if (disOne < disTwo)
                    {
                        secondBestLoc = tempLoc;
                    }
                }

            }

            if (utilityStates.NodeOccupied == false)
            {
                FinalLoc = bestLoc;
            }
            else if (utilityStates.NodeOccupied == true)
            {
                FinalLoc = secondBestLoc;
            }

            return FinalLoc;
        }

        #endregion


        private void GetCurrentAIState()
        {

            switch (utilityStates.currentActiveState)
            {
                case states.Hungry:
                    target.target = findCloseFood();
                    break;
                case states.Thirsty:
                    target.target = findCloseWater();
                    break;
                case states.Tried:
                    target.target = findCloseRest();
                    break;
                case states.Scared:
                    target.target = findCloseRest();
                    break;
            }
        }



        private void SetUpKeywords()
        {
            int keywordAmount = 0;
            keywordAmount = Random.Range(0, 2);
            int i;
           

            for (i = 0; i < keywordAmount; i++ )
            {
                int temp = 0;
                temp = Random.Range(1, 4);
                Keyword.Add((Keywords)temp);
            }
        }



        private void FindBestFoodKeyword()
        {
            float temp = 0;
            Keywords tempKeyword;

            switch(Keyword.Count)
            {
                case 0:
                    FoodKeyword = Keywords.None;
                    break;

                case 1:
                    FoodKeyword = Keyword[0];
                    break;
                case 2:
                    tempKeyword = Keyword[0];
                    temp = ReturnFoodValue(Keyword[0]);

                    if (temp < ReturnFoodValue(Keyword[1]))
                    {
                        tempKeyword = Keyword[1];
                    }

                    FoodKeyword = tempKeyword;
                    break;

            }
        }



        private float ReturnFoodValue(Keywords keyword)
        {
            float FoodValueReturn = 0;


            switch (keyword)
            {
                case Keywords.None:
                    FoodValueReturn = Resources.Load<GameObject>("Food").GetComponent<HungerTrigger>().hungerHeal;
                    break;
                case Keywords.HardClaws:
                    FoodValueReturn = Resources.Load<GameObject>("Dig Food").GetComponent<HungerTrigger>().hungerHeal;
                    break;
                case Keywords.Toxic:
                    FoodValueReturn = Resources.Load<GameObject>("Poison Food").GetComponent<HungerTrigger>().hungerHeal;
                    break;
                case Keywords.Aquatic:
                    FoodValueReturn = Resources.Load<GameObject>("Water Food").GetComponent<HungerTrigger>().hungerHeal;
                    break;
                case Keywords.HardHide:
                    FoodValueReturn = Resources.Load<GameObject>("Spikey Food").GetComponent<HungerTrigger>().hungerHeal;
                    break;
            }   


            return FoodValueReturn;
        }



        public static void Call_Death(GameObject sender)
        {
            if (Death != null)
            {
                Death(sender);
            }
        }


      

    }
}

