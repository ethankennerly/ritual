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
		ControllerUtil.SetupButtons(this, model.view.buttons);
		view.SetupTiles(model.tileCountMax);
		view.SetupLevels(model.levelCountMax, model.levelButtonNames, model.levelButtonTexts);
		view.SetupWishes(model.wishButtonNames, model.wishButtonTexts);
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
		string stateChange = model.Update();
		if (null != stateChange)
		{
			view.UpdateLevels(model.levelCount,
				model.wishIsCompletes[model.wishName], model.wishesIsCompletes);
			ViewUtil.SetState(view.scene, stateChange);
			ViewUtil.SetText(view.wishText, model.wishName);
			ViewUtil.SetText(view.gridsCompleteText, model.gridsCompleteText);
		}
		ViewUtil.SetText(view.levelText, model.levelText);
		view.UpdateLetters(model.tileLetters, model.invisible);
		view.UpdateSelecteds(model.tileSelecteds);
		ViewUtil.SetText(view.message, model.message);
	}
}
