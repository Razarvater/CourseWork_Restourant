using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindowControllers.Themes
{
    /// <summary>
    /// Логика взаимодействия для PopupImage.xaml
    /// </summary>
    public partial class PopupImage : UserControl
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(PopupImage), new UIPropertyMetadata(null));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set 
            {
                SetValue(SourceProperty, value); 
            }
        }

        public PopupImage()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mainPopup.IsOpen = true;
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mainPopup.IsOpen = false;
        }
    }
}
