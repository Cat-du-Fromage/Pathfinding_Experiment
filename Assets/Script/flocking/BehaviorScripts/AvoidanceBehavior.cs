using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //add all points together and average (position moyenne)
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        //List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in context/*filteredContext*/)
        {
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius) //apparently sqrMagnitude == magnitude but is faster
            {
                nAvoid++;
                avoidanceMove += (agent.transform.position - item.position);
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;

        return avoidanceMove; 
        // the offset seems to be low, need to do some test with variable "avoidanceRadiusMultiplier" in Flock.cs
    }
}