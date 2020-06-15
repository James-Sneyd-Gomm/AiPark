using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Controller;
using AI.States;

public class HungerTrigger : MonoBehaviour
{

    public float cooldown;
    public int hungerHeal;
    private int hungerHealInternal;
    private float cooldownInternal;
    bool active;
    bool occupied;
    IUtilityStates target;
    Collider2D user;


   void Start()
    {
        active = false;
        cooldownInternal = cooldown;
        hungerHealInternal = hungerHeal;
    }


    private void Update()
    {
        
        if (active == true)
        {
            cooldownInternal -= Time.deltaTime;

            if (cooldownInternal <= 0)
            {
                setHunger();
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
            if (occupied == false)
            {
                user = other;
                active = true;
                occupied = true;

                temp.oblivious = true;
                temp.NodeOccupied = false;
            }
            else if (occupied == true)
            {
              
                temp.NodeOccupied = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("AI"))
        {
            if (other == user)
            {
                user = null;
                active = false;
                occupied = false;

                other.GetComponent<UtilityStates>().oblivious = false;
            }
        }


    }



    private void setHunger()
    {
        if (user.GetComponent<IUtilityStates>() != null)
        {
            Debug.Log("Eating");
            target = user.GetComponent<IUtilityStates>();
            target.healHunger(hungerHealInternal);
        }
    }


 }

