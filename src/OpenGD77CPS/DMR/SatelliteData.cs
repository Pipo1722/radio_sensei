using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DMR
{
	public class SatelliteData
	{
		public static List<KepAndSatData> satConstData;

		private const int MAX_SATELLITES = 25;

		private const int SATELLITE_NAME_LENGTH = 8;

		public static bool loadSatellitesTxtData(string satelliteDataFile)
		{
			int i = 1;
			string text = "";
			try
			{
				string[] array = File.ReadAllText(satelliteDataFile).Split('\n');
				satConstData = new List<KepAndSatData>();
				for (; i < Math.Min(array.Length, 26); i++)
				{
					text = array[i].Trim();
					if (text.Length > 0)
					{
						satConstData.Add(new KepAndSatData(text));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}\n\nMalformed line number {i + 1}\n{text}\n\nFix the \"{satelliteDataFile}\" file and retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return true;
		}

		private static string ConvertEPOCH(string epochStr)
		{
			DateTime dateTime = DateTime.Parse(epochStr, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
			DateTime dateTime2 = new DateTime(dateTime.Year, 1, 1);
			double num = (dateTime - dateTime2).TotalDays + 1.0;
			return dateTime.Year - 2000 + num.ToString("F8");
		}

		private static int CalculateChecksum(string tleLine)
		{
			int num = Math.Min(tleLine.Length, 68);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				char c = tleLine[i];
				if (char.IsDigit(c))
				{
					num2 += c - 48;
				}
				else if (c == '-')
				{
					num2++;
				}
			}
			return num2 % 10;
		}

		public static string ParseKepXML(string kepsStr, ref int kepBytesPos, ref byte[] kepsBytes)
		{
			string text = "";
			int num = 0;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(kepsStr);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("//ndm");
			string text2 = "";
			if (xmlNode != null)
			{
				foreach (XmlNode item in xmlNode.SelectNodes("omm"))
				{
					XmlNode xmlNode2 = item.SelectSingleNode("body//segment//metadata");
					XmlNode xmlNode3 = item.SelectSingleNode("body//segment//data//meanElements");
					XmlNode xmlNode4 = item.SelectSingleNode("body//segment//data//tleParameters");
					string innerText = xmlNode2.SelectSingleNode("OBJECT_NAME").InnerText;
					int num2 = Math.Min(innerText.Length, 24);
					text2 = text2 + innerText.Substring(0, num2) + "                        ".Substring(num2);
					text2 += "\r\n";
					int num3 = int.Parse(xmlNode4.SelectSingleNode("NORAD_CAT_ID").InnerText);
					string innerText2 = xmlNode4.SelectSingleNode("CLASSIFICATION_TYPE").InnerText;
					string text3 = xmlNode4.SelectSingleNode("NORAD_CAT_ID").InnerText + xmlNode4.SelectSingleNode("CLASSIFICATION_TYPE").InnerText;
					foreach (KepAndSatData item2 in satConstData.ToList())
					{
						if (num < 25 && item2.CatalogueNumber == text3)
						{
							string text4 = item2.DisplayName.Trim();
							if (text4.Length > 8)
							{
								text4 = text4.Substring(0, 8);
							}
							text = text + text4 + ", ";
							string text5 = "1 " + num3.ToString("00000") + innerText2 + " ";
							string text6 = xmlNode2.SelectSingleNode("OBJECT_ID").InnerText.Substring(2).Replace("-", "");
							text5 += text6;
							int length = text6.Length;
							text5 += "    ".Substring(length - 5);
							text5 = text5 + ConvertEPOCH(xmlNode3.SelectSingleNode("EPOCH").InnerText) + " ";
							string text7 = double.Parse(xmlNode4.SelectSingleNode("MEAN_MOTION_DOT").InnerText).ToString(".00000000", CultureInfo.InvariantCulture);
							if (text7[0] != '-')
							{
								text5 += " ";
							}
							text5 = text5 + text7 + " ";
							string innerText3 = xmlNode4.SelectSingleNode("MEAN_MOTION_DDOT").InnerText;
							double.Parse(innerText3);
							if (innerText3 == "0")
							{
								text5 += " 00000+0";
							}
							else
							{
								string[] array = double.Parse(innerText3).ToString(".00000E+0").Split('E');
								string text8 = array[0].Replace(".", "") + array[1];
								text5 = text5 + "   ".Substring(text8.Length - 5) + text8;
							}
							text5 += " ";
							string innerText4 = xmlNode4.SelectSingleNode("BSTAR").InnerText;
							if (innerText4 == "0")
							{
								text5 += " 00000+0";
							}
							else
							{
								string text9 = double.Parse(innerText4).ToString(".00000E+0");
								string[] array2 = text9.Split('E');
								text9 = array2[0].Replace(".", "") + array2[1];
								text5 = text5 + "   ".Substring(text9.Length - 5) + text9;
							}
							text5 += " ";
							text5 = text5 + xmlNode4.SelectSingleNode("EPHEMERIS_TYPE").InnerText + "  ";
							text5 += xmlNode4.SelectSingleNode("ELEMENT_SET_NO").InnerText;
							text5 += CalculateChecksum(text5);
							text2 = text2 + text5 + "\r\n";
							string text10 = "2 " + num3.ToString("00000") + " ";
							string text11 = xmlNode3.SelectSingleNode("INCLINATION").InnerText;
							int length2 = text11.Length;
							if (length2 < 8)
							{
								text11 = "       ".Substring(length2 - 1) + text11;
							}
							text10 = text10 + text11 + " ";
							string text12 = xmlNode3.SelectSingleNode("RA_OF_ASC_NODE").InnerText;
							int length3 = text12.Length;
							if (length3 < 8)
							{
								text12 = "       ".Substring(length3 - 1) + text12;
							}
							text10 = text10 + text12 + " ";
							string text13 = xmlNode3.SelectSingleNode("ECCENTRICITY").InnerText;
							int length4 = text13.Length;
							if (length4 < 8)
							{
								text13 += "0000000".Substring(length4 - 1);
							}
							text10 = text10 + text13.Substring(1, 7) + " ";
							string text14 = xmlNode3.SelectSingleNode("ARG_OF_PERICENTER").InnerText;
							int length5 = text14.Length;
							if (length5 < 8)
							{
								text14 = "       ".Substring(length5 - 1) + text14;
							}
							text10 = text10 + text14 + " ";
							string text15 = xmlNode3.SelectSingleNode("MEAN_ANOMALY").InnerText;
							int length6 = text15.Length;
							if (length6 < 8)
							{
								text15 = "       ".Substring(length6 - 1) + text15;
							}
							text10 = text10 + text15 + " ";
							string text16 = xmlNode3.SelectSingleNode("MEAN_MOTION").InnerText;
							int length7 = text16.Length;
							if (length7 < 11)
							{
								text16 = "          ".Substring(length7 - 1) + text16;
							}
							text10 += text16;
							string text17 = xmlNode4.SelectSingleNode("REV_AT_EPOCH").InnerText;
							int length8 = text17.Length;
							if (length8 < 5)
							{
								text17 = "    ".Substring(length8 - 1) + text17;
							}
							text10 += text17;
							text10 += CalculateChecksum(text10);
							text2 = text2 + text10 + "\r\n";
							kepBytesPos = AddSatelliteToData(text5.Substring(18), text10.Substring(8), 8, kepsBytes, kepBytesPos, text4, item2);
							num++;
							if (satConstData.Count == num)
							{
								return text;
							}
							break;
						}
					}
				}
			}
			return text;
		}

		public static string parseCSVKeps(string kepsStr, ref int kepBytesPos, ref byte[] kepsBytes)
		{
			string text = "";
			int num = 0;
			IDictionary<string, int> dictionary = new Dictionary<string, int>();
			string text2 = "";
			int[] array = new int[17];
			using (StringReader stringReader = new StringReader(kepsStr))
			{
				string[] array2 = stringReader.ReadLine().Split(',');
				for (int i = 0; i < array2.Length; i++)
				{
					dictionary[array2[i]] = i;
				}
				int num2 = 0;
				array[num2++] = dictionary["OBJECT_NAME"];
				array[num2++] = dictionary["NORAD_CAT_ID"];
				array[num2++] = dictionary["CLASSIFICATION_TYPE"];
				array[num2++] = dictionary["OBJECT_ID"];
				array[num2++] = dictionary["EPOCH"];
				array[num2++] = dictionary["MEAN_MOTION_DOT"];
				array[num2++] = dictionary["MEAN_MOTION_DDOT"];
				array[num2++] = dictionary["BSTAR"];
				array[num2++] = dictionary["EPHEMERIS_TYPE"];
				array[num2++] = dictionary["ELEMENT_SET_NO"];
				array[num2++] = dictionary["INCLINATION"];
				array[num2++] = dictionary["RA_OF_ASC_NODE"];
				array[num2++] = dictionary["ECCENTRICITY"];
				array[num2++] = dictionary["ARG_OF_PERICENTER"];
				array[num2++] = dictionary["MEAN_ANOMALY"];
				array[num2++] = dictionary["MEAN_MOTION"];
				array[num2++] = dictionary["REV_AT_EPOCH"];
			}
			using (StringReader stringReader2 = new StringReader(kepsStr))
			{
				stringReader2.ReadLine();
				string text3;
				while ((text3 = stringReader2.ReadLine()) != null)
				{
					string[] array3 = text3.Split(',');
					string text4 = array3[array[0]];
					int num3 = Math.Min(text4.Length, 24);
					text2 = text2 + text4.Substring(0, num3) + "                        ".Substring(num3);
					text2 += "\r\n";
					Console.WriteLine(text4);
					string text5 = array3[array[1]];
					string text6 = array3[array[2]];
					text5 += text6;
					foreach (KepAndSatData item in satConstData.ToList())
					{
						if (num < 25 && item.CatalogueNumber == text5)
						{
							int num4 = int.Parse(array3[array[1]]);
							string text7 = item.DisplayName.Trim();
							if (text7.Length > 8)
							{
								text7 = text7.Substring(0, 8);
							}
							text = text + text7 + ", ";
							string text8 = "1 " + num4.ToString("00000") + text6 + " ";
							string text9 = array3[array[3]].Substring(2).Replace("-", "");
							text8 += text9;
							int length = text9.Length;
							text8 += "    ".Substring(length - 5);
							text8 = text8 + ConvertEPOCH(array3[array[4]]) + " ";
							string text10 = double.Parse(array3[array[5]]).ToString(".00000000", CultureInfo.InvariantCulture);
							if (text10[0] != '-')
							{
								text8 += " ";
							}
							text8 = text8 + text10 + " ";
							string text11 = array3[array[6]];
							double.Parse(text11);
							if (text11 == "0")
							{
								text8 += " 00000+0";
							}
							else
							{
								string[] array4 = double.Parse(text11).ToString(".00000E+0").Split('E');
								string text12 = array4[0].Replace(".", "") + array4[1];
								text8 = text8 + "   ".Substring(text12.Length - 5) + text12;
							}
							text8 += " ";
							string text13 = array3[array[7]];
							if (text13 == "0")
							{
								text8 += " 00000+0";
							}
							else
							{
								string text14 = double.Parse(text13).ToString(".00000E+0");
								string[] array5 = text14.Split('E');
								text14 = array5[0].Replace(".", "") + array5[1];
								text8 = text8 + "   ".Substring(text14.Length - 5) + text14;
							}
							text8 += " ";
							text8 = text8 + array3[array[8]] + "  ";
							text8 += array3[array[9]];
							text8 += CalculateChecksum(text8);
							text2 = text2 + text8 + "\r\n";
							string text15 = "2 " + num4.ToString("00000") + " ";
							string text16 = array3[array[10]];
							int length2 = text16.Length;
							if (length2 < 8)
							{
								text16 = "       ".Substring(length2 - 1) + text16;
							}
							text15 = text15 + text16 + " ";
							string text17 = array3[array[11]];
							int length3 = text17.Length;
							if (length3 < 8)
							{
								text17 = "       ".Substring(length3 - 1) + text17;
							}
							text15 = text15 + text17 + " ";
							string text18 = array3[array[12]];
							int length4 = text18.Length;
							if (length4 < 8)
							{
								text18 += "0000000".Substring(length4 - 1);
							}
							text15 = text15 + text18.Substring(1, 7) + " ";
							string text19 = array3[array[13]];
							int length5 = text19.Length;
							if (length5 < 8)
							{
								text19 = "       ".Substring(length5 - 1) + text19;
							}
							text15 = text15 + text19 + " ";
							string text20 = array3[array[14]];
							int length6 = text20.Length;
							if (length6 < 8)
							{
								text20 = "       ".Substring(length6 - 1) + text20;
							}
							text15 = text15 + text20 + " ";
							string text21 = array3[array[15]];
							int length7 = text21.Length;
							if (length7 < 11)
							{
								text21 = "          ".Substring(length7 - 1) + text21;
							}
							text15 += text21;
							string text22 = array3[array[16]];
							int length8 = text22.Length;
							if (length8 < 5)
							{
								text22 = "    ".Substring(length8 - 1) + text22;
							}
							text15 += text22;
							text15 += CalculateChecksum(text15);
							text2 = text2 + text15 + "\r\n";
							kepBytesPos = AddSatelliteToData(text8.Substring(18), text15.Substring(8), 8, kepsBytes, kepBytesPos, text7, item);
							num++;
							if (satConstData.Count == num)
							{
								return text;
							}
							break;
						}
					}
				}
				return text;
			}
		}

		public static string parseTLEKeps(string kepsStr, ref int kepBytesPos, ref byte[] kepsBytes)
		{
			string text = "";
			int num = 0;
			List<string> list = new List<string>();
			StringReader stringReader = new StringReader(kepsStr);
			string text2;
			do
			{
				text2 = stringReader.ReadLine();
				if (text2 != null)
				{
					list.Add(text2);
				}
			}
			while (text2 != null);
			string[] array = list.ToArray();
			foreach (KepAndSatData satConstDatum in satConstData)
			{
				for (int i = 0; i < array.Length / 3; i++)
				{
					try
					{
						if (num < 25 && satConstDatum.CatalogueNumber == array[i * 3 + 1].Substring(2, 6))
						{
							string text3 = satConstDatum.DisplayName.Trim();
							if (text3.Length > 8)
							{
								text3 = text3.Substring(0, 8);
							}
							text = text + text3 + ", ";
							string kepsTLE = array[i * 3 + 1].PadRight(70).Substring(18);
							string kepsTLE2 = array[i * 3 + 2].PadRight(70).Substring(8);
							kepBytesPos = AddSatelliteToData(kepsTLE, kepsTLE2, 8, kepsBytes, kepBytesPos, text3, satConstDatum);
							num++;
							break;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return text;
		}

		private static int AddSatelliteToData(string kepsTLE1, string kepsTLE2, int SATELLITE_NAME_LENGTH, byte[] kepsBytes, int kepBytesPos, string satelliteName, KepAndSatData satellite)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(satelliteName);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, bytes.Length);
			kepBytesPos += SATELLITE_NAME_LENGTH;
			byte[] array = compressTLEText(kepsTLE1.Substring(0, 2) + kepsTLE1.Substring(2, 12) + kepsTLE1.Substring(15, 10));
			Array.Copy(array, 0, kepsBytes, kepBytesPos, array.Length);
			kepBytesPos += array.Length;
			array = compressTLEText(kepsTLE2.Substring(0, 8) + kepsTLE2.Substring(9, 8) + kepsTLE2.Substring(18, 7) + kepsTLE2.Substring(26, 8) + kepsTLE2.Substring(35, 8) + kepsTLE2.Substring(44, 11) + kepsTLE2.Substring(55, 5) + " ");
			Array.Copy(array, 0, kepsBytes, kepBytesPos, array.Length);
			kepBytesPos += array.Length;
			bytes = BitConverter.GetBytes(satellite.Rx1);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 4);
			kepBytesPos += 4;
			bytes = BitConverter.GetBytes(satellite.Tx1);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 4);
			kepBytesPos += 4;
			bytes = BitConverter.GetBytes(satellite.TxCTCSS1);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 2);
			kepBytesPos += 2;
			bytes = BitConverter.GetBytes(satellite.ArmCTCSS1);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 2);
			kepBytesPos += 2;
			bytes = BitConverter.GetBytes(satellite.Rx2);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 4);
			kepBytesPos += 4;
			bytes = BitConverter.GetBytes(satellite.Tx2);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 4);
			kepBytesPos += 4;
			bytes = BitConverter.GetBytes(satellite.Rx3);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 4);
			kepBytesPos += 4;
			bytes = BitConverter.GetBytes(satellite.Tx3);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, 4);
			kepBytesPos += 4;
			string text = satellite.Reserved.Trim();
			if (text.Length > 24)
			{
				text = text.Substring(0, 24);
			}
			bytes = Encoding.ASCII.GetBytes(text);
			Array.Copy(bytes, 0, kepsBytes, kepBytesPos, bytes.Length);
			kepBytesPos += 24;
			return kepBytesPos;
		}

		private static byte[] compressTLEText(string s)
		{
			List<byte> list = new List<byte>();
			int length = s.Length;
			for (int i = 0; i < length; i += 2)
			{
				int num = "0123456789. +-*".IndexOf(s[i]) << 4;
				int num2 = "0123456789. +-*".IndexOf(s[i + 1]);
				byte item = (byte)(num + num2);
				list.Add(item);
			}
			return list.ToArray();
		}
	}
}
