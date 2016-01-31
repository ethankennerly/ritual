using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions
using System.Collections.Generic;  // Dictionary

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

	private string gridDelimiter = "\n\n";
	private string lineDelimiter = "\n";
	public string gridText = Toolkit.Read("Assets/Data/grids.txt");
	public string wordsText = Toolkit.Read("Assets/Data/word_list_moby_crossword.flat.txt");
	public Dictionary<string, bool> words;
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
		string[] gridStrings = Toolkit.Split(gridText, gridDelimiter);
		string[][] grids = new string[gridStrings.Length][];
		for (int i = 0; i < gridStrings.Length; i++)
		{
			string text = gridStrings[i];
			string[] lines = Toolkit.Split(text, lineDelimiter);
			grids[i] = lines;
			Debug.Log("Model.ParseGrids: " + i 
				+ ": " + Join(grids[i]));
		}
		return grids;
	}

	public string Join(string[] grid)
	{
		return String.Join(lineDelimiter, grid);
	}

	public Dictionary<string, bool> ParseWords(string wordsText)
	{
		Dictionary<string, bool> words = new Dictionary<string, bool>();
		string[] wordList = Toolkit.Split(wordsText, lineDelimiter);
		for (int wordIndex = 0; wordIndex < wordList.Length; wordIndex++)
		{
			string word = wordList[wordIndex];
			words[word] = true;
		}
		return words;
	}

	public void Start()
	{
		grids = ParseGrids(gridText);
		words = ParseWords(wordsText);
		PopulateGrid(0);
	}

	public void PopulateGrid(int newGridIndex)
	{
		gridIndex = newGridIndex % grids.Length;
		grid = grids[gridIndex];
		Debug.Log("Model.PopulateGrid: index " + gridIndex 
			+ ": " + Join(grid));
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

	public void SelectAll(bool isSelected)
	{
		for (int tileIndex = 0; tileIndex < tileSelecteds.Length; tileIndex++)
		{
			tileSelecteds[tileIndex] = isSelected;
		}
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

	private bool IsEmpty()
	{
		int count = 0;
		for (int tileIndex = 0; tileIndex < tileCountMax; tileIndex++)
		{
			string letter = tileLetters[tileIndex];
			if (null != letter && invisible != letter)
			{
				count++;
			}
		}
		return 0 == count;
	}

	private bool IsWord(string submission)
	{
		return words.ContainsKey(submission);
	}

	public void Submit()
	{
		if (IsWord(submission))
		{
			RemoveSelected();
			if (IsEmpty())
			{
				PopulateGrid(++gridIndex);
			}
		}
		else
		{
			SelectAll(false);
		}
		Debug.Log("Model.Submit: " + submission);
		submission = "";
	}
}
