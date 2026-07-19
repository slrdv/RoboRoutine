# RoboRoutine

RoboRoutine is a Unity-based grid puzzle game where the player builds a visual sequence of commands to control a robot that must pick up and combine numeric operands to match target slots on a grid. The primary goal is to demonstrate clean, scalable game architecture and the use of software design patterns in a real Unity project, rather than to showcase gameplay or content depth.

<br>

## Technologies

- UniTask - async/await flow for animations, state transitions, and command execution
- VContainer - the project's dependency injection container, used as composition root for the entire architecture.
- R3 - reactive properties for UI binding
- Newtonsoft.Json - serialization of command sequences for save/load

## Architectural patterns

- Dependency Injection - all services, systems and presenters are registered and resolved via VContainer scopes.
- FSM - drives application flow.
- Command pattern - the core gameplay mechanic of the game: the player visually programs the robot's behavior by building a sequence of commands (e.g. Move, Pick, Put, Eval, Condition), each an executable command object built from serializable data.
- Factory pattern - supports the Open/Closed Principle: creates commands and grid entities through a common interface, new command or grid entity types can be added by registering a new factory implementation, without modifying existing code.
- Repository pattern - loads and looks up ScriptableObject configs.
- Object Pool - reuses UI views instead of instantiating them.
- Memento / Snapshot pattern - captures/restores state of grid, robot, and operands per layer to implement Undo/Rollback.
- MVC - structures core gameplay entities
- MVP - structures UI features
- Strategy pattern - helps configure a command view in the list/palette and how its edit panel is built.
- Observer pattern - C# events (and R3 reactive properties) are used for communication between entities.

## Examples

### Adding a new Command

1. Add a new value to `CommandType` enum

```csharp
    public enum CommandType
    {
        None = 0,
        Move = 1,
        ...
    }
```

2. Inherit or use base `CommandData` (implement `ITargetIndexCommandData` if it needs to jump to another index in command sequence like `ConditionCommandData`).

```csharp
    [Serializable]
    public sealed class MoveCommandData : CommandData
    {
        public MoveDirection MoveDirection { get; private set; }
        public int Distance { get; private set; }

        [JsonConstructor]
        public MoveCommandData([JsonProperty("MoveDirection")] MoveDirection direction, [JsonProperty("Distance")] int distance) : base(CommandType.Move)
        {
            MoveDirection = direction;
            Distance = distance;
        }

        public override CommandData Clone()
        {
            return new MoveCommandData(MoveDirection, Distance);
        }
    }
```

3. Create command class with specific CommandType and SnapshotLayer (which state layers this command affects - Robot, Grid, Operand, Slot or None)

```csharp
    public sealed class MoveCommand : ICommand, IDisposable
    {
        public CommandType CommandType => CommandType.Move;
        public SnapshotLayer SnapshotLayer => SnapshotLayer.Robot;

        ...

        public async UniTask<CommandResult> ExecuteAsync()
        {
            ...
            CommandResult result = await _movementSystem.MoveAsync(_direction, _distance);
            ...
        }

        public void Cancel()
        {
            _movementSystem.Stop();
        }
    }
```

4. Create a system containing the actual gameplay logic used by the command.

```csharp
    public sealed class MovementSystem
    {
        public async UniTask<CommandResult> MoveAsync(MoveDirection moveDirection, int cellDistance)
        {
            // gameplay logic
        }

        public void Stop()
        {
            _robot.StopCurrentOperation();
        }
    }
```

5. Create command factory.

```csharp
    public sealed class MoveCommandFactory : ICommandFactory
    {
        public CommandType CommandType => CommandType.Move;

        ...

        public ICommand Create(CommandData commandData)
        {
            MoveCommandData data = (MoveCommandData)commandData;
            return new MoveCommand(_movementSystem, data.MoveDirection, data.Distance);
        }

        public CommandData CreateDefaultData()
        {
            return new MoveCommandData(MoveDirection.Up, 1);
        }
    }
```

6. Create specific `CommandItemViewSetup` (or inherit `DefaultCommandItemViewSetup` if no custom setup is needed) to control how the command looks in the palette or list.

```csharp
    public sealed class MoveCommandItemViewSetup : ICommandItemViewSetup
    {
        public CommandType CommandType => CommandType.Move;

        public void Setup(CommandItemView view, CommandConfig config, CommandData commandData)
        {
            MoveCommandData moveData = (MoveCommandData)commandData;
            view.SetIcon(config.Icon);
            ...
        }
    }
```

7. Create specific `CommandEditViewSetup` (or inherit `DefaultEditViewSetup` if it has no parameters) to build the edit-panel inputs and get input values back to create new `CommandData`.

```csharp
    public sealed class MoveCommandEditViewSetup : ICommandEditViewSetup
    {
        public CommandType CommandType => CommandType.Move;

        private LabeledDropdown _dropdown;
        private LabeledNumberSelector _numberSelector;

        public CommandData Apply()
        {
            return new MoveCommandData(EnumUtils.GetMember<MoveDirection>(_dropdown.GetCurrentName()), _numberSelector.Input.Value);
        }

        public void Setup(CommandEditPanelView view, CommandData data)
        {
            MoveCommandData moveData = (MoveCommandData)data;

            _dropdown = view.AddDropdown("Direction", EnumUtils.GetNames<MoveDirection>());
            _dropdown.SetSelected(moveData.MoveDirection.ToString());

            _numberSelector = view.AddNumberSelector("Distance", 1, 10);
            _numberSelector.Input.SetValue(moveData.Distance);
        }
    }
```

8. Create a `CommandConfig` (or a custom subclass like `ConditionCommandConfig`) ScriptableObject asset, set its Type, DisplayName, Icon.

9. DI registration (`LevelLifetimeScope`)

```csharp
    private void RegisterCommandSpecific(IContainerBuilder builder)
    {
        builder.Register<MoveCommandFactory>(Lifetime.Singleton).As<ICommandFactory>();
        builder.Register<MovementSystem>(Lifetime.Singleton);
        builder.Register<MoveCommandItemViewSetup>(Lifetime.Singleton).As<ICommandItemViewSetup>();
        builder.Register<MoveCommandEditViewSetup>(Lifetime.Singleton).As<ICommandEditViewSetup>();

        ...
    }
```

### Adding a new Grid Entity

1. Add a new value to `EntityType` enum.

```csharp
public enum EntityType
{
    None = 0,
    Obstacle = 1,
    Operand = 2,
    ...
}
```

2. Inherit `GridEntityAuthoring` if the entity needs extra scene-editable data or use base class.

```csharp
public class NumericEntityAuthoring : GridEntityAuthoring
{
    [SerializeField] private int _value;

    public int Value => _value;
}
```

3. Inherit `GridEntityModel` if the entity needs extra runtime data or use base class.

```csharp
public class NumericEntityModel : GridEntityModel
{
    private int _value;

    public int Value => _value;

    public NumericEntityModel(Vector2Int size, EntityType entityType, int value) : base(size, entityType)
    {
        _value = value;
    }

    public void SetValue(int value)
    {
        _value = value;
    }
}
```

4. Inherit `GridEntityView` if the entity needs custom visuals or use base class.

```csharp
public class NumericEntityView : GridEntityView
{
    [SerializeField] TMP_Text _label;

    public void SetLabel(int value)
    {
        _label.text = value.ToString();
    }
}
```

5. Create controller by inheriting `GridEntityController<TModel, TView>`. Implement `ISnapshotable` and `IDisposable` if the entity participates in Undo/Rollback and needs registry cleanup.

```csharp
public sealed class OperandEntityController : GridEntityController<NumericEntityModel, NumericEntityView>, ISnapshotable, IDisposable
{
    ...

    public SnapshotLayer SnapshotLayer => SnapshotLayer.Operand;

    public OperandEntityController(NumericEntityModel model, NumericEntityView view, ISnapshotableRegistry snapshotableRegistry) : base(model, view)
    {
        ...

        _snapshotableRegistry.Register(this);
    }

    public object CaptureState()
    {
        return new OperandHistoryState { Value = _model.Value };
    }

    public void RestoreState(object state)
    {
        if (state is not OperandHistoryState operandState) throw new ArgumentException($"Invalid history state type: {state.GetType().Name}");
        SetValue(operandState.Value);
    }

    public void Dispose()
    {
        _snapshotableRegistry.Remove(this);
    }
}
```

6. (Optional) Create a matching history state class for capture and restore state.

```csharp
public sealed class OperandHistoryState
{
    public int Value;
}
```

7. Create the entity factory by inheriting `GridEntityFactoryBase<TModel, TView, TAuthoring>`.

```csharp
public sealed class OperandEntityFactory : GridEntityFactoryBase<NumericEntityModel, NumericEntityView, NumericEntityAuthoring>
{
    ...

    public override EntityType EntityType => EntityType.Operand;

    public OperandEntityFactory(ISnapshotableRegistry snapshotableRegistry)
    {
        ...
    }

    public override IGridEntityController CreateController(NumericEntityAuthoring authoring, NumericEntityView view)
    {
        NumericEntityModel model = new NumericEntityModel(authoring.GetActualSize(), authoring.EntityType, authoring.Value);
        return new OperandEntityController(model, view, _snapshotableRegistry);
    }
}
```

8. DI registration (`LevelLifetimeScope`)

```csharp
private void RegisterGrid(IContainerBuilder builder)
{
    ...
    builder.Register<OperandEntityFactory>(Lifetime.Singleton).As<IGridEntityFactory>();
    ...
}
```

9. Place the Authoring component with the matching View as a child component on a scene object or prefab and set its EntityType. `GridBuilder` automatically discovers all `GridEntityAuthoring` components and creates the corresponding controllers through `GridEntityFactoryProvider`.