namespace RoboRoutine
{
    public interface ICommandItemFactory
    {
        CommandItemPresenter Create(CommandItemModel model);
        CommandItemPresenter CreatePaletteItem(CommandItemModel model);
    }
}
