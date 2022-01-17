using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GyverMatrix.Helpers {
    internal static class UdpHelper {
        private static UdpClient UdpClient = new UdpClient();
        public static string Ip;
        public static int Port;
        public static bool Connect(string ipAdress, int port) {
            try {
                UdpClient.Connect(ipAdress, port);
                ConnectHelper.connected = true;
                return true;
            } catch {
                ConnectHelper.connected = false;
                return false;
            }
        }
        public static void CloseConnect() {
            UdpClient.Close();
            ConnectHelper.connected = false;
        }

        public static bool Reconnect() {
            try {
                UdpClient.Dispose();
                UdpClient = new UdpClient();
                return Connect(Ip, Port);
            } catch {
                return false;
            }
        }
        private static string _text;
        private static bool _x;
        public static async Task<bool> Send(string message) {
            try {
                var data = Encoding.UTF8.GetBytes(message);
                await UdpClient.SendAsync(data, data.Length);
                var data2 = await UdpClient.ReceiveAsync();
                _text = Encoding.UTF8.GetString(data2.Buffer);
                _x = true;
            } catch {
                _text = "";
                _x = false;
            }
            return _x;
        }
        public static Task<string> Receive() {
            return Task.FromResult(_text);
        }
    }
}