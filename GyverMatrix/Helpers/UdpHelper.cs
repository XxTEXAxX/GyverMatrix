using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GyverMatrix.Helpers {
    internal static class UdpHelper {
        private static readonly UdpClient UdpClient = new UdpClient();
        public static bool Connect(string ipAdress, int port) {
            try {
                UdpClient.Connect(ipAdress, port);
                return true;
            } catch {
                return false;
            }
        }
        public static void CloseConnect() =>
            UdpClient.Close();
        public static async Task<bool> Send(string message) {
            try {
                var data = Encoding.UTF8.GetBytes(message);
                await UdpClient.SendAsync(data, data.Length);
                return true;
            } catch {
                return false;
            }
        }
        public static async Task<string> Receive() {
            try {
                var data = await UdpClient.ReceiveAsync();
                return Encoding.UTF8.GetString(data.Buffer);
            } catch {
                return "";
            }
        }
    }
}