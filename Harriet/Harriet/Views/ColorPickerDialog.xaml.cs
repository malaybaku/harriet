using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Harriet.Views
{
    /// <summary>
    /// ColorPickerDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

         public ColorPickerDialog(Color initialSelectedColor) : this()
        {
            colorCanvas.SelectedColor = initialSelectedColor;
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnColorSelected(object sender, RoutedEventArgs e)
        {
            if (ColorSelected != null)
            {
                ColorSelected(this, new ColorEventArgs(colorCanvas.SelectedColor));
            }
            Close();
        }

        public event EventHandler<ColorEventArgs> ColorSelected;
    }

    public class ColorEventArgs : EventArgs
    {
        public ColorEventArgs(Color c)
        {
            Color = c;
        }

        public Color Color { get; private set; }
    }

}
