using UnityEngine;  // Debug.Log
// using System.IO;  // ReadAllText

public class Toolkit
{
	public static string normalizeLines(string text)
	{
		return text.Replace("\r\n", "\n");
	}

	public static string Read(string path)
	{
		string text = System.IO.File.ReadAllText(path);
		text = normalizeLines(text);
		// Debug.Log("Toolkit.Read: " + text);
		return text;
	}
}
