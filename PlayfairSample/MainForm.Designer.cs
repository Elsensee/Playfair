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
namespace PlayfairSample
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.textBoxCipher = new System.Windows.Forms.TextBox();
			this.labelCipher = new System.Windows.Forms.Label();
			this.textBoxDecipher = new System.Windows.Forms.TextBox();
			this.labelDecipher = new System.Windows.Forms.Label();
			this.labelKeyword = new System.Windows.Forms.Label();
			this.textBoxKeyword = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBoxCipher
			// 
			this.textBoxCipher.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			resources.ApplyResources(this.textBoxCipher, "textBoxCipher");
			this.textBoxCipher.Name = "textBoxCipher";
			this.textBoxCipher.TextChanged += new System.EventHandler(this.TextBoxCipherTextChanged);
			// 
			// labelCipher
			// 
			resources.ApplyResources(this.labelCipher, "labelCipher");
			this.labelCipher.Name = "labelCipher";
			// 
			// textBoxDecipher
			// 
			this.textBoxDecipher.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			resources.ApplyResources(this.textBoxDecipher, "textBoxDecipher");
			this.textBoxDecipher.Name = "textBoxDecipher";
			this.textBoxDecipher.TextChanged += new System.EventHandler(this.TextBoxDecipherTextChanged);
			// 
			// labelDecipher
			// 
			resources.ApplyResources(this.labelDecipher, "labelDecipher");
			this.labelDecipher.Name = "labelDecipher";
			// 
			// labelKeyword
			// 
			resources.ApplyResources(this.labelKeyword, "labelKeyword");
			this.labelKeyword.Name = "labelKeyword";
			// 
			// textBoxKeyword
			// 
			this.textBoxKeyword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			resources.ApplyResources(this.textBoxKeyword, "textBoxKeyword");
			this.textBoxKeyword.Name = "textBoxKeyword";
			this.textBoxKeyword.TextChanged += new System.EventHandler(this.TextBoxKeywordTextChanged);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.textBoxKeyword);
			this.Controls.Add(this.labelKeyword);
			this.Controls.Add(this.labelDecipher);
			this.Controls.Add(this.textBoxDecipher);
			this.Controls.Add(this.labelCipher);
			this.Controls.Add(this.textBoxCipher);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox textBoxKeyword;
		private System.Windows.Forms.Label labelKeyword;
		private System.Windows.Forms.Label labelDecipher;
		private System.Windows.Forms.TextBox textBoxDecipher;
		private System.Windows.Forms.Label labelCipher;
		private System.Windows.Forms.TextBox textBoxCipher;
	}
}
