using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class CohesionBehavior : FlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //add all points together and average (position moyenne)
        Vector3 cohesionMove = Vector3.zero;
        //List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in context/*filteredContext*/)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count; // get the average position (position moyenne)

        //create offset from agent position
        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }
}
