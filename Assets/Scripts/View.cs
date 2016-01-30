using UnityEngine;

public class View
{
	public GameObject main;
	public GameObject canvas;
	
	public void Start()
	{
		if (null == main) {
			main = GameObject.Find("Main");
			canvas = GameObject.Find("Canvas");
		}
	}
}
