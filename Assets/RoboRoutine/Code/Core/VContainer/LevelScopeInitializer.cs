using VContainer.Unity;

namespace RoboRoutine
{
    public sealed class LevelScopeInitializer : IInitializable
    {
        private readonly GridBuilder _gridBuilder;

        public LevelScopeInitializer(GridBuilder gridBuilder)
        {
            _gridBuilder = gridBuilder;
        }

        public void Initialize()
        {
            _gridBuilder.Build();
        }
    }
}