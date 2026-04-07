using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComicVerse.Views.UserControls
{
    public partial class NumericUpDown : UserControl
    {
        // Регистрируем свойство зависимости, чтобы оно было доступно в XAML
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public NumericUpDown()
        {
            InitializeComponent();
            // Устанавливаем привязку внутреннего TextBox к свойству Value этого контрола
            ValueHolder.DataContext = this;
            ValueHolder.SetBinding(TextBox.TextProperty, new Binding("Value")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            Value += 10;
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            if (Value >= 10) Value -= 10;
        }
    }
}