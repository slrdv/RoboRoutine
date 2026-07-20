using System;
using System.Collections.Generic;

namespace RoboRoutine.Features
{
    [Serializable]
    public sealed class SequenceData
    {
        public List<CommandData> Commands = new();
    }
}
