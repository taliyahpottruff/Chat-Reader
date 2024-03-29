﻿using System.ComponentModel;
using System.Windows;

namespace Chat_Reader {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		Bot bot;

		public MainWindow(string username, string oauth) {
			bot = new Bot(username, oauth, this);

			InitializeComponent();

			Title += $" {App.GetVersionString()}";
		}

		protected override void OnClosing(CancelEventArgs e) {
			base.OnClosing(e);
			bot.HandleClosing();
		}

		private void LogOutButton_Click(object sender, RoutedEventArgs e) {
			YesNoWindow prompt = new YesNoWindow("Are you sure you want to log out?", this);
			prompt.ShowDialog();
			bool result = prompt.GetResult();

			if (result) {
				Auth.DeleteAuth();
				bot.Disconnect();
			}
		}

		private void AboutButton_Click(object sender, RoutedEventArgs e) {
			//TODO: Add a proper About Page
			MessageBox.Show("Chat Reader is in " + App.GetVersionString());
		}

		private void ExitMenuButton_Click(object sender, RoutedEventArgs e) {
			System.Environment.Exit(0);
		}
	}
}
