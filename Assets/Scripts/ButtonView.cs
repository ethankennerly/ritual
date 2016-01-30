using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;  // Required when using Event data.

public class ButtonView : MonoBehaviour, IPointerDownHandler  // required interface when using the OnPointerDown method.
{
	public static Controller controller;

	/**
	 * Test case:  Test.  Port.  Change view.  Expect minimal view code.  
	 *
	 * Test case:  2015 Another person is locking the editor file.  I want to edit the code of the button callback.
	 *
	 * Test case:  2013 Players want immediate responsiveness, as soon as the button is pressed.
	 *
	 * Workaround for Unity:
 	 * What is a more convenient technique to call OnMouseDown?
	 * What is a more convenient technique to add listener outside of the editor?
	 * Adapted from:
	 * http://answers.unity3d.com/questions/829594/ui-button-onmousedown.html
	 * http://docs.unity3d.com/ScriptReference/UI.Selectable.OnPointerDown.html
	 */
	public void OnPointerDown (PointerEventData eventData) 
	{
		controller.OnMouseDown(transform.parent.gameObject.name);
	}
}
