using UnityEngine;  // Debug.Log

public class Controller
{
	public Model model = new Model();
	public View view = new View();

	/**
	 * Message behind buttons.
	 * Test case:  2016-01-30 Expect to select 12th tile.  Would not select.
	 */
	public void Start()
	{
		model.Start();
		view.Start();
		view.SetupTiles(model.tileCountMax);
	}

	public void OnMouseDown(string name)
	{
		// Debug.Log("Controller.OnMouseDown: " + name);
		model.OnMouseDown(name);
	}

	public void OnMouseEnter(string name)
	{
		// Debug.Log("Controller.OnMouseEnter: " + name);
		model.OnMouseEnter(name);
	}

	public void Update()
	{
		if (view.IsMouseUpNow())
		{
			model.OnMouseUp();
		}
		view.UpdateLetters(model.tileLetters, model.invisible);
		view.UpdateSelecteds(model.tileSelecteds);
		ViewUtil.SetText(view.message, model.message);
	}
}
