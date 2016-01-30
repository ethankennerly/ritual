using UnityEngine;  // Debug.Log

public class Controller
{
	public Model model = new Model();
	public View view = new View();

	public void Start()
	{
		model.Start();
		view.Start();
		view.SetupTiles(model.tileCountMax);
	}

	public void OnMouseDown(string name)
	{
		Debug.Log("OnMouseDown: " + name);
	}

	public void Update()
	{
		view.UpdateTiles(model.tileLetters, model.invisible);
	}
}
