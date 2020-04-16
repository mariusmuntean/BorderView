using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BorderView.Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomBackgroundBorderViewPage : ContentPage
    {
        public CustomBackgroundBorderViewPage()
        {
            Datas = new List<Data>()
            {
                new Data()
                {
                    BackgroundImageUrl = "https://image.freepik.com/free-vector/cute-monkeys-pattern-background_25030-18285.jpg",
                    MainImageUrl = "https://i.pinimg.com/474x/9c/89/7d/9c897dbe7928cd5dc09f0ac3c40e8687.jpg",
                    Name = "Coco",
                    Age = "Age: 2",
                    Favorite = "Favorite food: banana split",
                    Hobby = "Hobbies: Xamarin"
                },
                new Data()
                {
                    BackgroundImageUrl = "https://image.freepik.com/free-vector/cute-animals-seamless-pattern-background_39663-185.jpg",
                    MainImageUrl = "https://i.pinimg.com/474x/ce/53/c5/ce53c5bcd350ba856e5c53c343376fb2.jpg",
                    Name = "Archie",
                    Age = "Age: 1",
                    Favorite = "Favorite book: The Tempest",
                    Hobby = "Hobbies: classical music"
                }
            };
            InitializeComponent();
        }

        public List<Data> Datas { get; set; }
    }

    public class Data
    {
        public string BackgroundImageUrl { get; set; }
        public string MainImageUrl { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Favorite { get; set; }
        public string Hobby { get; set; }
    }
}