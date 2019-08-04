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
	/// Interaction logic for ErrorWindow.xaml
	/// </summary>
	public partial class ErrorWindow : Window {
		Window sender;
		bool closeSenderOnConfirm;

		public ErrorWindow(string errorText) : this(errorText, null, false) { }

		public ErrorWindow(string errorText, Window sender, bool closeSenderOnConfirm) {
			InitializeComponent();

			ErrorText.Text = errorText;
			this.sender = sender;
			this.closeSenderOnConfirm = closeSenderOnConfirm;
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e) {
			if (closeSenderOnConfirm) {
				this.sender.Close();
			}

			this.Close();
		}
	}
}
