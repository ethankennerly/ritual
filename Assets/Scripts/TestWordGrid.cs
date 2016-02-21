using System.Collections.Generic;  // List
using System;  // Exception

public class TestWordGrid
{
	public static void AssertEquals(object expected, object got)
	{
		if (expected != got) {
			throw new Exception("Expected <" + expected + ">.  Got <" + got + ">.");
		}
	}

	public TestWordGrid()
	{
		testFindWords();
	}

	public void testFindWords()
	{
		WordGrid grid = new WordGrid();
		grid.SetDictionary("ab\nad\nback\ncab");
		List<string> words = grid.FindWords(
			new string[]{"c", "b", ".", 
			 "a", "k", "."},
			3, 2, new int[]{1, 3});
		AssertEquals(2, words.Count);
		AssertEquals("back", words[0]);
		AssertEquals("ab", words[1]);
	}
}
