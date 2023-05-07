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
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_SaveImage_LocalFolder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class itm
    {
        BitmapImage bitmap;
        SoftwareBitmap softwareBitmap;
        String annotation;

        public BitmapImage Bitmap { get => bitmap; set => bitmap = value; }
        public SoftwareBitmap SoftwareBitmap { get => softwareBitmap; set => softwareBitmap = value; }
        public string Annotation { get => annotation; set => annotation = value; }
        
    }
    public class lst
    {
        List<itm> itms = new List<itm>();
        int count = 0;

        public int Count { get => count; set => count = value; }

        public void Add(itm myitem)
        {
            itms.Add(myitem);
            count++;
        }

        public itm GetItm(int position)
        {
            return itms[position];
        }
        public SoftwareBitmap GetSoftwareBitmap(int position)
        {
            return itms[position].SoftwareBitmap;
        }

        public string GetAnnotation(int position)
        {
            return itms[position].Annotation;
        }
        public void UpdateAnnotation(int position, String annotation)
        {
            itms[position].Annotation = annotation;
        }

    }
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        lst mylist = new lst();

        string message = "";
        int position = -1;

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
                    itm myitem = new itm();
                    // we're doing 2 things with the screenshot. We want a SoftwareBitmap, so that we can export it later
                    using (var imageStream = await imageReceived.OpenReadAsync())
                    {
                        SoftwareBitmap softwareBitmap;
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream);
                        softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                        //softwareBitmaps.Add(softwareBitmap);
                        myitem.SoftwareBitmap = softwareBitmap;
                    }
                    // we want a BitmapImage, because we can attach this to the ViewBox
                    using (var imageStream = await imageReceived.OpenReadAsync())
                    {
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(imageStream);
                        myitem.Bitmap = bitmapImage;
                        myitem.Annotation = Input_Text_Box.Text;
                        message = "Image is retrieved from the clipboard and pasted successfully.";

                        ShowNext_Click(sender, e);
                    }
                    mylist.Add(myitem);
                }
            }
            else
            {
                message = "Bitmap format is not available in clipboard";
            }

            message_block.Text = message;

        }
        private void ShowNext_Click(object sender, RoutedEventArgs e)
        {
            position++;
            Next.Content = String.Format("showing position {0}", position);
            try
            {
                if (mylist.Count == 0)
                {
                    message = "there are no images to show";
                    position = -1;
                }
                else if (position < mylist.Count)
                {
                    update_image(position);
                    message = String.Format("this is a string{0}", position);
                }
                else if (position >= mylist.Count)
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
            ViewBoxImage.Source = mylist.GetItm(position).Bitmap;
            annotation_block.Text = mylist.GetItm(position).Annotation;
        }

        

        private async void SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile)
        {
            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);


                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);


                // Set additional encoding parameters, if needed
                //encoder.BitmapTransform.ScaledWidth = 320;
                //encoder.BitmapTransform.ScaledHeight = 240;
                
                //encoder.BitmapTransform.Rotation = Windows.Graphics.Imaging.BitmapRotation.Clockwise90Degrees;
                //encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.IsThumbnailGenerated = true;

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            // If the encoder does not support writing a thumbnail, then try again
                            // but disable thumbnail generation.
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }

                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }
            }
        }

        async private void Save_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach(itm myitem in mylist)
            {

            }
            writeAnnotation(position);
            writeImage(position);

        }
        async void writeAnnotation(int position)
        {
            string fileName = String.Format("{0}_annotation.txt", position);

            StorageFile sampleFile = await localFolder.CreateFileAsync(fileName,CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, mylist.GetAnnotation(position));
        }

        async void writeImage(int position)
        {
            string fileName = String.Format("{0}.jpg", position);
            StorageFile sampleFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await sampleFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                encoder.SetSoftwareBitmap(mylist.GetSoftwareBitmap(position));
                encoder.IsThumbnailGenerated = true;
                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            // If the encoder does not support writing a thumbnail, then try again
                            // but disable thumbnail generation.
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }
                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }
            }
        }

        async private void Save_Image(object sender, RoutedEventArgs e)
        {
            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
            fileSavePicker.SuggestedFileName = "image";

            var outputFile = await fileSavePicker.PickSaveFileAsync();

            if (outputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }

            

            SaveSoftwareBitmapToFile(mylist.GetItm(position).SoftwareBitmap, outputFile);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {

        }

        private void annotation_block_TextChanged(object sender, TextChangedEventArgs e)
        {
            mylist.UpdateAnnotation(position, annotation_block.Text);
        }

        private void ScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }
    }
}
