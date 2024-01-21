using BehaviorTree;
public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 4f;
    protected override Node SetupTree()
    {
        Node root = new TaskPatrol(transform, waypoints);

        return root;
    }
}
