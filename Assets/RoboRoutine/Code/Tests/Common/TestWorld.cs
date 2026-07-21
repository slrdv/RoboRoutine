using System;
using Cysharp.Threading.Tasks;
using RoboRoutine.Core;
using RoboRoutine.Features;
using UnityEngine;

namespace RoboRoutine.Tests
{
    public sealed class TestWorld : IDisposable
    {
        public ICommandFactoryProvider CommandFactoryProvider { get; private set; }
        public TickService TickService { get; private set; }
        public SnapshotService SnapshotService { get; private set; }
        public CommandSequence Sequence { get; private set; }
        public GridController Grid { get; private set; }
        public RobotController Robot { get; private set; }

        public TestWorld()
        {
            SnapshotService = new SnapshotService();
            TickService = new TickService();
            Sequence = new CommandSequence();

            TestRobotView robotView = new TestRobotView();
            Robot = new RobotController(robotView, TickService, SnapshotService);

            TestGridView gridView = new TestGridView();
            GridModel gridModel = new GridModel();
            Grid = new GridController(gridModel, gridView, SnapshotService);

            CreateFactories();

            SetRobotPositionAtCell(Vector2Int.zero);
        }

        public void Tick(float dt)
        {
            TickService.Tick(dt);
        }

        public void SetRobotPositionAtCell(Vector2Int cell)
        {
            Robot.View.SetPositionXZ(Grid.GetCellCenterWorldPositionXZ(cell));
        }

        public CommandResult RunCommandWithTick(ICommand command, float dt = 0.1f, int maxTicks = 1000)
        {
            UniTask<CommandResult> task = command.ExecuteAsync();

            int count = 0;
            while (task.Status == UniTaskStatus.Pending)
            {
                Tick(dt);

                if (++count >= maxTicks)
                {
                    throw new TimeoutException("Command not complete");
                }
            }

            return task.GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            SnapshotService.Dispose();
            Sequence.Dispose();
            Grid.Dispose();
            Robot.Dispose();
        }

        private void CreateFactories()
        {
            ICommandFactory[] commandFactories = new ICommandFactory[]
            {
                new MoveCommandFactory(new MovementSystem(Robot, Grid)),
                new PickCommandFactory(new PickSystem(Robot, Grid)),
                new PutCommandFactory(new PutSystem(Robot, Grid, new TestLevelCompleteListener())),
                new EvalCommandFactory(new EvalSystem(Robot, Grid)),
                new ConditionCommandFactory(new ConditionCommandSystem(Robot, Sequence)),
            };

            CommandFactoryProvider = new CommandFactoryProvider(commandFactories);
        }
    }
}
