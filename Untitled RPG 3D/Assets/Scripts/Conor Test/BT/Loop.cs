using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : Node
{
    BehaviourTree dependancy;
    public Loop(string n, BehaviourTree d)
    {
        name = n;
        dependancy = d;
    }

    public override Status Process()
    {
        //When Dependancy fails it will be Successful to restart Tree, otherwise loop
        if(dependancy.Process() == Status.FAILURE)
        {
            return Status.SUCCESS;
        }

        //Check which child node is Running
        Status childstatus = children[currentChild].Process();

        if (childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.FAILURE)
        {
            return childstatus;
        }

        currentChild++;

        if (currentChild >= children.Count)
        {
            //Reset Sequence
            currentChild = 0;
      
        }



        return Status.RUNNING;

    }

}
