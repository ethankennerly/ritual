using System.Collections.Generic;  // List

/**
 * http://stackoverflow.com/questions/647533/recursive-generic-types
 */
public class CharToDictionary : Dictionary<char, CharToDictionary>{}

public class WordGrid
{
	public CharToDictionary prefixes;
	public int minimumLength = 1;
	public char endOfWord = ';';

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
			parent[endOfWord] = new CharToDictionary();
		}
	}

	/**
	 * List cells to search.  If a word is found, append it.  Search adjacent cells.
	 * @param	cellLetters	Not handled:  Special case "qu" might be one cell.  Expects each cell leter is not null.
	 * @return	Longest first down to minimum length.
	 * Example @see Editor/Tests/TestWordGrid
	 * References:
	 * http://exceptional-code.blogspot.com/2012/02/solving-boggle-game-recursion-prefix.html
	 * http://stackoverflow.com/questions/746082/how-to-find-list-of-possible-words-from-a-letter-matrix-boggle-solver
	 */
	public List<string> FindWords(string[] cellLetters, int columnCount, int rowCount, int[] startCells)
	{
		List<string> words = new List<string>();
		List<int> searchCells = new List<int>();
		int[] offsets = new int[]{
			- columnCount,
			columnCount,
			-1,
			1,
			- columnCount - 1,
			- columnCount + 1,
			columnCount - 1,
			columnCount + 1
		};
		int cellCount = columnCount * rowCount;
		for (int startIndex = 0; startIndex < startCells.Length; startIndex++) {
			int startCell = startCells[startIndex];
			searchCells.Add(startCell);
			CharToDictionary parent = prefixes;
			bool[] isVisits = new bool[cellLetters.Length];
			string word = "";
			while (1 <= searchCells.Count) {
				int searchCell = searchCells[0];
				searchCells.Remove(searchCell);
				char letter = cellLetters[searchCell][0];
				if (!isVisits[searchCell] && parent.ContainsKey(letter)) {
					isVisits[searchCell] = true;
					parent = parent[letter];
					word += letter;
					for (int offsetIndex = 0; offsetIndex < offsets.Length; offsetIndex++) {
						int adjacent = searchCell + offsets[offsetIndex];
						if (0 <= adjacent && adjacent < cellCount) {
							if (!isVisits[adjacent]) {
								searchCells.Add(adjacent);
							}
						}
					}

				}
				if (0 == parent.Count && minimumLength <= word.Length) {
					if (words.IndexOf(word) <= -1) {
						int wordIndex;
						for (wordIndex = 0; wordIndex < words.Count; wordIndex++) {
							if (words[wordIndex].Length < word.Length) {
								break;
							}
						}
						words.Insert(wordIndex, word);
					}
				}
			}
		}
		return words;
	}
}
