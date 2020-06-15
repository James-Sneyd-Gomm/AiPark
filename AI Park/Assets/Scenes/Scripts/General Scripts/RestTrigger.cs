using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Controller;
using AI.States;

public class RestTrigger : MonoBehaviour
{

    public float cooldown;
    public int energyHeal;
    private int energyHealInternal;
    private float cooldownInternal;
    bool active;
    bool occupied;
    UtilityStates target;
    Collider2D user;


    void Start()
    {
        active = false;
        cooldownInternal = cooldown;
        energyHealInternal = energyHeal;
    }


    private void Update()
    {

        if (active == true)
        {
            cooldownInternal -= Time.deltaTime;

            if (cooldownInternal <= 0)
            {
                setEnergy();
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

                temp.hidden = true;
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

                other.GetComponent<UtilityStates>().hidden = false;

            }
        }


    }



    private void setEnergy()
    {
        if (user.GetComponent("UtilityStates") != null)
        {
            Debug.Log("Eating");
            target = user.GetComponent<UtilityStates>();
            target.healEnergy(energyHealInternal);
        }
    }


}

