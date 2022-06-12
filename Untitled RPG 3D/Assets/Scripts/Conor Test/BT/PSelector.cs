using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSelector : Node
{
    bool isOrdering = false;
    Node[] nodeArray;
    public PSelector(string n)
    {
        name = n;
    }


    void OrderNodes()
    {
        nodeArray = children.ToArray();
        Sort(nodeArray, 0, children.Count - 1);
        children = new List<Node>(nodeArray);
    }

    public override Status Process()
    {
        if (!isOrdering)
        {
            OrderNodes();
            isOrdering = true;
        }
        //Check which child node is Running
        Status childstatus = children[currentChild].Process();

        if (childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.SUCCESS)
        {
            //Change sort Order
      //      children[currentChild].priortity = 1;
            currentChild = 0;
            isOrdering = false;
            return Status.SUCCESS;
        }
        //else
        //{
         //   children[currentChild].priortity = 10;
        //}

        currentChild++;

        if (currentChild >= children.Count)
        {
            //Reset Sequence
            currentChild = 0;
            isOrdering = false;
            return Status.FAILURE;
        }

        return Status.RUNNING;

    }

    //QuickSort
    int Partition(Node[] array, int low,
                                int high)
    {
        Node pivot = array[high];

        int lowIndex = (low - 1);

        //2. Reorder the collection.
        for (int j = low; j < high; j++)
        {
            if (array[j].priortity <= pivot.priortity)
            {
                lowIndex++;

                Node temp = array[lowIndex];
                array[lowIndex] = array[j];
                array[j] = temp;
            }
        }

        Node temp1 = array[lowIndex + 1];
        array[lowIndex + 1] = array[high];
        array[high] = temp1;

        return lowIndex + 1;
    }

    void Sort(Node[] array, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(array, low, high);
            Sort(array, low, partitionIndex - 1);
            Sort(array, partitionIndex + 1, high);
        }
    }
}

