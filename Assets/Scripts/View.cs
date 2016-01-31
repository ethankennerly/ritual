using UnityEngine;
using UnityEngine.UI;
using System;

public class View
{
	public Text message;
	public delegate GameObject InstantiatePrefabDelegate(GameObject prefab, 
		Vector3 position);
	public InstantiatePrefabDelegate InstantiatePrefab;
	
	private GameObject main;
	private GameObject grid;
	private GameObject[] tiles;
	private Text[] letters;
	private GameObject[] buttons;
	private GameObject[] selecteds;

	private GameObject levelGrid;
	private GameObject[] levels;

	public void Start()
	{
		if (null == main) {
			main = GameObject.Find("Main");
			message = GameObject.Find("Message").GetComponent<Text>();
			grid = GameObject.Find("Grid");
			levelGrid = GameObject.Find("LevelGrid");
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

	public void SetupLevels(int levelCountMax)
	{
		levels = new GameObject[levelCountMax];
		string address = "LevelTile";
		Transform transform = levelGrid.transform.Find(address);
		GameObject tilePrefab = transform.gameObject;
		GameObject tile = tilePrefab;
		for (int tileIndex = 0; tileIndex < levelCountMax; tileIndex++)
		{
			if (1 <= tileIndex)
			{
				tile = InstantiatePrefab(tilePrefab, tilePrefab.transform.position);
				tile.transform.SetParent(tilePrefab.transform.parent.transform, false);
			}
			tile.name = "level_" + tileIndex;
			levels[tileIndex] = tile;
			GameObject button = tile.transform.Find("LevelButton").gameObject;
			GameObject textObject = button.transform.Find("Text").gameObject;
			Text txt = textObject.GetComponent<Text>();
			ViewUtil.SetText(txt, (tileIndex + 1).ToString());
		}
	}

	public void UpdateLevels(int levelCount)
	{
		for (int tileIndex = 0; tileIndex < levels.Length; tileIndex++)
		{
			bool isActive = tileIndex < levelCount;
			GameObject tile = levels[tileIndex];
			tile.SetActive(isActive);
		}
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
