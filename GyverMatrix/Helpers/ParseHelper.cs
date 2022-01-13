using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GyverMatrix.Helpers {
    internal static class ParseHelper {
        public static async Task SetSettings(string message) {
            string[] parsedArray = message.Split(' ', ';')[1].Split('|');
            foreach (var t in parsedArray) {
                string[] parsedArray3 = t.Split(':');
                try {
                    await SecureStorage.SetAsync(parsedArray3[0] + " - " + parsedArray3[0], parsedArray3[1]);
                    Console.WriteLine(await SecureStorage.GetAsync(parsedArray3[0]));
                } catch (Exception ex) {
                    Console.WriteLine("Storage err - " + ex);
                }
                //Console.WriteLine(parsedArray3[0] + " - " + parsedArray3[1]);
            }
        }
        public static async Task SetEffects(string message) {
            string[] parsedArray = message.Split(':')[1].Split('[', ']');
            await SecureStorage.SetAsync("Effects", parsedArray[1]);
            Console.WriteLine(parsedArray[1]);
        }
        public static async Task SetGames(string message) {
            string[] parsedArray = message.Split(':')[1].Split('[', ']');
            await SecureStorage.SetAsync("Games", parsedArray[1]);
            Console.WriteLine(parsedArray[1]);
        }
    }
}