using CZToolKit.GraphProcessor;
using UnityEngine;

namespace CZToolKit.GOAP.Actions.Movement
{
    [NodeMenuItem("Movement", "Seek")]
    public class Seek : NavMeshMovement
    {
        [Tooltip("The GameObject that the agent is seeking")]
        public GameObject target;
        [Tooltip("If target is null then use the target position")]
        public Vector3 targetPosition;

        public override void OnCreated()
        {
            base.OnCreated();
            SetPrecondition("HasTarget", true);
            SetEffect("InXXXRange", true);
            target = null;
            targetPosition = Vector3.zero;
        }

        public override bool IsUsable()
        {
            return !Agent.GetState("HasTarget");
        }

        public override void OnPrePerform()
        {
            base.OnPrePerform();
            Agent.Memory.TryGetData("Target", out target, null);
            SetDestination(Target());
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override GOAPActionStatus OnPerform()
        {
            if (HasArrived())
                return GOAPActionStatus.Success;
            SetDestination(Target());
            return  GOAPActionStatus.Running;
        }

        // Return targetPosition if target is null
        private Vector3 Target()
        {
            if (target != null)
                return target.transform.position;
            return targetPosition;
        }
    }
}