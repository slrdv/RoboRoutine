using NUnit.Framework;
using RoboRoutine.Features;
using UnityEngine;

namespace RoboRoutine.Tests
{
    public class MoveCommandTests
    {
        [Test]
        public void Execute_NoObstacles_ReturnsSuccess()
        {
            TestWorld world = new TestWorld();
            ICommand command = world.CommandFactoryProvider.Create(new MoveCommandData(MoveDirection.Up, 2));

            CommandResult result = world.RunCommandWithTick(command);

            Assert.That(result, Is.EqualTo(CommandResult.Success));

            Vector2Int cell = world.Grid.WorldToCell(world.Robot.GetPositionXZ());
            Assert.That(cell, Is.EqualTo(new Vector2Int(0, 2)));

            world.Dispose();
        }

        [Test]
        public void Execute_ObstacleInPath_ReturnsFailed()
        {
            TestWorld world = new TestWorld();

            GridEntityModel model = new GridEntityModel(Vector2Int.one, EntityType.Obstacle);
            IGridEntityController obstacle = new GridEntityController<GridEntityModel, IGridEntityView>(model, new TestGridEntityView());
            world.Grid.AddEntity(obstacle, new Vector2Int(0, 2));

            ICommand command = world.CommandFactoryProvider.Create(new MoveCommandData(MoveDirection.Up, 2));

            CommandResult result = world.RunCommandWithTick(command);

            Assert.That(result, Is.EqualTo(CommandResult.Failed));

            Vector2Int cell = world.Grid.WorldToCell(world.Robot.GetPositionXZ());
            Assert.That(cell, Is.EqualTo(new Vector2Int(0, 1)));

            world.Dispose();
        }
    }
}
