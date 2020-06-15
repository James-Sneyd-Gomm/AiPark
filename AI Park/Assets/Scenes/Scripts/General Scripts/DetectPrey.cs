using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.States;
public class DetectPrey: MonoBehaviour
{
    List<GameObject> preyInRange;
    GameObject tempObj;


    protected GameObject _prey;
    public GameObject prey { set { _prey = value; } get { return _prey; } }

    private void Start()
    {
        preyInRange = new List<GameObject>();
        
    }

    public void ScanPrey()
    {
        preyInRange.Clear();

        foreach (Collider2D prey in Physics2D.OverlapCircleAll(gameObject.transform.position, 7.0f))
        {
            if (prey.gameObject.tag.Contains("AI"))
            {
                if (!prey.GetComponent<UtilityStates>().hidden)
                {
                    preyInRange.Add(prey.gameObject);
                }
             
            }
        }

        if (preyInRange.Count >= 1)
        {
            foreach(GameObject obj in preyInRange)
            {
                if(tempObj == null)
                {
                    tempObj = obj;
                }
                else
                {
                    if ( Vector2.Distance(gameObject.transform.position, tempObj.transform.position) > Vector2.Distance(gameObject.transform.position, obj.transform.position))
                    {
                        tempObj = obj;
                    }
                }
            }
            _prey = tempObj;
            tempObj = null;
        }
    }
    
    public predStates changeState()
    {
        if (_prey != null)
        {
            return predStates.Hunt;
        }
        else
        {
            return predStates.Patrol;
        }
    }

}
