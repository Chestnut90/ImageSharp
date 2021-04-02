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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageProcessing.ThumbNails
{
    /// <summary>
    /// UcThumbNailList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcThumbNailList : UserControl
    {
        public UcThumbNailList()
        {
            InitializeComponent();
            //this.DataContext = this;

            ItemsPanelTemplate panelTemplate = new ItemsPanelTemplate();

            this.ListBox.ItemsSource = this.ThumbNails;

        }

        public void InsertItem(ViewModels.ThumbNails.ThumbNailViewModel thumbnail)
        {
            this.ThumbNails.Add(thumbnail);
        }

        public ViewModels.ThumbNails.ThumbNailsViewModel ThumbNails
        { get; private set; } = new ViewModels.ThumbNails.ThumbNailsViewModel();


        public delegate void ThumbNailList_ItemClick(object sender, EventArgs e);
        public event ThumbNailList_ItemClick ThumbNailClickEvent;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ListBox)sender).SelectedItem;
            if (selectedItem is null)
            {
                return;
            }

            // changed.
            this.ThumbNailClickEvent?.Invoke(selectedItem, new EventArgs());
        }
    }
}
