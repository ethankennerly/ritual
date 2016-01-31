using UnityEngine;
using UnityEngine.UI;
using System;

public class View
{
	public Text levelText;
	public Text wishText;
	public Text message;
	public GameObject scene;
	public delegate GameObject InstantiatePrefabDelegate(GameObject prefab, 
		Vector3 position);
	public InstantiatePrefabDelegate InstantiatePrefab;
	
	private GameObject main;
	private GameObject grid;
	private GameObject[] tiles;
	private Text[] letters;
	private GameObject[] buttons;
	private GameObject[] selecteds;
	private GameObject[] levelCompleteds;
	private GameObject[] wishCompleteds;

	private GameObject levelGrid;
	private GameObject wishGrid;
	private GameObject[] levels;
	private GameObject[] wishes;

	public void Start()
	{
		if (null == main) {
			main = GameObject.Find("Main");
			message = GameObject.Find("Message").GetComponent<Text>();
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			wishText = GameObject.Find("WishText").GetComponent<Text>();
			scene = GameObject.Find("Scene");
			grid = GameObject.Find("Grid");
			levelGrid = GameObject.Find("LevelGrid");
			wishGrid = GameObject.Find("WishGrid");
		}
	}

	public void SetupTiles(int tileCountMax)
	{
		tiles = new GameObject[tileCountMax];
		letters = new Text[tileCountMax];
		buttons = new GameObject[tileCountMax];
		selecteds = new GameObject[tileCountMax];
		string address = "Tile";
		GameObject tilePrefab = grid.transform.Find(address).gameObject;
		GameObject tile = tilePrefab;
		for (int tileIndex = 0; tileIndex < tileCountMax; tileIndex++)
		{
			if (1 <= tileIndex)
			{
				tile = InstantiatePrefab(tilePrefab, tilePrefab.transform.position);
				tile.transform.SetParent(tilePrefab.transform.parent.transform, false);
			}
			tile.name = "tile_" + tileIndex;
			tiles[tileIndex] = tile;
			GameObject button = tile.transform.Find("Button").gameObject;
			buttons[tileIndex] = button;
			GameObject textObject = button.transform.Find("Text").gameObject;
			Text letter = textObject.GetComponent<Text>();
			letters[tileIndex] = letter;
			GameObject selected = tile.transform.Find("Selected").gameObject;
			selecteds[tileIndex] = selected;
		}
	}

	private GameObject[] GenerateTiles(GameObject grid, int tileCountMax, string[] tileNames, string[] tileTexts)
	{
		GameObject[] tiles = new GameObject[tileCountMax];
		string address = "Tile";
		Transform transform = grid.transform.Find(address);
		GameObject tilePrefab = transform.gameObject;
		GameObject tile = tilePrefab;
		for (int tileIndex = 0; tileIndex < tileCountMax; tileIndex++)
		{
			if (1 <= tileIndex)
			{
				tile = InstantiatePrefab(tilePrefab, tilePrefab.transform.position);
				tile.transform.SetParent(tilePrefab.transform.parent.transform, false);
			}
			tile.name = tileNames[tileIndex];
			tiles[tileIndex] = tile;
			GameObject button = tile.transform.Find("Button").gameObject;
			GameObject textObject = button.transform.Find("Text").gameObject;
			Text txt = textObject.GetComponent<Text>();
			ViewUtil.SetText(txt, tileTexts[tileIndex]);
		}
		return tiles;
	}

	public void UpdateLevels(int levelCount, bool[] isCompletes, bool[] wishesIsCompletes)
	{
		int tileIndex = 0;
		for (tileIndex = 0; tileIndex < levels.Length; tileIndex++)
		{
			bool isActive = tileIndex < levelCount;
			GameObject tile = levels[tileIndex];
			tile.SetActive(isActive);
			if (isActive)
			{
				levelCompleteds[tileIndex].SetActive(isCompletes[tileIndex]);
			}
		}
		for (tileIndex = 0; tileIndex < wishes.Length; tileIndex++)
		{
			wishCompleteds[tileIndex].SetActive(wishesIsCompletes[tileIndex]);
		}
	}

	public GameObject[] SetupCompleteds(GameObject[] parents)
	{
		GameObject[] completeds = new GameObject[parents.Length];
		for (int tileIndex = 0; tileIndex < parents.Length; tileIndex++)
		{
			GameObject tile = parents[tileIndex];
			completeds[tileIndex] = tile.transform.Find("Completed").gameObject;
		}
		return completeds;
	}

	public void SetupLevels(int levelCountMax, string[] levelNames, string[] levelTexts)
	{
		levels = GenerateTiles(levelGrid, levelCountMax, levelNames, levelTexts);
		levelCompleteds = SetupCompleteds(levels);
	}

	public void SetupWishes(string[] wishNames, string[] wishTexts)
	{
		wishes = GenerateTiles(wishGrid, wishNames.Length, wishNames, wishTexts);
		wishCompleteds = SetupCompleteds(wishes);
	}

	/**
	 * @param	invisible	This character's tile is invisible yet active to retain auto grid layout.
	 */
	public void UpdateLetters(string[] tileLetters, string invisible)
	{
		for (int tileIndex = 0; tileIndex < tileLetters.Length; tileIndex++)
		{
			string letter = tileLetters[tileIndex];
			bool isActive = null != letter;
			GameObject tile = tiles[tileIndex];
			tile.SetActive(isActive);
			if (isActive)
			{
				bool isVisible = isActive && letter != invisible;
				ViewUtil.SetText(letters[tileIndex], letter);
				GameObject button = buttons[tileIndex];
				button.SetActive(isVisible);
			}
		}
	}

	public void UpdateSelecteds(bool[] tileSelecteds)
	{
		for (int tileIndex = 0; tileIndex < tileSelecteds.Length; tileIndex++)
		{
			bool isSelected = tileSelecteds[tileIndex];
			GameObject selected = selecteds[tileIndex];
			selected.SetActive(isSelected);
		}
	}

	public bool IsMouseUpNow()
	{
		return Input.GetMouseButtonUp(0);
	}
}
