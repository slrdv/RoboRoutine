using R3;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class CommandItemModel
    {
        public ReadOnlyReactiveProperty<int> Index => _index;
        public ReadOnlyReactiveProperty<string> Label => _label;
        public ReadOnlyReactiveProperty<Sprite> Icon => _icon;

        private readonly ReactiveProperty<int> _index = new();
        private readonly ReactiveProperty<string> _label = new();
        private readonly ReactiveProperty<Sprite> _icon = new();

        public void SetIcon(Sprite icon)
        {
            _icon.Value = icon;
        }

        public void SetLabel(string label)
        {
            _label.Value = label;
        }

        public void SetIndex(int index)
        {
            _index.Value = index;
        }
    }
}