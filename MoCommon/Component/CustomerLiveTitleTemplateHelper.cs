using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.IO;
using MoCommon.EntityModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using ImageTools.IO.Png;
using ImageTools;
using System.Windows.Documents;
using Microsoft.Phone.Controls;

namespace MoCommon.Component
{
    public class CustomerLiveTitleTemplateHelper
    {
        public event EventHandler UpdateTitleComplated;
             

        private readonly string SaveRootPath = "isostore:/";
        private readonly string SaveVisitPath = "Shared/ShellContent/titles"+@"/";

        //Wide
        private string WideBackBackgroundImagePath ="WideBackBackgroundImage.png";
        private string WideSaveBackgroundImagePath ="WideBackgroundImage.png";
        private readonly double _WideHeight = 336;
        private readonly double _WideWidth = 691;


        //Middle
        private string MiddleBackBackgroundPath ="MiddleBackBackgroundImage.png";
        private string MiddleSaveBackgroundImagePath ="MiddleBackgroundImage.png";
        private readonly double _MiddleHeight = 336;
        private readonly double _MiddleWidth = 336;


        //Small 
        private readonly string SmallBackgroundImagePath = "/Assets/Logo159.png";


        public void UpdateCustomerLiveTitle(CustomerLiveTitleData customerLiveTitleData, string navigationUrl)
        {
            //Transfer To City Id
            FormatSavePath(customerLiveTitleData.CityId);

            //Wide LiveTitle
            CustomerTitleTemplateElement(LiveTitleType.Wide, customerLiveTitleData);
            CustomerTitleTemplateElement(LiveTitleType.WideBack, customerLiveTitleData);

            //Middle LiveTile
            CustomerTitleTemplateElement(LiveTitleType.Middle, customerLiveTitleData);
            CustomerTitleTemplateElement(LiveTitleType.MiddleBack, customerLiveTitleData);

            //Small LiveTitle
            CustomerTitleTemplateElement(LiveTitleType.Small, customerLiveTitleData);

            CustomLiveTitleHelper customTitleHelper = new CustomLiveTitleHelper();
            customTitleHelper.UpdateFilpLiveTitle
                (
                    navigationUrl, "", "", "", "",
                    SmallBackgroundImagePath,
                    SaveRootPath + MiddleSaveBackgroundImagePath,
                    SaveRootPath + WideSaveBackgroundImagePath,
                    SaveRootPath + MiddleBackBackgroundPath,
                    SaveRootPath + WideBackBackgroundImagePath
                );
            if (UpdateTitleComplated != null)
                UpdateTitleComplated("ok", null);
        }             

        private void CustomerTitleTemplateElement(LiveTitleType titleType, CustomerLiveTitleData customerLiveTitleData)
        {
            FrameworkElement customElement = null;
            switch (titleType)
            {
                case LiveTitleType.Wide:
                    customElement = CustomWideTitleElement(customerLiveTitleData);
                    WriteableBitmap writeBitmap = GetWriteBitMapByElement(customElement,_WideHeight,_WideWidth);
                    SaveCaptureImageToLocal(writeBitmap,WideSaveBackgroundImagePath);
                    break;
                case LiveTitleType.Middle:
                    customElement = CustomMiddleTitleElement(customerLiveTitleData);
                    WriteableBitmap bitmapImage = GetWriteBitMapByElement(customElement, _MiddleHeight, _MiddleWidth);
                    SaveCaptureImageToLocal(bitmapImage, MiddleSaveBackgroundImagePath);
                    break;
                case LiveTitleType.WideBack:
                    customElement = CustomBackgroundElement(customerLiveTitleData,_WideHeight,_WideWidth);
                    WriteableBitmap backBitmap = GetWriteBitMapByElement(customElement,_WideHeight,_WideWidth);
                    SaveCaptureImageToLocal(backBitmap,WideBackBackgroundImagePath);
                    break;
                case LiveTitleType.MiddleBack:
                    customElement = CustomBackgroundElement(customerLiveTitleData,_MiddleHeight,_MiddleWidth);
                    WriteableBitmap middleBackMap = GetWriteBitMapByElement(customElement, _MiddleHeight, _MiddleWidth);
                    SaveCaptureImageToLocal(middleBackMap, MiddleBackBackgroundPath);
                    break;
                case LiveTitleType.Small:
                    break;
            }
        }

        private WriteableBitmap GetWriteBitMapByElement(FrameworkElement controlElement,double height,double width)
        {
            var controlSize = new Size(width, height);
            controlElement.MinHeight = height;
            controlElement.MinWidth = width;
            controlElement.UpdateLayout();

            controlElement.Measure(controlSize);
            controlElement.Arrange(new Rect(new Point(0, 0), controlSize));
            controlElement.UpdateLayout();

            WriteableBitmap writeBitmap = new WriteableBitmap((int)width, (int)height);                
            if (writeBitmap != null)
            {
                writeBitmap.Render(controlElement, null);
                writeBitmap.Invalidate();                
            }
              
            //CompensateForRender(writeBitmap.Pixels);
            GC.Collect();//(2, GCCollectionMode.Forced, true);

            return writeBitmap;
        }

        private void SaveCaptureImageToLocal(WriteableBitmap saveWriteBitmap,string savePath)
        {        
            string thirdDicUrl = "Shared/ShellContent/titles";
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists(thirdDicUrl))
                    store.CreateDirectory(thirdDicUrl);

                if (store.FileExists(savePath))
                    store.DeleteFile(savePath);

                using (var stream = store.OpenFile(savePath, FileMode.OpenOrCreate))
                {
                    //saveWriteBitmap.WritePNG(stream);
                    saveWriteBitmap.SaveJpeg(stream, saveWriteBitmap.PixelWidth, saveWriteBitmap.PixelHeight, 0, 100);
                }         
            }
            GC.Collect();
        }

        public FrameworkElement CustomWideTitleElement(CustomerLiveTitleData customerLiveTitleData)
        {
            Grid rootGrid = new Grid();
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            //new version modify city name
            StackPanel tempStackGrid = new StackPanel() { Orientation=Orientation.Horizontal};
            TextBlock temperatureBlock = new TextBlock()
            {
                Text = customerLiveTitleData.Temperature,
                FontSize = 88,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = new SolidColorBrush(Colors.White)
            };
            temperatureBlock.Margin = new Thickness(25, 25, 0, 0);
            tempStackGrid.Children.Add(temperatureBlock);


            StackPanel corePanel = new StackPanel();
            TextBlock coreBlock = new TextBlock() { Text = customerLiveTitleData.Temperature.Equals("")?"":"°", FontSize = 88, Foreground = new SolidColorBrush(Colors.White) };
            coreBlock.Margin = new Thickness(10, 25, 0, 0);
            corePanel.Children.Add(coreBlock);

            TextBlock weatherBlock = new TextBlock() { Text = customerLiveTitleData.WeatherInfo, FontSize = 40, Foreground = new SolidColorBrush(Colors.White) };
            weatherBlock.Margin = new Thickness(10, -50, 0, 0);
            corePanel.Children.Add(weatherBlock);

            tempStackGrid.Children.Add(corePanel);

            Grid.SetRow(tempStackGrid, 0);
            rootGrid.Children.Add(tempStackGrid);


            //new version modify temperature current 
            TextBlock weatherDataBlock = new TextBlock() { Text = customerLiveTitleData.Temperature, FontSize = 80, Visibility=Visibility.Collapsed, Foreground = new SolidColorBrush(Colors.White) };
            weatherDataBlock.Margin = new Thickness(25, 10, 0, 0);
            Grid.SetRow(weatherDataBlock, 1);
            rootGrid.Children.Add(weatherDataBlock);

            StackPanel temperaturTendPanel = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(30, 15, 0, 0), };
            temperaturTendPanel.Children.Add(new TextBlock() { Text = (customerLiveTitleData.LowestTemp.Equals("")?"":"L:" + customerLiveTitleData.LowestTemp + "°"), FontSize = 38, Foreground = new SolidColorBrush(Colors.White) });
            temperaturTendPanel.Children.Add(new TextBlock() { Text = (customerLiveTitleData.HighestTemp.Equals("")?"":"H:" + customerLiveTitleData.HighestTemp + "°"), FontSize = 38, Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(15, 0, 0, 0) });
            Grid.SetRow(temperaturTendPanel, 1);
            rootGrid.Children.Add(temperaturTendPanel);

            //new version modify to weather information 
            StackPanel lastGrid = new StackPanel() { Margin = new Thickness(30, 10, 0, 0),Orientation=Orientation.Horizontal };

            BitmapImage loctionBitmap = new BitmapImage(new Uri("/Assets/location.png", UriKind.RelativeOrAbsolute));
            Image locationIcon = new Image() { Source = loctionBitmap,Width=17,Height=27 };
            lastGrid.Children.Add(locationIcon);

            TextBlock cityBlock = new TextBlock() { Text = customerLiveTitleData.CityName,  FontSize = 38, Margin = new Thickness(5, 5, 25, 0), Foreground = new SolidColorBrush(Colors.White) };
            lastGrid.Children.Add(cityBlock);

            Grid.SetRow(lastGrid, 2);
            rootGrid.Children.Add(lastGrid);

            #region add icon
            Grid logoGrid = new Grid();

            StackPanel logopanel = new StackPanel() { Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Right};
            BitmapImage logoBitmap = new BitmapImage(new Uri("/Assets/mojilogo.png", UriKind.RelativeOrAbsolute));
            Image logoicon = new Image() { Source = logoBitmap,Width=46,Height=31,Margin=new Thickness(0,5,0,0) };
            logopanel.Children.Add(logoicon);


            TextBlock logoBlock = new TextBlock() { Text ="墨迹天气", FontSize = 38,   Foreground = new SolidColorBrush(Colors.White),Margin=new Thickness(10,25,25,0) };
            logopanel.Children.Add(logoBlock);

            logoGrid.Children.Add(logopanel);
            rootGrid.Children.Add(logoGrid);
            Grid.SetRow(logoGrid, 3);
            #endregion

            #region background image
            BitmapImage bitmapImage = new BitmapImage(new Uri(customerLiveTitleData.WideBackgroundImageUrl, UriKind.RelativeOrAbsolute));
            rootGrid.Background = new ImageBrush() { ImageSource = bitmapImage };

            //set height and width
            rootGrid.Height = _WideHeight;
            rootGrid.Width = _WideWidth;
            rootGrid.UpdateLayout();

            //use zindex set background image
            Grid forgrid = new Grid();
            Canvas.SetZIndex(rootGrid, 1);
            forgrid.Children.Add(rootGrid);

            Image convert = new Image();
            Canvas.SetZIndex(convert, 0);
            convert.Source = bitmapImage;
            forgrid.Children.Add(convert);
            #endregion

            return forgrid;
        }

        private void CompensateForRender(int[] bitmapPixels)
        {
            if (bitmapPixels.Length == 0) return;

            for (int i = 0; i < bitmapPixels.Length; i++)
            {
                uint pixel = unchecked((uint)bitmapPixels[i]);

                double a = (pixel >> 24) & 255;
                if ((a == 255) || (a == 0)) continue;

                double r = (pixel >> 16) & 255;
                double g = (pixel >> 8) & 255;
                double b = (pixel) & 255;

                double factor = 255 / a;
                uint newR = (uint)Math.Round(r * factor);
                uint newG = (uint)Math.Round(g * factor);
                uint newB = (uint)Math.Round(b * factor);

                // compose 
                bitmapPixels[i] = unchecked((int)((pixel & 0xFF000000) | (newR << 16) | (newG << 8) | newB));
            }
        }

        private FrameworkElement CustomMiddleTitleElement(CustomerLiveTitleData customerLiveTitleData)
        {
            Grid rootGrid = new Grid();
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            //highest and lowest temperature 
            Grid temperatureGrid = new Grid() {  Margin=new Thickness(0,20,20,0)};
            TextBlock highestTemperature = new TextBlock() { Text = (customerLiveTitleData.HighestTemp.Equals("") ? (customerLiveTitleData.LowestTemp.Equals("") ? "" : "L:" + customerLiveTitleData.LowestTemp + "°") : (customerLiveTitleData.LowestTemp + "°" + "/" + customerLiveTitleData.HighestTemp + "°")), HorizontalAlignment = HorizontalAlignment.Right };
            highestTemperature.Foreground = new SolidColorBrush(Colors.White);
            highestTemperature.FontSize = 38;
            temperatureGrid.Children.Add(highestTemperature);

            Grid.SetRow(temperatureGrid, 0);
            rootGrid.Children.Add(temperatureGrid);

            //location cityname
            Grid locationGrid = new Grid() { Margin = new Thickness(0, 10, 20, 0) };
            TextBlock locationTextBlock = new TextBlock() { Text = customerLiveTitleData.CityName, HorizontalAlignment = HorizontalAlignment.Right };
            locationTextBlock.Foreground = new SolidColorBrush(Colors.White);
            locationTextBlock.FontSize = 38;
            locationGrid.Children.Add(locationTextBlock);

            Grid.SetRow(locationGrid,1);
            rootGrid.Children.Add(locationGrid);

            //temperature information 
            Grid tempGrid = new Grid() { Margin = new Thickness(20, 70, 0, 0) };
            TextBlock tempTextBlock = new TextBlock() { Text = customerLiveTitleData.Temperature.Equals("") ? "" : customerLiveTitleData.Temperature + "°", HorizontalAlignment = HorizontalAlignment.Left };
            tempTextBlock.Foreground = new SolidColorBrush(Colors.White);
            tempTextBlock.FontSize = 50;
            tempGrid.Children.Add(tempTextBlock);

            Grid.SetRow(tempGrid, 2);
            rootGrid.Children.Add(tempGrid);

            //waether infor
            Grid weatherGrid = new Grid() { Margin = new Thickness(20, 5, 0, 0) };
            TextBlock weatherTextBlock = new TextBlock() {Text=customerLiveTitleData.WeatherInfo, HorizontalAlignment=HorizontalAlignment.Left };
            weatherTextBlock.Foreground = new SolidColorBrush(Colors.White);
            weatherTextBlock.FontSize = 38;
            weatherGrid.Children.Add(weatherTextBlock);

            Grid.SetRow(weatherGrid,3);
            rootGrid.Children.Add(weatherGrid);


            //set height and widht
            rootGrid.Height = _MiddleHeight;
            rootGrid.Width = _MiddleWidth;
            rootGrid.UpdateLayout();


            //user zinde set background image
            BitmapImage bitmapImage = new BitmapImage(new Uri(customerLiveTitleData.MiddleBackgrondImageUrl, UriKind.RelativeOrAbsolute));
            Image backgroundImage = new Image() { Source=bitmapImage };

            Grid forgrid = new Grid();
            Canvas.SetZIndex(rootGrid, 1);
            forgrid.Children.Add(rootGrid);

            Canvas.SetZIndex(backgroundImage, 0);
            forgrid.Children.Add(backgroundImage);

            return forgrid;
        }

        private FrameworkElement CustomBackgroundElement(CustomerLiveTitleData customerLiveTitleData, double height, double width)
        {
            Grid rootGrid = new Grid();

            Color accentClr = (Color)Application.Current.Resources["PhoneAccentColor"];
            rootGrid.Background = new SolidColorBrush(accentClr);

            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Grid airGrid = new Grid() { Margin=new Thickness(0,50,0,0)};

            //air title
            TextBlock controlTextBlock = new TextBlock()
            {
                Text = "污染",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 38,
                Margin = new Thickness(25, 25, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            airGrid.Children.Add(controlTextBlock);

            //air right value
            TextBlock ariValueBlock = new TextBlock()
            {
                Text = customerLiveTitleData.Air==null ? "暂无" : customerLiveTitleData.Air,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 50,
                Margin=new Thickness(0,25,25,0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            airGrid.Children.Add(ariValueBlock);

            Grid.SetRow(airGrid, 0);
            rootGrid.Children.Add(airGrid);

            Grid dressGrid = new Grid() { Margin = new Thickness(0, 50, 0, 0) };
            //drees title
            TextBlock dressBlock = new TextBlock()
            {
                Text = "穿衣",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 38,
                Margin = new Thickness(25, 25, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            dressGrid.Children.Add(dressBlock);

            //dress right value
            TextBlock dressValue = new TextBlock()
            {
                Text = customerLiveTitleData.Dress==null ? "暂无" : customerLiveTitleData.Dress,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 50,
                Margin = new Thickness(0, 25, 25, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            dressGrid.Children.Add(dressValue);

            Grid.SetRow(dressGrid, 1);
            rootGrid.Children.Add(dressGrid);

            //set height and width
            rootGrid.Height = height;
            rootGrid.Width = width;
            rootGrid.UpdateLayout();

            return rootGrid;
        }

        public void FormatSavePath(string cityId)
        {
            WideBackBackgroundImagePath = SaveVisitPath + cityId + WideBackBackgroundImagePath;
            WideSaveBackgroundImagePath = SaveVisitPath + cityId + WideSaveBackgroundImagePath;

            MiddleBackBackgroundPath = SaveVisitPath + cityId + MiddleBackBackgroundPath;
            MiddleSaveBackgroundImagePath = SaveVisitPath + cityId + MiddleSaveBackgroundImagePath;
        }
    }

    public enum LiveTitleType
    {
        Wide,
        WideBack,
        Middle,
        MiddleBack,
        Small
    }
}
