using UnityEngine;  // GameObject
using UnityEngine.UI;  // Text

/**
 * Sharing code between projects:
 * Git repo or submodule.
 * Symlink on mac.
 * Copy code.
 * http://www.rivellomultimediaconsulting.com/symlinks-for-unity-game-development/
 */
public class ViewUtil
{
	/**
	 * Call animator.Play instead of animator.SetTrigger, in case the animator is in transition.
	 * Test case:  2015-11-15 Enter "SAT".  Type "RAT".  Expect R selected.  Got "R" resets to unselected.
	 * http://answers.unity3d.com/questions/801875/mecanim-trigger-getting-stuck-in-true-state.html
	 *
	 * Do not call until initialized.  Test case:  2015-11-15 Got warning "Animator has not been initialized"
	 * http://answers.unity3d.com/questions/878896/animator-has-not-been-initialized-1.html
	 *
	 * In editor, deleted and recreated animator state transition.  Test case:  2015-11-15 Got error "Transition '' in state 'selcted' uses parameter 'none' which is not compatible with condition type"
	 * http://answers.unity3d.com/questions/1070010/transition-x-in-state-y-uses-parameter-z-which-is.html
	 */
	public static void SetState(GameObject gameObject, string state, bool isRestart = true)
	{
		Animator animator = gameObject.GetComponent<Animator>();
		if (null != animator && animator.isInitialized)
		{
			// Debug.Log("ToyView.SetState: " + gameObject + ": " + state);
			if (isRestart)
			{
				animator.Play(state);
			}
			else
			{
				animator.Play(state, -1, 0f);
			}
		}
	}

	public static void SetText(Text textComponent, string text)
	{
		textComponent.text = text;
	}
}
