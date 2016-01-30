using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions

public class Model
{
	public int columnCountMax = 3;
	public string invisible = ".";
	public int rowCountMax = 5;
	public int tileCountMax = 15;
	public string[] tileLetters;

	// I wish the API were as simple as JavaScript and Python:
	// http://stackoverflow.com/questions/1126915/how-do-i-split-a-string-by-a-multi-character-delimiter-in-c
	private string[] gridDelimiter = new string[] {"\n\n"};
	private string[] lineDelimiter = new string[] {"\n"};
	public string gridText = Toolkit.Read("Assets/Data/grids.txt");
	/**
	 * grid:
	 *	line
	 */
	public string[][] grids;
	public string[] grid;
	public int gridIndex = -1;

	/**
	 * Each line is a string, which may be accessed by index.
	 * Allows jagged arrays.
	 */
	public string[][] ParseGrids(string gridText)
	{
		string[] gridStrings = gridText.Split(gridDelimiter, StringSplitOptions.None);
		string[][] grids = new string[gridStrings.Length][];
		for (int i = 0; i < gridStrings.Length; i++)
		{
			string text = gridStrings[i];
			string[] lines = text.Split(lineDelimiter, StringSplitOptions.None);
			grids[i] = lines;
			Debug.Log("Model.ParseGrids: " + String.Join(lineDelimiter[0], grids[i]));
		}
		return grids;
	}

	public void Start()
	{
		if (null == grids)
		{
			grids = ParseGrids(gridText);
		}
		PopulateGrid(0);
	}

	public void PopulateGrid(int newGridIndex)
	{
		gridIndex = newGridIndex;
		grid = grids[gridIndex];
		tileLetters = GetTileLetters(grid);
	}

	/**
	 * A period denotes an invisible tile.
	 */
	public string[] GetTileLetters(string[] grid)
	{
		string[] letters = new string[tileCountMax];
		for (int tileIndex = 0; tileIndex < tileCountMax; tileIndex++)
		{
			string character = null;
			int lineIndex = tileIndex / columnCountMax;
			bool isVisible = lineIndex < grid.Length;
			if (isVisible) 
			{
				string line = grid[lineIndex];
				int stringIndex = tileIndex % columnCountMax;
				isVisible = stringIndex < line.Length;
				if (isVisible)
				{
					character = line[stringIndex].ToString();
				}
			}
			letters[tileIndex] = character;
		}
		return letters;
	}
}
