using UnityEngine;
using System.Collections;

public class MainView : MonoBehaviour
{
	private Controller controller = new Controller();

	void Start ()
	{
		ButtonView.controller = controller;
		controller.Start();
	}
	
	void Update ()
	{
	}
}
