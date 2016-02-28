using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions

/**
 * Bridge between portable game and platform-specific filesystem, game engine or toolkit.
 * Animation, sound, UI, or other view are in ViewUtils instead.
 */
public class Toolkit
{
	public static string lineDelimiter = "\n";

	public static void Log(string message)
	{
		Debug.Log(message);
	}

	public static int ParseIndex(string tileName)
	{
		int tileIndex = int.Parse(tileName.Split('_')[1]);
		return tileIndex;
	}

	private static string NormalizeLines(string text)
	{
		return text.Replace("\r\n", "\n");
	}

	/**
	 * @param	path	Unconventionally, Unity expects the file extension is omitted.  This utility will try again to remove file extension if it can't load the first time.
	 * Normalize line endings and trim whitespace.
	 * Expects path is relative to "Assets/Resources/" folder.
	 * Unity automatically embeds resource files.  Does not dynamically load file, because file system is incompatible on mobile device or HTML5.
	 */
	public static string Read(string path)
	{
		TextAsset asset = (TextAsset) Resources.Load(path);
		if (null == asset) {
			string basename = System.IO.Path.ChangeExtension(path, null);
			asset = (TextAsset) Resources.Load(basename);
			if (null == asset) {
				Debug.Log("Did you omit the file extension?  Did you place the file in the Assets/Resources/ folder?  Path was " + path + " and without extension was " + basename);
			}
		}
		string text = asset.text;
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
	 * Portable wrapper for languages without ToCharArray
	 */
	public static char[] SplitLetters(string word)
	{
		return word.ToCharArray();
	}

	/**
	 * This was the most concise way I found to split a string without depending on other libraries.
	 * In ActionScript splitting a string is concise:  s.split("");
	 */
	public static string[] SplitString(string text)
	{
		char [] letters = text.ToCharArray();
		string[] available = new string[letters.Length];
		for (int i = 0; i < letters.Length; i++) {
			available[i] = letters[i].ToString();
		}
		return available;
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
