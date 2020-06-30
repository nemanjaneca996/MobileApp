using ResvoyageMobileApp.Views;
using System;
using Xamarin.Forms;
using ResvoyageMobileApp.Services;
using Xamarin.Forms.Xaml;
using RestSharp;
using Newtonsoft.Json;
using ResvoyageMobileApp.Models;

[assembly: ExportFont("SF Pro Display Bold.ttf", Alias ="DisplayBold")]
[assembly: ExportFont("SF Pro Display Light.ttf", Alias ="DisplayLight")]
[assembly: ExportFont("SF Pro Display Medium.ttf", Alias ="DisplayMedium")]
[assembly: ExportFont("SF Pro Display Regular.ttf", Alias ="DisplayRegular")]
[assembly: ExportFont("SF Pro Display Semibold.ttf", Alias ="DisplaySemibold")]
[assembly: ExportFont("SF Pro Display Thin.ttf", Alias ="DisplayThin")]
[assembly: ExportFont("SF Pro Text Medium.ttf", Alias ="TextMedium")]
[assembly: ExportFont("SF Pro Text Regular.ttf", Alias = "TextRegular")]
[assembly: ExportFont("SF Pro Text Semibold.ttf", Alias = "TextSemibold")]
namespace ResvoyageMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU2OTA3QDMxMzgyZTMxMmUzME4vcWlJT09XNDRjR3pSZ0FvNGd1V3JpR21aSU41dGY2M3ZuUnlYKzFGV0U9");
            GenerateToken();
            InitializeComponent();
            Device.SetFlags(new string[] { "RadioButton_Experimental" });
            MainPage = new HomePage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected void GenerateToken()
        {
            RestClient _client = new RestClient("http://restb2b.resvoyage.com");
            string token = null;
            var request = new RestRequest("api/v1/public/token?clientname=vg60vo4kI4d");
            request.AddHeader("Content-Type", "application/json-patch+json");
            request.AddHeader("Accept", "application/json");

            var response = _client.Get(request);
            var responseData = JsonConvert.DeserializeObject<UserToken>(response.Content);

            token = responseData.Token;
            Application.Current.Properties["RVToken"] = token;
        }
    }
}
