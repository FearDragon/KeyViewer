using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyViewer
{
    /// <summary>
    /// Interaction logic for KeyViewerWindow.xaml
    /// </summary>
    public partial class KeyViewerWindow : Window
    {
        public KeyModelCollection KeyModels { get; set; }

        private SettingsWindow settingsWindow;

        public SettingsWindow Settings => settingsWindow;

        public KeyViewerWindow() {
            InitializeComponent();

            DataContext = this;

            KeyModels = KeyModelCollection.Instance;

            // Left = Config.Instance.WindowX;
            // Top = Config.Instance.WindowY;
            // Left = 0;
            // Top = 0;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.Manual;

            settingsWindow = new SettingsWindow();

            Activated += CreateSettingsWindow;
        }

        private void CreateSettingsWindow(object sender, EventArgs e) {
            Activated -= CreateSettingsWindow;

            settingsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settingsWindow.Show();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);

            if (!settingsWindow.MoveIndividually)
                DragMove();
        }

        protected override void OnLocationChanged(EventArgs e) {
            base.OnLocationChanged(e);

            Config.Instance.WindowX = Left;
            Config.Instance.WindowY = Top;
        }
    }
}
