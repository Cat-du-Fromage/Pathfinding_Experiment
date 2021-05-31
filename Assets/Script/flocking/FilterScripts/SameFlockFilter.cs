using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original)
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>(); //get the actual flock the agent belongs to
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) //did the flock == the flock of the agent we refer to?
            {
                filtered.Add(item); // if yes we add to the list of flock(transform)
            }
        }
        return filtered;
    }
}
