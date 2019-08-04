using System.Windows;

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
