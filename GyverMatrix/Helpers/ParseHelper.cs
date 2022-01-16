using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GyverMatrix.Helpers {
    internal static class ParseHelper {
        public static async Task SetSettings(string message) {
            string[] parsedArray = message.Split(' ', ';')[1].Split('|');
            foreach (var t in parsedArray) {
                string[] parsedArray3 = t.Split(':');
                
                 await SecureStorage.SetAsync(parsedArray3[0], parsedArray3[1]);
                Console.WriteLine(parsedArray3[0] + " " + parsedArray3[1]);

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

        public static async Task SetSettingsNet(string message)
        {

            message = message.Remove(0, 4);
            message = message.Remove(message.Length-1, 1);

            string[] parsedArray = message.Split('|');

            //string[] parsedArray2 = new string[2];
            //1

            string[] parsedArray2 = parsedArray[0].Split(':');
            await SecureStorage.SetAsync(parsedArray2[0], parsedArray2[1]);
            Console.WriteLine(parsedArray2[0] + " " + parsedArray2[1]);
            //parsedArray2.М

            //2-5
            for (int i = 1; i < 5; i++)
            {
                string[] parsedArray3 = parsedArray[i].Split(':');
                //await SecureStorage.SetAsync(parsedArray3[0], parsedArray3[1].Remove(0, 1).Remove(parsedArray3[1].Length - 1, 1));
                //Console.WriteLine(parsedArray3[0] + " " + parsedArray3[1].Remove(0, 1).Remove(parsedArray3[1].Length - 1, 1));
                //string a = parsedArray3[1].
                await SecureStorage.SetAsync(parsedArray3[0], parsedArray3[1].Split('[',']')[1]);
                Console.WriteLine(parsedArray3[0] + " " + parsedArray3[1].Split('[', ']')[1]);
            }


            //6

            string[] parsedArray4 = parsedArray[5].Split(':');
            await SecureStorage.SetAsync(parsedArray4[0], parsedArray4[1].Remove(parsedArray4[1].Length - 1, 1));
            Console.WriteLine(parsedArray4[0] + " " + parsedArray4[1].Remove(parsedArray4[1].Length-1,1));
        }

        public static async Task<string> Effects(string text)
        {

           
            Console.WriteLine(text);
            if (text.Remove(3, text.Length - 3) != "ack")
            {
                string[] message = text.Split(' ', ';');
                Console.WriteLine(message[1]);
                return message[1];
            }
            else
            {
                Console.WriteLine(text.Remove(3, text.Length - 3));
                return "ack";
            }
        }

        public static async Task<string> Text(string text)
        {


            Console.WriteLine(text);
            string message = text.Remove(0, 4);
            message = message.Remove(message.Length-2, message.Length - (message.Length - 2));
            Console.WriteLine(message);
            return message;
        }
    }
}