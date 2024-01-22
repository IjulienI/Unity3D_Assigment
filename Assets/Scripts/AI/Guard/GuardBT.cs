using System.Collections.Generic;
using BehaviorTree;
public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 1f;
    public static float fovRange = 6f;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInFOVRange(transform),
                new TaskGoToTarget(transform),
            }),
            new TaskPatrol(transform, waypoints),
        });

        return root;
    }
}
