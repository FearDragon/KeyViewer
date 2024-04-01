﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyViewer
{
    /// <summary>
    /// Interaction logic for KeyView.xaml
    /// </summary>
    public partial class KeyView : UserControl
    {
        #region Dependency Properties

        public static DependencyProperty KeyProperty =
            DependencyPropertyExtension.Register(
                "Key",
                typeof(Key),
                typeof(KeyView),
                Key.None,
                new PropertyChangedCallback((d, e) => (d as KeyView)?.KeyChanged(d, e)));

        public static DependencyProperty FillProperty =
            DependencyPropertyExtension.Register(
                "Fill",
                typeof(Brush),
                typeof(KeyView),
                new BrushConverter().ConvertFrom("#80808080"),
                new PropertyChangedCallback((d, e) => (d as KeyView)?.FillChanged(d, e)));

        public static DependencyProperty OutlineProperty =
            DependencyPropertyExtension.Register(
                "Outline",
                typeof(Brush),
                typeof(KeyView),
                Brushes.White,
                new PropertyChangedCallback((d, e) => (d as KeyView)?.OutlineChanged(d, e)));

        public static DependencyProperty FontForegroundProperty =
            DependencyPropertyExtension.Register(
                "FontForeground",
                typeof(Brush),
                typeof(KeyView),
                Brushes.White,
                new PropertyChangedCallback((d, e) => (d as KeyView)?.FontForegroundChanged(d, e)));

        public static DependencyProperty ViewSizeProperty =
            DependencyPropertyExtension.Register(
                "ViewSize",
                typeof(double),
                typeof(KeyView),
                100.0,
                new PropertyChangedCallback((d, e) => (d as KeyView)?.ViewSizeChanged(d, e)));

        #endregion Dependency Properties

        #region Properties

        public Key Key {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public Brush Fill {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public Brush Outline {
            get { return (Brush)GetValue(OutlineProperty); }
            set { SetValue(OutlineProperty, value); }
        }

        public Brush FontForeground {
            get { return (Brush)GetValue(FontForegroundProperty); }
            set { SetValue(FontForegroundProperty, value); }
        }

        public double ViewSize {
            get { return (double)GetValue(ViewSizeProperty); }
            set { SetValue(ViewSizeProperty, value); }
        }

        #endregion Properties

        public KeyView() {
            InitializeComponent();

            rectangle.Fill = Fill;
            rectangle.Stroke = Outline;
            label.Foreground = FontForeground;
            label.Content = ConvertKeyToString(Key);
        }

        private string ConvertKeyToString(Key k) {
            if (Constants.KEY_TO_STRING.ContainsKey(k)) {
                return Constants.KEY_TO_STRING[k];
            }
            return k.ToString();
        }

        private void KeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (!e.NewValue?.Equals(e.OldValue) ?? false) {
                Key key = (Key)e.NewValue;
                label.Content = ConvertKeyToString(key);
            }
        }

        private void FillChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (!e.NewValue?.Equals(e.OldValue) ?? false) {
                Brush fill = (Brush)e.NewValue;
                rectangle.Fill = fill;
            }
        }

        private void OutlineChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (!e.NewValue?.Equals(e.OldValue) ?? false) {
                Brush outline = (Brush)e.NewValue;
                rectangle.Stroke = outline;
            }
        }

        private void FontForegroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (!e.NewValue?.Equals(e.OldValue) ?? false) {
                Brush foreground = (Brush)e.NewValue;
                label.Foreground = foreground;
            }
        }

        private void ViewSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (!e.NewValue?.Equals(e.OldValue) ?? false) {
                double size = (double)e.NewValue;
                grid.MinWidth = size;
                grid.MinHeight = size;
                rectangle.StrokeThickness = size * 3 / 100;
                label.FontSize = Math.Max(1, size / 2);
                Margin = new Thickness(size / 10, 0, 0, 0);
            }
        }

        private bool _pressed;
        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            _pressed = true;
            Mouse.Capture(this);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            _pressed = false;
            Mouse.Capture(null);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            if (_pressed && ((KeyViewerWindow)Window.GetWindow(this)).Settings.MoveIndividually)
            {
                GridTransform.X = Mouse.GetPosition(this).X;
                GridTransform.Y = Mouse.GetPosition(this).Y;
            }
        }
    }
}
