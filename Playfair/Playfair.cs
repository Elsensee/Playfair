using System;
using System.Collections.Generic;
using System.Text;

namespace Playfair
{
	/// <summary>
	/// Provides (static) functions for Playfair cipher
	/// </summary>
	public class Playfair
	{
		const int L_ALPHABET = 25;
		const int A = 65;
		const int J = 74;
		const int Z = 90;
		char[] playfairLine;

		/// <summary>
		/// Creates a new instance of Playfair class.
		/// </summary>
		/// <param name="keyword">The secret keyword.</param>
		public Playfair(string keyword)
		{
			this.playfairLine = CreatePlayfairLine(keyword);
		}

		/// <summary>
		/// Ciphers a text with the provided secret keyword.
		/// </summary>
		/// <param name="plaintext">Text which should be encrypted.</param>
		/// <returns>Returns the encrypted text.</returns>
		public string Cipher(string plaintext)
		{
			return Cipher(plaintext, this.playfairLine);
		}

		/// <summary>
		/// Deciphers a text with the provided secret keyword.
		/// </summary>
		/// <param name="encryptedText">Text which should be decrypted.</param>
		/// <returns>Returns the decrypted text.</returns>
		public string Decipher(string plaintext)
		{
			return Decipher(plaintext, this.playfairLine);
		}

		/// <summary>
		/// Ciphers a text.
		/// </summary>
		/// <param name="plaintext">Text which should be encrypted.</param>
		/// <param name="playfairLine">Playfair Line which is created off the keyword.</param>
		/// <returns>Returns the encrypted text.</returns>
		public static string Cipher(string plaintext, char[] playfairLine)
		{
			char[] text = CleanString(plaintext);
			StringBuilder result = new StringBuilder(text.Length);
			int[] bufferPos = new int[2];
			int[] pos = new int[2];
			for (int i = 0; i < text.Length; i++)
			{
				// TRUE for 0, 2, 4, 6, 8, ... (btw this is faster than mod)
				if ((i & 1) == 0)
				{
					bufferPos = SearchInArray(text[i], playfairLine);
					continue;
				}
				pos = SearchInArray(text[i], playfairLine);
				// We can put two if-clauses into one :O
				if (bufferPos[0] == pos[0])
				{
					// Same row
					bufferPos[1] -= ((++bufferPos[1] * 7) >> 5) * 5;
					pos[1] -= ((++pos[1] * 7) >> 5) * 5;

					if (bufferPos[1] == pos[1])
					{
						// Last letters are identical... (possible for XX)
						bufferPos[0] -= ((++bufferPos[0] * 7) >> 5) * 5;
						pos[0] -= ((++pos[0] * 7) >> 5) * 5;
						result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
						result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
						continue;
					}
				}
				else if (bufferPos[1] == pos[1])
				{
					// Same column
					bufferPos[0] -= ((++bufferPos[0] * 7) >> 5) * 5;
					pos[0] -= ((++pos[0] * 7) >> 5) * 5;
				}
				else
				{
					// Anything else
					int buffer = bufferPos[1];
					bufferPos[1] = pos[1];
					pos[1] = buffer;
				}
				result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
				result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
			}
			return result.ToString();
		}

		/// <summary>
		/// Deciphers a text.
		/// </summary>
		/// <param name="encryptedText">Text which should be decrypted.</param>
		/// <param name="playfairLine">Playfair Line which is created off the keyword.</param>
		/// <returns>Returns the decrypted text.</returns>
		public static string Decipher(string encryptedText, char[] playfairLine)
		{
			char[] text = CleanString(encryptedText);
			StringBuilder result = new StringBuilder(text.Length);

			int[] bufferPos = new int[2];
			int[] pos = new int[2];
			for (int i = 0; i < text.Length; i++)
			{
				// TRUE for 0, 2, 4, 6, 8, ... (btw this is faster than mod)
				if ((i & 1) == 0)
				{
					bufferPos = SearchInArray(text[i], playfairLine);
					continue;
				}
				pos = SearchInArray(encryptedText[i], playfairLine);
				// We can put two if-clauses into one :O
				if (bufferPos[0] == pos[0])
				{
					// Same row
					bufferPos[1] += 9;
					bufferPos[1] -= ((bufferPos[1] * 7) >> 5) * 5;
					pos[1] += 9;
					pos[1] -= ((pos[1] * 7) >> 5) * 5;

					if (bufferPos[1] == pos[1])
					{
						// Last letters are identical... (possible for XX)
						bufferPos[0] += 9;
						bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
						pos[0] += 9;
						pos[0] -= ((pos[0] * 7) >> 5) * 5;
						result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
						result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
						continue;
					}
				}
				else if (bufferPos[1] == pos[1])
				{
					// Same column
					bufferPos[0] += 9;
					bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
					pos[0] += 9;
					pos[0] -= ((pos[0] * 7) >> 5) * 5;
				}
				else
				{
					// Anything else
					int buffer = bufferPos[1];
					bufferPos[1] = pos[1];
					pos[1] = buffer;
				}
				result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
				result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
			}
			return result.ToString();
		}

		/// <summary>
		/// Ciphers a text.
		/// </summary>
		/// <param name="plaintext">Text which should be encrypted.</param>
		/// <param name="keyword">The secret keyword.</param>
		/// <returns>Returns the encrypted text.</returns>
		public static string Cipher(string plaintext, string keyword)
		{
			return Cipher(plaintext, CreatePlayfairLine(keyword));
		}

		/// <summary>
		/// Deciphers a text.
		/// </summary>
		/// <param name="encryptedText">Text which should be decrypted.</param>
		/// <param name="keyword">The secret keyword</param>
		/// <returns>Returns the decrpyted text.</returns>
		public static string Decipher(string encryptedText, string keyword)
		{
			return Decipher(encryptedText, CreatePlayfairLine(keyword));
		}

		/// <summary>
		/// Creates a Playfair line with the provided keyword.
		/// </summary>
		/// <param name="keyword">The secret keyword.</param>
		/// <returns>Returns a char array containing the Playfair line</returns>
		public static char[] CreatePlayfairLine(string keyword)
		{
			char[] input = CleanString(keyword);
			char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
			char[] result = new char[25];
			bool[] alreadyGiven = new bool[25];

			int pointer = 0;
			int value = 0;
			for (int i = 0; i < input.Length; i++)
			{
				value = (int)input[i];
				// Replace J with I (classical)
				if (value >= J)
				{
					value--;
				}
				if (!alreadyGiven[value - A])
				{
					alreadyGiven[value - A] = true;
					result[pointer++] = input[i];
				}
			}
			// alphabet.Length = L_ALPHABET = 25 (it's faster than detecting length of that array)
			if (result.Length < L_ALPHABET)
			{
				for (int i = 0; i < L_ALPHABET; i++)
				{
					if (!alreadyGiven[i])
					{
						result[pointer++] = alphabet[i];
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Cleans a string by converting into upper writing and converting umlauts into its two-chars equals.
		/// </summary>
		/// <param name="text">The text which schould be cleaned.</param>
		/// <returns>Cleaned string.</returns>
		private static char[] CleanString(string text)
		{
			char[] input = text.ToUpper().ToCharArray();
			List<char> result = new List<char>(input.Length);
			for (int i = 0; i < input.Length; i++)
			{
				// Convert umlauts into two characters
				switch (input[i])
				{
					// Ä or Æ -> AE
					case 'Ä':
					case 'Æ':
						result.Add('A');
						result.Add('E');
						continue;
					// Ö or Ø -> OE
					case 'Ö':
					case 'Ø':
						result.Add('O');
						result.Add('E');
						continue;
					// Ü -> UE
					case 'Ü':
						result.Add('U');
						result.Add('E');
						continue;
					// Å -> AA
					case 'Å':
						result.Add('A');
						result.Add('A');
						continue;
					// ß -> SS
					case 'ß':
						result.Add('S');
						result.Add('S');
						continue;
				}
				// We will remove all other characters
				if ((int)input[i] < A || (int)input[i] > Z)
				{
					continue;
				}
				result.Add(input[i]);
			}
			return result.ToArray();
		}

		/// <summary>
		/// Searches in an array
		/// </summary>
		/// <param name="c">The char which I'm looking for...</param>
		/// <param name="square">The array in which I'm looking for the char c...</param>
		/// <returns>Returns the position in a two elements big int-array.</returns>
		private static int[] SearchInArray(char c, char[] array)
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (array[(i * 5) + j] == c)
					{
						return new int[] { i, j };
					}
				}
			}
			return null;
		}
	}
}
