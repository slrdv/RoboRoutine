using System;
using System.Collections.Generic;

namespace RoboRoutine
{
    [Serializable]
    public sealed class SequenceData
    {
        public List<CommandData> Commands;
    }
}