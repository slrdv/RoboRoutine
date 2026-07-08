namespace RoboRoutine
{
    public interface ICommandSequence
    {
        void Add(ICommand command);
        void Insert(int index, ICommand command);
        void RemoveAt(int index);
        void Move(int fromIndex, int toIndex);
        void SetIndex(int index);
        void Clear();
        void ExecuteNext();
        void RequestCancel();
        void SetIndexAfterExecution(int index);
    }
}
