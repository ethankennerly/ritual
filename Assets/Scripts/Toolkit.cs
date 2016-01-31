using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions
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

	/**
	 * I wish C# API were as simple as JavaScript and Python:
	 * http://stackoverflow.com/questions/1126915/how-do-i-split-a-string-by-a-multi-character-delimiter-in-c
	 */
	public static string[] Split(string text, string delimiter)
	{
		string[] delimiters = new string[] {delimiter};
		string[] parts = text.Split(delimiters, StringSplitOptions.None);
		return parts;
	}
}
