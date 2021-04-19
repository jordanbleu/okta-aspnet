// <copyright file="InMemoryIdxContextProvider.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace Okta.AspNetCore
{
    public class InMemoryIdxContextProvider
    : IIdxContextProvider
    {
        private static Dictionary<string, object> sessionState;

        static InMemoryIdxContextProvider()
        {
            sessionState = new Dictionary<string, object>();
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            return (T)Get(key, (object)defaultValue);
        }

        public object Get(string key, object defaulValue = null)
        {
            if (sessionState.ContainsKey(key))
            {
                return sessionState[key];
            }

            return defaulValue;
        }

        public void Set(string key, object value)
        {
            if (sessionState.ContainsKey(key))
            {
                sessionState[key] = value;
            }
            else
            {
                sessionState.Add(key, value);
            }
        }
    }
}
