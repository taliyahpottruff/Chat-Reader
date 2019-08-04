using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat_Reader {
	/// <summary>
	/// Interaction logic for Startup.xaml
	/// </summary>
	public partial class Startup : Window {
		public Startup() {
			//Check to if auth details exist
			if (Auth.AuthExist()) {
				AuthObject obj = Auth.LoadAuth();
				new MainWindow(obj.username, obj.key).Show();
				this.Close();
				return;
			}

			//If not, continue
			InitializeComponent();

			Closed += Startup_Closed;
		}

		private void Startup_Closed(object sender, EventArgs e) {
			Closed -= Startup_Closed;
			System.Environment.Exit(0);
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void BtnTwitchLogin_Click(object sender, RoutedEventArgs e) {
			string username = UsernameTextField.Text;
			string oauth = KeyTextField.Password;

			try {
				new MainWindow(username, oauth).Show();
				Closed -= Startup_Closed;
				this.Close();
			} catch (Exception) {
				//Twitch login was probably bad
				UsernameTextField.Text = "";
				KeyTextField.Clear();
				new ErrorWindow("The Twitch username you submitted is invalid!").ShowDialog();
			}
		}
	}
}
