using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{

    public Selector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        //Check which child node is Running
        Status childstatus = children[currentChild].Process();

        if (childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;

        if (currentChild >= children.Count)
        {
            //Reset Sequence
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;

    }
}
