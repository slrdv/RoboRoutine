namespace RoboRoutine
{
    public sealed class CommandItemFactory
    {
        private readonly GameObjectPool<CommandItemView> _pool;

        public CommandItemFactory(GameObjectPool<CommandItemView> pool)
        {
            _pool = pool;
        }

        public CommandItemPresenter Create(CommandItemModel model)
        {
            CommandItemView view = _pool.Get();
            return new CommandItemPresenter(model, view);
        }
    }
}