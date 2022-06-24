using System;
using System.Diagnostics;
using UnityEngine;

namespace TBox
{

    public static class Logger
    {

        /// <summary> Logs a message to the console. </summary>
        public static void Log(object message, string owner = null)
        {
            var caller = owner ?? GetCaller();
            UnityEngine.Debug.Log($"<color=#{Color(caller)}>[{caller}]</color> {message}");
        }

        /// <summary> Logs a warn to the console. </summary>
        public static void LogWarn(object message, string owner = null)
        {
            var caller = owner ?? GetCaller();
            UnityEngine.Debug.LogWarning($"<color=#{Color(caller)}>[{caller}]</color> {message}");
        }

        /// <summary> Logs an error to the console. </summary>
        public static void LogError(object message, string owner = null, UnityEngine.Object obj = null)
        {
            var caller = owner ?? GetCaller();
            UnityEngine.Debug.LogError($"<color=#{Color(caller)}>[{caller}]</color> {message}", obj);
        }

        /// <summary> Returns the color of the string asociated with the given owner. </summary>
        /// <param name="owner"> The owner of the log. </param>
        private static string Color(string value) => value switch
        {
            "GSO" => "c087fa",
            "Game" => "fcbf5b",
            "Map" => "85b4ff",
            _ => "FFFFFF",
        };

        /// <summary> Gets the name of the Class that called the log. </summary>
        private static string GetCaller()
        {
            if (!Application.isEditor)
                return "";

            string fullName;
            Type declaringType;
            int skipFrames = 2;

            do
            {
                var method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;

                if (declaringType == null)
                    return method.Name;

                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            string[] split;
            string name;

            split = fullName.Split('+');
            name = split[0];

            split = name.Split('.');
            name = split[split.Length - 1];

            return name;
        }

    }

}
