using System;
using System.Collections;
using System.Collections.Generic;


namespace Shed.Runtime.Utilities.ConfigManagement
{
    public interface IConfigsLoader
    {
        IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded);
    }
}
