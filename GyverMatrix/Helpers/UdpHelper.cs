using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GyverMatrix.Helpers {
    internal class UdpHelper {
        private static readonly UdpClient UdpClient = new UdpClient();
        public static void Connect(string ipAdress, int port) =>
            UdpClient.Connect(ipAdress, port);
        public static void CloseConnect() =>
            UdpClient.Close();
        public static void Send(string message) {
            var data = Encoding.UTF8.GetBytes(message);
            UdpClient.Send(data, data.Length);
        }
        public static string Receive() {
            IPEndPoint ip = null;
            var data = UdpClient.Receive(ref ip);
            return Encoding.UTF8.GetString(data);
        }
    }
}