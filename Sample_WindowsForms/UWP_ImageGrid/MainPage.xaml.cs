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

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_ImageGrid
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<BitmapImage> imageList = new List<BitmapImage>();
        List<String> annotationList = new List<string>();

        string message = "";

        public MainPage()
        {
            this.InitializeComponent();

            //Gallery.ItemsSource = imageList;
            ShowNext.Content = "Show next image";
        }

        async private void add_screenshot_Click(object sender, RoutedEventArgs e)
        {
            // Get the bitmap and display it.
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Bitmap))
            {
                IRandomAccessStreamReference imageReceived = null;
                try
                {
                    imageReceived = await dataPackageView.GetBitmapAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //MainPage.NotifyUser("Error retrieving image from Clipboard: " + ex.Message, /*NotifyType*/.ErrorMessage);
                }

                if (imageReceived != null)
                {
                    using (var imageStream = await imageReceived.OpenReadAsync())
                    {
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(imageStream);
                        //Screenshot.Source = bitmapImage;
                        //Screenshot.Visibility = Visibility.Visible;
                        imageList.Add(bitmapImage);
                        annotationList.Add("");
                        message = "Image is retrieved from the clipboard and pasted successfully.";
                    }
                }
            }
            else
            {
                message = "Bitmap format is not available in clipboard";
                //Screenshot.Visibility = Visibility.Collapsed;
            }

            message_block.Text = message;

        }

        int position = -1;
        private void ShowNext_Click(object sender, RoutedEventArgs e)
        {
            position++;
            ShowNext.Content = String.Format("showing position {0}", position);
            try
            {
                if (imageList.Count == 0)
                {
                    message = "there are no images to show";
                    position = -1;
                }
                else if(position < imageList.Count)
                {
                    update_image(position);
                    message = String.Format("this is a string{0}", position);
                }
                else if (position >= imageList.Count)
                {
                    position = 0;
                    message = String.Format("end of the list, resetting {0}", position);
                    // we can safely do this, because we know it will never be called unless position >= imageList
                    // if position=0 and imageList.Count=0?

                    update_image(position);
                }
                message_block.Text = message;
            }
            catch
            {
                message_block.Text = "there was an error";
            }

            

        }

        private void update_image(int position)
        {
            ViewBoxImage.Source = imageList[position];
            annotation_block.Text = annotationList[position];
        }

        private void Control1_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }

        private void annotation_block_TextChanged(object sender, TextChangedEventArgs e)
        {
            annotationList[position] = annotation_block.Text;
        }
    }
}
