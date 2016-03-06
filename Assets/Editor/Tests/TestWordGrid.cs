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
		string text;
		if (aList.Count <= 0) {
			text = "[]";
		}
		else {
			text = "[" + aList[0];
			for (int index = 1; index < aList.Count; index++) {
				text += ", " + aList[index];
			}
			text += "]";
		}
		return text;
	}

	public static void AssertCount<TKey, TValue>(int expectedCount, IDictionary<TKey, TValue> hash) {
		Assert.AreEqual(expectedCount, hash.Count, ToDebugString(hash));
	}

	[Test]
	public void SetDictionary()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary("ab\nabs\nad\nback\ncab");
		AssertCount(3, grid.prefixes);
		AssertCount(2, grid.prefixes['a']);
		AssertCount(2, grid.prefixes['a']['b']);
		AssertCount(0, grid.prefixes['a']['b'][grid.endOfWord]);
		AssertCount(0, grid.prefixes['a']['b']['s'][grid.endOfWord]);
		AssertCount(1, grid.prefixes['a']['d']);
		AssertCount(0, grid.prefixes['a']['d'][grid.endOfWord]);
		AssertCount(1, grid.prefixes['b']);
		AssertCount(1, grid.prefixes['b']['a']);
		AssertCount(1, grid.prefixes['b']['a']['c']);
		AssertCount(0, grid.prefixes['b']['a']['c']['k'][grid.endOfWord]);
		AssertCount(1, grid.prefixes['c']);
		AssertCount(1, grid.prefixes['c']['a']);
		AssertCount(0, grid.prefixes['c']['a']['b'][grid.endOfWord]);
	}

	[Test]
	public void FindWordsWhole()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary("ab\nad\nback\ncab");
		grid.cellLetters = new string[]{
			 "c", "b", ".", 
			 "a", "k", "."};
		grid.SetSize(3, 2);
		List<string> words;
		words = grid.FindWords(1);
		Assert.AreEqual(1, words.Count, ListToString(words));
		Assert.AreEqual("back", words[0]);
		words = grid.FindWords(3);
		Assert.AreEqual(1, words.Count, ListToString(words));
		Assert.AreEqual("ab", words[0]);
		words = grid.FindWords(4);
		Assert.AreEqual(0, words.Count, ListToString(words));
	}

	private WordGrid SetupWishGrid()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary("HI\nHIS\nIS\nWISE\nWISH\nWISHES");
		grid.cellLetters = new string[]{
			 "W", "I", "S", 
			 ".", ".", "H",
			 null, null, null};
		grid.SetSize(3, 3);
		return grid;
	}

	[Test]
	public void FindWordsSuffixes()
	{
		WordGrid grid = SetupWishGrid();
		List<string> words;

		words = grid.FindWords(0);
		Assert.AreEqual(1, words.Count, ListToString(words));
		Assert.AreEqual("WISH", words[0]);

		words = grid.FindWords(1);
		Assert.AreEqual(1, words.Count, ListToString(words));
		Assert.AreEqual("IS", words[0]);

		words = grid.FindWords(5);
		Assert.AreEqual(2, words.Count, ListToString(words));
		Assert.AreEqual("HIS", words[0]);
		Assert.AreEqual("HI", words[1]);
	}

	[Test]
	public void FindWordsSnakeNotWrap()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary(
		 "AT\nATE\nACTION\nAFT\nAFFECTION\nAFFECTIONATE\nAFFECTIONATELY");
		grid.cellLetters = new string[]{
			 "A", "F", "F", 
			 "T", "C", "E",
			 "I", "A", "T",
			 "O", "N", "E",
			 null, null, null};
		grid.SetSize(3, 5);
		List<string> words;

		words = grid.FindWords(0);
		Assert.AreEqual(5, words.Count, ListToString(words));
		Assert.AreEqual("AFFECTIONATE", words[0]);
		Assert.AreEqual("AFFECTION", words[1]);
		Assert.AreEqual("ACTION", words[2]);
		Assert.AreEqual("AFT", words[3]);
		Assert.AreEqual("AT", words[4]);
	}

	[Test]
	public void FindLongestWord()
	{
		WordGrid grid = SetupWishGrid();
		Assert.AreEqual("WISH", 
			grid.FindLongestWord(new int[]{0, 1}));
		Assert.AreEqual("HIS", 
			grid.FindLongestWord(new int[]{1, 5}));
		Assert.AreEqual("HIS", 
			grid.FindLongestWord(new int[]{5, 1}));
		Assert.AreEqual("WISH", 
			grid.FindLongestWord(new int[]{1, 0}));
		Assert.AreEqual("", 
			grid.FindLongestWord(new int[]{2, 3}));
		Assert.AreEqual("WISH", 
			grid.FindLongestWord(new int[]{0, 0}));
		Assert.AreEqual("WISH", 
			grid.FindLongestWord(new int[]{2, 0, 1}));
		Assert.AreEqual("", 
			grid.FindLongestWord(new int[]{}));
	}

	[Test]
	public void FindWordsPath()
	{
		WordGrid grid = SetupWishGrid();
		List<string> words;

		words = grid.FindWords(0);
		Assert.AreEqual(1, words.Count, ListToString(words));
		Assert.AreEqual("WISH", words[0]);
		Assert.AreEqual(new List<int>(){0, 1, 2, 5},
			grid.wordPaths["WISH"]);
	}

	[Test]
	public void FindLongestSet()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary(
		 "MAGIC\nSCAMPI\nSPELL");
		grid.cellLetters = new string[]{
			 ".", "A", "G", 
			 "M", "C", "I",
			 "S", "P", "E",
			 ".", "L", "L",
			 null, null, null};
		grid.SetSize(3, 5);
		Assert.AreEqual("", 
			grid.FindLongestSet(new int[]{}));
		Assert.AreEqual("MAGIC, SPELL", 
			grid.FindLongestSet(new int[]{3, 6}));
		Assert.AreEqual(new List<int>(){3, 1, 2, 5, 4, 6, 7, 8, 11, 10},
			grid.wordPaths["MAGIC, SPELL"]);
	}
}
