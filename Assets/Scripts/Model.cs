using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions

public class Model
{
	public int columnCountMax = 3;
	public string invisible = ".";
	public bool isSelecting;
	public int rowCountMax = 5;
	public int tileCountMax = 15;
	public string[] tileLetters;
	public bool[] tileSelecteds;
	public string submission;

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
		tileSelecteds = new bool[tileCountMax];
		isSelecting = false;
		submission = "";
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

	public void Select(string tileName)
	{
		int tileIndex = int.Parse(tileName.Split('_')[1]);

		bool wasSelected = tileSelecteds[tileIndex];
		if (wasSelected)
		{
			submission = submission.Substring(0, submission.Length - 1);
		}
		else
		{
			submission += tileLetters[tileIndex];
		}
		tileSelecteds[tileIndex] = !wasSelected;
	}

	public void RemoveSelected()
	{
		for (int tileIndex = 0; tileIndex < tileSelecteds.Length; tileIndex++)
		{
			bool isSelected = tileSelecteds[tileIndex];
			if (isSelected)
			{
				tileSelecteds[tileIndex] = false;
				tileLetters[tileIndex] = invisible;
			}
		}
	}

	public void OnMouseDown(string tileName)
	{
		if (!isSelecting)
		{
			isSelecting = true;
		}
		Select(tileName);
	}

	private string overPreviously;

	public void OnMouseOver(string tileName)
	{
		if (isSelecting)
		{
			if (null != tileName 
			&& overPreviously != tileName 
			&& 0 == tileName.IndexOf("tile_"))
			{
				Select(tileName);
			}
		}
		overPreviously = tileName;
	}

	public void OnMouseEnter(string tileName)
	{
		if (isSelecting)
		{
			Select(tileName);
		}
	}

	public void OnMouseUp()
	{
		if (isSelecting)
		{
			isSelecting = false;
			Submit();
		}
	}

	public void Submit()
	{
		Debug.Log("Model.Submit: " + submission);
		submission = "";
		RemoveSelected();
	}
}
