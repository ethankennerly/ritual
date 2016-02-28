using System.Collections.Generic;  // List

/**
 * http://stackoverflow.com/questions/647533/recursive-generic-types
 */
public class CharToDictionary : Dictionary<char, CharToDictionary>{}

public class WordGrid
{
	public CharToDictionary prefixes;

	/**
	 * Construct prefix tree from list of words, one per line.
	 * Example @see Editor/Tests/TestWordGrid
	 */
	public void SetDictionary(string dictionaryText)
	{
		prefixes = new CharToDictionary();
		string[] words = Toolkit.Split(dictionaryText, Toolkit.lineDelimiter);
		for (int index = 0; index < words.Length; index++) {
			string word = words[index];
			char[] letters = Toolkit.SplitLetters(word);
			CharToDictionary parent = prefixes;
			for (int a = 0; a < letters.Length; a++) {
				char letter = letters[a];
				if (!parent.ContainsKey(letter)) {
					parent[letter] = new CharToDictionary();
				}
				parent = parent[letter];
			}
		}
	}

	/**
	 * @return	Longest first.
	 * Example @see Editor/Tests/TestWordGrid
	 * References:
	 * http://exceptional-code.blogspot.com/2012/02/solving-boggle-game-recursion-prefix.html
	 * http://stackoverflow.com/questions/746082/how-to-find-list-of-possible-words-from-a-letter-matrix-boggle-solver
	 */
	public List<string> FindWords(string[] cellLetters, int columnCount, int rowCount, int[] startCells)
	{
		List<string> words = new List<string>();
		return words;
	}
}
