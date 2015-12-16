using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace anyBar
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            notifyIcon = new NotifyIcon {Icon = Icons.Commands["question"]()};
            notifyIcon.Click += (_, __) => { ShowMenu(); };
            notifyIcon.Visible = true;
        }

        private async Task CommandReceiver(int port)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, port);
            Socket winSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            winSocket.Bind(serverEndPoint);
            while (true)
            {
                var input = await ReceiveAMessage(winSocket);
                if (!string.IsNullOrEmpty(input))
                {
                    var cmd = input.ToLowerInvariant();
                    Dispatcher.Invoke(() =>
                    {
                        if (Icons.Commands.ContainsKey(cmd))
                            notifyIcon.Icon = Icons.Commands[cmd]();
                    });
                }
            }
        }


        private async Task<string> ReceiveAMessage(Socket clientSocket)
        {
            var content = new byte[512];

            var len = await Task.Factory.FromAsync(
                (cb, s) => clientSocket.BeginReceive(content, 0, content.Length, SocketFlags.None, cb, s),
                clientSocket.EndReceive,
                null);
            var str = Encoding.ASCII.GetString(content, 0, len);
            return str;
        }

        private Popup ContextMenu => (Popup) FindResource("TrayPopup");

        private void ShowMenu()
        {
            ContextMenu.IsOpen = true;
        }


        private readonly NotifyIcon notifyIcon;

        private void ExitClicked(object sender, EventArgs eventArgs)
        {
            Close();
        }

        public void Init(int port)
        {
            Task.Factory.StartNew(() => CommandReceiver(port));
        }
    }
}