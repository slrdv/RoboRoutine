using RoboRoutine.Core;
using UnityEngine;

namespace RoboRoutine.Features
{
    public sealed class RobotMovementOperation : TickOperationBase
    {
        private readonly IRobotView _view;

        private Vector2 _targetPosition;

        public RobotMovementOperation(IRobotView view)
        {
            _view = view;
        }

        public void Setup(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        protected override void OnStart()
        {
            _view.LookAtXZ(_targetPosition);
        }

        protected override void OnTick(float dt)
        {
            _view.MoveTowardsXZ(_targetPosition, dt);

            if (Vector2.Distance(_view.GetPositionXZ(), _targetPosition) <= MathConstants.Epsilon)
            {
                _view.SetPositionXZ(_targetPosition);
                Complete();
            }
        }
    }
}
