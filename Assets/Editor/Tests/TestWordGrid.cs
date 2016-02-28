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
[Category("My Tests")]
internal class TestWordGrid
{
	/**
	 * Tim Rogers, 2011-05
	 * http://stackoverflow.com/questions/5899171/is-there-anyway-to-handy-convert-a-dictionary-to-string
	 */
	public static string ToDebugString<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
	{
	    return "{" + string.Join(",", dictionary.Select(kv => kv.Key.ToString() + "=" + kv.Value.ToString()).ToArray()) + "}";
	}

	public static string ListToString<T>(List<T> aList)
	{
		string text = "[" + aList[0];
		for (int index = 1; index < aList.Count; index++) {
			text += ", " + aList[index];
		}
		text += "]";
		return text;
	}
	
	[Test]
	public void SetDictionary()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary("ab\nad\nback\ncab");
		Assert.AreEqual(3, grid.prefixes.Count, 
			ToDebugString(grid.prefixes));
		Assert.AreEqual(2, grid.prefixes['a'].Count);
		Assert.AreEqual(0, grid.prefixes['a']['b'].Count);
		Assert.AreEqual(0, grid.prefixes['a']['d'].Count);
		Assert.AreEqual(1, grid.prefixes['b'].Count);
		Assert.AreEqual(1, grid.prefixes['b']['a'].Count);
		Assert.AreEqual(1, grid.prefixes['b']['a']['c'].Count);
		Assert.AreEqual(0, grid.prefixes['b']['a']['c']['k'].Count);
		Assert.AreEqual(1, grid.prefixes['c'].Count);
		Assert.AreEqual(1, grid.prefixes['c']['a'].Count);
		Assert.AreEqual(0, grid.prefixes['c']['a']['b'].Count);
	}

	[Test]
	public void FindWords()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary("ab\nad\nback\ncab");
		List<string> words = grid.FindWords(
			new string[]{"c", "b", ".", 
			 "a", "k", "."},
			3, 2, new int[]{1, 3});
		Assert.AreEqual(2, words.Count, ListToString(words));
		Assert.AreEqual("back", words[0]);
		Assert.AreEqual("ab", words[1]);
	}
}
