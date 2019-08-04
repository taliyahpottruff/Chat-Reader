using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Windows;
using TwitchLib.Client;

namespace Chat_Reader {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		TwitchClient client = new TwitchClient();
		SpeechSynthesizer speech = new SpeechSynthesizer();
		Dictionary<string, ChatVoice> voices = new Dictionary<string, ChatVoice>();
		List<string> chats = new List<string>();

		Auth currentAuth;
		string lastUser = "";
		Random random;

		public MainWindow(string username, string oauth) {
			random = new Random();
			speech.Volume = 50;

			Console.WriteLine("Beginning connection...");
			currentAuth = new Auth(username, oauth);
			client.Initialize(currentAuth, username);
			client.OnMessageReceived += Client_OnMessageReceived;
			client.OnConnected += Client_OnConnected;
			client.OnIncorrectLogin += Client_OnIncorrectLogin;
			client.OnDisconnected += Client_OnDisconnected;
			client.Connect();

			InitializeComponent();

			Title += $" {App.GetVersionString()}";
		}

		private void Client_OnDisconnected(object sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e) {
			new Startup().Show();
			this.Close();
		}

		private void Client_OnIncorrectLogin(object sender, TwitchLib.Client.Events.OnIncorrectLoginArgs e) {
			Console.WriteLine("Connection error! Likely bad credentials.");
			Dispatcher.Invoke(new Action(() => {
				new ErrorWindow("There was a problem authenticating your credentials. Check your oauth key!").ShowDialog();
				new Startup().Show();
				this.Close();
			}));
		}

		private void Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e) {
			Console.WriteLine("Chat Reader connected!");

			Auth.SaveAuth(currentAuth.TwitchUsername, currentAuth.TwitchOAuth);
		}

		protected override void OnClosing(CancelEventArgs e) {
			base.OnClosing(e);
			client.OnMessageReceived -= Client_OnMessageReceived;
			client.OnConnected -= Client_OnConnected;
			client.OnIncorrectLogin -= Client_OnIncorrectLogin;
			client.OnDisconnected -= Client_OnDisconnected;
			speech.Dispose();
		}

		private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e) {
			string user = e.ChatMessage.Username;
			string message = e.ChatMessage.Message;

			string[] parts = message.Split(' ');
			List<string> words = new List<string>(parts);
			int unique = words.Distinct().Count();
			int wordCount = words.Count;
			float uniquePercent = (float)unique / (float)wordCount;

			//Loop through and remove spam words
			List<int> removeIndexes = new List<int>();
			for (int i = 0; i < wordCount; i++) {
				if (words[i].Length > 25) {
					removeIndexes.Add(i);
				}
			}
			for (int i = removeIndexes.Count - 1; i >= 0; i--) {
				words.RemoveAt(removeIndexes[i]);
			}

			//Reconstruct message
			string newMessage = "";
			foreach (string word in words) {
				newMessage += word + " ";
			}

			newMessage = Regex.Replace(newMessage, @"[^\w\s0-9\.,`!;]", " ");
			newMessage = Regex.Replace(newMessage, @"http[^\s]*", "link");

			if (!message.StartsWith("!") && (uniquePercent > 0.5f || wordCount <= 3) && words.Count > 0) {
				chats.Add($"{user}: {message}");
				Dispatcher.Invoke(new Action(() => {
					Chat.ItemsSource = null;
					Chat.ItemsSource = chats;
				}));

				if (!voices.ContainsKey(user)) {
					ReadOnlyCollection<InstalledVoice> installed = speech.GetInstalledVoices();
					int randomName = random.Next(0, 2);
					int randomRate = random.Next(0, 4);
					Console.WriteLine($"Name index ({randomName}) Rate value ({randomRate})");
					voices.Add(user, new ChatVoice() {
						name = installed[randomName].VoiceInfo.Name,
						rate = randomRate
					});
				}

				speech.SelectVoice(voices[user].name);
				speech.Rate = voices[user].rate;
				string userToSay = (lastUser == user) ? "" : $"{user.Replace("_", " ")} says: ";
				Prompt p = speech.SpeakAsync($"{userToSay}{newMessage}");
				lastUser = user;
			}
		}

		private void LogOutButton_Click(object sender, RoutedEventArgs e) {
			Auth.DeleteAuth();
			client.Disconnect();
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
