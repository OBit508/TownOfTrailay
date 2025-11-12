using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfTrailay.Helpers.Utilities
{
    public class ChangeableValue<T>
    {
        public T Value;
        public ChangeableValue(T value)
        {
            Value = value;
        }
    }
}
