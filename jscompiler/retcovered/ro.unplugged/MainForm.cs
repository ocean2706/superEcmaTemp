using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ro.unplugged
{
	public class MainForm : Form
	{
		private TextBox textBox1;

		private TextBox textBox2;

		private Button button1;

		private TextBox textBox4;

		public MainForm()
		{
			this.InitializeComponent();
			this.button1.Click += new EventHandler(this.DoRegexpOnClick);
		}

		public void DoRegexpOnClick(object sender, EventArgs e)
		{
			try
			{
				this.textBox4.Text = "";
				List<Match> matches = new List<Match>();
				MatchCollection matchCollections = Regex.Matches(this.textBox1.Text, this.textBox2.Text);
				IEnumerator enumerator = matchCollections.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Match current = (Match)enumerator.Current;
						matches.Add(current);
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					IDisposable disposable1 = disposable;
					if (disposable != null)
					{
						disposable1.Dispose();
					}
				}
				matches.ForEach((Match Val) => {
					TextBox textBox = this.textBox4;
					textBox.Text = string.Concat(textBox.Text, Val.Value, "\r\n");
				});
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.textBox4.Text = string.Concat(exception.Message, exception.StackTrace);
			}
		}

		private void InitializeComponent()
		{
			this.textBox1 = new TextBox();
			this.textBox2 = new TextBox();
			this.button1 = new Button();
			this.textBox4 = new TextBox();
			this.textBox1.Name = "textBox1";
			this.textBox1.ForeColor = SystemColors.WindowText;
			this.textBox1.ScrollBars = ScrollBars.Vertical;
			this.textBox1.Cursor = Cursors.IBeam;
			this.textBox1.Location = new Point(24, 32);
			this.textBox1.TabIndex = 0;
			this.textBox1.Size = new System.Drawing.Size(264, 336);
			this.textBox1.BackColor = SystemColors.Window;
			this.textBox1.Multiline = true;
			this.textBox1.Text = "textBox1";
			this.textBox2.Name = "textBox2";
			this.textBox2.ForeColor = SystemColors.WindowText;
			this.textBox2.ScrollBars = ScrollBars.Vertical;
			this.textBox2.Cursor = Cursors.IBeam;
			this.textBox2.Location = new Point(304, 40);
			this.textBox2.TabIndex = 1;
			this.textBox2.Size = new System.Drawing.Size(352, 320);
			this.textBox2.BackColor = SystemColors.Window;
			this.textBox2.Multiline = true;
			this.textBox2.Text = "textBox2";
			this.button1.Name = "button1";
			this.button1.Location = new Point(432, 368);
			this.button1.TabIndex = 2;
			this.button1.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.button1.Text = "Regexp";
			this.button1.UseVisualStyleBackColor = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.ForeColor = SystemColors.WindowText;
			this.textBox4.Cursor = Cursors.IBeam;
			this.textBox4.Location = new Point(688, 48);
			this.textBox4.TabIndex = 3;
			this.textBox4.Size = new System.Drawing.Size(312, 304);
			this.textBox4.BackColor = SystemColors.Window;
			this.textBox4.Multiline = true;
			this.textBox4.Text = "textBox4";
			base.Name = "Main";
			base.ClientSize = new System.Drawing.Size(1019, 403);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox4);
			this.Text = "Main";
		}
	}
}