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
using System.Windows.Shapes;
using System.ComponentModel;
using WPF.WebAPI;
using System.Net;

namespace WPF
{
    /// <summary>
    /// Interaction logic for EditDistrictSalespersons.xaml
    /// </summary>
    public partial class EditDistrictSalespersons : Window
    {
        private District district;
        private MainWindow parent;
        private bool editsMade;
        private List<Salesperson> remainingSalespersons;
        private DistrictAPI districtAPI = new DistrictAPI();
        private SalespersonAPI salespersonAPI = new SalespersonAPI();
        public EditDistrictSalespersons(District district, MainWindow parent)
        {
            InitializeComponent();
            this.district = district;
            this.parent = parent;
            init();            
        }

        private void init()
        {
            if (district != null)
            {
                DistrictInfo.Content = district;
            }
            if (district.Salespersons != null || district.Salespersons.Count() != 0)
            {
                currSalespersonsLV.ItemsSource = district.Salespersons;
            }
            if (district.Stores != null || district.Stores.Count() != 0)
            {
                StoresLV.ItemsSource = district.Stores;
            }
            IEnumerable<Salesperson> salespersons = salespersonAPI.GetAll();
            remainingSalespersons = new List<Salesperson>();
            foreach(Salesperson s in salespersons)
            {
                if (district.Salespersons.FirstOrDefault(sp => sp.Id == s.Id) == null)
                {
                    remainingSalespersons.Add(s);
                }
            }
            if(salespersons != null)
            {
                allSalespersonsLV.ItemsSource = remainingSalespersons;
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Salesperson s = allSalespersonsLV.SelectedItem as Salesperson;
            if (s != null)
            {
                editsMade = true;
                remainingSalespersons.Remove(s);
                var salespersons = district.Salespersons as List<Salesperson>;
                salespersons.Add(s);
                district.Salespersons = salespersons;
                currSalespersonsLV.Items.Refresh();
                allSalespersonsLV.Items.Refresh();
            }
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            Salesperson s = currSalespersonsLV.SelectedItem as Salesperson;
            if (s != null)
            {
                if(s.Id != district.PrimarySalesperson.Id)
                {
                    editsMade = true;
                    remainingSalespersons.Add(s);
                    var salespersons = district.Salespersons as List<Salesperson>;
                    salespersons.Remove(s);
                    district.Salespersons = salespersons;
                    currSalespersonsLV.Items.Refresh();
                    allSalespersonsLV.Items.Refresh();
                }
                else
                {
                    MessageBox.Show(String.Format("Cannot remove salesperson: {0}. \nThey are currently the primary salesperson for this district. " +
                        "\nSet a new primary salesperson before removing.", s.Name));
                }
                
            }
        }

        private void Set_Primary_Button_Click(object sender, RoutedEventArgs e)
        {
            Salesperson s = currSalespersonsLV.SelectedItem as Salesperson;
            if(s != null)
            {
                editsMade = true;
                district.PrimarySalesperson = s;
                DistrictInfo.Content = null;
                DistrictInfo.Content = district;
            }
        }

        private void Save_Changes_Button_Click(object sender, RoutedEventArgs e)
        {
            if (editsMade)
            {
                var result = districtAPI.Update(district);
                if(result.StatusCode == HttpStatusCode.Accepted)
                {
                    editsMade = false;
                    MessageBox.Show("Changes Saved");
                    this.Close();
                    parent.Show();
                    parent.DistrictLV.ItemsSource = districtAPI.GetAll();
                    parent.DistrictInfo.Content = null;
                    parent.DistrictInfo.Content = district;
                    parent.SalespersonsLV.ItemsSource = district.Salespersons;
                }
                else
                {
                    MessageBox.Show("Error saving changes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditDistrictSalespersons_Closing(Object sender, CancelEventArgs e)
        {
            // If data is dirty, notify user and ask for a response
            if (this.editsMade)
            {
                string msg = "Changes haven't been saved. Close without saving?";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "Warning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    // If user doesn't want to close, cancel closure
                    e.Cancel = true;
                }
                else
                {
                    parent.Show();
                }
            }
            else
            {
                parent.Show();
            }
        }

        
    }
}
