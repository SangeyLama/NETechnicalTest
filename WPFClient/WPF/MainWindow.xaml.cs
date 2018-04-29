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
using WPF.WebAPI;


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
            DistrictLV.ItemsSource = DistrictAPI.GetAll();
        }

        private void DistrictSelected(object sender, RoutedEventArgs e)
        {
            var district = DistrictLV.SelectedItem as District;
            if (district != null)
            {
                district = DistrictAPI.GetById(district);
                DistrictInfo.Content = district;
                if (district.Salespersons != null || district.Salespersons.Count() != 0)
                {
                    SalespersonsLV.ItemsSource = district.Salespersons;
                }
                if (district.Stores != null || district.Stores.Count() != 0)
                {
                    StoresLV.ItemsSource = district.Stores;
                }
            }
        }

        private void Edit_District_Button_Click(object sender, RoutedEventArgs e)
        {
            var district = DistrictLV.SelectedItem as District;
            if (district != null)
            {
                district = DistrictAPI.GetById(district);
                this.Hide();
                EditDistrictSalespersons edit = new EditDistrictSalespersons(district, this);
                edit.Show();
            }

        }
    }
}
