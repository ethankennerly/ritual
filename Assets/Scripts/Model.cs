using UnityEngine;  // Debug.Log
using System;  // String, StringSplitOptions
using System.Collections.Generic;  // Dictionary

public class Model
{
	public ViewModel view = new ViewModel();
	public int columnCountMax = 3;
	public string invisible = ".";
	public bool isSelecting;
	public bool isSwapLettersMode = false;
	public string message;
	public int rowCountMax = 5;
	public int levelCountMax = 21;
	public int levelCount;
	public int tileCountMax = 15;
	public string[] tileLetters;
	public bool[] tileSelecteds;
	public string submission;
	public string[] levelButtonNames;
	public string[] levelButtonTexts;
	public string[] wishButtonNames;
	public string[] wishButtonTexts;
	public int gridsComplete;
	public int gridsTotal;
	public string gridsCompleteText;

	public string stateChange = null;
	private string stateNext = null;

	private string gridDelimiter = "\n\n";
	private Dictionary <string, string[][]> wishGrids;
	private Dictionary <string, string[]> wishMessages;
	public Dictionary <string, bool[]> wishIsCompletes;
	public bool[] wishesIsCompletes;

	public string namesText;
	public string creditsText;
	public string wordsText;
	public string messagesText;
	public string[] gridTexts;
	public string[] gridNames;
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
	public int wishIndex;

	public void ReadTexts()
	{
		string directory = isSwapLettersMode ? "swap" : "spell";
		creditsText = Toolkit.Read(directory + "/word_credits.txt");
		wordsText = Toolkit.Read(directory + "/word_list_moby_crossword.flat.txt");
		messagesText = Toolkit.Read(directory + "/tutorial_messages.txt");
		gridNames = new string[]{
			"tutorial_grids.txt",
			"love_grids.txt",
			"health_grids.txt",
			"money_grids.txt",
			"charm_grids.txt",
			"success_grids.txt",
			"skill_grids.txt",
			"healing_grids.txt",
			"relaxation_grids.txt",
			"grief_grids.txt",
			"banishing_grids.txt",
			"credits_grids.txt"
		};
		gridTexts = new string[gridNames.Length];
		for (int index = 0; index < gridNames.Length; index++) {
			gridTexts[index] = Toolkit.Read(directory + "/" + gridNames[index]);
		}
	}

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

	public string[] LoadAllWishes()
	{
		gridsTotal = 0;
		wishGrids = new Dictionary<string, string[][]>();
		wishMessages = new Dictionary<string, string[]>();
		wishIsCompletes = new Dictionary<string, bool[]>();
		string[] wishNames = new string[gridTexts.Length];
		wishesIsCompletes = new bool[wishNames.Length];
		for (int wishIndex = 0; wishIndex < wishNames.Length; wishIndex++)
		{
			string gridsFileName = gridNames[wishIndex];
			string name = Toolkit.Split(gridsFileName, "_grids")[0];
			name = name.ToUpper()[0] + name.Substring(1);
			wishNames[wishIndex] = name;
			// Debug.Log("Model.LoadAllWishes: <" + name + ">");
			string gridsText = gridTexts[wishIndex];
			string[][] grids = ParseGrids(gridsText);
			wishGrids[name] = grids;
			wishIsCompletes[name] = new bool[grids.Length];
			gridsTotal += grids.Length;
			for (int gridIndex = 0; gridIndex < grids.Length; gridIndex++)
			{
				wishIsCompletes[name][gridIndex] = false;
			}
			wishesIsCompletes[wishIndex] = false;
			string[] messages;
			if (0 == wishIndex)
			{
				messages = Toolkit.Split(messagesText, Toolkit.lineDelimiter);
			}
			else
			{
				messages = new string[0];
			}
			wishMessages[name] = messages;
		}
		Debug.Log("Model.LoadAllWishes: " + gridsTotal + " grids");
		gridsComplete = 0;
		formatGridsComplete();

		return wishNames;
	}

	private void formatGridsComplete()
	{
		gridsCompleteText = "Spells cast: " 
			+ gridsComplete
			+ " of "
			+ gridsTotal;
	}

	public void SetupWish(int index)
	{
		wishIndex = index;
		wishName = wishButtonTexts[index];
		grids = wishGrids[wishName];
		messages = wishMessages[wishName];
		PopulateGrid(0);
		levelCount = grids.Length;
	}

	public void Start()
	{
		ReadTexts();
		words = ParseWords(wordsText);
		credits = ParseWords(creditsText);
		wishButtonTexts = LoadAllWishes();
		SetupWishButtons(wishButtonTexts);
		SetupLevelButtons(levelCountMax);
		SetupWish(0);
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
		if (isSwapLettersMode) {
			if (2 <= submission.Length) {
				return;
			}
		}
		int tileIndex = Toolkit.ParseIndex(tileName);
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
			gridIndex = Toolkit.ParseIndex(tileName);
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
			int wishIndex = Toolkit.ParseIndex(tileName);
			SetupWish(wishIndex);
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

	public bool SetComplete(int gridIndex)
	{
		bool[] isCompletes = wishIsCompletes[wishName];
		if (!isCompletes[gridIndex]) {
			gridsComplete++;
			formatGridsComplete();
			isCompletes[gridIndex] = true;
		}
		bool isAllComplete = true;
		for (int tileIndex = 0; tileIndex < isCompletes.Length; tileIndex++)
		{
			isAllComplete = isAllComplete && isCompletes[tileIndex];
		}
		wishesIsCompletes[wishIndex] = isAllComplete;
		return isAllComplete;
	}
	
	private void SubmitWord()
	{
		if (IsWord(submission))
		{
			RemoveSelected();
			if (IsEmpty())
			{
				SetComplete(gridIndex);
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
		// Debug.Log("Model.SubmitWord: " + submission);
	}

	private void SwapLetters()
	{
		int previous = -1;
		string letter = invisible;
		for (int tileIndex = 0; tileIndex < tileSelecteds.Length; tileIndex++)
		{
			bool isSelected = tileSelecteds[tileIndex];
			if (isSelected) {
				if (invisible == letter) {
					letter = tileLetters[tileIndex];
					previous = tileIndex;
				}
				else {
					tileLetters[previous] = tileLetters[tileIndex];
					tileLetters[tileIndex] = letter;
					break;
				}
			}
		}
	}

	private void Submit()
	{
		if (isSwapLettersMode) {
			SwapLetters();
			SelectAll(false);
		}
		else {
			SubmitWord();
		}
		submission = "";
	}

	public string Update()
	{
		stateChange = stateNext;
		stateNext = null;
		return stateChange;
	}
}
