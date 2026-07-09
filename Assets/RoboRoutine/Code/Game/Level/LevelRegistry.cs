namespace RoboRoutine
{
    public sealed class LevelRegistry
    {
        private readonly IRepository<int, LevelConfig> _repository;

        public LevelRegistry(IRepository<int, LevelConfig> repository)
        {
            _repository = repository;
        }

        public bool HasNextLevel(int level)
        {
            return _repository.Contains(level + 1);
        }

        public LevelConfig Get(int level)
        {
            return _repository.Get(level);
        }

        public LevelConfig GetNext(int level)
        {
            return Get(level + 1);
        }
    }
}
