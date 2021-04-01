using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;
    //========
    //spawning
    [Range(10, 500)]
    public int startingCount = 50;
    const float AgentDensity = 1.18f;
    //=========
    [Range(1f, 100f)]
    public float driveFactor = 10f; //multiplicator for slow movement(small adjustment like rotation)
    [Range(1f, 100f)]
    public float maxSpeed = 5f; //max speed the whole group (not individual entity)
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f; //Radius/Range the entity will interact with members of his own group
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f; //multiplicator (with neighborRadius) for avoidance => need smaller radius but still related to the neighborRadius

    //X^2
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier; //avoidance always depends on neighborRadius

        for (int i = 0; i < startingCount; i++)
        {
            //tricks so all prefabs spawn above the terrain
            Vector3 spawnPosition = new Vector3(Random.insideUnitSphere.x * startingCount * AgentDensity, transform.position.y, Random.insideUnitSphere.z * startingCount * AgentDensity);

            //Spawn our agents
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                spawnPosition,
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)), //Carful we use Degree here, but will be in Radians when using ECS
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this); //programm knows which agent belongs to THIS specific flock
            agents.Add(newAgent);
        }
    }

    void Update()
    {
        
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //FOR DEMO ONLY
            //agent.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);
            
            Vector3 move = behavior.CalculateMove(agent, context, this);
            //Debug.Log($"Init move = {move}");
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            move.y = Mathf.Clamp(move.y, 0, 10);
            //Debug.Log($"move = {move}");
            agent.Move(move);
            
        }
        
    }
    /// <summary>
    /// Cast a sphere collider around the agent
    /// return all hit entities'transform(position,rotation,scale)
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
