/*
 * Copyright (c) 2013 Oliver Schramm
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Playfair;

namespace PlayfairSample
{
	public partial class MainForm : Form
	{
		bool byProgram = false;
		bool cipher = true;
		Playfair.Playfair pf;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}

		void TextBoxKeywordTextChanged(object sender, EventArgs e)
		{
			this.pf = new Playfair.Playfair(this.textBoxKeyword.Text);
			if (cipher && !String.IsNullOrEmpty(this.textBoxCipher.Text))
			{
				this.textBoxDecipher.Text = this.pf.Cipher(this.textBoxCipher.Text);
			}
			else if (!cipher && !String.IsNullOrEmpty(this.textBoxDecipher.Text))
			{
				this.textBoxCipher.Text = this.pf.Decipher(this.textBoxDecipher.Text);
			}
		}

		void TextBoxCipherTextChanged(object sender, EventArgs e)
		{
			if (!byProgram)
			{
				byProgram = true;
				this.textBoxDecipher.Text = this.pf.Cipher(this.textBoxCipher.Text);
				cipher = true;
			}
			byProgram = false;
		}

		void TextBoxDecipherTextChanged(object sender, EventArgs e)
		{
			if (!byProgram)
			{
				byProgram = true;
				this.textBoxCipher.Text = this.pf.Decipher(this.textBoxDecipher.Text);
				cipher = false;
			}
			byProgram = false;
		}
	}
}
