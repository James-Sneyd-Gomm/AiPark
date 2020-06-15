using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public interface IAIController
    {
        Vector3 findCloseWater();
        Vector3 findCloseFood();
        Vector3 findCloseRest();

        //void healHunger();
        //void healThirst();
        //void healEnergy();

    }


