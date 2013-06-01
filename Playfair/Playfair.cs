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
		const string ALPHABET = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		const int A = 65;
		const int E = 69;
		const int J = 74;
		const int O = 79;
		const int S = 83;
		const int U = 85;
		const int Z = 90;
		const int Ä = 196;
		const int Å = 197;
		const int Æ = 198;
		const int Ö = 214;
		const int Ø = 216;
		const int Ü = 220;
		const int ß = 223;
		char[,] playfairSquare = new char[5, 5];

		public char[,] PlayfairSquare
		{
			get
			{
				return this.playfairSquare;
			}
		}

		/// <summary>
		/// Creates a new instance of Playfair class.
		/// </summary>
		/// <param name="keyword">The secret keyword.</param>
		public Playfair(string keyword)
		{
			this.playfairSquare = CreatePlayfairSquare(keyword);
		}

		/// <summary>
		/// Ciphers a text with the provided secret keyword.
		/// </summary>
		/// <param name="plaintext">Text which should be encrypted.</param>
		/// <returns>Returns the encrypted text.</returns>
		public string Cipher(string plaintext)
		{
			plaintext = plaintext.ToUpper();
			int corrections = 0;
			if (plaintext.EndsWith("\0"))
			{
				corrections = -1;
			}
			List<char> chars = new List<char>(plaintext.Length + corrections + ((plaintext.Length + corrections) & 1));
			#region plaintext preperations
			bool inUmlaut = false;
			// We don't know how long the text could be, but 2 BILLION characters.. wow.. that MUST be enough
			// If not, somebody wants to cipher the bible.. Don't even think about it!
			for (int i = 0; i < plaintext.Length; i++)
			{
				char c = plaintext[i];

				// Converting umlauts into two chars
				switch ((int)c)
				{
					// Ä or AE -> AE
					case 196:
					case 198:
						if (!inUmlaut)
						{
							c = 'A';
							i--;
						}
						else
						{
							c = 'E';
						}
						inUmlaut = !inUmlaut;
						break;
					// Ö -> OE
					case 214:
						if (!inUmlaut)
						{
							c = 'O';
							i--;
						}
						else
						{
							c = 'E';
						}
						inUmlaut = !inUmlaut;
						break;
					// Ü -> UE
					case 220:
						if (!inUmlaut)
						{
							c = 'U';
							i--;
						}
						else
						{
							c = 'E';
						}
						inUmlaut = !inUmlaut;
						break;
					// ß -> SS
					case 223:
						if (!inUmlaut)
							i--;
						c = 'S';
						inUmlaut = !inUmlaut;
						break;
				}
				if ((int)c < A || (int)c > Z)
				{
					corrections--;
					continue;
				}

				// Tricky.. what if the char before is the same and its the last char of this pair?
				if ((chars.Count & 1) == 1 && chars[chars.Count - 1] == c && chars[chars.Count - 1] != 'X')
				{
					chars.Add('X');
					i--;
					inUmlaut = !inUmlaut;
					continue;
				}

				if (inUmlaut)
					corrections++;

				chars.Add(c);
			}

			if (((plaintext.Length + corrections) & 1) == 1)
				chars.Add('X');
			#endregion
			StringBuilder result = new StringBuilder(chars.Count);
			#region Encryption
			int[] bufferPos = new int[2];
			int[] pos = new int[2];
			bool even = false;
			for (int i = 0; i < chars.Count; i++)
			{
				if (!even)
				{
					even = true;
					bufferPos = SearchInSquare(chars[i], this.playfairSquare);
					continue;
				}
				even = false;
				pos = SearchInSquare(chars[i], this.playfairSquare);
				// Last letters are identical... (possible for XX)
				if (bufferPos[0] == pos[0] && bufferPos[1] == pos[1])
				{
					bufferPos[0] -= ((++bufferPos[0] * 7) >> 5) * 5;
					bufferPos[1] -= ((++bufferPos[1] * 7) >> 5) * 5;
					pos[0] -= ((++pos[0] * 7) >> 5) * 5;
					pos[1] -= ((++pos[1] * 7) >> 5) * 5;
				}
				// Same row
				else if (bufferPos[0] == pos[0])
				{
					bufferPos[1] -= ((++bufferPos[1] * 7) >> 5) * 5;
					pos[1] -= ((++pos[1] * 7) >> 5) * 5;
				}
				// Same column
				else if (bufferPos[1] == pos[1])
				{
					bufferPos[0] -= ((++bufferPos[0] * 7) >> 5) * 5;
					pos[0] -= ((++pos[0] * 7) >> 5) * 5;
				}
				// Anything else
				else
				{
					int buffer = bufferPos[1]; // Again...
					bufferPos[1] = pos[1];
					pos[1] = buffer;
				}
				result.Append(this.playfairSquare[bufferPos[0], bufferPos[1]]);
				result.Append(this.playfairSquare[pos[0], pos[1]]);
			}
			#endregion
			return result.ToString();
		}

		/// <summary>
		/// Deciphers a text with the provided secret keyword.
		/// </summary>
		/// <param name="encryptedText">Text which should be decrypted.</param>
		/// <returns>Returns the decrypted text.</returns>
		public string Decipher(string encryptedText)
		{
			encryptedText = encryptedText.ToUpper();
			StringBuilder result = new StringBuilder(encryptedText.Length);

			int[] bufferPos = new int[2];
			int[] pos = new int[2];
			bool even = false;
			for (int i = 0; i < encryptedText.Length; i++)
			{
				if (!even)
				{
					even = true;
					bufferPos = SearchInSquare(encryptedText[i], this.playfairSquare);
					continue;
				}
				even = false;
				pos = SearchInSquare(encryptedText[i], this.playfairSquare);
				// Last letters are identical... (possible for XX)
				if (bufferPos[0] == pos[0] && bufferPos[1] == pos[1])
				{
					bufferPos[0] += 9;
					bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
					bufferPos[1] += 9;
					bufferPos[1] -= ((bufferPos[1] * 7) >> 5) * 5;
					pos[0] += 9;
					pos[0] -= ((pos[0] * 7) >> 5) * 5;
					pos[1] += 9;
					pos[1] -= ((pos[1] * 7) >> 5) * 5;
				}
				// Same row
				else if (bufferPos[0] == pos[0])
				{
					bufferPos[1] += 9;
					bufferPos[1] -= ((bufferPos[1] * 7) >> 5) * 5;
					pos[1] += 9;
					pos[1] -= ((pos[1] * 7) >> 5) * 5;
				}
				// Same column
				else if (bufferPos[1] == pos[1])
				{
					bufferPos[0] += 9;
					bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
					pos[0] += 9;
					pos[0] -= ((pos[0] * 7) >> 5) * 5;
				}
				// Anything else
				else
				{
					int buffer = bufferPos[1]; // Again...
					bufferPos[1] = pos[1];
					pos[1] = buffer;
				}
				result.Append(this.playfairSquare[bufferPos[0], bufferPos[1]]);
				result.Append(this.playfairSquare[pos[0], pos[1]]);
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
			return new Playfair(keyword).Cipher(plaintext);
		}

		/// <summary>
		/// Deciphers a text.
		/// </summary>
		/// <param name="encryptedText">Text which should be decrypted.</param>
		/// <param name="keyword">The secret keyword</param>
		/// <returns>Returns the decrpyted text.</returns>
		public static string Decipher(string encryptedText, string keyword)
		{
			return new Playfair(keyword).Decipher(encryptedText);
		}

		/// <summary>
		/// Searches in a Square
		/// </summary>
		/// <param name="c">The char which I'm looking for...</param>
		/// <param name="square">The square in which I'm looking for the char c...</param>
		/// <returns>Returns the position in a two elements big byte-array.</returns>
		private int[] SearchInSquare(char c, char[,] square)
		{
			for (int i = 0; i < square.GetLength(0); i++)
			{
				for (int j = 0; j < square.GetLength(1); j++)
				{
					if (square[i, j] == c)
						return new int[] { i, j };
				}
			}
			return null;
		}

		/// <summary>
		/// Creates a Playfair square with the provided keyword.
		/// </summary>
		/// <param name="keyword">The secret keyword.</param>
		/// <returns>Returns a char array containing the Playfair square</returns>
		private char[,] CreatePlayfairSquare(string keyword)
		{
			char[,] result = new char[5, 5];
			char[] playfairLine = ToNoRepeatCharArray(CleanString(keyword) + ALPHABET);

			if (playfairLine.Length > 25)
			{
				throw new ApplicationException("Internal Error! Array too big! (more than 25 elements)");
			}
			// Even if we don't need that much... It costs much time to convert it into integer... 
			// So we have to draw a line... Less time or less memory?
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					result[i, j] = playfairLine[(i * 5) + j];
				}
			}
			return result;
		}

		/// <summary>
		/// Creates a char array from the string text in which every char shows up only one time and the J is replaced with I.
		/// </summary>
		/// <param name="text">The text which should be converted into a char array without repeations.</param>
		/// <returns>Returns a char array without any repeations.</returns>
		private char[] ToNoRepeatCharArray(string text)
		{
			// Let's reserve some space because changing size of arrays is slow and needs more space
			char[] input = text.ToCharArray();
			char[] result = new char[25];
			bool[] alreadyGiven = new bool[25];

			int pointer = 0;
			int value = 0;
			for (int i = 0; i < text.Length; i++)
			{
				value = (int)input[i];
				// Replace J with I (classical...)
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
			// Hopefully this condition will never be true...
			if (pointer < 25)
			{
				Array.Resize<char>(ref result, pointer);
			}

			return result;
		}

		/// <summary>
		/// Cleans a string by converting into upper writing and converting umlauts into its two-chars equals.
		/// </summary>
		/// <param name="text">The text which schould be cleaned.</param>
		/// <returns>Cleaned string.</returns>
		private string CleanString(string text)
		{
			char[] input = text.ToUpper().ToCharArray();
			StringBuilder result = new StringBuilder(text.Length);
			bool inUmlaut = false;
			int c;
			for (int i = 0; i < text.Length; i++)
			{
				c = (int)input[i];

				// Convert umlauts into two characters
				switch (c)
				{
					// Ä or Æ -> AE
					case Ä:
					case Æ:
						if (!inUmlaut)
						{
							c = A;
							i--;
						}
						else
						{
							c = E;
						}
						inUmlaut = !inUmlaut;
						break;
					// Ö or Ø -> OE
					case Ö:
					case Ø:
						if (!inUmlaut)
						{
							c = O;
							i--;
						}
						else
						{
							c = E;
						}
						inUmlaut = !inUmlaut;
						break;
					// Ü -> UE
					case Ü:
						if (!inUmlaut)
						{
							c = U;
							i--;
						}
						else
						{
							c = E;
						}
						inUmlaut = !inUmlaut;
						break;
					// Å -> AA
					case Å:
						if (!inUmlaut)
						{
							i--;
						}
						c = A;
						inUmlaut = !inUmlaut;
						break;
					// ß -> SS
					case ß:
						if (!inUmlaut)
						{
							i--;
						}
						c = S;
						inUmlaut = !inUmlaut;
						break;
				}
				// We will remove all other characters
				if (c < A || c > Z)
				{
					continue;
				}
				result.Append((char)c);
			}
			return result.ToString();
		}
	}
}
