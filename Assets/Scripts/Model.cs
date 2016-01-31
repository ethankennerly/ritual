using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions
using System.Collections.Generic;  // Dictionary

public class Model
{
	public int columnCountMax = 3;
	public string invisible = ".";
	public bool isSelecting;
	public string message;
	public int rowCountMax = 5;
	public int levelCountMax = 21;
	public int levelCount;
	public int tileCountMax = 15;
	public string[] tileLetters;
	public bool[] tileSelecteds;
	public string submission;
	public string stateChange = null;
	private string stateNext = null;

	private string gridDelimiter = "\n\n";
	private string wishesText = Toolkit.Read("Assets/Data/wishes.txt");
	private string wordsText = Toolkit.Read("Assets/Data/word_list_moby_crossword.flat.txt");
	private string creditsText = Toolkit.Read("Assets/Data/word_credits.txt");
	private string[] messages;
	private Dictionary<string, bool> credits;
	private Dictionary<string, bool> words;
	/**
	 * grid:
	 *	line
	 */
	private string[][] grids;
	public string[] grid;
	public int gridIndex = -1;
	public string levelText;
	public string wishName;

	/**
	 * Each line is a string, which may be accessed by index.
	 * Allows jagged arrays.
	 */
	public string[][] ParseGrids(string gridsText)
	{
		string[] gridStrings = Toolkit.Split(gridsText, gridDelimiter);
		string[][] grids = new string[gridStrings.Length][];
		for (int i = 0; i < gridStrings.Length; i++)
		{
			string text = gridStrings[i];
			string[] lines = Toolkit.Split(text, Toolkit.lineDelimiter);
			grids[i] = lines;
			// Debug.Log("Model.ParseGrids: " + i + ": " + Join(grids[i]));
		}
		return grids;
	}

	public string Join(string[] grid)
	{
		return String.Join(Toolkit.lineDelimiter, grid);
	}

	public Dictionary<string, bool> ParseWords(string wordsText)
	{
		Dictionary<string, bool> words = new Dictionary<string, bool>();
		string[] wordList = Toolkit.Split(wordsText, Toolkit.lineDelimiter);
		for (int wordIndex = 0; wordIndex < wordList.Length; wordIndex++)
		{
			string word = wordList[wordIndex];
			words[word] = true;
		}
		return words;
	}

	public string[] levelButtonNames;
	public string[] levelButtonTexts;
	public string[] wishButtonNames;
	public string[] wishButtonTexts;

	private void SetupLevelButtons(int levelCountMax)
	{
		levelButtonNames = new string[levelCountMax];
		levelButtonTexts = new string[levelCountMax];
		for (int tileIndex = 0; tileIndex < levelCountMax; tileIndex++)
		{
			levelButtonNames[tileIndex] = "level_" + tileIndex;
			levelButtonTexts[tileIndex] = (tileIndex + 1).ToString();
		}
	}

	private void SetupWishButtons(string[] wishButtonTexts)
	{
		wishButtonNames = new string[wishButtonTexts.Length];
		for (int tileIndex = 0; tileIndex < wishButtonTexts.Length; tileIndex++)
		{
			wishButtonNames[tileIndex] = "wish_" + tileIndex;
		}
	}

	private Dictionary <string, string[][]> wishGrids;
	private Dictionary <string, string[]> wishMessages;

	public string[] LoadAllWishes()
	{
		string[][] wishTable = Toolkit.ParseCsv(wishesText);
		int nameColumn = 0;
		int gridsColumn = 1;
		int messagesColumn = 2;
		string path = "Assets/Data/";
		wishGrids = new Dictionary<string, string[][]>();
		wishMessages = new Dictionary<string, string[]>();
		int afterHeaderRow = 1;
		string[] wishNames = new string[wishTable.Length - afterHeaderRow];
		for (int wishIndex = afterHeaderRow; wishIndex < wishTable.Length; wishIndex++)
		{
			string[] wishRow = wishTable[wishIndex];
			string name = wishRow[nameColumn];
			wishNames[wishIndex - 1] = name;
			// Debug.Log("Model.LoadAllWishes: <" + name + ">");
			string gridsFileName = wishRow[gridsColumn];
			string messagesFileName = wishRow[messagesColumn];
			string gridsText = Toolkit.Read(path + gridsFileName);
			string[][] grids = ParseGrids(gridsText);
			wishGrids[name] = grids;
			string[] messages;
			if (null == messagesFileName || "" == messagesFileName)
			{
				messages = new string[0];
			}
			else
			{
				string messagesText = Toolkit.Read(path + messagesFileName);
				messages = Toolkit.Split(messagesText, Toolkit.lineDelimiter);
			}
			wishMessages[name] = messages;
		}
		return wishNames;
	}

	public void SetupWish(string wishButtonText)
	{
		wishName = wishButtonText;
		grids = wishGrids[wishName];
		messages = wishMessages[wishName];
		PopulateGrid(0);
		levelCount = grids.Length;
	}

	public void Start()
	{
		words = ParseWords(wordsText);
		credits = ParseWords(creditsText);
		wishButtonTexts = LoadAllWishes();
		SetupWishButtons(wishButtonTexts);
		SetupLevelButtons(levelCountMax);
		SetupWish("Tutorial");
	}

	public void PopulateGrid(int newGridIndex)
	{
		gridIndex = newGridIndex % grids.Length;
		grid = grids[gridIndex];
		// Debug.Log("Model.PopulateGrid: index " + gridIndex + ": " + Join(grid));
		tileLetters = GetTileLetters(grid);
		tileSelecteds = new bool[tileCountMax];
		isSelecting = false;
		submission = "";
		if (gridIndex < messages.Length)
		{
			message = messages[gridIndex];
		}
		else
		{
			message = "";
		}
		levelText = wishName
			+ " " + (gridIndex + 1).ToString()
			+ " of "
			+ grids.Length.ToString();
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
		int tileIndex = Toolkit.parseIndex(tileName);
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

	public string OnMouseDown(string tileName)
	{
		stateNext = null;
		if (0 == tileName.IndexOf("level_"))
		{
			stateNext = "levelEnter";
			gridIndex = Toolkit.parseIndex(tileName);
			PopulateGrid(gridIndex);
		}
		else if ("LevelExit" == tileName)
		{
			stateNext = "levelExit";
		}
		else if ("Wishes" == tileName)
		{
			stateNext = "wishes";
		}
		else if ("Restart" == tileName)
		{
			PopulateGrid(gridIndex);
		}
		else if (0 == tileName.IndexOf("wish_"))
		{
			stateNext = "levelExit";
			int wishIndex = Toolkit.parseIndex(tileName);
			SetupWish(wishButtonTexts[wishIndex]);
		}
		else
		{
			if (!isSelecting)
			{
				isSelecting = true;
			}
			Select(tileName);
		}
		return stateNext;
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
		return words.ContainsKey(submission)
			|| credits.ContainsKey(submission);
	}

	
	public void Submit()
	{
		if (IsWord(submission))
		{
			RemoveSelected();
			if (IsEmpty())
			{
				++gridIndex;
				if (gridIndex < levelCount)
				{
					PopulateGrid(gridIndex);
				}
				else
				{
					stateNext = "wishes";
				}
			}
		}
		else
		{
			SelectAll(false);
		}
		// Debug.Log("Model.Submit: " + submission);
		submission = "";
	}

	public string Update()
	{
		stateChange = stateNext;
		stateNext = null;
		return stateChange;
	}
}
