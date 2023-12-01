using System.IO;
using UnityEditor;
using UnityEngine;

public class GameUtils
{
    public static void FileWrite(string path, string content)
    {
#if UNITY_EDITOR
        File.WriteAllText(path, content);
#endif
    }

    public static string FileRead(string path)
    {
        string readAllText = "";

#if UNITY_EDITOR
        readAllText = File.ReadAllText(path);
        return readAllText;
#else
        return readAllText;
#endif

    }
}