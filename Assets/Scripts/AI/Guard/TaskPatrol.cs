using UnityEngine;

namespace BehaviorTree
{
    public class TaskPatrol : Node
    {
        private Transform _transform;
        private Transform[] _waypoints;
        private int _currentWaypointIndex = 0;
        private float _waitTime = 1f;
        private float _waitCounter = 0f;
        private bool _waiting = false;
        private Animator _animator;

        public TaskPatrol(Transform transform, Transform[] waypoints)
        {
            _transform = transform;
            _waypoints = waypoints;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                    _waiting = false;
                _animator.SetBool("Walking", true);
            }
            else
            {
                Transform wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
                {
                    _transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                    _animator.SetBool("Walking", false);
                }
                else
                {
                    Vector3 direction = wp.position - _transform.position;
                    direction.y = 0f; 

                    _transform.rotation = Quaternion.LookRotation(direction);

                    _transform.position = Vector3.MoveTowards(_transform.position, wp.position, GuardBT.speed * Time.deltaTime);
                }
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
