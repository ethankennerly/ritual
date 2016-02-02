using UnityEngine;
using System.Collections;

public class MainView : MonoBehaviour
{
	public TextAsset credits;
	public TextAsset wordList;
	public TextAsset messages;
	public TextAsset[] grids;

	private Controller controller = new Controller();

	private void WireTextAssets()
	{
		Model model = controller.model;
		model.creditsText = credits.text;
		model.wordsText = wordList.text;
		model.messagesText = messages.text;
		model.gridTexts = new string[grids.Length];
		model.gridNames = new string[grids.Length];
		for (int i = 0; i < grids.Length; i++)
		{
			model.gridTexts[i] = grids[i].text;
			model.gridNames[i] = grids[i].name;
		}
	}

	void Start()
	{
		WireTextAssets();
		ButtonView.controller = controller;
		controller.view.InstantiatePrefab = InstantiatePrefab;
		controller.Start();
	}
	
	void Update()
	{
		controller.Update();
	}

	/**
	 * MonoBehaviour enables Instantiate.
	 * "You are trying to create a MonoBehaviour using the 'new' keyword.  This is not allowed.  MonoBehaviours can only be added using AddComponent().  Alternatively, your script can inherit from ScriptableObject or no base class at all"
	 */
	private GameObject InstantiatePrefab(GameObject prefab, Vector3 position)
	{
		GameObject instance = (GameObject) Instantiate(
			prefab, position, Quaternion.identity);
		instance.SetActive(true);
		return instance;
	}
}
