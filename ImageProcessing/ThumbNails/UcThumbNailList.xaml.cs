using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Views.ThumbNails
{
    /// <summary>
    /// UcThumbNailList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcThumbNailList : UserControl
    {
        public UcThumbNailList()
        {
            InitializeComponent();
        }

        public delegate void ThumbNailClick(object sender, EventArgs e);

        public event ThumbNailClick ThumbNailClik;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox == null || listbox.SelectedItem == null)
            {
                return;
            }
            this.ThumbNailClik?.Invoke(listbox.SelectedItem, new EventArgs());

        }

        public void InsertThumbNail(Models.ViewModels.ThumbNails.ThumbNailViewModel thumbnail)
        {
            UcThumbNail ucThumbNail = new UcThumbNail();
            ucThumbNail.DataContext = thumbnail;

            this.ListBox.Items.Add(thumbnail);
        }
    }
}
