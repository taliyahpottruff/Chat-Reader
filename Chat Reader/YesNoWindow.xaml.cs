using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat_Reader {
	/// <summary>
	/// Interaction logic for YesNoWindow.xaml
	/// </summary>
	public partial class YesNoWindow : Window {
		bool result = false;

		public YesNoWindow(string text, Window parent) {
			InitializeComponent();

			this.Owner = parent;

			PromptText.Text = text;
		}

		public bool GetResult() {
			return result;
		}

		private void YesButton_Click(object sender, RoutedEventArgs e) {
			result = true;
			this.Close();
		}

		private void NoButton_Click(object sender, RoutedEventArgs e) {
			result = false;
			this.Close();
		}
	}
}
