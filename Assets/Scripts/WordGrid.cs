using System.Collections.Generic;  // List
using System;  // String

/**
 * http://stackoverflow.com/questions/647533/recursive-generic-types
 */
public class CharToDictionary : Dictionary<char, CharToDictionary>{}

public class WordGrid
{
	public CharToDictionary prefixes;
	public int minimumLength = 1;
	public char endOfWord = ';';
	public string[] cellLetters;
	public int columnCount;
	public int rowCount;
	public Dictionary<string, List<int>> wordPaths = new Dictionary<
		string, List<int>>();
	private int cellCount;
	private int[] offsets;

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

	public void SetSize(int columns, int rows)
	{
		columnCount = columns;
		rowCount = rows;
		cellCount = columnCount * rowCount;
		offsets = new int[]{
			- columnCount,
			columnCount,
			-1,
			1,
			- columnCount - 1,
			- columnCount + 1,
			columnCount - 1,
			columnCount + 1
		};
	}

	/**
	 * Side-effect:  set wordPaths[word] to path of the word.
	 *
	 * Like Darius Bacon's without coroutines or nested return type.
	 * More portable.
	 * http://stackoverflow.com/questions/746082/how-to-find-list-of-possible-words-from-a-letter-matrix-boggle-solver
	 */
	private void ExtendWords(List<string> words, List<int> path, 
				CharToDictionary parent, string word)
	{
		if (parent.ContainsKey(endOfWord) 
		&& minimumLength <= word.Length
		&& words.IndexOf(word) <= -1) {
			int wordIndex;
			for (wordIndex = 0; wordIndex < words.Count; 
					wordIndex++) {
				if (words[wordIndex].Length < word.Length) {
					break;
				}
			}
			words.Insert(wordIndex, word);
			wordPaths[word] = path;
		}
		int cell = path[path.Count - 1];
		for (int offsetIndex = 0; offsetIndex < offsets.Length; 
				offsetIndex++) {
			int adjacent = cell + offsets[offsetIndex];
			int columnOffset = ((cell % columnCount) - 
				(adjacent % columnCount));
			if (0 <= adjacent && adjacent < cellCount
			&& -1 <= columnOffset && columnOffset <= 1) {
				if (null != cellLetters[adjacent] 
				&& !path.Contains(adjacent)) {
					char letter = cellLetters[
						adjacent][0];
					if (parent.ContainsKey(letter)) {
						List<int> nextCells = 
							new List<int>(
							path);
						nextCells.Add(adjacent);
						ExtendWords(words, 
							nextCells,
							parent[letter],
							word + letter);
					}
				}
			}
		}
	}

	/**
	 * List cells to search.  If a word is found, append it.  Search adjacent cells.
	 * @param	cellLetters	Not handled:  Special case "qu" might be one cell.  Expects each cell leter is not null.
	 * @return	Longest first down to minimum length.
	 * Example @see Editor/Tests/TestWordGrid
	 * References:
	 * http://stackoverflow.com/questions/746082/how-to-find-list-of-possible-words-from-a-letter-matrix-boggle-solver
	 * http://exceptional-code.blogspot.com/2012/02/solving-boggle-game-recursion-prefix.html
	 */
	public List<string> FindWords(int startCell)
	{
		List<string> words = new List<string>();
		List<int> path = new List<int>(){startCell};
		string prefix = cellLetters[startCell];
		char letter = prefix[0];
		if (prefixes.ContainsKey(letter)) {
			ExtendWords(words, path, prefixes[letter], prefix);
		}
		return words;
	}

	/**
	 * Starting with one of the indexes.
	 * Like Boggle:
	 * Searching adjacent letters.
	 * Using a letter only once.
	 * If none found, return "".
	 * Example @see TestWordGrid
	 */
	public string FindLongestWord(int[] startCells)
	{
		string word = "";
		for (int index = 0; index < startCells.Length; index++) {
			List<string> words = FindWords(startCells[index]);
			if (1 <= words.Count) {
				if (word.Length < words[0].Length) {
					word = words[0];
				}
			}
		}
		return word;
	}

	/**
	 * Longest set of words with non-overlapping paths
	 * that start at these cells.
	 */
	public string FindLongestSet(int[] startCells)
	{
		string message = "";
		if (1 <= startCells.Length) {
			List<string> words;
			List<string>[] lists = new List<string>[startCells.Length];
			for (int index = 0; index < startCells.Length; index++) {
				lists[index] = FindWords(startCells[index]);
				words = lists[index];
				message = string.Join(", ", words.ToArray());
			}
		}
		return message;
	}
}
