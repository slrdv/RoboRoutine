namespace RoboRoutine
{
    public interface ILevelManager
    {
        void OnLoadLevelComplete();
        void OnLevelComplete(LevelResult result);
        void RunLevel(int level);
        void LoadMenu();
    }
}
