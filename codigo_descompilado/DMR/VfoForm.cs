using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class VfoForm : DockContent, IDisp
	{
		public enum ChModeE
		{
			MODE_FM,
			MODE_DMR,
			MODE_FM_BROADCAST,
			MODE_AM
		}

		private enum LibreDMR_Power_E
		{
			Power_NoOverride,
			Power_50mW,
			Power_250mW,
			Power_500mW,
			Power_750mW,
			Power_1W,
			Power_2W,
			Power_3W,
			Power_4W,
			Power_5W,
			Power_5W_Plus
		}

		private enum RefFreqE
		{
			Low,
			Middle,
			High
		}

		private enum AdmitCritericaE
		{
			Always = 0,
			ChFree = 1,
			CtcssDcs = 2,
			ColorCode = 2,
			CorectPl = 3
		}

		private enum BandwidthE
		{
			Band12_5,
			Band25
		}

		private enum SquelchE
		{
			Tight,
			Normal
		}

		private enum VoiceEmphasisE
		{
			None,
			DeEmphasisAndPreEmphasis,
			DeEmphasis,
			PreEmphasis
		}

		private enum SteE
		{
			Frequency,
			Ste120,
			Ste180,
			Ste240
		}

		private enum NoneSteE
		{
			Off,
			Frequency
		}

		private enum SignalingSystemE
		{
			Off,
			Dtmf
		}

		private enum UnmuteRuleE
		{
			StdUnmuteMute,
			AndUnmuteMute,
			AndUnmuteOrMute
		}

		private enum PttidTypeE
		{
			None,
			Front,
			Post,
			FrontAndPost
		}

		private enum TimingPreference
		{
			First,
			Qualified,
			Unqualified
		}

		public class ChModeChangeEventArgs : EventArgs
		{
			public int ChIndex { get; set; }

			public int ChMode { get; set; }

			public ChModeChangeEventArgs(int chIndex, int chMode)
			{
				ChIndex = chIndex;
				ChMode = chMode;
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ChannelOne : IVerify<ChannelOne>
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			private byte[] name;

			private uint rxFreq;

			private uint txFreq;

			private byte chMode;

			private byte libreDMR_Power;

			private byte locationLat0;

			private byte tot;

			private byte locationLat1;

			private byte locationLat2;

			private byte locationLon0;

			private byte locationLon1;

			private ushort rxTone;

			private ushort txTone;

			private byte locationLon2;

			private byte _UNUSED_1;

			private byte LibreDMR_flag1;

			private byte rxSignaling;

			private byte artsInterval;

			private byte encrypt;

			private byte _UNUSED_2;

			private byte rxGroupList;

			private byte txColor;

			private byte aprsSystem;

			private ushort contact;

			private byte flag1;

			private byte flag2;

			private byte flag3;

			private byte flag4;

			private ushort offsetFreq;

			private byte flag5;

			private byte sql;

			public string Name
			{
				get
				{
					return Settings.smethod_25(name);
				}
				set
				{
					byte[] array = Settings.smethod_23(value);
					name.Fill(byte.MaxValue);
					Array.Copy(array, 0, name, 0, Math.Min(array.Length, name.Length));
				}
			}

			public string RxFreq
			{
				get
				{
					try
					{
						uint uint_ = 0u;
						double double_ = (double)int.Parse($"{rxFreq:x}") / 100000.0;
						if (Settings.checkFrequecyIsInValidRange(double_, ref uint_) == -1)
						{
							double_ = uint_;
						}
						return double_.ToString("f5");
					}
					catch (Exception)
					{
						return "";
					}
				}
				set
				{
					try
					{
						string s = new Regex("[^0-9]").Replace(value, "");
						rxFreq = uint.Parse(s, NumberStyles.HexNumber);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						rxFreq = uint.MaxValue;
					}
				}
			}

			public uint UInt32_0
			{
				get
				{
					return rxFreq;
				}
				set
				{
					rxFreq = value;
				}
			}

			public uint RxFreqDec
			{
				get
				{
					return Settings.smethod_34(rxFreq);
				}
				set
				{
					rxFreq = Settings.smethod_35(value);
				}
			}

			public string TxFreq
			{
				get
				{
					try
					{
						uint uint_ = 0u;
						double double_ = (double)int.Parse($"{txFreq:x}") / 100000.0;
						if (Settings.checkFrequecyIsInValidRange(double_, ref uint_) == -1)
						{
							double_ = uint_;
						}
						return double_.ToString("f5");
					}
					catch (Exception)
					{
						return "";
					}
				}
				set
				{
					try
					{
						string s = new Regex("[^0-9]").Replace(value, "");
						txFreq = uint.Parse(s, NumberStyles.HexNumber);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						rxFreq = uint.MaxValue;
					}
				}
			}

			public uint UInt32_1
			{
				get
				{
					return txFreq;
				}
				set
				{
					txFreq = value;
				}
			}

			public uint TxFreqDec
			{
				get
				{
					return Settings.smethod_34(txFreq);
				}
				set
				{
					txFreq = Settings.smethod_35(value);
				}
			}

			public int ChMode
			{
				get
				{
					if (Enum.IsDefined(typeof(ChModeE), (int)chMode))
					{
						return chMode;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(ChModeE), value))
					{
						chMode = (byte)value;
					}
				}
			}

			public int LibreDMR_Power
			{
				get
				{
					if (Enum.IsDefined(typeof(LibreDMR_Power_E), (int)libreDMR_Power))
					{
						return libreDMR_Power;
					}
					return 0;
				}
				set
				{
					if (Enum.IsDefined(typeof(LibreDMR_Power_E), value))
					{
						libreDMR_Power = (byte)value;
					}
				}
			}

			public string Longitude
			{
				get
				{
					return ChannelForm.ChannelOne.LatLonBin24ToString((uint)((locationLon2 << 16) + (locationLon1 << 8) + locationLon0));
				}
				set
				{
					try
					{
						double num = double.Parse(value);
						if (num >= -180.0 || num <= 180.0)
						{
							uint num2 = ChannelForm.ChannelOne.LatLonStringToBin24(value);
							locationLon0 = (byte)(num2 & 0xFF);
							locationLon1 = (byte)((num2 >> 8) & 0xFF);
							locationLon2 = (byte)((num2 >> 16) & 0xFF);
							return;
						}
						throw new Exception();
					}
					catch
					{
						locationLon0 = (locationLon1 = (locationLon2 = 0));
					}
				}
			}

			public string Latitude
			{
				get
				{
					return ChannelForm.ChannelOne.LatLonBin24ToString((uint)((locationLat2 << 16) + (locationLat1 << 8) + locationLat0));
				}
				set
				{
					try
					{
						double num = double.Parse(value);
						if (num >= -90.0 || num <= 90.0)
						{
							uint num2 = ChannelForm.ChannelOne.LatLonStringToBin24(value);
							locationLat0 = (byte)(num2 & 0xFF);
							locationLat1 = (byte)((num2 >> 8) & 0xFF);
							locationLat2 = (byte)((num2 >> 16) & 0xFF);
							return;
						}
						throw new Exception();
					}
					catch
					{
						locationLat0 = (locationLat1 = (locationLat2 = 0));
					}
				}
			}

			public string LibreDMR_DMRidChannel
			{
				get
				{
					if ((LibreDMR_flag1 & 0x80) != 0)
					{
						int num = (rxSignaling << 16) | (artsInterval << 8) | encrypt;
						if (num >= 1 && num <= 16777215)
						{
							string text = num.ToString();
							if (!string.IsNullOrEmpty(text) && ContactForm.IsValidId(text))
							{
								return text;
							}
						}
					}
					return "";
				}
				set
				{
					if (!string.IsNullOrEmpty(value) && ContactForm.IsValidId(value))
					{
						int num = int.Parse(value);
						if (num >= 1 && num <= 16777215)
						{
							LibreDMR_flag1 |= 128;
							rxSignaling = (byte)((num >> 16) & 0xFF);
							artsInterval = (byte)((num >> 8) & 0xFF);
							encrypt = (byte)(num & 0xFF);
						}
					}
					else
					{
						LibreDMR_flag1 = (byte)(LibreDMR_flag1 & -129);
						rxSignaling = 0;
						artsInterval = 22;
						encrypt = 0;
					}
				}
			}

			public bool LibreDMR_NoBeepChannel
			{
				get
				{
					return (LibreDMR_flag1 & 0x40) != 0;
				}
				set
				{
					if (value)
					{
						LibreDMR_flag1 |= 64;
					}
					else
					{
						LibreDMR_flag1 = (byte)(LibreDMR_flag1 & -65);
					}
				}
			}

			public bool LibreDMR_NoEcoChannel
			{
				get
				{
					return (LibreDMR_flag1 & 0x20) != 0;
				}
				set
				{
					if (value)
					{
						LibreDMR_flag1 |= 32;
					}
					else
					{
						LibreDMR_flag1 = (byte)(LibreDMR_flag1 & -33);
					}
				}
			}

			public bool LibreDMR_DMRForceDMO
			{
				get
				{
					return (LibreDMR_flag1 & 4) != 0;
				}
				set
				{
					if (value)
					{
						LibreDMR_flag1 |= 4;
					}
					else
					{
						LibreDMR_flag1 = (byte)(LibreDMR_flag1 & -5);
					}
				}
			}

			public decimal Tot
			{
				get
				{
					if (tot >= 0 && tot <= 33)
					{
						return tot * 15;
					}
					return 0m;
				}
				set
				{
					int num = (int)(value / 15m);
					if (num >= 0 && num <= 33)
					{
						tot = (byte)num;
					}
					else
					{
						tot = 0;
					}
				}
			}

			public string RxTone
			{
				get
				{
					if (rxTone != ushort.MaxValue && rxTone != 0)
					{
						if ((rxTone & 0xC000) == 49152)
						{
							return $"D{rxTone & 0xFFF:x3}I";
						}
						if ((rxTone & 0xC000) == 32768)
						{
							return $"D{rxTone & 0xFFF:x3}N";
						}
						double num = (double)(int)Settings.smethod_32(rxTone) * 0.1;
						if (num > 60.0 && num < 260.0)
						{
							return num.ToString("f1");
						}
						return Settings.SZ_NONE;
					}
					return Settings.SZ_NONE;
				}
				set
				{
					string text = "";
					rxTone = ushort.MaxValue;
					if (string.IsNullOrEmpty(value) || !(value != Settings.SZ_NONE))
					{
						return;
					}
					if (new Regex("D[0-7]{3}N$").IsMatch(value))
					{
						text = value.Substring(1, 3);
						rxTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 32768);
						return;
					}
					if (new Regex("D[0-7]{3}I$").IsMatch(value))
					{
						text = value.Substring(1, 3);
						rxTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 49152);
						return;
					}
					double num = double.Parse(value);
					if (num > 60.0 && num < 260.0)
					{
						rxTone = Settings.smethod_33(Convert.ToUInt16(num * 10.0));
					}
				}
			}

			public string TxTone
			{
				get
				{
					if (txTone != ushort.MaxValue && txTone != 0)
					{
						if ((txTone & 0xC000) == 49152)
						{
							return $"D{txTone & 0xFFF:x3}I";
						}
						if ((txTone & 0xC000) == 32768)
						{
							return $"D{txTone & 0xFFF:x3}N";
						}
						double num = (double)(int)Settings.smethod_32(txTone) * 0.1;
						if (num > 60.0 && num < 260.0)
						{
							return num.ToString("f1");
						}
						return Settings.SZ_NONE;
					}
					return Settings.SZ_NONE;
				}
				set
				{
					string text = "";
					txTone = ushort.MaxValue;
					if (string.IsNullOrEmpty(value) || !(value != Settings.SZ_NONE))
					{
						return;
					}
					if (new Regex("D[0-7]{3}N$").IsMatch(value))
					{
						text = value.Substring(1, 3);
						txTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 32768);
						return;
					}
					if (new Regex("D[0-7]{3}I$").IsMatch(value))
					{
						text = value.Substring(1, 3);
						txTone = (ushort)(ushort.Parse(text, NumberStyles.HexNumber) + 49152);
						return;
					}
					double num = double.Parse(value);
					if (num > 60.0 && num < 260.0)
					{
						txTone = Settings.smethod_33(Convert.ToUInt16(num * 10.0));
					}
				}
			}

			public int UnmuteRule
			{
				get
				{
					return LibreDMR_flag1;
				}
				set
				{
					LibreDMR_flag1 = (byte)value;
				}
			}

			public int RxSignaling
			{
				get
				{
					return rxSignaling;
				}
				set
				{
					rxSignaling = (byte)value;
				}
			}

			public decimal ArtsInterval
			{
				get
				{
					return artsInterval;
				}
				set
				{
					artsInterval = (byte)value;
				}
			}

			public int Key
			{
				get
				{
					return encrypt;
				}
				set
				{
					encrypt = (byte)value;
				}
			}

			public int RxGroupList
			{
				get
				{
					return rxGroupList;
				}
				set
				{
					if (value <= RxGroupListForm.data.Count)
					{
						rxGroupList = (byte)value;
					}
				}
			}

			public decimal TxColor
			{
				get
				{
					if (txColor >= 0 && txColor <= 15)
					{
						return txColor;
					}
					return 0m;
				}
				set
				{
					if (value >= 0m && value <= 15m)
					{
						txColor = (byte)value;
					}
				}
			}

			public int APRS_System
			{
				get
				{
					if (aprsSystem <= APRSForm.data.Count)
					{
						return aprsSystem;
					}
					return 0;
				}
				set
				{
					if (value <= APRSForm.data.Count)
					{
						aprsSystem = (byte)value;
					}
				}
			}

			public int Contact
			{
				get
				{
					if (contact <= ContactForm.data.Count)
					{
						return contact;
					}
					return 0;
				}
				set
				{
					if (value <= ContactForm.data.Count)
					{
						contact = (byte)value;
					}
				}
			}

			public byte Flag1
			{
				get
				{
					return flag1;
				}
				set
				{
					flag1 = value;
				}
			}

			public byte Flag2
			{
				get
				{
					return flag2;
				}
				set
				{
					flag2 = value;
				}
			}

			public byte Flag3
			{
				get
				{
					return flag3;
				}
				set
				{
					flag3 = value;
				}
			}

			public byte Flag4
			{
				get
				{
					return flag4;
				}
				set
				{
					flag4 = value;
				}
			}

			public int Power
			{
				get
				{
					return (flag4 & 0x80) >> 7;
				}
				set
				{
					flag4 &= 127;
					value = (value << 7) & 0x80;
					flag4 |= (byte)value;
				}
			}

			public bool Vox
			{
				get
				{
					return Convert.ToBoolean(flag4 & 0x40);
				}
				set
				{
					if (value)
					{
						flag4 |= 64;
					}
					else
					{
						flag4 &= 191;
					}
				}
			}

			public bool AutoScan
			{
				get
				{
					return Convert.ToBoolean(flag4 & 0x20);
				}
				set
				{
					if (value)
					{
						flag4 |= 32;
					}
					else
					{
						flag4 &= 223;
					}
				}
			}

			public bool LoneWoker
			{
				get
				{
					return Convert.ToBoolean(flag4 & 0x10);
				}
				set
				{
					if (value)
					{
						flag4 |= 16;
					}
					else
					{
						flag4 &= 239;
					}
				}
			}

			public bool AllowTalkaround
			{
				get
				{
					return Convert.ToBoolean(flag4 & 8);
				}
				set
				{
					if (value)
					{
						flag4 |= 8;
					}
					else
					{
						flag4 &= 247;
					}
				}
			}

			public bool OnlyRx
			{
				get
				{
					return Convert.ToBoolean(flag4 & 4);
				}
				set
				{
					if (value)
					{
						flag4 |= 4;
					}
					else
					{
						flag4 &= 251;
					}
				}
			}

			public int Bandwidth
			{
				get
				{
					return (flag4 & 2) >> 1;
				}
				set
				{
					flag4 &= 253;
					value = (value << 1) & 2;
					flag4 |= (byte)value;
				}
			}

			public int Squelch
			{
				get
				{
					return flag4 & 1;
				}
				set
				{
					flag4 &= 254;
					flag4 |= (byte)(value & 1);
				}
			}

			public int Ste
			{
				get
				{
					return (flag3 & 0xC0) >> 6;
				}
				set
				{
					value = (value << 6) & 0xC0;
					flag3 &= 63;
					flag3 |= (byte)value;
				}
			}

			public int NonSte
			{
				get
				{
					return (flag3 & 0x20) >> 5;
				}
				set
				{
					value = (value << 5) & 0x20;
					flag3 &= 223;
					flag3 |= (byte)value;
				}
			}

			public bool DataPl
			{
				get
				{
					return Convert.ToBoolean(flag3 & 0x10);
				}
				set
				{
					if (value)
					{
						flag3 |= 16;
					}
					else
					{
						flag3 &= 239;
					}
				}
			}

			public int PttidType
			{
				get
				{
					return (flag3 & 0xC) >> 2;
				}
				set
				{
					value = (value << 2) & 0xC;
					flag3 &= 243;
					flag3 |= (byte)value;
				}
			}

			public bool DualCapacity
			{
				get
				{
					return Convert.ToBoolean(flag3 & 1);
				}
				set
				{
					if (value)
					{
						flag3 |= 1;
					}
					else
					{
						flag3 &= 254;
					}
				}
			}

			public int TimingPreference
			{
				get
				{
					return (flag2 & 0x80) >> 7;
				}
				set
				{
					value = (value << 7) & 0x80;
					flag2 &= 127;
					flag2 |= (byte)value;
				}
			}

			public int RepateSlot
			{
				get
				{
					return (flag2 & 0x40) >> 6;
				}
				set
				{
					value = (value << 6) & 0x40;
					flag2 &= 191;
					flag2 |= (byte)value;
				}
			}

			public int Ars
			{
				get
				{
					return (flag2 & 0x20) >> 5;
				}
				set
				{
					value = (value << 5) & 0x20;
					flag2 &= 223;
					flag2 |= (byte)value;
				}
			}

			public int KeySwitch
			{
				get
				{
					return (flag2 & 0x10) >> 4;
				}
				set
				{
					value = (value << 4) & 0x10;
					flag2 &= 239;
					flag2 |= (byte)value;
				}
			}

			public bool UdpDataHead
			{
				get
				{
					return Convert.ToBoolean(flag2 & 8);
				}
				set
				{
					if (value)
					{
						flag2 |= 8;
					}
					else
					{
						flag2 &= 247;
					}
				}
			}

			public bool AllowTxInterupt
			{
				get
				{
					return Convert.ToBoolean(flag2 & 4);
				}
				set
				{
					if (value)
					{
						flag2 |= 4;
					}
					else
					{
						flag2 &= 251;
					}
				}
			}

			public bool TxInteruptFreq
			{
				get
				{
					return Convert.ToBoolean(flag2 & 2);
				}
				set
				{
					if (value)
					{
						flag2 |= 2;
					}
					else
					{
						flag2 &= 253;
					}
				}
			}

			public bool PrivateCall
			{
				get
				{
					return Convert.ToBoolean(flag2 & 1);
				}
				set
				{
					if (value)
					{
						flag2 |= 1;
					}
					else
					{
						flag2 &= 254;
					}
				}
			}

			public int TaTxTS1
			{
				get
				{
					return flag1 & 3;
				}
				set
				{
					value &= 3;
					flag1 &= 252;
					flag1 |= (byte)value;
				}
			}

			public int TaTxTS2
			{
				get
				{
					return (flag1 & 0xC) >> 2;
				}
				set
				{
					value <<= 2;
					flag1 &= 243;
					flag1 |= (byte)value;
				}
			}

			public decimal OffsetFreq
			{
				get
				{
					if (offsetFreq < 1 || offsetFreq > 38400)
					{
						return 10.00m;
					}
					return (decimal)offsetFreq * 0.01m;
				}
				set
				{
					int num = Convert.ToInt32(value / 0.01m);
					if (num >= 1 && num <= 38400)
					{
						offsetFreq = (ushort)num;
					}
					else
					{
						offsetFreq = 1000;
					}
				}
			}

			public int OffsetStep
			{
				get
				{
					int num = flag5 >> 4;
					if (num >= SZ_OFFSET_STEP.Length)
					{
						return 0;
					}
					return num;
				}
				set
				{
					value &= 0xF;
					flag5 &= 15;
					flag5 |= (byte)(value << 4);
				}
			}

			public int OffsetDirection
			{
				get
				{
					int num = (flag5 & 0xC) >> 2;
					if (num >= SZ_OFFSET_DIRECTION.Length)
					{
						return 0;
					}
					return num;
				}
				set
				{
					value &= 3;
					flag5 &= 243;
					flag5 |= (byte)(value << 2);
				}
			}

			public int Sql
			{
				get
				{
					if (sql >= 0 && sql <= 21)
					{
						return sql;
					}
					return 0;
				}
				set
				{
					if (value >= 0 && sql <= 21)
					{
						sql = (byte)value;
					}
				}
			}

			public ChannelOne(int index)
			{
				this = default(ChannelOne);
				name = new byte[16];
			}

			public void Default()
			{
				ChMode = DefaultCh.ChMode;
				LibreDMR_Power = DefaultCh.LibreDMR_Power;
				Tot = DefaultCh.Tot;
				RxTone = DefaultCh.RxTone;
				TxTone = DefaultCh.TxTone;
				UnmuteRule = 0;
				RxSignaling = 0;
				ArtsInterval = 22m;
				Key = 0;
				TxColor = DefaultCh.TxColor;
				APRS_System = DefaultCh.APRS_System;
				Contact = DefaultCh.Contact;
				Flag1 = DefaultCh.Flag1;
				Flag2 = DefaultCh.Flag2;
				Flag3 = DefaultCh.Flag3;
				Flag4 = DefaultCh.Flag4;
				Sql = DefaultCh.Sql;
				Latitude = ChannelForm.DefaultCh.Latitude;
				Longitude = ChannelForm.DefaultCh.Longitude;
			}

			public ChannelOne Clone()
			{
				return Settings.cloneObject(this);
			}

			public void Verify(ChannelOne def)
			{
				if (!Enum.IsDefined(typeof(ChModeE), ChMode))
				{
					ChMode = def.ChMode;
				}
				if (!Enum.IsDefined(typeof(RefFreqE), LibreDMR_Power))
				{
					LibreDMR_Power = def.LibreDMR_Power;
				}
				Settings.ValidateNumberRangeWithDefault(ref tot, 0, 33, def.tot);
				if (rxGroupList != 0 && rxGroupList <= 76)
				{
					if (!RxGroupListForm.data.DataIsValid(rxGroupList - 1))
					{
						rxGroupList = 0;
					}
				}
				else
				{
					rxGroupList = 0;
				}
				if (aprsSystem != 0 && aprsSystem <= APRSForm.data.Count)
				{
					if (!APRSForm.data.DataIsValid(aprsSystem - 1))
					{
						aprsSystem = 0;
					}
				}
				else
				{
					aprsSystem = 0;
				}
				if (new Regex("D[0-7]{3}[N|I]$").IsMatch(RxTone))
				{
					Ste = 0;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class Vfo : IData
		{
			public delegate void ChModeDelegate(object sender, ChModeChangeEventArgs e);

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			private ChannelOne[] chList;

			public ChannelOne this[int index]
			{
				get
				{
					if (index >= 4)
					{
						throw new ArgumentOutOfRangeException();
					}
					return chList[index];
				}
				set
				{
					if (index >= 4)
					{
						throw new ArgumentOutOfRangeException();
					}
					chList[index] = value;
				}
			}

			public int Count => 4;

			public string Format
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public bool ListIsEmpty
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public event ChModeDelegate ChModeChangeEvent;

			public Vfo()
			{
				int num = 0;
				chList = new ChannelOne[4];
				for (num = 0; num < chList.Length; num++)
				{
					chList[num] = new ChannelOne(num);
				}
			}

			public void Verify()
			{
				int num = 0;
				uint num2 = 0u;
				uint uint_ = 0u;
				for (num = 0; num < 4; num++)
				{
					num2 = chList[num].RxFreqDec / 100000;
					if (Settings.checkFrequecyIsInValidRange(num2, ref uint_) < 0)
					{
						num2 = uint_ * 100000;
						chList[num].RxFreqDec = num2;
					}
					if (Settings.checkFrequecyIsInValidRange(chList[num].TxFreqDec / 100000, ref uint_) < 0)
					{
						chList[num].TxFreqDec = chList[num].RxFreqDec;
					}
					chList[num].Verify(DefaultCh);
				}
			}

			public void SetDefaultFreq(int index)
			{
				chList[index].UInt32_0 = Settings.smethod_35(Settings.MIN_FREQ[0] * 100000);
				chList[index].UInt32_1 = chList[index].UInt32_0;
			}

			public byte[] ToEerom()
			{
				int num = 0;
				int num2 = 0;
				byte[] array = new byte[4 * SPACE_CH];
				for (num = 0; num < 4; num++)
				{
					byte[] array2 = Settings.objectToByteArray(chList[num], Marshal.SizeOf(chList[num]));
					Array.Copy(array2, 0, array, num2, array2.Length);
					num2 += array2.Length;
				}
				return array;
			}

			public void FromEerom(byte[] data)
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					byte[] array = new byte[SPACE_CH];
					Array.Copy(data, num, array, 0, array.Length);
					chList[i] = (ChannelOne)Settings.byteArrayToObject(array, typeof(ChannelOne));
					if (chList[i].ChMode != 1 && chList[i].DualCapacity)
					{
						chList[i].DualCapacity = false;
					}
					if (GeneralSetForm.data.CodeplugVersion == 0)
					{
						chList[i].LibreDMR_Power = 0;
					}
					num += array.Length;
				}
			}

			public void ChangeAPRS_Index(int oldIndex, int newIndex)
			{
				for (int i = 0; i < Count; i++)
				{
					if (chList[i].APRS_System == oldIndex + 1)
					{
						chList[i].APRS_System = newIndex + 1;
					}
				}
			}

			public void ClearByAPRS(int aprsIndex)
			{
				for (int i = 0; i < Count; i++)
				{
					if (chList[i].APRS_System == aprsIndex + 1)
					{
						chList[i].APRS_System = 0;
					}
				}
			}

			public int GetMinIndex()
			{
				throw new NotImplementedException();
			}

			public string GetMinName(TreeNode node)
			{
				throw new NotImplementedException();
			}

			public bool DataIsValid(int index)
			{
				throw new NotImplementedException();
			}

			public void SetIndex(int index, int value)
			{
				throw new NotImplementedException();
			}

			public void ClearIndex(int index)
			{
				throw new NotImplementedException();
			}

			public void SetName(int index, string text)
			{
				throw new NotImplementedException();
			}

			public string GetName(int index)
			{
				throw new NotImplementedException();
			}

			public void Default(int index)
			{
				throw new NotImplementedException();
			}

			public void Paste(int from, int to)
			{
				throw new NotImplementedException();
			}

			public int GetChMode(int index)
			{
				return chList[index].ChMode;
			}

			public void SetChannelDMRid(int index, string dmrid)
			{
				chList[index].LibreDMR_DMRidChannel = dmrid;
			}

			public void SetChannelNoBeep(int index, bool value)
			{
				chList[index].LibreDMR_NoBeepChannel = value;
			}

			public void SetChannelNoEco(int index, bool value)
			{
				chList[index].LibreDMR_NoEcoChannel = value;
			}

			public void SetChannelDMRForceDMO(int index, bool value)
			{
				chList[index].LibreDMR_DMRForceDMO = value;
			}
		}

		public const int CNT_VFO_CH = 4;

		public const int LEN_CH_NAME = 16;

		public const string SZ_CH_MODE_NAME = "ChMode";

		private const int LEN_FREQ = 9;

		private const int SCL_FREQ = 100000;

		public const string SZ_REF_FREQ_NAME = "RefFreq";

		public const string SZ_POWER_NAME = "Power";

		private const string SZ_INFINITE = "无穷";

		private const int MIN_TOT = 0;

		private const int MAX_TOT = 33;

		private const int INC_TOT = 1;

		private const int SCL_TOT = 15;

		private const int LEN_TOT = 3;

		private const int MIN_TOT_REKEY = 0;

		private const int MAX_TOT_REKEY = 255;

		private const int INC_TOT_REKEY = 1;

		private const int SCL_TOT_REKEY = 1;

		private const int LEN_TOT_REKEY = 3;

		public const string SZ_ADMIT_CRITERICA_NAME = "AdmitCriterica";

		public const string SZ_ADMIT_CRITERICA_D_NAME = "AdmitCritericaD";

		private const int MIN_RSSI_THRESHOLD = 80;

		private const int MAX_RSSI_THRESHOLD = 124;

		private const int INC_RSSI_THRESHOLD = 1;

		private const int SCL_RSSI_THRESHOLD = -1;

		private const int LEN_RSSI_THRESHOLD = 4;

		public const string SZ_SQUELCH_NAME = "Squelch";

		public const string SZ_SQUELCH_LEVEL_NAME = "SquelchLevel";

		public const string SZ_VOICE_EMPHASIS_NAME = "VoiceEmphasis";

		public const string SZ_STE_NAME = "Ste";

		public const string SZ_NON_STE_NAME = "NonSte";

		public const string SZ_SIGNALING_SYSTEM_NAME = "SignalingSystem";

		public const string SZ_UNMUTE_RULE_NAME = "UnmuteRule";

		public const string SZ_PTTID_TYPE_NAME = "PttidType";

		public const string SZ_ARTS_NAME = "Arts";

		private const int MIN_COLOR_CODE = 0;

		private const int MAX_COLOR_CODE_DUAL_CAPACITY = 14;

		private const int MAX_COLOR_CODE = 15;

		private const int INC_COLOR_CODE = 1;

		private const int SCL_COLOR_CODE = 1;

		private const int LEN_COLOR_CODE = 2;

		private const int MIN_ARTS_INTERVAL = 22;

		private const int MAX_ARTS_INTERVAL = 55;

		private const int INC_ARTS_INTERVAL = 1;

		private const int SCL_ARTS_INTERVAL = 1;

		private const int LEN_ARTS_INTERVAL = 2;

		public const string SZ_TIMING_PREFERENCE_NAME = "TimingPreference";

		public const string SZ_ARS_NAME = "Ars";

		public const string SZ_KEY_SWITCH_NAME = "KeySwitch";

		public const string SZ_OFFSET_DIRECTION_NAME = "OffsetDirection";

		private const int INC_OFFSET_FREQ = 1;

		public static decimal SCL_OFFSET_FREQ;

		private const int LEN_OFFSET_FREQ = 6;

		private const int MIN_OFFSET_FREQ = 1;

		private const int MAX_OFFSET_FREQ = 38400;

		private const int DEF_OFFSET_FREQ = 1000;

		private const int SCL_OFFSET_FREQ_MHZ = 1000;

		private CheckBox chkEnhancedChAccess;

		private CheckBox chkEmgConfirmed;

		private CheckBox chkTxInteruptFreq;

		private CheckBox chkAllowTxInterupt;

		private Label lblContact;

		private CustomCombo cmbContact;

		private Label lblEmgSystem;

		private CustomCombo cmbAPRS_System;

		private Label lblTxColor;

		private Label lblRxGroup;

		private CustomCombo cmbRxGroup;

		private Label lblRxColor;

		private Label lblKey;

		private CustomCombo cmbKey;

		private Label lblKeySwitch;

		private ComboBox cmbKeySwitch;

		private Label lblArs;

		private ComboBox cmbArs;

		private Label lblRepeaterSlot;

		private ComboBox cmbRepeaterSlot;

		private Label lblTimingPreference;

		private ComboBox cmbTimingPreference;

		private CheckBox chkDualCapacity;

		private Label lblTxTone;

		private ComboBox cmbTxTone;

		private Label lblArtsInterval;

		private CustomNumericUpDown nudArtsInterval;

		private Label lblRxTone;

		private ComboBox cmbRxTone;

		private Label lblNonSte;

		private ComboBox cmbNonSte;

		private Label lblSte;

		private ComboBox cmbSte;

		private Label lblVoiceEmphasis;

		private ComboBox cmbVoiceEmphasis;

		private Label lblChBandwidth;

		private ComboBox cmbChBandwidth;

		private Label lblChMode;

		private ComboBox cmbChMode;

		private Label lblChName;

		private SGTextBox txtName;

		private Label lblRxFreq;

		private TextBox txtRxFreq;

		private ComboBox cmbLibreDMR_Power;

		private Label lblTxFreq;

		private TextBox txtTxFreq;

		private Label lblPower;

		private Label lblTot;

		private CustomNumericUpDown nudTot;

		private CheckBox chkVox;

		private CheckBox chkOpenGD77ScanZoneSkip;

		private CheckBox chkOpenGD77ScanAllSkip;

		private CheckBox chkRxOnly;

		private CheckBox chkNoBeep;

		private CheckBox chkNoEco;

		private DoubleClickGroupBox grpAnalog;

		private DoubleClickGroupBox grpDigit;

		private CustomNumericUpDown nudTxColor;

		private CustomCombo cmbTS1TaTx;

		private Label lblTS1TaTx;

		private ComboBox cmbSql;

		private Label lblSql;

		private MenuStrip mnsCh;

		private ToolStripMenuItem tsmiCh;

		private ToolStripMenuItem tsmiFirst;

		private ToolStripMenuItem tsmiPrev;

		private ToolStripMenuItem tsmiNext;

		private ToolStripMenuItem tsmiLast;

		private ToolStripMenuItem tsmiAdd;

		private ToolStripMenuItem tsmiDel;

		private CustomPanel pnlChannel;

		private Button btnCopy;

		private ComboBox cmbOffsetStep;

		private Label lblOffsetStep;

		public static readonly int SPACE_CH;

		public static readonly string[] SZ_CH_MODE;

		private static readonly string[] SZ_REF_FREQ;

		private static readonly string[] SZ_LIBREDMR_POWER;

		public static readonly string[] SZ_POWER;

		private static readonly string[] SZ_ADMIT_CRITERICA;

		private static readonly string[] SZ_ADMIT_CRITERICA_D;

		private static readonly string[] SZ_BANDWIDTH;

		private static readonly string[] SZ_SQUELCH;

		private static readonly string[] SZ_SQUELCH_LEVEL;

		private static readonly string[] SZ_VOICE_EMPHASIS;

		private static readonly string[] SZ_STE;

		private static readonly string[] SZ_NON_STE;

		private static readonly string[] SZ_SIGNALING_SYSTEM;

		private static readonly string[] SZ_UNMUTE_RULE;

		private static readonly string[] SZ_PTTID_TYPE;

		private static readonly string[] SZ_TIMING_PREFERENCE;

		private static readonly string[] SZ_REPEATER_SOLT;

		private static readonly string[] SZ_ARS;

		private static readonly string[] SZ_KEY_SWITCH;

		private static readonly string[] SZ_OFFSET_DIRECTION;

		private static readonly string[] SZ_OFFSET_STEP;

		public static ChannelOne DefaultCh;

		public static Vfo data;

		private Label lblOverrideMasterDmrId;

		private CustomCombo cmbTS2TaTx;

		private Label lblTS2TaTx;

		private CheckBox chkDMRForceDMO;

		private Label lblDmrForceDmo;

		private SGTextBox txtRadioId;

		public static int CurCntCh { get; set; }

		public TreeNode Node { get; set; }

		private void txtRadioId_Validating(object sender, CancelEventArgs e)
		{
			if (!string.IsNullOrEmpty(txtRadioId.Text))
			{
				int num = Convert.ToInt32(txtRadioId.Text);
				if (num < 1 || num > 16776415)
				{
					txtRadioId.Text = "";
				}
			}
		}

		private void txtRadioId_Leave(object sender, EventArgs e)
		{
			if (txtRadioId.Text.Length <= 8)
			{
				int index = Convert.ToInt32(base.Tag);
				data.SetChannelDMRid(index, txtRadioId.Text);
				txtRadioId.Text = data[index].LibreDMR_DMRidChannel;
			}
		}

		private void chkNoBeep_CheckedChanged(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(base.Tag);
			data.SetChannelNoBeep(num % 1024, chkNoBeep.Checked);
		}

		private void chkNoEco_CheckedChanged(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(base.Tag);
			data.SetChannelNoEco(num % 1024, chkNoEco.Checked);
		}

		private void chkDMRForceDMO_CheckedChanged(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(base.Tag);
			data.SetChannelDMRForceDMO(num % 1024, chkDMRForceDMO.Checked);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.mnsCh = new System.Windows.Forms.MenuStrip();
			this.tsmiCh = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFirst = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiNext = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiLast = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlChannel = new CustomPanel();
			this.cmbOffsetStep = new System.Windows.Forms.ComboBox();
			this.lblOffsetStep = new System.Windows.Forms.Label();
			this.btnCopy = new System.Windows.Forms.Button();
			this.txtName = new DMR.SGTextBox();
			this.grpDigit = new DoubleClickGroupBox();
			this.nudTxColor = new CustomNumericUpDown();
			this.cmbTimingPreference = new System.Windows.Forms.ComboBox();
			this.cmbRepeaterSlot = new System.Windows.Forms.ComboBox();
			this.lblTimingPreference = new System.Windows.Forms.Label();
			this.cmbArs = new System.Windows.Forms.ComboBox();
			this.lblRepeaterSlot = new System.Windows.Forms.Label();
			this.cmbKeySwitch = new System.Windows.Forms.ComboBox();
			this.lblArs = new System.Windows.Forms.Label();
			this.cmbKey = new CustomCombo();
			this.lblKeySwitch = new System.Windows.Forms.Label();
			this.lblKey = new System.Windows.Forms.Label();
			this.cmbRxGroup = new CustomCombo();
			this.lblTxColor = new System.Windows.Forms.Label();
			this.cmbTS2TaTx = new CustomCombo();
			this.cmbTS1TaTx = new CustomCombo();
			this.cmbContact = new CustomCombo();
			this.lblTS2TaTx = new System.Windows.Forms.Label();
			this.lblContact = new System.Windows.Forms.Label();
			this.lblTS1TaTx = new System.Windows.Forms.Label();
			this.chkDualCapacity = new System.Windows.Forms.CheckBox();
			this.chkDMRForceDMO = new System.Windows.Forms.CheckBox();
			this.chkAllowTxInterupt = new System.Windows.Forms.CheckBox();
			this.chkTxInteruptFreq = new System.Windows.Forms.CheckBox();
			this.chkEmgConfirmed = new System.Windows.Forms.CheckBox();
			this.chkEnhancedChAccess = new System.Windows.Forms.CheckBox();
			this.lblRxColor = new System.Windows.Forms.Label();
			this.lblRxGroup = new System.Windows.Forms.Label();
			this.lblDmrForceDmo = new System.Windows.Forms.Label();
			this.lblOverrideMasterDmrId = new System.Windows.Forms.Label();
			this.txtRadioId = new DMR.SGTextBox();
			this.chkRxOnly = new System.Windows.Forms.CheckBox();
			this.cmbLibreDMR_Power = new System.Windows.Forms.ComboBox();
			this.grpAnalog = new DoubleClickGroupBox();
			this.nudArtsInterval = new CustomNumericUpDown();
			this.cmbChBandwidth = new System.Windows.Forms.ComboBox();
			this.lblChBandwidth = new System.Windows.Forms.Label();
			this.cmbVoiceEmphasis = new System.Windows.Forms.ComboBox();
			this.cmbSte = new System.Windows.Forms.ComboBox();
			this.lblVoiceEmphasis = new System.Windows.Forms.Label();
			this.cmbNonSte = new System.Windows.Forms.ComboBox();
			this.lblSte = new System.Windows.Forms.Label();
			this.cmbRxTone = new System.Windows.Forms.ComboBox();
			this.lblNonSte = new System.Windows.Forms.Label();
			this.cmbSql = new System.Windows.Forms.ComboBox();
			this.lblRxTone = new System.Windows.Forms.Label();
			this.lblSql = new System.Windows.Forms.Label();
			this.cmbAPRS_System = new CustomCombo();
			this.lblEmgSystem = new System.Windows.Forms.Label();
			this.lblArtsInterval = new System.Windows.Forms.Label();
			this.cmbTxTone = new System.Windows.Forms.ComboBox();
			this.lblTxTone = new System.Windows.Forms.Label();
			this.chkOpenGD77ScanAllSkip = new System.Windows.Forms.CheckBox();
			this.chkVox = new System.Windows.Forms.CheckBox();
			this.chkOpenGD77ScanZoneSkip = new System.Windows.Forms.CheckBox();
			this.chkNoBeep = new System.Windows.Forms.CheckBox();
			this.chkNoEco = new System.Windows.Forms.CheckBox();
			this.cmbChMode = new System.Windows.Forms.ComboBox();
			this.lblChName = new System.Windows.Forms.Label();
			this.txtTxFreq = new System.Windows.Forms.TextBox();
			this.lblChMode = new System.Windows.Forms.Label();
			this.lblTot = new System.Windows.Forms.Label();
			this.txtRxFreq = new System.Windows.Forms.TextBox();
			this.lblRxFreq = new System.Windows.Forms.Label();
			this.lblPower = new System.Windows.Forms.Label();
			this.lblTxFreq = new System.Windows.Forms.Label();
			this.nudTot = new CustomNumericUpDown();
			this.mnsCh.SuspendLayout();
			this.pnlChannel.SuspendLayout();
			this.grpDigit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.nudTxColor).BeginInit();
			this.grpAnalog.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.nudArtsInterval).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.nudTot).BeginInit();
			base.SuspendLayout();
			this.mnsCh.AllowMerge = false;
			this.mnsCh.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.tsmiCh });
			this.mnsCh.Location = new System.Drawing.Point(0, 0);
			this.mnsCh.Name = "mnsCh";
			this.mnsCh.Size = new System.Drawing.Size(1104, 25);
			this.mnsCh.TabIndex = 32;
			this.mnsCh.Text = "menuStrip1";
			this.mnsCh.Visible = false;
			this.tsmiCh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsmiFirst, this.tsmiPrev, this.tsmiNext, this.tsmiLast, this.tsmiAdd, this.tsmiDel });
			this.tsmiCh.Name = "tsmiCh";
			this.tsmiCh.Size = new System.Drawing.Size(43, 21);
			this.tsmiCh.Text = "操作";
			this.tsmiFirst.Name = "tsmiFirst";
			this.tsmiFirst.Size = new System.Drawing.Size(119, 22);
			this.tsmiFirst.Text = "First";
			this.tsmiPrev.Name = "tsmiPrev";
			this.tsmiPrev.Size = new System.Drawing.Size(119, 22);
			this.tsmiPrev.Text = "Previous";
			this.tsmiNext.Name = "tsmiNext";
			this.tsmiNext.Size = new System.Drawing.Size(119, 22);
			this.tsmiNext.Text = "Next";
			this.tsmiLast.Name = "tsmiLast";
			this.tsmiLast.Size = new System.Drawing.Size(119, 22);
			this.tsmiLast.Text = "Last";
			this.tsmiAdd.Name = "tsmiAdd";
			this.tsmiAdd.Size = new System.Drawing.Size(119, 22);
			this.tsmiAdd.Text = "Add";
			this.tsmiDel.Name = "tsmiDel";
			this.tsmiDel.Size = new System.Drawing.Size(119, 22);
			this.tsmiDel.Text = "Delete";
			this.pnlChannel.AutoScroll = true;
			this.pnlChannel.AutoSize = true;
			this.pnlChannel.Controls.Add(this.cmbOffsetStep);
			this.pnlChannel.Controls.Add(this.lblOffsetStep);
			this.pnlChannel.Controls.Add(this.btnCopy);
			this.pnlChannel.Controls.Add(this.txtName);
			this.pnlChannel.Controls.Add(this.grpDigit);
			this.pnlChannel.Controls.Add(this.chkRxOnly);
			this.pnlChannel.Controls.Add(this.cmbLibreDMR_Power);
			this.pnlChannel.Controls.Add(this.grpAnalog);
			this.pnlChannel.Controls.Add(this.cmbAPRS_System);
			this.pnlChannel.Controls.Add(this.lblEmgSystem);
			this.pnlChannel.Controls.Add(this.chkOpenGD77ScanAllSkip);
			this.pnlChannel.Controls.Add(this.chkVox);
			this.pnlChannel.Controls.Add(this.chkOpenGD77ScanZoneSkip);
			this.pnlChannel.Controls.Add(this.chkNoBeep);
			this.pnlChannel.Controls.Add(this.chkNoEco);
			this.pnlChannel.Controls.Add(this.cmbChMode);
			this.pnlChannel.Controls.Add(this.lblChName);
			this.pnlChannel.Controls.Add(this.txtTxFreq);
			this.pnlChannel.Controls.Add(this.lblChMode);
			this.pnlChannel.Controls.Add(this.lblTot);
			this.pnlChannel.Controls.Add(this.txtRxFreq);
			this.pnlChannel.Controls.Add(this.lblRxFreq);
			this.pnlChannel.Controls.Add(this.lblPower);
			this.pnlChannel.Controls.Add(this.lblTxFreq);
			this.pnlChannel.Controls.Add(this.nudTot);
			this.pnlChannel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlChannel.Location = new System.Drawing.Point(0, 0);
			this.pnlChannel.Name = "pnlChannel";
			this.pnlChannel.Size = new System.Drawing.Size(1101, 430);
			this.pnlChannel.TabIndex = 0;
			this.pnlChannel.TabStop = true;
			this.cmbOffsetStep.AccessibleDescription = "Step";
			this.cmbOffsetStep.AccessibleName = "Step";
			this.cmbOffsetStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbOffsetStep.FormattingEnabled = true;
			this.cmbOffsetStep.Location = new System.Drawing.Point(347, 56);
			this.cmbOffsetStep.Name = "cmbOffsetStep";
			this.cmbOffsetStep.Size = new System.Drawing.Size(120, 24);
			this.cmbOffsetStep.TabIndex = 7;
			this.lblOffsetStep.Location = new System.Drawing.Point(228, 56);
			this.lblOffsetStep.Name = "lblOffsetStep";
			this.lblOffsetStep.Size = new System.Drawing.Size(113, 20);
			this.lblOffsetStep.TabIndex = 6;
			this.lblOffsetStep.Text = "Offset Step";
			this.lblOffsetStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnCopy.AccessibleDescription = "Copy Rx frequency to Tx frequency";
			this.btnCopy.AccessibleName = "Copy Rx frequency to Tx frequency";
			this.btnCopy.Location = new System.Drawing.Point(473, 26);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(33, 23);
			this.btnCopy.TabIndex = 10;
			this.btnCopy.Text = ">>";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(btnCopy_Click);
			this.txtName.AccessibleDescription = "Name";
			this.txtName.AccessibleName = "Name";
			this.txtName.Enabled = false;
			this.txtName.InputString = null;
			this.txtName.Location = new System.Drawing.Point(77, 56);
			this.txtName.MaxByteLength = 0;
			this.txtName.Name = "txtName";
			this.txtName.OnlyAllowInputStringAndCapitaliseCharacters = false;
			this.txtName.Size = new System.Drawing.Size(120, 23);
			this.txtName.TabIndex = 3;
			this.grpDigit.Controls.Add(this.nudTxColor);
			this.grpDigit.Controls.Add(this.cmbTimingPreference);
			this.grpDigit.Controls.Add(this.cmbRepeaterSlot);
			this.grpDigit.Controls.Add(this.lblTimingPreference);
			this.grpDigit.Controls.Add(this.cmbArs);
			this.grpDigit.Controls.Add(this.lblRepeaterSlot);
			this.grpDigit.Controls.Add(this.cmbKeySwitch);
			this.grpDigit.Controls.Add(this.lblArs);
			this.grpDigit.Controls.Add(this.cmbKey);
			this.grpDigit.Controls.Add(this.lblKeySwitch);
			this.grpDigit.Controls.Add(this.lblKey);
			this.grpDigit.Controls.Add(this.cmbRxGroup);
			this.grpDigit.Controls.Add(this.lblTxColor);
			this.grpDigit.Controls.Add(this.cmbTS2TaTx);
			this.grpDigit.Controls.Add(this.cmbTS1TaTx);
			this.grpDigit.Controls.Add(this.cmbContact);
			this.grpDigit.Controls.Add(this.lblTS2TaTx);
			this.grpDigit.Controls.Add(this.lblContact);
			this.grpDigit.Controls.Add(this.lblTS1TaTx);
			this.grpDigit.Controls.Add(this.chkDualCapacity);
			this.grpDigit.Controls.Add(this.chkDMRForceDMO);
			this.grpDigit.Controls.Add(this.chkAllowTxInterupt);
			this.grpDigit.Controls.Add(this.chkTxInteruptFreq);
			this.grpDigit.Controls.Add(this.chkEmgConfirmed);
			this.grpDigit.Controls.Add(this.chkEnhancedChAccess);
			this.grpDigit.Controls.Add(this.lblRxColor);
			this.grpDigit.Controls.Add(this.lblRxGroup);
			this.grpDigit.Controls.Add(this.lblDmrForceDmo);
			this.grpDigit.Controls.Add(this.lblOverrideMasterDmrId);
			this.grpDigit.Controls.Add(this.txtRadioId);
			this.grpDigit.Location = new System.Drawing.Point(411, 136);
			this.grpDigit.Name = "grpDigit";
			this.grpDigit.Size = new System.Drawing.Size(388, 267);
			this.grpDigit.TabIndex = 30;
			this.grpDigit.TabStop = false;
			this.grpDigit.Text = "Digital";
			this.nudTxColor.AccessibleDescription = "Colour code";
			this.nudTxColor.AccessibleName = "Colour code";
			this.nudTxColor.Location = new System.Drawing.Point(238, 53);
			this.nudTxColor.Name = "nudTxColor";
			this.nudTxColor.Size = new System.Drawing.Size(120, 23);
			this.nudTxColor.TabIndex = 34;
			this.cmbTimingPreference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTimingPreference.FormattingEnabled = true;
			this.cmbTimingPreference.Location = new System.Drawing.Point(101, 344);
			this.cmbTimingPreference.Name = "cmbTimingPreference";
			this.cmbTimingPreference.Size = new System.Drawing.Size(120, 24);
			this.cmbTimingPreference.TabIndex = 2;
			this.cmbTimingPreference.Visible = false;
			this.cmbRepeaterSlot.AccessibleDescription = "Timeslot";
			this.cmbRepeaterSlot.AccessibleName = "Timeslot";
			this.cmbRepeaterSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRepeaterSlot.FormattingEnabled = true;
			this.cmbRepeaterSlot.Location = new System.Drawing.Point(238, 110);
			this.cmbRepeaterSlot.Name = "cmbRepeaterSlot";
			this.cmbRepeaterSlot.Size = new System.Drawing.Size(120, 24);
			this.cmbRepeaterSlot.TabIndex = 38;
			this.lblTimingPreference.Location = new System.Drawing.Point(-52, 344);
			this.lblTimingPreference.Name = "lblTimingPreference";
			this.lblTimingPreference.Size = new System.Drawing.Size(143, 20);
			this.lblTimingPreference.TabIndex = 1;
			this.lblTimingPreference.Text = "Timing Leader Prefernce";
			this.lblTimingPreference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblTimingPreference.Visible = false;
			this.cmbArs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbArs.FormattingEnabled = true;
			this.cmbArs.Location = new System.Drawing.Point(101, 370);
			this.cmbArs.Name = "cmbArs";
			this.cmbArs.Size = new System.Drawing.Size(120, 24);
			this.cmbArs.TabIndex = 6;
			this.cmbArs.Visible = false;
			this.lblRepeaterSlot.Location = new System.Drawing.Point(11, 110);
			this.lblRepeaterSlot.Name = "lblRepeaterSlot";
			this.lblRepeaterSlot.Size = new System.Drawing.Size(215, 20);
			this.lblRepeaterSlot.TabIndex = 37;
			this.lblRepeaterSlot.Text = "Repeater/Time Slot";
			this.lblRepeaterSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cmbKeySwitch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbKeySwitch.FormattingEnabled = true;
			this.cmbKeySwitch.Location = new System.Drawing.Point(361, 350);
			this.cmbKeySwitch.Name = "cmbKeySwitch";
			this.cmbKeySwitch.Size = new System.Drawing.Size(120, 24);
			this.cmbKeySwitch.TabIndex = 8;
			this.cmbKeySwitch.Visible = false;
			this.cmbKeySwitch.SelectedIndexChanged += new System.EventHandler(cmbKeySwitch_SelectedIndexChanged);
			this.lblArs.Location = new System.Drawing.Point(-52, 370);
			this.lblArs.Name = "lblArs";
			this.lblArs.Size = new System.Drawing.Size(143, 20);
			this.lblArs.TabIndex = 5;
			this.lblArs.Text = "ARS";
			this.lblArs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblArs.Visible = false;
			this.cmbKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbKey.FormattingEnabled = true;
			this.cmbKey.Location = new System.Drawing.Point(361, 380);
			this.cmbKey.Name = "cmbKey";
			this.cmbKey.Size = new System.Drawing.Size(120, 24);
			this.cmbKey.TabIndex = 10;
			this.cmbKey.Visible = false;
			this.lblKeySwitch.Location = new System.Drawing.Point(183, 350);
			this.lblKeySwitch.Name = "lblKeySwitch";
			this.lblKeySwitch.Size = new System.Drawing.Size(166, 20);
			this.lblKeySwitch.TabIndex = 7;
			this.lblKeySwitch.Text = "Privacy";
			this.lblKeySwitch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblKeySwitch.Visible = false;
			this.lblKey.Location = new System.Drawing.Point(183, 380);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(166, 20);
			this.lblKey.TabIndex = 9;
			this.lblKey.Text = "Privacy Group";
			this.lblKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblKey.Visible = false;
			this.cmbRxGroup.AccessibleDescription = "Talkgroup list";
			this.cmbRxGroup.AccessibleName = "Talkgroup list";
			this.cmbRxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRxGroup.FormattingEnabled = true;
			this.cmbRxGroup.Location = new System.Drawing.Point(238, 24);
			this.cmbRxGroup.Name = "cmbRxGroup";
			this.cmbRxGroup.Size = new System.Drawing.Size(120, 24);
			this.cmbRxGroup.TabIndex = 32;
			this.cmbRxGroup.SelectedIndexChanged += new System.EventHandler(cmbRxGrp_SelectedIndexChanged);
			this.lblTxColor.Location = new System.Drawing.Point(17, 53);
			this.lblTxColor.Name = "lblTxColor";
			this.lblTxColor.Size = new System.Drawing.Size(209, 20);
			this.lblTxColor.TabIndex = 33;
			this.lblTxColor.Text = "Color Code";
			this.lblTxColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cmbTS2TaTx.AccessibleDescription = "TS2 Talker alias transmit";
			this.cmbTS2TaTx.AccessibleName = "TS2 Talker alias transmit";
			this.cmbTS2TaTx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS2TaTx.FormattingEnabled = true;
			this.cmbTS2TaTx.Location = new System.Drawing.Point(240, 171);
			this.cmbTS2TaTx.Name = "cmbTS2TaTx";
			this.cmbTS2TaTx.Size = new System.Drawing.Size(118, 24);
			this.cmbTS2TaTx.TabIndex = 42;
			this.cmbTS1TaTx.AccessibleDescription = "TS1 Talker alias transmit";
			this.cmbTS1TaTx.AccessibleName = "TS1 Talker alias transmit";
			this.cmbTS1TaTx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTS1TaTx.FormattingEnabled = true;
			this.cmbTS1TaTx.Location = new System.Drawing.Point(240, 140);
			this.cmbTS1TaTx.Name = "cmbTS1TaTx";
			this.cmbTS1TaTx.Size = new System.Drawing.Size(118, 24);
			this.cmbTS1TaTx.TabIndex = 40;
			this.cmbContact.AccessibleDescription = "Contact name";
			this.cmbContact.AccessibleName = "Contact name";
			this.cmbContact.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbContact.FormattingEnabled = true;
			this.cmbContact.Location = new System.Drawing.Point(238, 82);
			this.cmbContact.Name = "cmbContact";
			this.cmbContact.Size = new System.Drawing.Size(120, 24);
			this.cmbContact.TabIndex = 36;
			this.cmbContact.SelectedIndexChanged += new System.EventHandler(cmbContact_SelectedIndexChanged);
			this.lblTS2TaTx.Location = new System.Drawing.Point(5, 171);
			this.lblTS2TaTx.Name = "lblTS2TaTx";
			this.lblTS2TaTx.Size = new System.Drawing.Size(222, 24);
			this.lblTS2TaTx.TabIndex = 41;
			this.lblTS2TaTx.Text = "Talker Alias Tx";
			this.lblTS2TaTx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblContact.Location = new System.Drawing.Point(14, 82);
			this.lblContact.Name = "lblContact";
			this.lblContact.Size = new System.Drawing.Size(212, 20);
			this.lblContact.TabIndex = 35;
			this.lblContact.Text = "Contact Name";
			this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblTS1TaTx.Location = new System.Drawing.Point(5, 140);
			this.lblTS1TaTx.Name = "lblTS1TaTx";
			this.lblTS1TaTx.Size = new System.Drawing.Size(222, 24);
			this.lblTS1TaTx.TabIndex = 39;
			this.lblTS1TaTx.Text = "Talker Alias Tx";
			this.lblTS1TaTx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkDualCapacity.AutoSize = true;
			this.chkDualCapacity.Location = new System.Drawing.Point(155, 329);
			this.chkDualCapacity.Name = "chkDualCapacity";
			this.chkDualCapacity.Size = new System.Drawing.Size(194, 20);
			this.chkDualCapacity.TabIndex = 0;
			this.chkDualCapacity.Text = "Dual Capacity Direct Mode";
			this.chkDualCapacity.UseVisualStyleBackColor = true;
			this.chkDualCapacity.Visible = false;
			this.chkDualCapacity.CheckedChanged += new System.EventHandler(chkDualCapacity_CheckedChanged);
			this.chkDMRForceDMO.AccessibleDescription = "DMR Force DMO";
			this.chkDMRForceDMO.AccessibleName = "DMR Force DMO";
			this.chkDMRForceDMO.Location = new System.Drawing.Point(238, 229);
			this.chkDMRForceDMO.Name = "chkDMRForceDMO";
			this.chkDMRForceDMO.Size = new System.Drawing.Size(121, 20);
			this.chkDMRForceDMO.TabIndex = 20;
			this.chkDMRForceDMO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkDMRForceDMO.UseVisualStyleBackColor = true;
			this.chkDMRForceDMO.CheckedChanged += new System.EventHandler(chkDMRForceDMO_CheckedChanged);
			this.chkAllowTxInterupt.AutoSize = true;
			this.chkAllowTxInterupt.Location = new System.Drawing.Point(285, 303);
			this.chkAllowTxInterupt.Name = "chkAllowTxInterupt";
			this.chkAllowTxInterupt.Size = new System.Drawing.Size(134, 20);
			this.chkAllowTxInterupt.TabIndex = 22;
			this.chkAllowTxInterupt.Text = "Allow Interruption";
			this.chkAllowTxInterupt.UseVisualStyleBackColor = true;
			this.chkAllowTxInterupt.Visible = false;
			this.chkTxInteruptFreq.AutoSize = true;
			this.chkTxInteruptFreq.Location = new System.Drawing.Point(285, 329);
			this.chkTxInteruptFreq.Name = "chkTxInteruptFreq";
			this.chkTxInteruptFreq.Size = new System.Drawing.Size(203, 20);
			this.chkTxInteruptFreq.TabIndex = 23;
			this.chkTxInteruptFreq.Text = "Tx Interruptible Frequencies";
			this.chkTxInteruptFreq.UseVisualStyleBackColor = true;
			this.chkTxInteruptFreq.Visible = false;
			this.chkEmgConfirmed.AutoSize = true;
			this.chkEmgConfirmed.Location = new System.Drawing.Point(155, 303);
			this.chkEmgConfirmed.Name = "chkEmgConfirmed";
			this.chkEmgConfirmed.Size = new System.Drawing.Size(162, 20);
			this.chkEmgConfirmed.TabIndex = 26;
			this.chkEmgConfirmed.Text = "Emergency Alarm Ack";
			this.chkEmgConfirmed.UseVisualStyleBackColor = true;
			this.chkEmgConfirmed.Visible = false;
			this.chkEnhancedChAccess.AutoSize = true;
			this.chkEnhancedChAccess.Location = new System.Drawing.Point(285, 354);
			this.chkEnhancedChAccess.Name = "chkEnhancedChAccess";
			this.chkEnhancedChAccess.Size = new System.Drawing.Size(195, 20);
			this.chkEnhancedChAccess.TabIndex = 27;
			this.chkEnhancedChAccess.Text = "Enhanced Channel Access";
			this.chkEnhancedChAccess.UseVisualStyleBackColor = true;
			this.chkEnhancedChAccess.Visible = false;
			this.lblRxColor.Location = new System.Drawing.Point(-52, 314);
			this.lblRxColor.Name = "lblRxColor";
			this.lblRxColor.Size = new System.Drawing.Size(143, 20);
			this.lblRxColor.TabIndex = 12;
			this.lblRxColor.Text = "Rx Color Code";
			this.lblRxColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblRxColor.Visible = false;
			this.lblRxGroup.Location = new System.Drawing.Point(36, 24);
			this.lblRxGroup.Name = "lblRxGroup";
			this.lblRxGroup.Size = new System.Drawing.Size(190, 20);
			this.lblRxGroup.TabIndex = 31;
			this.lblRxGroup.Text = "Rx Group List";
			this.lblRxGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblDmrForceDmo.Location = new System.Drawing.Point(8, 226);
			this.lblDmrForceDmo.Name = "lblDMRForceDMO";
			this.lblDmrForceDmo.Size = new System.Drawing.Size(218, 24);
			this.lblDmrForceDmo.TabIndex = 29;
			this.lblDmrForceDmo.Text = "Force DMO";
			this.lblDmrForceDmo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblOverrideMasterDmrId.Location = new System.Drawing.Point(8, 200);
			this.lblOverrideMasterDmrId.Name = "lblOverrideMasterDmrId";
			this.lblOverrideMasterDmrId.Size = new System.Drawing.Size(218, 24);
			this.lblOverrideMasterDmrId.TabIndex = 29;
			this.lblOverrideMasterDmrId.Text = "Override masterDMR ID";
			this.lblOverrideMasterDmrId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.txtRadioId.AccessibleDescription = "DMR ID";
			this.txtRadioId.AccessibleName = "DMR ID";
			this.txtRadioId.InputString = null;
			this.txtRadioId.Location = new System.Drawing.Point(238, 201);
			this.txtRadioId.MaxByteLength = 0;
			this.txtRadioId.Name = "txtRadioId";
			this.txtRadioId.OnlyAllowInputStringAndCapitaliseCharacters = false;
			this.txtRadioId.Size = new System.Drawing.Size(120, 23);
			this.txtRadioId.TabIndex = 30;
			this.txtRadioId.Leave += new System.EventHandler(txtRadioId_Leave);
			this.txtRadioId.Validating += new System.ComponentModel.CancelEventHandler(txtRadioId_Validating);
			this.chkRxOnly.AccessibleDescription = "Rx only";
			this.chkRxOnly.AccessibleName = "Rx only";
			this.chkRxOnly.Location = new System.Drawing.Point(785, 63);
			this.chkRxOnly.Name = "chkRxOnly";
			this.chkRxOnly.Size = new System.Drawing.Size(295, 20);
			this.chkRxOnly.TabIndex = 18;
			this.chkRxOnly.Text = "Rx Only";
			this.chkRxOnly.UseVisualStyleBackColor = true;
			this.chkRxOnly.CheckedChanged += new System.EventHandler(chkRxOnly_CheckedChanged);
			this.cmbLibreDMR_Power.AccessibleDescription = "Channel power";
			this.cmbLibreDMR_Power.AccessibleName = "Channel power";
			this.cmbLibreDMR_Power.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLibreDMR_Power.FormattingEnabled = true;
			this.cmbLibreDMR_Power.Location = new System.Drawing.Point(348, 85);
			this.cmbLibreDMR_Power.Name = "cmbLibreDMR_Power";
			this.cmbLibreDMR_Power.Size = new System.Drawing.Size(119, 24);
			this.cmbLibreDMR_Power.TabIndex = 9;
			this.cmbLibreDMR_Power.Visible = false;
			this.grpAnalog.Controls.Add(this.nudArtsInterval);
			this.grpAnalog.Controls.Add(this.cmbChBandwidth);
			this.grpAnalog.Controls.Add(this.lblChBandwidth);
			this.grpAnalog.Controls.Add(this.cmbVoiceEmphasis);
			this.grpAnalog.Controls.Add(this.cmbSte);
			this.grpAnalog.Controls.Add(this.lblVoiceEmphasis);
			this.grpAnalog.Controls.Add(this.cmbNonSte);
			this.grpAnalog.Controls.Add(this.lblSte);
			this.grpAnalog.Controls.Add(this.cmbRxTone);
			this.grpAnalog.Controls.Add(this.lblNonSte);
			this.grpAnalog.Controls.Add(this.cmbSql);
			this.grpAnalog.Controls.Add(this.lblRxTone);
			this.grpAnalog.Controls.Add(this.lblSql);
			this.grpAnalog.Controls.Add(this.lblArtsInterval);
			this.grpAnalog.Controls.Add(this.cmbTxTone);
			this.grpAnalog.Controls.Add(this.lblTxTone);
			this.grpAnalog.Location = new System.Drawing.Point(24, 136);
			this.grpAnalog.Name = "grpAnalog";
			this.grpAnalog.Size = new System.Drawing.Size(365, 197);
			this.grpAnalog.TabIndex = 29;
			this.grpAnalog.TabStop = false;
			this.grpAnalog.Text = "Analog";
			this.nudArtsInterval.Location = new System.Drawing.Point(401, 263);
			this.nudArtsInterval.Name = "nudArtsInterval";
			this.nudArtsInterval.Size = new System.Drawing.Size(120, 23);
			this.nudArtsInterval.TabIndex = 25;
			this.nudArtsInterval.Visible = false;
			this.cmbChBandwidth.AccessibleDescription = "Bandwidth";
			this.cmbChBandwidth.AccessibleName = "Bandwidth";
			this.cmbChBandwidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbChBandwidth.FormattingEnabled = true;
			this.cmbChBandwidth.Location = new System.Drawing.Point(223, 23);
			this.cmbChBandwidth.Name = "cmbChBandwidth";
			this.cmbChBandwidth.Size = new System.Drawing.Size(120, 24);
			this.cmbChBandwidth.TabIndex = 24;
			this.lblChBandwidth.Location = new System.Drawing.Point(21, 24);
			this.lblChBandwidth.Name = "lblChBandwidth";
			this.lblChBandwidth.Size = new System.Drawing.Size(196, 20);
			this.lblChBandwidth.TabIndex = 23;
			this.lblChBandwidth.Text = "Channel Bandwidth [KHz]";
			this.lblChBandwidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cmbVoiceEmphasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbVoiceEmphasis.FormattingEnabled = true;
			this.cmbVoiceEmphasis.Location = new System.Drawing.Point(401, 376);
			this.cmbVoiceEmphasis.Name = "cmbVoiceEmphasis";
			this.cmbVoiceEmphasis.Size = new System.Drawing.Size(120, 24);
			this.cmbVoiceEmphasis.TabIndex = 5;
			this.cmbVoiceEmphasis.Visible = false;
			this.cmbSte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSte.FormattingEnabled = true;
			this.cmbSte.Location = new System.Drawing.Point(401, 317);
			this.cmbSte.Name = "cmbSte";
			this.cmbSte.Size = new System.Drawing.Size(120, 24);
			this.cmbSte.TabIndex = 7;
			this.cmbSte.Visible = false;
			this.lblVoiceEmphasis.Location = new System.Drawing.Point(247, 376);
			this.lblVoiceEmphasis.Name = "lblVoiceEmphasis";
			this.lblVoiceEmphasis.Size = new System.Drawing.Size(143, 20);
			this.lblVoiceEmphasis.TabIndex = 4;
			this.lblVoiceEmphasis.Text = "Voice Emphasis";
			this.lblVoiceEmphasis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblVoiceEmphasis.Visible = false;
			this.cmbNonSte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNonSte.FormattingEnabled = true;
			this.cmbNonSte.Location = new System.Drawing.Point(401, 347);
			this.cmbNonSte.Name = "cmbNonSte";
			this.cmbNonSte.Size = new System.Drawing.Size(120, 24);
			this.cmbNonSte.TabIndex = 9;
			this.cmbNonSte.Visible = false;
			this.lblSte.Location = new System.Drawing.Point(247, 317);
			this.lblSte.Name = "lblSte";
			this.lblSte.Size = new System.Drawing.Size(143, 20);
			this.lblSte.TabIndex = 6;
			this.lblSte.Text = "STE";
			this.lblSte.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblSte.Visible = false;
			this.cmbRxTone.AccessibleDescription = "Rx CTCSS / DCS";
			this.cmbRxTone.AccessibleName = "Rx CTCSS / DCS";
			this.cmbRxTone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRxTone.FormattingEnabled = true;
			this.cmbRxTone.Location = new System.Drawing.Point(223, 86);
			this.cmbRxTone.MaxLength = 5;
			this.cmbRxTone.Name = "cmbRxTone";
			this.cmbRxTone.Size = new System.Drawing.Size(120, 24);
			this.cmbRxTone.TabIndex = 22;
			this.cmbRxTone.SelectedIndexChanged += new System.EventHandler(cmbRxTone_SelectedIndexChanged);
			this.cmbRxTone.KeyDown += new System.Windows.Forms.KeyEventHandler(cmbRxTone_KeyDown);
			this.cmbRxTone.Validating += new System.ComponentModel.CancelEventHandler(cmbRxTone_Validating);
			this.lblNonSte.Location = new System.Drawing.Point(247, 347);
			this.lblNonSte.Name = "lblNonSte";
			this.lblNonSte.Size = new System.Drawing.Size(143, 20);
			this.lblNonSte.TabIndex = 8;
			this.lblNonSte.Text = "Non STE";
			this.lblNonSte.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblNonSte.Visible = false;
			this.cmbSql.AccessibleDescription = "Channel squelch level";
			this.cmbSql.AccessibleName = "Channel squelch level";
			this.cmbSql.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSql.FormattingEnabled = true;
			this.cmbSql.Location = new System.Drawing.Point(223, 116);
			this.cmbSql.Name = "cmbSql";
			this.cmbSql.Size = new System.Drawing.Size(120, 24);
			this.cmbSql.TabIndex = 28;
			this.lblRxTone.Location = new System.Drawing.Point(40, 85);
			this.lblRxTone.Name = "lblRxTone";
			this.lblRxTone.Size = new System.Drawing.Size(177, 20);
			this.lblRxTone.TabIndex = 21;
			this.lblRxTone.Text = "Rx CTCSS/DCS [Hz]";
			this.lblRxTone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblSql.Location = new System.Drawing.Point(37, 120);
			this.lblSql.Name = "lblSql";
			this.lblSql.Size = new System.Drawing.Size(180, 20);
			this.lblSql.TabIndex = 27;
			this.lblSql.Text = "Squelch Level";
			this.lblSql.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cmbAPRS_System.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAPRS_System.FormattingEnabled = true;
			this.cmbAPRS_System.Location = new System.Drawing.Point(243, 346);
			this.cmbAPRS_System.Name = "cmbAPRS_System";
			this.cmbAPRS_System.Size = new System.Drawing.Size(120, 24);
			this.cmbAPRS_System.TabIndex = 19;
			this.cmbAPRS_System.SelectedIndexChanged += new System.EventHandler(cmbAPRS_SelectedIndexChanged);
			this.lblEmgSystem.Location = new System.Drawing.Point(46, 347);
			this.lblEmgSystem.Name = "lblEmgSystem";
			this.lblEmgSystem.Size = new System.Drawing.Size(181, 20);
			this.lblEmgSystem.TabIndex = 18;
			this.lblEmgSystem.Text = "APRS";
			this.lblEmgSystem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblArtsInterval.Location = new System.Drawing.Point(271, 263);
			this.lblArtsInterval.Name = "lblArtsInterval";
			this.lblArtsInterval.Size = new System.Drawing.Size(119, 20);
			this.lblArtsInterval.TabIndex = 24;
			this.lblArtsInterval.Text = "ARTS Interval [s]";
			this.lblArtsInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblArtsInterval.Visible = false;
			this.cmbTxTone.AccessibleDescription = "Tx CTCSS / DCS";
			this.cmbTxTone.AccessibleName = "Tx CTCSS / DCS";
			this.cmbTxTone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTxTone.FormattingEnabled = true;
			this.cmbTxTone.Location = new System.Drawing.Point(223, 53);
			this.cmbTxTone.MaxLength = 5;
			this.cmbTxTone.Name = "cmbTxTone";
			this.cmbTxTone.Size = new System.Drawing.Size(120, 24);
			this.cmbTxTone.TabIndex = 26;
			this.cmbTxTone.SelectedIndexChanged += new System.EventHandler(SwsqRwFuko);
			this.cmbTxTone.KeyDown += new System.Windows.Forms.KeyEventHandler(cmbTxTone_KeyDown);
			this.cmbTxTone.Validating += new System.ComponentModel.CancelEventHandler(cmbTxTone_Validating);
			this.lblTxTone.Location = new System.Drawing.Point(36, 54);
			this.lblTxTone.Name = "lblTxTone";
			this.lblTxTone.Size = new System.Drawing.Size(181, 20);
			this.lblTxTone.TabIndex = 25;
			this.lblTxTone.Text = "Tx CTCSS/DCS [Hz]";
			this.lblTxTone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkOpenGD77ScanAllSkip.AccessibleDescription = "Scan. All skip";
			this.chkOpenGD77ScanAllSkip.AccessibleName = "Scan. All skip";
			this.chkOpenGD77ScanAllSkip.Location = new System.Drawing.Point(785, 25);
			this.chkOpenGD77ScanAllSkip.Name = "chkOpenGD77ScanAllSkip";
			this.chkOpenGD77ScanAllSkip.Size = new System.Drawing.Size(295, 20);
			this.chkOpenGD77ScanAllSkip.TabIndex = 16;
			this.chkOpenGD77ScanAllSkip.Text = "Scan: All skip";
			this.chkOpenGD77ScanAllSkip.UseVisualStyleBackColor = true;
			this.chkVox.AccessibleDescription = "VOX";
			this.chkVox.AccessibleName = "VOX";
			this.chkVox.Location = new System.Drawing.Point(785, 44);
			this.chkVox.Name = "chkVox";
			this.chkVox.Size = new System.Drawing.Size(295, 20);
			this.chkVox.TabIndex = 17;
			this.chkVox.Text = "Vox";
			this.chkVox.UseVisualStyleBackColor = true;
			this.chkOpenGD77ScanZoneSkip.AccessibleDescription = "Scan. Zone skip";
			this.chkOpenGD77ScanZoneSkip.AccessibleName = "Scan. Zone skip";
			this.chkOpenGD77ScanZoneSkip.Location = new System.Drawing.Point(785, 6);
			this.chkOpenGD77ScanZoneSkip.Name = "chkOpenGD77ScanZoneSkip";
			this.chkOpenGD77ScanZoneSkip.Size = new System.Drawing.Size(295, 20);
			this.chkOpenGD77ScanZoneSkip.TabIndex = 15;
			this.chkOpenGD77ScanZoneSkip.Text = "Scan: Zone skip";
			this.chkOpenGD77ScanZoneSkip.UseVisualStyleBackColor = true;
			this.chkNoBeep.AccessibleDescription = "No beep";
			this.chkNoBeep.AccessibleName = "No beep";
			this.chkNoBeep.Location = new System.Drawing.Point(785, 82);
			this.chkNoBeep.Name = "chkNoBeep";
			this.chkNoBeep.Size = new System.Drawing.Size(236, 20);
			this.chkNoBeep.TabIndex = 19;
			this.chkNoBeep.Text = "No Beep";
			this.chkNoBeep.UseVisualStyleBackColor = true;
			this.chkNoBeep.CheckedChanged += new System.EventHandler(chkNoBeep_CheckedChanged);
			this.chkNoEco.AccessibleDescription = "No economy";
			this.chkNoEco.AccessibleName = "No economy";
			this.chkNoEco.Location = new System.Drawing.Point(785, 101);
			this.chkNoEco.Name = "chkNoEco";
			this.chkNoEco.Size = new System.Drawing.Size(236, 20);
			this.chkNoEco.TabIndex = 20;
			this.chkNoEco.Text = "No Eco";
			this.chkNoEco.UseVisualStyleBackColor = true;
			this.chkNoEco.CheckedChanged += new System.EventHandler(chkNoEco_CheckedChanged);
			this.cmbChMode.AccessibleDescription = "Mode";
			this.cmbChMode.AccessibleName = "Mode";
			this.cmbChMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbChMode.FormattingEnabled = true;
			this.cmbChMode.Location = new System.Drawing.Point(76, 26);
			this.cmbChMode.Name = "cmbChMode";
			this.cmbChMode.Size = new System.Drawing.Size(120, 24);
			this.cmbChMode.TabIndex = 1;
			this.cmbChMode.SelectedIndexChanged += new System.EventHandler(modeChangedHandler);
			this.lblChName.Location = new System.Drawing.Point(21, 56);
			this.lblChName.Name = "lblChName";
			this.lblChName.Size = new System.Drawing.Size(47, 20);
			this.lblChName.TabIndex = 2;
			this.lblChName.Text = "Name";
			this.lblChName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.txtTxFreq.AccessibleDescription = "Tx frequency";
			this.txtTxFreq.AccessibleName = "Tx frequency";
			this.txtTxFreq.Location = new System.Drawing.Point(634, 26);
			this.txtTxFreq.Name = "txtTxFreq";
			this.txtTxFreq.Size = new System.Drawing.Size(120, 23);
			this.txtTxFreq.TabIndex = 12;
			this.txtTxFreq.Validating += new System.ComponentModel.CancelEventHandler(txtTxFreq_Validating);
			this.lblChMode.Location = new System.Drawing.Point(21, 26);
			this.lblChMode.Name = "lblChMode";
			this.lblChMode.Size = new System.Drawing.Size(47, 20);
			this.lblChMode.TabIndex = 0;
			this.lblChMode.Text = "Mode";
			this.lblChMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblTot.Location = new System.Drawing.Point(507, 56);
			this.lblTot.Name = "lblTot";
			this.lblTot.Size = new System.Drawing.Size(119, 20);
			this.lblTot.TabIndex = 13;
			this.lblTot.Text = "TOT [s]";
			this.lblTot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.txtRxFreq.AccessibleDescription = "Rx frequency";
			this.txtRxFreq.AccessibleName = "Rx frequency";
			this.txtRxFreq.BackColor = System.Drawing.SystemColors.Window;
			this.txtRxFreq.Location = new System.Drawing.Point(347, 26);
			this.txtRxFreq.Name = "txtRxFreq";
			this.txtRxFreq.Size = new System.Drawing.Size(120, 23);
			this.txtRxFreq.TabIndex = 5;
			this.txtRxFreq.Validating += new System.ComponentModel.CancelEventHandler(txtRxFreq_Validating);
			this.lblRxFreq.Location = new System.Drawing.Point(228, 26);
			this.lblRxFreq.Name = "lblRxFreq";
			this.lblRxFreq.Size = new System.Drawing.Size(113, 20);
			this.lblRxFreq.TabIndex = 4;
			this.lblRxFreq.Text = "Rx Frequency [MHz]";
			this.lblRxFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblPower.Location = new System.Drawing.Point(220, 86);
			this.lblPower.Name = "lblPower";
			this.lblPower.Size = new System.Drawing.Size(119, 20);
			this.lblPower.TabIndex = 8;
			this.lblPower.Text = "Power Level";
			this.lblPower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblPower.Visible = false;
			this.lblTxFreq.Location = new System.Drawing.Point(507, 26);
			this.lblTxFreq.Name = "lblTxFreq";
			this.lblTxFreq.Size = new System.Drawing.Size(119, 20);
			this.lblTxFreq.TabIndex = 11;
			this.lblTxFreq.Text = "Tx Frequency [MHz]";
			this.lblTxFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.nudTot.AccessibleDescription = "Timeout";
			this.nudTot.AccessibleName = "Timeout";
			this.nudTot.Location = new System.Drawing.Point(634, 56);
			this.nudTot.Name = "nudTot";
			this.nudTot.Size = new System.Drawing.Size(120, 23);
			this.nudTot.TabIndex = 14;
			this.nudTot.Value = new decimal(new int[4] { 50, 0, 0, 0 });
			this.nudTot.ValueChanged += new System.EventHandler(nudTot_ValueChanged);
			base.ClientSize = new System.Drawing.Size(1101, 430);
			base.Controls.Add(this.pnlChannel);
			base.Controls.Add(this.mnsCh);
			this.Font = new System.Drawing.Font("Arial", 10f);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MainMenuStrip = this.mnsCh;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "VfoForm";
			this.Text = "VFO";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(VfoForm_FormClosing);
			base.Load += new System.EventHandler(VfoForm_Load);
			this.mnsCh.ResumeLayout(false);
			this.mnsCh.PerformLayout();
			this.pnlChannel.ResumeLayout(false);
			this.pnlChannel.PerformLayout();
			this.grpDigit.ResumeLayout(false);
			this.grpDigit.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.nudTxColor).EndInit();
			this.grpAnalog.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.nudArtsInterval).EndInit();
			((System.ComponentModel.ISupportInitialize)this.nudTot).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public void SaveData()
		{
			int index = Convert.ToInt32(base.Tag);
			ValidateChildren();
			ChannelOne value = new ChannelOne(index);
			if (txtRadioId.Focused)
			{
				txtRadioId_Leave(txtRadioId, null);
			}
			value.ChMode = cmbChMode.SelectedIndex;
			value.Name = txtName.Text;
			value.RxFreq = txtRxFreq.Text;
			value.LibreDMR_Power = cmbLibreDMR_Power.SelectedIndex;
			value.TxFreq = txtTxFreq.Text;
			value.Tot = nudTot.Value;
			value.Vox = chkVox.Checked;
			value.AutoScan = chkOpenGD77ScanZoneSkip.Checked;
			value.LoneWoker = chkOpenGD77ScanAllSkip.Checked;
			value.OnlyRx = chkRxOnly.Checked;
			value.Bandwidth = cmbChBandwidth.SelectedIndex;
			value.Sql = cmbSql.SelectedIndex;
			value.Ste = cmbSte.SelectedIndex;
			value.NonSte = cmbNonSte.SelectedIndex;
			value.RxTone = cmbRxTone.Text;
			value.UnmuteRule = data[index].UnmuteRule;
			value.RxSignaling = data[index].RxSignaling;
			value.ArtsInterval = data[index].ArtsInterval;
			value.Key = data[index].Key;
			value.TxTone = cmbTxTone.Text;
			value.TaTxTS1 = cmbTS1TaTx.method_3();
			value.TaTxTS2 = cmbTS2TaTx.method_3();
			value.DualCapacity = chkDualCapacity.Checked;
			value.TimingPreference = cmbTimingPreference.SelectedIndex;
			value.RepateSlot = cmbRepeaterSlot.SelectedIndex;
			value.Ars = cmbArs.SelectedIndex;
			value.KeySwitch = cmbKeySwitch.SelectedIndex;
			value.RxGroupList = cmbRxGroup.method_3();
			value.TxColor = nudTxColor.Value;
			value.APRS_System = cmbAPRS_System.method_3();
			value.Contact = cmbContact.method_3();
			value.AllowTxInterupt = chkAllowTxInterupt.Checked;
			value.TxInteruptFreq = chkTxInteruptFreq.Checked;
			value.OffsetStep = cmbOffsetStep.SelectedIndex;
			data[index] = value;
		}

		public void DispData()
		{
			int index = Convert.ToInt32(base.Tag);
			Text = Node.Text;
			ChannelOne channelOne = data[index];
			if (channelOne.Flag1 == byte.MaxValue && channelOne.Flag2 == byte.MaxValue && channelOne.Flag3 == 254 && channelOne.Flag4 == byte.MaxValue)
			{
				channelOne = data[0];
			}
			if (channelOne.ChMode == 1)
			{
				if (channelOne.RxGroupList != 0)
				{
					channelOne.Contact = 0;
				}
			}
			else
			{
				channelOne.RxGroupList = 0;
				channelOne.Contact = 0;
			}
			method_2();
			cmbChMode.SelectedIndex = channelOne.ChMode;
			txtName.Text = Text;
			txtRxFreq.Text = channelOne.RxFreq;
			cmbLibreDMR_Power.SelectedIndex = channelOne.LibreDMR_Power;
			txtTxFreq.Text = channelOne.TxFreq;
			nudTot.Value = channelOne.Tot;
			chkVox.Checked = channelOne.Vox;
			chkOpenGD77ScanZoneSkip.Checked = channelOne.AutoScan;
			chkOpenGD77ScanAllSkip.Checked = channelOne.LoneWoker;
			chkRxOnly.Checked = channelOne.OnlyRx;
			method_14(channelOne.RxTone);
			cmbChBandwidth.SelectedIndex = channelOne.Bandwidth;
			cmbSql.SelectedIndex = channelOne.Sql;
			cmbSte.SelectedIndex = channelOne.Ste;
			cmbNonSte.SelectedIndex = channelOne.NonSte;
			cmbRxTone.Text = channelOne.RxTone;
			txtRadioId.Text = channelOne.LibreDMR_DMRidChannel;
			chkNoBeep.Checked = channelOne.LibreDMR_NoBeepChannel;
			chkNoEco.Checked = channelOne.LibreDMR_NoEcoChannel;
			chkDMRForceDMO.Checked = channelOne.LibreDMR_DMRForceDMO;
			cmbTxTone.Text = channelOne.TxTone;
			cmbTS1TaTx.method_2(channelOne.TaTxTS1);
			cmbTS2TaTx.method_2(channelOne.TaTxTS2);
			chkDualCapacity.Checked = channelOne.DualCapacity;
			cmbTimingPreference.SelectedIndex = channelOne.TimingPreference;
			cmbRepeaterSlot.SelectedIndex = channelOne.RepateSlot;
			cmbArs.SelectedIndex = channelOne.Ars;
			cmbKeySwitch.SelectedIndex = channelOne.KeySwitch;
			cmbRxGroup.method_2(channelOne.RxGroupList);
			nudTxColor.Value = channelOne.TxColor;
			cmbAPRS_System.method_2(channelOne.APRS_System);
			cmbContact.method_2(channelOne.Contact);
			chkAllowTxInterupt.Checked = channelOne.AllowTxInterupt;
			chkTxInteruptFreq.Checked = channelOne.TxInteruptFreq;
			cmbOffsetStep.SelectedIndex = channelOne.OffsetStep;
			method_17();
		}

		public void RefreshName()
		{
			int index = Convert.ToInt32(base.Tag);
			txtName.Text = data[index].Name;
		}

		public VfoForm()
		{
			InitializeComponent();
			base.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
			CurCntCh = 1024;
		}

		private void method_1()
		{
			Settings.smethod_37(cmbChMode, SZ_CH_MODE);
			txtName.MaxByteLength = 15;
			txtName.KeyPress += Settings.smethod_54;
			txtRxFreq.MaxLength = 9;
			txtRxFreq.KeyPress += Settings.smethod_55;
			txtTxFreq.MaxLength = 9;
			txtTxFreq.KeyPress += Settings.smethod_55;
			Settings.smethod_37(cmbLibreDMR_Power, SZ_LIBREDMR_POWER);
			Settings.smethod_36(nudTot, new Class13(0, 33, 1, 15m, 3));
			nudTot.method_4(0m);
			nudTot.method_6("∞");
			Settings.smethod_37(cmbChBandwidth, SZ_BANDWIDTH);
			Settings.smethod_37(cmbSql, SZ_SQUELCH_LEVEL);
			Settings.smethod_37(cmbVoiceEmphasis, SZ_VOICE_EMPHASIS);
			Settings.smethod_37(cmbSte, SZ_STE);
			Settings.smethod_37(cmbNonSte, SZ_NON_STE);
			Settings.smethod_39(cmbTS1TaTx, ChannelForm.SZ_TA_TX);
			Settings.smethod_39(cmbTS2TaTx, ChannelForm.SZ_TA_TX);
			Settings.smethod_37(cmbTimingPreference, SZ_TIMING_PREFERENCE);
			Settings.smethod_37(cmbRepeaterSlot, SZ_REPEATER_SOLT);
			Settings.smethod_37(cmbArs, SZ_ARS);
			Settings.smethod_37(cmbKeySwitch, SZ_KEY_SWITCH);
			Settings.smethod_36(nudTxColor, new Class13(0, 15, 1, 1m, 2));
			Settings.smethod_37(cmbOffsetStep, SZ_OFFSET_STEP);
			txtRadioId.MaxLength = 8;
			txtRadioId.InputString = "0123456789\b";
		}

		private void method_2()
		{
			Settings.smethod_44(cmbRxGroup, RxGroupListForm.data);
			Settings.smethod_44(cmbAPRS_System, APRSForm.data);
			Settings.smethod_44(cmbContact, ContactForm.data, Settings.SZ_NA);
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(VfoForm).Name;
			Settings.smethod_78("ChMode", SZ_CH_MODE, name);
			Settings.smethod_78("RefFreq", SZ_REF_FREQ, name);
			Settings.smethod_78("RefFreq", SZ_REF_FREQ, name);
			Settings.smethod_78("LibreDMR_Power", SZ_LIBREDMR_POWER, name);
			Settings.smethod_78("Power", SZ_POWER, name);
			Settings.smethod_78("AdmitCriterica", SZ_ADMIT_CRITERICA, name);
			Settings.smethod_78("AdmitCritericaD", SZ_ADMIT_CRITERICA_D, name);
			Settings.smethod_78("Squelch", SZ_SQUELCH, name);
			Settings.smethod_78("SquelchLevel", SZ_SQUELCH_LEVEL, name);
			Settings.smethod_78("VoiceEmphasis", SZ_VOICE_EMPHASIS, name);
			Settings.smethod_78("Ste", SZ_STE, name);
			Settings.smethod_78("NonSte", SZ_NON_STE, name);
			Settings.smethod_78("SignalingSystem", SZ_SIGNALING_SYSTEM, name);
			Settings.smethod_78("UnmuteRule", SZ_UNMUTE_RULE, name);
			Settings.smethod_78("PttidType", SZ_PTTID_TYPE, name);
			Settings.smethod_78("TaTx", ChannelForm.SZ_TA_TX, name);
			Settings.smethod_78("TimingPreference", SZ_TIMING_PREFERENCE, name);
			Settings.smethod_78("Ars", SZ_ARS, name);
			Settings.smethod_78("KeySwitch", SZ_KEY_SWITCH, name);
			Settings.smethod_78("OffsetDirection", SZ_OFFSET_DIRECTION, name);
		}

		private void VfoForm_Load(object sender, EventArgs e)
		{
			try
			{
				Settings.smethod_59(base.Controls);
				Settings.UpdateComponentTextsFromLanguageXmlData(this);
				data.ChModeChangeEvent += method_3;
				populateRxAndTxToneComboboxesFromToneTxtFile();
				method_1();
				int index = Convert.ToInt32(base.Tag);
				txtRadioId.Text = data[index].LibreDMR_DMRidChannel;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void VfoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveData();
		}

		private void modeChangedHandler(object sender, EventArgs e)
		{
			int num = 0;
			switch (cmbChMode.SelectedIndex)
			{
			case 0:
				num = 2;
				grpAnalog.Enabled = true;
				grpDigit.Enabled = false;
				chkDualCapacity.Checked = false;
				break;
			case 1:
				num = 6;
				grpAnalog.Enabled = false;
				grpDigit.Enabled = true;
				break;
			case 2:
				num = 2;
				grpAnalog.Enabled = false;
				grpDigit.Enabled = false;
				break;
			case 3:
				num = 2;
				grpAnalog.Enabled = false;
				grpDigit.Enabled = false;
				break;
			}
			Node.SelectedImageIndex = num;
			Node.ImageIndex = num;
		}

		private void method_3(object sender, ChModeChangeEventArgs e)
		{
			if (base.MdiParent is MainForm mainForm)
			{
				TreeNode treeNodeByTypeAndIndex = mainForm.GetTreeNodeByTypeAndIndex(typeof(ChannelForm), e.ChIndex, Node.Parent.Nodes);
				if (e.ChMode == 0)
				{
					treeNodeByTypeAndIndex.ImageIndex = 2;
					treeNodeByTypeAndIndex.SelectedImageIndex = 2;
				}
				else if (e.ChMode == 1)
				{
					treeNodeByTypeAndIndex.ImageIndex = 6;
					treeNodeByTypeAndIndex.SelectedImageIndex = 6;
				}
			}
		}

		private void txtRxFreq_Validating(object sender, CancelEventArgs e)
		{
			double num = 0.0;
			string text = txtRxFreq.Text;
			_ = txtTxFreq.Text;
			try
			{
				uint uint_ = 0u;
				num = double.Parse(text);
				if (Settings.checkFrequecyIsInValidRange(num, ref uint_) >= 0)
				{
					num = Settings.convert10HzStepFreqToDecimalText(Settings.convertDecimalFreqTo10HzStepValue(num, 100000.0), 100000);
					txtRxFreq.Text = $"{num:f5}";
				}
				else
				{
					txtRxFreq.Text = $"{uint_:f5}";
				}
				num = double.Parse(txtRxFreq.Text);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				txtRxFreq.Text = $"{Settings.MIN_FREQ[0]:f5}";
			}
		}

		private void txtTxFreq_Validating(object sender, CancelEventArgs e)
		{
			double num = 0.0;
			string text = txtTxFreq.Text;
			_ = txtRxFreq.Text;
			try
			{
				uint uint_ = 0u;
				num = double.Parse(text);
				if (Settings.checkFrequecyIsInValidRange(num, ref uint_) >= 0)
				{
					num = Settings.convert10HzStepFreqToDecimalText(Settings.convertDecimalFreqTo10HzStepValue(num, 100000.0), 100000);
					txtTxFreq.Text = $"{num:f5}";
				}
				else
				{
					txtTxFreq.Text = $"{uint_:f5}";
				}
				num = double.Parse(txtTxFreq.Text);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				txtTxFreq.Text = $"{Settings.MIN_FREQ[0]:f5}";
			}
		}

		private void chkRxOnly_CheckedChanged(object sender, EventArgs e)
		{
			method_4();
		}

		private void method_4()
		{
			bool enabled = !chkRxOnly.Checked;
			txtTxFreq.Enabled = enabled;
			nudTot.Enabled = enabled;
			chkVox.Enabled = enabled;
		}

		private void nudTot_ValueChanged(object sender, EventArgs e)
		{
		}

		private void method_6()
		{
			if (base.MdiParent is MainForm mainForm)
			{
				mainForm.RefreshRelatedForm(typeof(ChannelForm));
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void chkDualCapacity_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void method_7()
		{
			cmbTimingPreference.Enabled = chkDualCapacity.Checked;
		}

		private void cmbRxGrp_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbRxGroup.SelectedIndex != 0)
			{
				cmbContact.SelectedIndex = 0;
			}
		}

		private void cmbContact_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbContact.SelectedIndex != 0)
			{
				cmbRxGroup.SelectedIndex = 0;
			}
		}

		private void cmbKeySwitch_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void cmbAPRS_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmbTxTone.Enabled = cmbAPRS_System.SelectedIndex == 0 || (cmbAPRS_System.SelectedIndex <= APRSForm.data.Count && APRSForm.data.APRS_list[cmbAPRS_System.SelectedIndex - 1].Frequency != "");
		}

		private void populateRxAndTxToneComboboxesFromToneTxtFile()
		{
			NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
			cmbRxTone.Items.Clear();
			cmbTxTone.Items.Clear();
			cmbRxTone.Items.Add(Settings.SZ_NONE);
			cmbTxTone.Items.Add(Settings.SZ_NONE);
			for (int i = 0; i < Settings.TONES_LIST.Length; i++)
			{
				string item = Settings.TONES_LIST[i].Replace(".", numberFormat.NumberDecimalSeparator);
				cmbRxTone.Items.Add(item);
				cmbTxTone.Items.Add(item);
			}
		}

		private void cmbRxTone_SelectedIndexChanged(object sender, EventArgs e)
		{
			method_17();
		}

		private void cmbRxTone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				SendKeys.Send("{tab}");
			}
		}

		private void cmbRxTone_Validating(object sender, CancelEventArgs e)
		{
			_ = string.Empty;
			string text = cmbRxTone.Text;
			try
			{
				if (!(text == Settings.SZ_NONE) && !string.IsNullOrEmpty(text))
				{
					if (new Regex("D[0-7]{3}N$").IsMatch(text))
					{
						if (Convert.ToUInt16(text.Substring(1, 3), 8) < 777)
						{
							return;
						}
						cmbRxTone.Text = Settings.SZ_NONE;
					}
					if (new Regex("D[0-7]{3}I$").IsMatch(text))
					{
						if (Convert.ToUInt16(text.Substring(1, 3), 8) < 777)
						{
							return;
						}
						cmbRxTone.Text = Settings.SZ_NONE;
					}
					double num = double.Parse(text);
					if (num >= 60.0 && num < 260.0)
					{
						cmbRxTone.Text = num.ToString("0.0");
					}
					else
					{
						cmbRxTone.Text = Settings.SZ_NONE;
					}
				}
				else
				{
					e.Cancel = false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				cmbRxTone.Text = Settings.SZ_NONE;
			}
			finally
			{
				method_17();
			}
		}

		private void SwsqRwFuko(object sender, EventArgs e)
		{
		}

		private void cmbTxTone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				SendKeys.Send("{tab}");
			}
		}

		private void cmbTxTone_Validating(object sender, CancelEventArgs e)
		{
			_ = string.Empty;
			string text = cmbTxTone.Text;
			try
			{
				if (text == Settings.SZ_NONE)
				{
					return;
				}
				if (new Regex("D[0-7]{3}N$").IsMatch(text))
				{
					if (Convert.ToUInt16(text.Substring(1, 3), 8) < 777)
					{
						return;
					}
					cmbTxTone.Text = Settings.SZ_NONE;
				}
				if (new Regex("D[0-7]{3}I$").IsMatch(text))
				{
					if (Convert.ToUInt16(text.Substring(1, 3), 8) < 777)
					{
						return;
					}
					cmbTxTone.Text = Settings.SZ_NONE;
				}
				double num = double.Parse(text);
				if (num > 60.0 && num < 260.0)
				{
					cmbTxTone.Text = num.ToString("0.0");
				}
				else
				{
					cmbTxTone.Text = Settings.SZ_NONE;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				cmbTxTone.Text = Settings.SZ_NONE;
			}
		}

		private void method_14(string string_0)
		{
			if (new Regex("D[0-7]{3}[N|I]$").IsMatch(string_0))
			{
				Settings.smethod_37(cmbSte, new string[1] { SZ_STE[0] });
			}
			else
			{
				Settings.smethod_37(cmbSte, SZ_STE);
			}
		}

		private void method_17()
		{
			double result = 0.0;
			string text = cmbRxTone.Text;
			Regex regex = new Regex("D[0-7]{3}[N|I]$");
			if (text == Settings.SZ_NONE)
			{
				cmbSte.Enabled = false;
				cmbNonSte.Enabled = true;
				return;
			}
			cmbSte.Enabled = true;
			cmbNonSte.Enabled = false;
			if (regex.IsMatch(text))
			{
				Settings.smethod_37(cmbSte, new string[1] { SZ_STE[0] });
				cmbSte.SelectedIndex = 0;
			}
			else if (double.TryParse(text, out result))
			{
				string text2 = cmbSte.Text;
				Settings.smethod_37(cmbSte, SZ_STE);
				if (cmbSte.FindString(text2) < 0)
				{
					cmbSte.SelectedIndex = 0;
				}
				else
				{
					cmbSte.SelectedItem = text2;
				}
			}
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			txtTxFreq.Text = txtRxFreq.Text;
		}

		static VfoForm()
		{
			SCL_OFFSET_FREQ = 0.01m;
			SPACE_CH = Marshal.SizeOf(typeof(ChannelOne));
			SZ_CH_MODE = new string[4] { "Analog", "Digital", "FM Broadcast", "AM" };
			SZ_LIBREDMR_POWER = new string[11]
			{
				"Master", "50mW | 50mW | 100mW ", "250mW | 250mW |250mW", "500mW |500mW | 500mW", "750mW | 750mW |750mW", "1W | 1W | 1W", "2W | 2W | 2W", "3W | 3W | 10W", "4W | 5W | 25W", "5W | 10W | 40W",
				"+W-"
			};
			SZ_REF_FREQ = new string[3] { "Low", "Middle", "High" };
			SZ_POWER = new string[2] { "Low", "High" };
			SZ_ADMIT_CRITERICA = new string[3] { "Always", "Channel Free", "CTCSS/DCS" };
			SZ_ADMIT_CRITERICA_D = new string[3] { "Always", "Channel Free", "Color Code" };
			SZ_BANDWIDTH = new string[2] { "12.5", "25" };
			SZ_SQUELCH = new string[2] { "Tight", "Normal" };
			SZ_SQUELCH_LEVEL = new string[22]
			{
				"Master", "Open", "5%", "10%", "15%", "20%", "25%", "30%", "35%", "40%",
				"45%", "50%", "55%", "60%", "65%", "70%", "75%", "80%", "85%", "90%",
				"95%", "Closed"
			};
			SZ_VOICE_EMPHASIS = new string[4] { "None", "De & Pre", "De Only", "Pre Only" };
			SZ_STE = new string[4] { "Frequency", "120°", "180°", "240°" };
			SZ_NON_STE = new string[2] { "Off", "Frequency" };
			SZ_SIGNALING_SYSTEM = new string[2] { "Off", "DTMF" };
			SZ_UNMUTE_RULE = new string[3] { "Std Unmute, Mute", "And Unmute, Mute", "And Unmute, Or Mute" };
			SZ_PTTID_TYPE = new string[4] { "None", "Only Front", "Only Post", "Front & Post" };
			SZ_TIMING_PREFERENCE = new string[3] { "Preferred", "Eligibel", "Ineligibel" };
			SZ_REPEATER_SOLT = new string[2] { "1", "2" };
			SZ_ARS = new string[2] { "Disable", "On System Change" };
			SZ_KEY_SWITCH = new string[2] { "Off", "On" };
			SZ_OFFSET_DIRECTION = new string[3] { "None", "+", "-" };
			SZ_OFFSET_STEP = new string[8] { "2.5", "5", "6.25", "10", "12.5", "25", "30", "50" };
			SCL_OFFSET_FREQ = 0.01m;
			data = new Vfo();
		}
	}
}
