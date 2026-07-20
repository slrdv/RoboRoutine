using RoboRoutine.Features;

namespace RoboRoutine.UI
{
    public sealed class MoveCommandItemViewSetup : ICommandItemViewSetup
    {
        public CommandType CommandType => CommandType.Move;

        private static float GetRotation(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.Up => 0f,
                MoveDirection.Right => -90f,
                MoveDirection.Down => 180f,
                MoveDirection.Left => 90f,
                _ => 0f
            };
        }

        public void Setup(CommandItemView view, CommandConfig config, CommandData commandData)
        {
            MoveCommandData moveData = (MoveCommandData)commandData;

            view.SetIcon(config.Icon);
            view.SetLabel($"{config.DisplayName} {moveData.Distance.ToString()}");
            view.SetIconRotation(GetRotation(moveData.MoveDirection));
        }
    }
}
