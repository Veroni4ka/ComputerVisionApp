using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceRecognitionApp
{
	public partial class MainPage : ContentPage
	{
		public  MainPage()
		{
			InitializeComponent();
            
        }

        async void SelectPicture()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var image = await CrossMedia.Current.PickPhotoAsync();
                var stream = image.GetStream();
                SelectedImage.Source = ImageSource.FromStream(() =>
                {
                    return stream;
                });
                var result = await GetImageDescription(image.GetStream());
                image.Dispose();
                foreach (string tag in result.Description.Tags)
                {
                    InfoLabel.Text = InfoLabel.Text + "\n" + tag;
                }
            }
        }

        public async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            VisionServiceClient visionClient = new VisionServiceClient("KEY");
            VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description };
            return await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            SelectPicture();
        }
    }
}
