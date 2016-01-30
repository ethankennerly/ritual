using UnityEngine;  // Debug.Log

public class Controller
{
	public Model model = new Model();
	public View view = new View();

	public void Start()
	{
		model.Start();
		view.Start();
	}

	public void OnMouseDown(string name)
	{
		Debug.Log("OnMouseDown: " + name);
	}
}
