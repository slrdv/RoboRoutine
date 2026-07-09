namespace RoboRoutine
{
    public interface ILevelManager
    {
        void OnLoadLevelComplete();
        void OnLevelComplete(LevelResult result);
        void LoadLevel(int level);
        void LoadNext();
        void LoadMenu();
    }
}
