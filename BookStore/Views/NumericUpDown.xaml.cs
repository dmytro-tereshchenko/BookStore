using System;
using System.Windows;
using System.Windows.Controls;

namespace BookStore.Views
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly RoutedEvent ValueChangedEvent;
        public static DependencyProperty MinValueProperty;
        public static DependencyProperty MaxValueProperty;
        public static DependencyProperty ValueProperty;
        public static DependencyProperty StepProperty;
        public int MinValue
        {
            get => (int)GetValue(MinValueProperty); 
            set => SetValue(MinValueProperty, value); 
        }
        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty); 
            set => SetValue(MaxValueProperty, value); 
        }
        public int Value
        {
            get {
                return int.Parse(GetValue(ValueProperty).ToString());
                }
            set {
                int newValue;
                if (int.TryParse(value.ToString(), out newValue))
                {
                    SetValue(ValueProperty, Math.Max(MinValue, Math.Min(MaxValue, newValue)));
                }
            }
        }
        public int Step
        {
            get => (int)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }
        static NumericUpDown()
        {
            MinValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(NumericUpDown));
            MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDown));
            ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueChanged)));
            StepProperty = DependencyProperty.Register("Step", typeof(int), typeof(NumericUpDown));
            ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumericUpDown));
        }
        public NumericUpDown()
        {
            InitializeComponent();
            MinValue = 0;
            MaxValue = 100;
            Step = 1;
        }
        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)sender;

            RoutedPropertyChangedEventArgs<int> e = new RoutedPropertyChangedEventArgs<int>(
                (int)args.OldValue, (int)args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }
        public event RoutedPropertyChangedEventHandler<int> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<int> args)
        {
            RaiseEvent(args);
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            Value += Step;
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            Value -= Step;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs args)
        {
            int newValue;
            if (int.TryParse(textBox.Text, out newValue))
            {
                RoutedPropertyChangedEventArgs<int> e = new RoutedPropertyChangedEventArgs<int>(
                    Value, newValue, ValueChangedEvent);
                Value = newValue;
                this.OnValueChanged(e);
            }
        }


    }
}
