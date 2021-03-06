using UnityEngine;
using System.Collections;

public class MainView : MonoBehaviour
{
	public bool isSwapLettersMode = false;

	private Controller controller = new Controller();

	void Start()
	{
		controller.model.isSwapLettersMode = isSwapLettersMode;
		ButtonView.isParent = true;
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
