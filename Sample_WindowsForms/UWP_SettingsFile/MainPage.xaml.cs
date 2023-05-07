using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_SettingsFile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }
        
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        
        List<String> values = new List<string>();


        private void Add_To_List_Click(object sender, RoutedEventArgs e)
        {
            values.Add(Input_Text_Box.Text);

            add_string_to_list_box(Input_Text_Box.Text);

        }

        private void add_string_to_list_box(string message)
        {
            ListBoxItem listBoxItem = new ListBoxItem();
            listBoxItem.Content = message;

            List_Box.Items.Add(listBoxItem);
            Input_Text_Box.Text = "";
        }

        private void ColorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataCompositeValue composite_list = new Windows.Storage.ApplicationDataCompositeValue();
            int itemCount = 0;
            foreach(ListBoxItem item in List_Box.Items)
            {
                composite_list[itemCount.ToString()] = item.Content;
                itemCount++;
            }
            localSettings.Values["item_list"] = composite_list;
            //localSettings.Values["item_count"] = itemCount;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataCompositeValue composite_list = (ApplicationDataCompositeValue)localSettings.Values["item_list"];
            List_Box.Items.Clear();
            if (composite_list != null)
            {
                //int itemCount = 0;
                foreach (var item in composite_list)
                {
                    add_string_to_list_box(item.Value.ToString());
                }
            }
            
        }
        
    }
}
