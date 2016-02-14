using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions

/**
 * Bridge between portable game and platform-specific filesystem, game engine or toolkit.
 * Animation, sound, UI, or other view are in ViewUtils instead.
 */
public class Toolkit
{
	public static string lineDelimiter = "\n";

	public static int ParseIndex(string tileName)
	{
		int tileIndex = int.Parse(tileName.Split('_')[1]);
		return tileIndex;
	}

	private static string NormalizeLines(string text)
	{
		return text.Replace("\r\n", "\n");
	}

	public static string Read(string path)
	{
		string text = System.IO.File.ReadAllText(path);
		text = NormalizeLines(text);
		text = text.Trim();
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

	/**
	 * Trim whitespace.  
	 * Test case:  Expect 5 rows.  Got 6.  Last row is empty, from final line delimiter at the end of the file's text.
	 *
	 * Would be nice when there's more time to generate hashes.
	 */
	public static string[][] ParseCsv(string text, string fieldDelimiter = ",")
	{
		text = text.Trim();
		string[] lines = Toolkit.Split(text, lineDelimiter);
		string[][] table = new string[lines.Length][];
		for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
		{
			string line = lines[lineIndex];
			string[] row = Toolkit.Split(line, fieldDelimiter);
			table[lineIndex] = row;
		}
		// Debug.Log("Toolkit.ParseCsv: lines " + table.Length);
		return table;
	}
}
