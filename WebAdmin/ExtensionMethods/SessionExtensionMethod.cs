﻿using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Student_WebAdmin.ExtensionMethods
{
    public static class SessionExtensionMethod
    {
        //save infor user loggin
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
