using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace Attempt1
{
    public partial class MainWindow : Window
    {
        private KeyboardHookListener _keyboardListener;
        private MouseHookListener _mouseListener;
        private const string ResourcesPauseIco = "resources/pause.ico";

        public MainWindow()
        {
            InitializeComponent();

            Icon = new BitmapImage(new Uri(ResourcesPauseIco, UriKind.Relative));

            var timer = new Timer();
            const int intervalSeconds = 5;
            timer.Interval = intervalSeconds * 5;
            timer.Tick += TimerOnTick;

            SetupNotifyIcon();
            SetupListeners();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void SetupListeners()
        {
            _keyboardListener = new KeyboardHookListener(new GlobalHooker());
            _keyboardListener.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.S)
                    args.Handled = true;
            };

            _mouseListener = new MouseHookListener(new GlobalHooker());
            _mouseListener.MouseDownExt += (sender, args) => args.Handled = true;

        }

        private void EnableListeners()
        {
            _keyboardListener.Enabled = true;
            _mouseListener.Enabled = true;
        }

        private void SetupNotifyIcon()
        {
            var icon = new NotifyIcon
            {
                Icon = new Icon(ResourcesPauseIco),
                Visible = true
            };
            icon.DoubleClick += (sender, args) =>
            {
                if (this.WindowState == WindowState.Minimized)
                    this.WindowState = WindowState.Normal;

                this.Activate();
            };
            //_icon.MouseClick += (sender, args) => { _icon.ContextMenu.Show(, Control.MousePosition); };
            icon.ContextMenu = GenerateContextMenu();
        }

        private ContextMenu GenerateContextMenu()
        {
            var menu = new ContextMenu();
            menu.MenuItems.Add("&Open");
            return menu;
        }
    }

}
