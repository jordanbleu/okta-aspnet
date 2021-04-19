using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public interface IIdxContextProvider
    {
        object Get(string key, object defaultValue = null);

        T Get<T>(string key, T defaultValue = default);

        void Set(string key, object value);
    }
}
