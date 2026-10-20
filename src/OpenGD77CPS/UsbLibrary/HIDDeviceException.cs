using System;
using System.Runtime.InteropServices;

namespace UsbLibrary
{
	public class HIDDeviceException : ApplicationException
	{
		public HIDDeviceException(string strMessage)
			: base(strMessage)
		{
		}

		public static HIDDeviceException GenerateWithWinError(string strMessage)
		{
			return new HIDDeviceException($"Msg:{strMessage} WinEr:{Marshal.GetLastWin32Error():X8}");
		}

		public static HIDDeviceException GenerateError(string strMessage)
		{
			return new HIDDeviceException($"Msg:{strMessage}");
		}
	}
}
