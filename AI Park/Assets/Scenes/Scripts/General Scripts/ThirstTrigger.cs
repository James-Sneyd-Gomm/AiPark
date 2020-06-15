using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Controller;
using AI.States;

public class ThirstTrigger : MonoBehaviour
{

    public float cooldown;
    public int thirstHeal;
    private int thirstHealInternal;
    private float cooldownInternal;
    bool active;
    bool occupied;
    UtilityStates target;
    Collider2D user;


    void Start()
    {
        active = false;
        cooldownInternal = cooldown;
        thirstHealInternal = thirstHeal;
    }


    private void Update()
    {

        if (active == true)
        {
            cooldownInternal -= Time.deltaTime;

            if (cooldownInternal <= 0)
            {
                setThirst();
                cooldownInternal = cooldown;
            }

        }
        else if (active == false)
        {
            cooldownInternal = cooldown;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("AI"))
        {
            UtilityStates temp = other.GetComponent<UtilityStates>();
            if (!occupied)
            {
                user = other;
                active = true;
                occupied = true;

                temp.oblivious = true;
                temp.NodeOccupied = false;

            }
            else if (occupied)
            {
                temp.NodeOccupied = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("AI"))
        {
            UtilityStates temp = other.GetComponent<UtilityStates>();
            if (other == user)
            {
                user = null;
                active = false;
                occupied = false;
                temp.oblivious = false;
            }
        }
    }



    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag.Contains("AI"))
    //    {
    //        if (occupied)
    //        {
    //            UtilityStates temp = other.GetComponent<UtilityStates>();
    //            temp.NodeOccupied = true;
    //        }
    //    }

    //}

    private void setThirst()
    {
        if (user.GetComponent("UtilityStates") != null)
        {
            Debug.Log("Drinking");
            target = user.GetComponent<UtilityStates>();
            target.healThirst(thirstHealInternal);
        }
    }
}