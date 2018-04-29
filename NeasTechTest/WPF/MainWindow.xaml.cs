using Model;
using RestSharp;
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


namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Get_Districts_Button_Click(object sender, RoutedEventArgs e)
        {
            DistrictLV.ItemsSource = GetAllDistrict();
        }       

        private void DistrictSelected(object sender, RoutedEventArgs e)
        {
            var district = DistrictLV.SelectedItem as District;
            if(district!= null)
            {
                district = GetDistrictById(district);
                DistrictInfo.Content = district;
            }       
            if(district.Salespersons != null || district.Salespersons.Count() != 0)
            {
                SalespersonsLV.ItemsSource = district.Salespersons;
            }
            if (district.Stores != null || district.Stores.Count() != 0)
            {
                StoresLV.ItemsSource = district.Stores;
            }
        }

        public static IEnumerable<District> GetAllDistrict()
        {
            var client = new RestClient("http://localhost:54048/api/District/");
            var request = new RestRequest(Method.GET);
            IRestResponse<List<District>> response = client.Execute<List<District>>(request);
            return response.Data;
        }
        public static District GetDistrictById(District district)
        {
            var client = new RestClient("http://localhost:54048/");
            var request = new RestRequest("api/District/{id}", Method.GET);
            request.AddUrlSegment("id", district.Id);
            IRestResponse<District> response = client.Execute<District>(request);
            return response.Data;
        }

    }
}
