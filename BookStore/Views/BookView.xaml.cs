using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BookStore.Views
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    internal partial class BookView : Window
    {
        public BookView()
        {
            InitializeComponent();
        }
        private void dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

        }
        public static string GetPropertyDisplayName(object descriptor)
        {

            PropertyDescriptor pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                DisplayNameAttribute dn = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (dn != null && dn != DisplayNameAttribute.Default)
                {
                    return dn.DisplayName;
                }
            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayNameAttribute dn = attributes[i] as DisplayNameAttribute;
                        if (dn != null && dn != DisplayNameAttribute.Default)
                        {
                            return dn.DisplayName;
                        }
                    }
                }
            }
            return null;
        }
    }
}
