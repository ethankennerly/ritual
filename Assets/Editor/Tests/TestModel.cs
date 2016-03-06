/**
 * This script must be in an Editor folder.
 * Test case:  2014-01 JimboFBX expects "using NUnit.Framework;"
 * Got "The type or namespace 'NUnit' could not be found."
 * http://answers.unity3d.com/questions/610988/unit-testing-unity-test-tools-v10-namespace-nunit.html
 */
using System;  // Exception
using System.Collections.Generic;  // List
using System.Linq;  // Dictionary Select
using System.Threading;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
internal class TestModel
{
	[Test]
	public void RemovePath()
	{
		Model model = new Model();
		model.tileLetters = new string[]{
			 "W", "I", "S", 
			 ".", ".", "H",
			 null, null, null};
		model.tileSelecteds = new bool[model.tileCountMax];
		model.RemovePath(new List<int>(){5, 1, 2});
		Assert.AreEqual(model.invisible, model.tileLetters[2]);
		Assert.AreEqual(model.invisible, model.tileLetters[5]);
		Assert.AreEqual(model.invisible, model.tileLetters[1]);
		model.RemovePath(new List<int>(){});
	}

	[Test]
	public void SwapLetters()
	{
		Model model = new Model();
		model.Start();
		Assert.AreEqual(false,
			model.SwapLetters(new int[]{-1, -1}));
		Assert.AreEqual(false,
			model.SwapLetters(new int[]{-1, 0}));
		Assert.AreEqual(false,
			model.SwapLetters(new int[]{3, 1}));
		model.tileSelecteds[1] = true;
		model.tileSelecteds[3] = true;
		Assert.AreEqual(false,
			model.SwapLetters(new int[]{0, 0}));
		Assert.AreEqual(true,
			model.SwapLetters(new int[]{3, 1}));
		Assert.AreEqual(false,
			model.SwapLetters(new int[]{1, 2}));
		Assert.AreEqual(true,
			model.SwapLetters(new int[]{1, 3}));
		Assert.AreEqual(true,
			model.SwapLetters(new int[]{1, 3}));
	}
}

