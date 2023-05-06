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
using Windows.Foundation.Metadata;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;


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
        List<SoftwareBitmap> softwareBitmaps = new List<SoftwareBitmap>();

        string message = "";
        int position = -1;

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

                        SoftwareBitmap softwareBitmap;
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream);
                        softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                        softwareBitmaps.Add(softwareBitmap);



                        //var bitmapImage = new BitmapImage();
                        //bitmapImage.SetSource(imageStream);
                        ////Screenshot.Source = bitmapImage;
                        ////Screenshot.Visibility = Visibility.Visible;
                        //imageList.Add(bitmapImage);
                        //annotationList.Add("");
                        //message = "Image is retrieved from the clipboard and pasted successfully.";

                        //ShowNext_Click(sender, e);
                    }
                    using (var imageStream = await imageReceived.OpenReadAsync())
                    {

                        ////get_software_bitmap(imageStream);

                        //SoftwareBitmap softwareBitmap;
                        //// Create the decoder from the stream
                        //BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream);
                        //softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                        //softwareBitmaps.Add(softwareBitmap);



                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(imageStream);
                        //Screenshot.Source = bitmapImage;
                        //Screenshot.Visibility = Visibility.Visible;
                        imageList.Add(bitmapImage);
                        annotationList.Add("");
                        message = "Image is retrieved from the clipboard and pasted successfully.";

                        ShowNext_Click(sender, e);
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

        async private void get_software_bitmap(IRandomAccessStreamWithContentType randomAccessStream)
        {
            ////IRandomAccessStream randomAccessStream = GetSomeRandomAccessStream();
            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.SetSource(randomAccessStream);
            //Image image = new Image();
            //image.Source = bitmapImage;
            //RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            //await renderTargetBitmap.RenderAsync(image);
            //IBuffer buffer = await renderTargetBitmap.GetPixelsAsync();
            //byte[] pixels = buffer.ToArray();
            //MemoryStream memoryStream = new MemoryStream(pixels);
            //memoryStream.Position = 0;


            SoftwareBitmap softwareBitmap;
            // Create the decoder from the stream
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);

                // Get the SoftwareBitmap representation of the file
            softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            softwareBitmaps.Add(softwareBitmap);
        
        }

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
                else if (position < imageList.Count)
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

        async private void Button_Click(object sender, RoutedEventArgs e)
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

            SaveSoftwareBitmapToFile(softwareBitmaps[position], outputFile);
        }


        private async void SaveBitmapToFile(ImageSource imageSource, BitmapImage bitmap, StorageFile outputFile)
        {

            
            //BitmapDecoder decoder = await BitmapDecoder.CreateAsync(bitmap.);
            //using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            //{
            //    // Create an encoder with the desired format
            //    //BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);

            //    //var _bitmap = new RenderTargetBitmap();
            //    //await _bitmap.RenderAsync(imageSource);
            //    //// Set the software bitmap
            //    //encoder.SetSoftwareBitmap(bitmap);

            //    SoftwareBitmap softwareBitmap = new SoftwareBitmap();
            //    bitmap.UriSource
            //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //    encoder.Frames.Add(BitmapFrame.Create(objImage));

            //    using (FileStream filestream = new FileStream(photolocation, FileMode.Create))
            //    {
            //        encoder.Save(filestream);
            //    }

            //    // Set additional encoding parameters, if needed
            //    encoder.BitmapTransform.ScaledWidth = 320;
            //    encoder.BitmapTransform.ScaledHeight = 240;
            //    encoder.BitmapTransform.Rotation = Windows.Graphics.Imaging.BitmapRotation.Clockwise90Degrees;
            //    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
            //    encoder.IsThumbnailGenerated = true;

            //    try
            //    {
            //        await encoder.FlushAsync();
            //    }
            //    catch (Exception err)
            //    {
            //        const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
            //        switch (err.HResult)
            //        {
            //            case WINCODEC_ERR_UNSUPPORTEDOPERATION:
            //                // If the encoder does not support writing a thumbnail, then try again
            //                // but disable thumbnail generation.
            //                encoder.IsThumbnailGenerated = false;
            //                break;
            //            default:
            //                throw;
            //        }
            //    }

            //    if (encoder.IsThumbnailGenerated == false)
            //    {
            //        await encoder.FlushAsync();
            //    }


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
                encoder.BitmapTransform.ScaledWidth = 320;
                encoder.BitmapTransform.ScaledHeight = 240;
                encoder.BitmapTransform.Rotation = Windows.Graphics.Imaging.BitmapRotation.Clockwise90Degrees;
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
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
    }
}
