using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace DMR
{
	public class FirmwareLoader_DM32 : Form
	{
		private SerialPort commPort;

		public static Dictionary<string, string> StringsDict = new Dictionary<string, string>();

		private IContainer components = null;

		private Label lblMessage;

		private ProgressBar Progress;

		private Button btnProgram;

		public FirmwareLoader_DM32()
		{
			InitializeComponent();
			base.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
		}

		private void btnProgram_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = Settings.dicCommon["FirmwareFilefilterBinary"];
			openFileDialog.Title = Settings.dicCommon["FirmwareSelectorTitle"];
			openFileDialog.InitialDirectory = IniFileUtils.getProfileStringWithDefault("Setup", "LastFirmwareLocation_DM32", null);
			if (DialogResult.OK != openFileDialog.ShowDialog())
			{
				return;
			}
			lblMessage.Text = "";
			Progress.Value = 0;
			if (!setupCommPort())
			{
				return;
			}
			try
			{
				commPort.Write(new byte[1] { 82 }, 0, 1);
				if (!compareBuffers(readBytes(1), new byte[1] { 6 }))
				{
					throw new Exception("No response from the radio");
				}
				commPort.Write(new byte[5] { 77, 0, 0, 0, 1 }, 0, 5);
				if (!compareBuffers(readBytes(3), new byte[3] { 77, 1, 9 }))
				{
					throw new Exception("Incorrect response from the radio");
				}
				commPort.Write(new byte[1] { 6 }, 0, 1);
				if (!compareBuffers(readBytes(9), Encoding.ASCII.GetBytes("BFUV32-V2")))
				{
					throw new Exception("Wrong radio type is connected");
				}
				commPort.Write(new byte[5] { 69, 0, 0, 0, 0 }, 0, 5);
				if (!compareBuffers(readBytes(1), new byte[1] { 6 }))
				{
					throw new Exception("Incorrect response from the radio");
				}
				commPort.Write(new byte[1] { 49 }, 0, 1);
				if (!compareBuffers(readBytes(1), new byte[1] { 67 }))
				{
					throw new Exception("Incorrect response from the radio");
				}
				commPort.Write(new byte[1] { 1 }, 0, 1);
				if (!compareBuffers(readBytes(1), new byte[1] { 67 }))
				{
					throw new Exception("Incorrect response from the radio");
				}
				int num = 1;
				long length = new FileInfo(openFileDialog.FileName).Length;
				long num2 = length / 1024;
				if (length % 1024 != 0L)
				{
					num2++;
				}
				long num3 = 0L;
				using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
				{
					IniFileUtils.WriteProfileString("Setup", "LastFirmwareLocation_DM32", Path.GetDirectoryName(openFileDialog.FileName));
					byte[] array = new byte[9];
					fileStream.Read(array, 0, 9);
					try
					{
						if (Encoding.UTF8.GetString(array) == "BFUV32-V2")
						{
							fileStream.Seek(256L, SeekOrigin.Begin);
						}
						else
						{
							fileStream.Seek(0L, SeekOrigin.Begin);
						}
					}
					catch
					{
						fileStream.Seek(0L, SeekOrigin.Begin);
					}
					lblMessage.Text = "Erasing...";
					commPort.Write(new byte[2] { 0, 255 }, 0, 2);
					byte[] array2 = Encoding.ASCII.GetBytes("OpenDM32\0" + length);
					Array.Resize(ref array2, 128);
					commPort.Write(array2, 0, array2.Length);
					byte[] bytes = BitConverter.GetBytes(crc16Xmodem(array2));
					Array.Reverse(bytes);
					commPort.Write(bytes, 0, 2);
					if (!compareBuffers(waitForEraseCommandResponse(2), new byte[2] { 6, 67 }))
					{
						throw new Exception("Incorrect response from the radio");
					}
					_ = Progress.Value;
					commPort.ReadTimeout = 500;
					lblMessage.Text = "Writing firmware to radio";
					int num4 = 0;
					int value = Progress.Value;
					while (true)
					{
						byte[] array3 = new byte[1024];
						int num5 = fileStream.Read(array3, 0, 1024);
						if (num5 == 0)
						{
							break;
						}
						if (num5 < 1024)
						{
							for (int i = num5; i < 1024; i++)
							{
								array3[i] = 0;
							}
						}
						commPort.Write(new byte[1] { 2 }, 0, 1);
						if (!compareBuffers(readBytes(1), new byte[1] { 67 }))
						{
							throw new Exception("Incorrect response from the radio");
						}
						commPort.Write(new byte[1] { (byte)num }, 0, 1);
						commPort.Write(new byte[1] { (byte)(255 - num) }, 0, 1);
						int num6 = (int)(num3 * (100 - value) / num2) + value;
						if (num6 != num4 && num6 <= Progress.Maximum)
						{
							num4 = num6;
							Progress.Value = num6;
						}
						commPort.Write(array3, 0, array3.Length);
						bytes = BitConverter.GetBytes(crc16Xmodem(array3));
						Array.Reverse(bytes);
						commPort.Write(bytes, 0, 2);
						if (!compareBuffers(readBytes(1), new byte[1] { 6 }))
						{
							throw new Exception("Incorrect response from the radio");
						}
						num++;
						if (num > 255)
						{
							num = 0;
						}
						num3++;
					}
				}
				commPort.Write(new byte[1] { 4 }, 0, 1);
				lblMessage.Text = "Upload complete";
			}
			catch (Exception ex)
			{
				if (ex is TimeoutException)
				{
					lblMessage.Text = "ERROR: The radio did not respond";
				}
				else
				{
					lblMessage.Text = "ERROR: " + ex.Message;
				}
			}
			Progress.Value = 0;
			btnProgram.Enabled = true;
			if (commPort == null)
			{
				return;
			}
			try
			{
				if (commPort.IsOpen)
				{
					commPort.Close();
				}
			}
			catch (Exception)
			{
			}
			commPort = null;
		}

		private byte[] waitForEraseCommandResponse(int length)
		{
			byte[] array = new byte[length];
			int num = 0;
			int num2 = 20;
			commPort.ReadTimeout = 1000;
			while (num < length)
			{
				try
				{
					int num3 = commPort.Read(array, num, length - num);
					if (num3 <= 0)
					{
						return null;
					}
					num += num3;
				}
				catch (Exception)
				{
					num2--;
					Progress.Value++;
					if (num2 == 0)
					{
						throw new Exception("No reponse from the radio");
					}
				}
			}
			return array;
		}

		private byte[] readBytes(int length)
		{
			byte[] array = new byte[length];
			int num;
			for (int i = 0; i < length; i += num)
			{
				num = commPort.Read(array, i, length - i);
				if (num <= 0)
				{
					return null;
				}
			}
			return array;
		}

		private bool compareBuffers(byte[] actual, byte[] expected)
		{
			if (actual.Length != expected.Length)
			{
				return false;
			}
			for (int i = 0; i < actual.Length; i++)
			{
				if (actual[i] != expected[i])
				{
					return false;
				}
			}
			return true;
		}

		private ushort crc16Xmodem(byte[] data)
		{
			ushort num = 0;
			foreach (byte b in data)
			{
				num ^= (ushort)(b << 8);
				for (int j = 0; j < 8; j++)
				{
					num = (((num & 0x8000) == 0) ? ((ushort)(num << 1)) : ((ushort)((num << 1) ^ 0x1021)));
					num &= 0xFFFF;
				}
			}
			return num;
		}

		private bool setupCommPort()
		{
			if (commPort != null)
			{
				try
				{
					if (commPort.IsOpen)
					{
						commPort.Close();
					}
				}
				catch (Exception)
				{
				}
				commPort = null;
			}
			try
			{
				string text = null;
				text = SetupDiWrap.ComPortNameFromFriendlyNamePrefix("OpenGD77");
				if (text == null)
				{
					text = IniFileUtils.getProfileStringWithDefault("Setup", "LastCommPort", "");
				}
				if (text == null || text == "")
				{
					CommPortSelector commPortSelector = new CommPortSelector();
					if (DialogResult.OK != commPortSelector.ShowDialog())
					{
						return false;
					}
					text = SetupDiWrap.ComPortNameFromFriendlyNamePrefix(commPortSelector.SelectedPort);
					IniFileUtils.WriteProfileString("Setup", "LastCommPort", text);
				}
				if (text == null)
				{
					MessageBox.Show(StringsDict["Please_connect_the_radio,_and_try_again."], StringsDict["Radio_not_detected."]);
				}
				else
				{
					commPort = new SerialPort(text, 115200, Parity.None, 8, StopBits.One);
					commPort.ReadTimeout = 1000;
				}
			}
			catch (Exception)
			{
				commPort = null;
				SystemSounds.Hand.Play();
				MessageBox.Show(StringsDict["Failed_to_open_comm_port"], StringsDict["Error"]);
				IniFileUtils.WriteProfileString("Setup", "LastCommPort", "");
				return false;
			}
			try
			{
				commPort.Open();
			}
			catch (Exception)
			{
				SystemSounds.Hand.Play();
				MessageBox.Show(StringsDict["Comm_port_not_available"]);
				IniFileUtils.WriteProfileString("Setup", "LastCommPort", "");
				commPort = null;
				return false;
			}
			return true;
		}

		private void FirmwareLoader_DM32_Load(object sender, EventArgs e)
		{
			Settings.AttachHelpHandlers(base.Controls);
			Settings.UpdateComponentTextsFromLanguageXmlData(this);
			Settings.ReadCommonsForSectionIntoDictionary(StringsDict, "OpenGD77Form");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.lblMessage = new System.Windows.Forms.Label();
			this.Progress = new System.Windows.Forms.ProgressBar();
			this.btnProgram = new System.Windows.Forms.Button();
			base.SuspendLayout();
			this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblMessage.Font = new System.Drawing.Font("Arial", 9f);
			this.lblMessage.Location = new System.Drawing.Point(12, 90);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(474, 24);
			this.lblMessage.TabIndex = 13;
			this.Progress.Location = new System.Drawing.Point(12, 65);
			this.Progress.Name = "Progress";
			this.Progress.Size = new System.Drawing.Size(474, 13);
			this.Progress.TabIndex = 12;
			this.btnProgram.Font = new System.Drawing.Font("Arial", 9f);
			this.btnProgram.Location = new System.Drawing.Point(145, 33);
			this.btnProgram.Name = "btnProgram";
			this.btnProgram.Size = new System.Drawing.Size(225, 26);
			this.btnProgram.TabIndex = 11;
			this.btnProgram.Text = "Select firmware file && update";
			this.btnProgram.UseVisualStyleBackColor = true;
			this.btnProgram.Click += new System.EventHandler(btnProgram_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(504, 133);
			base.Controls.Add(this.lblMessage);
			base.Controls.Add(this.Progress);
			base.Controls.Add(this.btnProgram);
			base.Name = "FirmwareLoader_DM32";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DM-32 Firmware loader";
			base.Load += new System.EventHandler(FirmwareLoader_DM32_Load);
			base.ResumeLayout(false);
		}
	}
}
