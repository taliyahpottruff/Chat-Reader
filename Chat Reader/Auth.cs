using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace Chat_Reader {
	class Auth : TwitchLib.Client.Models.ConnectionCredentials {
		static string directory = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Developer Labs\\Chat Reader";

		public Auth(string username, string key) : base(username, key) {
			
		}

		public static bool AuthExist() {
			return File.Exists(directory + "\\AUTH.dld");
		}

		public static void SaveAuth(string username, string key) {
			try {
				Directory.CreateDirectory(directory);
				string path = directory + "\\AUTH.dld";
				FileStream file = new FileStream(path, FileMode.OpenOrCreate);
				AuthObject data = new AuthObject() {
					username = username,
					key = key
				};
				BinaryFormatter writer = new BinaryFormatter();
				writer.Serialize(file, data);
				file.Close();
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		public static AuthObject LoadAuth() {
			try {
				
				Directory.CreateDirectory(directory);
				string path = directory + "\\AUTH.dld";
				FileStream file = new FileStream(path, FileMode.Open);
				BinaryFormatter reader = new BinaryFormatter();
				AuthObject obj = (AuthObject) reader.Deserialize(file);
				file.Close();
				return obj;
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
				return new AuthObject();
			}
		}

		public static void DeleteAuth() {
			string path = directory + "\\AUTH.dld";
			if (File.Exists(path)) {
				File.Delete(path);
			}
		}
	}

	[Serializable]
	struct AuthObject {
		public string username;
		public string key;
	}
}
