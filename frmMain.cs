using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace SteganoWave
{
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtSrcFile;
		private System.Windows.Forms.Button btnSrcFile;
		private System.Windows.Forms.Button btnDstFile;
		private System.Windows.Forms.TextBox txtDstFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnHide;
		private System.Windows.Forms.TabControl tabCtl;
		private System.Windows.Forms.TabPage tabHide;
		private System.Windows.Forms.TabPage tabExtract;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.TextBox txtExtractedMessage;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Label label1;
        private Label label3;
        private Label label4;
        private IContainer components;

		public frmMain(){
			InitializeComponent();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.txtSrcFile = new System.Windows.Forms.TextBox();
            this.btnSrcFile = new System.Windows.Forms.Button();
            this.btnDstFile = new System.Windows.Forms.Button();
            this.txtDstFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnHide = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tabCtl = new System.Windows.Forms.TabControl();
            this.tabHide = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tabExtract = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExtractedMessage = new System.Windows.Forms.TextBox();
            this.btnExtract = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tabCtl.SuspendLayout();
            this.tabHide.SuspendLayout();
            this.tabExtract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSrcFile
            // 
            this.errorProvider.SetIconAlignment(this.txtSrcFile, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtSrcFile.Location = new System.Drawing.Point(121, 18);
            this.txtSrcFile.Name = "txtSrcFile";
            this.txtSrcFile.Size = new System.Drawing.Size(318, 20);
            this.txtSrcFile.TabIndex = 0;
            this.txtSrcFile.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // btnSrcFile
            // 
            this.btnSrcFile.Location = new System.Drawing.Point(441, 18);
            this.btnSrcFile.Name = "btnSrcFile";
            this.btnSrcFile.Size = new System.Drawing.Size(85, 24);
            this.btnSrcFile.TabIndex = 1;
            this.btnSrcFile.Text = "Browse...";
            this.btnSrcFile.Click += new System.EventHandler(this.btnSrcFile_Click);
            // 
            // btnDstFile
            // 
            this.btnDstFile.Location = new System.Drawing.Point(495, 18);
            this.btnDstFile.Name = "btnDstFile";
            this.btnDstFile.Size = new System.Drawing.Size(85, 24);
            this.btnDstFile.TabIndex = 1;
            this.btnDstFile.Text = "Browse...";
            this.btnDstFile.Click += new System.EventHandler(this.btnDstFile_Click);
            // 
            // txtDstFile
            // 
            this.errorProvider.SetIconAlignment(this.txtDstFile, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtDstFile.Location = new System.Drawing.Point(162, 18);
            this.txtDstFile.Name = "txtDstFile";
            this.txtDstFile.Size = new System.Drawing.Size(319, 20);
            this.txtDstFile.TabIndex = 0;
            this.txtDstFile.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(42, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "Save Result as";
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(245, 196);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(137, 26);
            this.btnHide.TabIndex = 3;
            this.btnHide.Text = "Hide Message";
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // txtMessage
            // 
            this.errorProvider.SetIconAlignment(this.txtMessage, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtMessage.Location = new System.Drawing.Point(162, 58);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(319, 111);
            this.txtMessage.TabIndex = 4;
            this.txtMessage.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // tabCtl
            // 
            this.tabCtl.Controls.Add(this.tabHide);
            this.tabCtl.Controls.Add(this.tabExtract);
            this.tabCtl.Location = new System.Drawing.Point(12, 98);
            this.tabCtl.Name = "tabCtl";
            this.tabCtl.SelectedIndex = 0;
            this.tabCtl.Size = new System.Drawing.Size(606, 273);
            this.tabCtl.TabIndex = 5;
            // 
            // tabHide
            // 
            this.tabHide.Controls.Add(this.label3);
            this.tabHide.Controls.Add(this.txtMessage);
            this.tabHide.Controls.Add(this.btnHide);
            this.tabHide.Controls.Add(this.label2);
            this.tabHide.Controls.Add(this.btnDstFile);
            this.tabHide.Controls.Add(this.txtDstFile);
            this.tabHide.Location = new System.Drawing.Point(4, 22);
            this.tabHide.Name = "tabHide";
            this.tabHide.Size = new System.Drawing.Size(598, 247);
            this.tabHide.TabIndex = 0;
            this.tabHide.Text = "Hide";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Hide Message";
            // 
            // tabExtract
            // 
            this.tabExtract.Controls.Add(this.label4);
            this.tabExtract.Controls.Add(this.txtExtractedMessage);
            this.tabExtract.Controls.Add(this.btnExtract);
            this.tabExtract.Location = new System.Drawing.Point(4, 22);
            this.tabExtract.Name = "tabExtract";
            this.tabExtract.Size = new System.Drawing.Size(598, 247);
            this.tabExtract.TabIndex = 1;
            this.tabExtract.Text = "Extract";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Extracted Message";
            // 
            // txtExtractedMessage
            // 
            this.errorProvider.SetIconAlignment(this.txtExtractedMessage, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtExtractedMessage.Location = new System.Drawing.Point(147, 17);
            this.txtExtractedMessage.Multiline = true;
            this.txtExtractedMessage.Name = "txtExtractedMessage";
            this.txtExtractedMessage.Size = new System.Drawing.Size(316, 103);
            this.txtExtractedMessage.TabIndex = 7;
            this.txtExtractedMessage.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(224, 157);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(137, 26);
            this.btnExtract.TabIndex = 6;
            this.btnExtract.Text = "Extract Message";
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Wave File";
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(647, 392);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabCtl);
            this.Controls.Add(this.btnSrcFile);
            this.Controls.Add(this.txtSrcFile);
            this.Name = "frmMain";
            this.Text = "SteganoWave";
            this.tabCtl.ResumeLayout(false);
            this.tabHide.ResumeLayout(false);
            this.tabHide.PerformLayout();
            this.tabExtract.ResumeLayout(false);
            this.tabExtract.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void btnSrcFile_Click(object sender, System.EventArgs e) {
			//select the clean carrier file
			OpenFileDialog dlg = new OpenFileDialog();
			GetFileName(dlg, txtSrcFile, true);
		}

		private void btnDstFile_Click(object sender, System.EventArgs e) {
			//select a filename to save the carrier file
			SaveFileDialog dlg = new SaveFileDialog();
			GetFileName(dlg, txtDstFile, true);
		}
		
		private void btnHide_Click(object sender, System.EventArgs e) {
			//hide the message inside the carrier wave
            
			if(txtSrcFile.Text.Length == 0){
				errorProvider.SetError(txtSrcFile, "You forgot to choose a carrier file.");
			}
			else if(txtDstFile.Text.Length == 0){
				errorProvider.SetError(txtDstFile, "The resulting carrier file must be saved somewhere.");
			}
			else if(txtMessage.Text.Length == 0){
				errorProvider.SetError(txtMessage, "What am I supposed to hide?");
			}
			else{

				Stream sourceStream = null;
				FileStream destinationStream = null;
				WaveStream audioStream = null;
			
				//create a stream that contains the message, preceeded by its length
				Stream messageStream = GetMessageStream();
			
				try {
				    //use a .wav file as the carrier
					sourceStream = new FileStream(txtSrcFile.Text, FileMode.Open);
                    
                    this.Cursor = Cursors.WaitCursor;
				
					//create an empty file for the carrier wave
					destinationStream = new FileStream(txtDstFile.Text, FileMode.Create);
				
					//copy the carrier file's header
					audioStream = new WaveStream(sourceStream, destinationStream);
					if (audioStream.Length <= 0){
						throw new Exception("Invalid WAV file");
					}

					//hide the message
					WaveUtility utility = new WaveUtility(audioStream, destinationStream);
					utility.HidePhaseCoding(messageStream);
                    MessageBox.Show("Message encoded!");
				}
				catch(Exception ex) {
					this.Cursor = Cursors.Default;
					MessageBox.Show(ex.Message);
				}
				finally{
					if(messageStream != null){ messageStream.Close(); }
					if(audioStream != null){ audioStream.Close(); }
					if(sourceStream != null){ sourceStream.Close(); }
					if(destinationStream != null){ destinationStream.Close(); }
					this.Cursor = Cursors.Default;
				}
			}
		}

		private void btnExtract_Click(object sender, System.EventArgs e) {
			//extract the message from the carrier wave
			
			if(txtSrcFile.Text.Length == 0){
				errorProvider.SetError(txtSrcFile, "You forgot to choose a carrier file.");
			}
			else{

				this.Cursor = Cursors.WaitCursor;
			
				FileStream sourceStream = null;
				WaveStream audioStream = null;
				//create an empty stream to receive the extracted message
				MemoryStream messageStream = new MemoryStream();
			
				try {
					//open the carrier file
					sourceStream = new FileStream(txtSrcFile.Text, FileMode.Open);
					audioStream = new WaveStream(sourceStream);
					WaveUtility utility = new WaveUtility(audioStream);
			
					//exctract the message from the carrier wave
					utility.ExtractPhaseCoding(messageStream);
				
					messageStream.Seek(0, SeekOrigin.Begin);
					txtExtractedMessage.Text = new StreamReader(messageStream).ReadToEnd();					
				}
				catch(Exception ex) {
					this.Cursor = Cursors.Default;
					MessageBox.Show(ex.Message);
				}
				finally{
					if(messageStream != null){ messageStream.Close(); }
					if(audioStream != null){ audioStream.Close(); }
					if(sourceStream != null){ sourceStream.Close(); }
					this.Cursor = Cursors.Default;
				}
			}
		}

		/// <summary>Open a FileDialog and write the selected filename into a TextBox</summary>
		/// <param name="dialog">The OPen/Save-FileDialog</param>
		/// <param name="control">The corresponding TextBox</param>
		/// <param name="useFilter">Allow only .wav files</param>
		private void GetFileName(FileDialog dialog, TextBox control, bool useFilter){
			if(useFilter){ dialog.Filter = "Wave Audio (*.wav)|*.wav"; }
			if( dialog.ShowDialog(this) == DialogResult.OK){
				control.Text = dialog.FileName;
			}
		}

		/// <summary>Write length an content of the message file/text into a stream</summary>
		/// <returns></returns>
		private Stream GetMessageStream(){
			BinaryWriter messageWriter = new BinaryWriter(new MemoryStream());
			messageWriter.Write(txtMessage.Text.Length);
			messageWriter.Write(Encoding.ASCII.GetBytes(txtMessage.Text));
			messageWriter.Seek(0, SeekOrigin.Begin);
			return messageWriter.BaseStream;			
		}

		private void textField_TextChanged(object sender, System.EventArgs e) {
			errorProvider.SetError((Control)sender, String.Empty);
		}
	}
}
