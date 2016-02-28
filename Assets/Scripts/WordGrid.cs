using System.Collections.Generic;  // List

/**
 * http://stackoverflow.com/questions/647533/recursive-generic-types
 */
public class CharToDictionary : Dictionary<char, CharToDictionary>{}

public class WordGrid
{
	public CharToDictionary prefixes;
	public int minimumLength = 1;

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
	 * Minimum length 2.
	 * @param	cellLetters	Special case "qu" might be one cell.
	 * @return	Longest first.
	 * Example @see Editor/Tests/TestWordGrid
	 * References:
	 * http://exceptional-code.blogspot.com/2012/02/solving-boggle-game-recursion-prefix.html
	 * http://stackoverflow.com/questions/746082/how-to-find-list-of-possible-words-from-a-letter-matrix-boggle-solver
	 */
	public List<string> FindWords(string[] cellLetters, int columnCount, int rowCount, int[] startCells)
	{
		List<string> words = new List<string>();
		List<int> searchCells = new List<int>();
		int searchIndex;
		for (searchIndex = 0; searchIndex < startCells.Length; searchIndex++) {
			searchCells.Add(startCells[searchIndex]);
		}
		for (searchIndex = 0; searchIndex < searchCells.Count; searchIndex++) {
			int searchCell = searchCells[searchIndex];
			char letter = cellLetters[searchCell][0];
			CharToDictionary parent = prefixes;
			bool[] isVisits = new bool[cellLetters.Length];
			string word = "";
			if (!isVisits[searchCell] && parent.ContainsKey(letter)) {
				isVisits[searchCell] = true;
				word += letter;

			}
			if (minimumLength <= word.Length) {
				words.Add(word);
			}
		}
		return words;
	}
}
