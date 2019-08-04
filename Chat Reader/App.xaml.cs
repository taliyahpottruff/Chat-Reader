using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Chat_Reader {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public static uint MAJOR_VERSION = 0;
		public static uint MINOR_VERSION = 2;
		public static uint PATCH_NUMBER = 0;
		public static string SUFFIX = "b";

		public static string GetVersionString() {
			return $"v{MAJOR_VERSION}.{MINOR_VERSION}.{PATCH_NUMBER}{SUFFIX}";
		}
	}
}
