using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSelector : Node
{
    bool shuffled = false;
    public RSelector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        if (!shuffled)
        {
            //Shuffle the list from Utils
            children.Shuffle();
            shuffled = true;
        }
        //Check which child node is Running and Process
        Status childstatus = children[currentChild].Process();

        if (childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.SUCCESS)
        {
            //Reset Sqeuence
            currentChild = 0;
            shuffled = false;
            return Status.SUCCESS;
        }

        currentChild++;

        if (currentChild >= children.Count)
        {
            //Reset Sequence
            currentChild = 0;
            shuffled = false;
            return Status.FAILURE;
        }

        return Status.RUNNING;

    }
}
