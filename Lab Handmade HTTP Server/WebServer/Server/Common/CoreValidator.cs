﻿namespace WebServer.Server.Common
{
	using System;
	
    public static class CoreValidator
    {
		public static void ThrowIfNull(object obj, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string text, string name)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"{name} cannot be null or empty.");
            }
        }
    }
}