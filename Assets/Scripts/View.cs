using UnityEngine;
using UnityEngine.UI;
using System;

public class View
{
	private GameObject main;
	private GameObject grid;
	private GameObject[] tiles;
	private Text[] letters;
	private GameObject[] buttons;
	
	public void Start()
	{
		if (null == main) {
			main = GameObject.Find("Main");
			grid = GameObject.Find("Grid");
		}
	}

	public void SetupTiles(int tileCountMax)
	{
		tiles = new GameObject[tileCountMax];
		letters = new Text[tileCountMax];
		buttons = new GameObject[tileCountMax];
		for (int tileIndex = 0; tileIndex < tileCountMax; tileIndex++)
		{
			string address = String.Format("Tile ({0})", tileIndex);
			GameObject tile = grid.transform.Find(address).gameObject;
			tiles[tileIndex] = tile;
			GameObject button = tile.transform.Find("Button").gameObject;
			buttons[tileIndex] = button;
			GameObject textObject = button.transform.Find("Text").gameObject;
			Text letter = textObject.GetComponent<Text>();
			letters[tileIndex] = letter;
		}
	}

	/**
	 * @param	invisible	This character's tile is invisible yet active to retain auto grid layout.
	 */
	public void UpdateTiles(string[] tileLetters, string invisible)
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
				letters[tileIndex].text = letter;
				GameObject button = buttons[tileIndex];
				button.SetActive(isVisible);
			}
		}
	}
}
