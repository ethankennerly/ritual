/**
 * This script must be in an Editor folder.
 * Test case:  2014-01 JimboFBX expects "using NUnit.Framework;"
 * Got "The type or namespace 'NUnit' could not be found."
 * http://answers.unity3d.com/questions/610988/unit-testing-unity-test-tools-v10-namespace-nunit.html
 */
using System;  // Exception
using System.Collections.Generic;  // List
using System.Threading;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("My Tests")]
internal class TestWordGrid
{
	[Test]
	public void FindWords()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary("ab\nad\nback\ncab");
		List<string> words = grid.FindWords(
			new string[]{"c", "b", ".", 
			 "a", "k", "."},
			3, 2, new int[]{1, 3});
		Assert.AreEqual(2, words.Count);
		Assert.AreEqual("back", words[0]);
		Assert.AreEqual("ab", words[1]);
	}
}
