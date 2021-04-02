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

namespace ImageProcessing.Views.InputDialogs
{
    /// <summary>
    /// KmeanDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KmeanDialog : Window
    {
        public KmeanDialog()
        {
            InitializeComponent();
        }

        public int KmeanValue
        {
            get;
            private set;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    int kmeanValue;
                    if (!Int32.TryParse(this.TextKMean.Text, out kmeanValue))
                    {
                        throw new Exception("kmean value must be integer more than 0.");
                    }

                    if (kmeanValue < 1)
                    {
                        throw new Exception("kmean value must be larger than 0.");
                    }
                    this.KmeanValue = kmeanValue;
                }
                catch (Exception ex0)
                {
                    MessageBox.Show(ex0.ToString(), "wrong value" , MessageBoxButton.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
            }
            
            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
