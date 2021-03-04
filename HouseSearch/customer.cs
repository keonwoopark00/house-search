using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HouseSearch
{
    public partial class Customer : Form
    {
        projectEntities context = new projectEntities();

        public Customer()
        {
            InitializeComponent();
        }

        private void EmptyAll()
        {
            comboRegion.Text = "";
            comboCity.Text = "";
            comboNbBathrooms.Text = "";
            comboNbRooms.Text = "";
            comboPriceFrom.Text = "";
            comboPriceTo.Text = "";
            radioGarage.Checked = false;
            radioFireplace.Checked = false;
            radioPool.Checked = false;
        }

        private void ComboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboCity.Items.Clear();
            comboCity.Text = "";
            int i = comboRegion.SelectedIndex + 1;
            var cityList = from city in context.Cities
                           where city.REG_code == i
                           select city;
            foreach (var city in cityList)
            {
                comboCity.Items.Add(city.CT_name);
            }
        }

        private void InitializeBoxes()
        {
            var regList = from reg in context.Regions
                          select reg;
            foreach (var reg in regList)
            {
                comboRegion.Items.Add(reg.REG_name);
            }


            for (int i = 0; i < 5; i++)
            {
                comboNbRooms.Items.Add(String.Format("{0}+", i));
                comboNbBathrooms.Items.Add(String.Format("{0}+", i));
            }

            for (int i = 0; i < 10; i++)
            {
                comboPriceFrom.Items.Add(String.Format("{0:C0}", i * 50000));
                comboPriceTo.Items.Add(String.Format("{0:C0}", i * 50000));
            }
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            InitializeBoxes();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (comboCity.SelectedItem == null)
            {
                MessageBox.Show("Please select a city", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string ct = comboCity.SelectedItem.ToString();
                int ctCode = 0;
                int priceFrom = 0;
                int priceTo = 0;
                int nbRooms = 0;
                int nbBathrooms = 0;
                bool garage = false;
                bool pool = false;
                bool fireplace = false;

                // get city code
                var cities = from city in context.Cities
                             where city.CT_name == ct
                             select city;
                foreach (var c in cities)
                {
                    ctCode = c.CT_code;
                }

                // get price range
                if (comboPriceTo.SelectedItem != null && comboPriceFrom.SelectedItem != null)
                {
                    priceFrom = comboPriceFrom.SelectedIndex * 50000;
                    priceTo = comboPriceTo.SelectedIndex * 50000;
                }
                else if (comboPriceFrom.SelectedItem == null && comboPriceTo.SelectedItem != null)
                {
                    priceTo = comboPriceTo.SelectedIndex * 50000;
                }
                else if (comboPriceTo.SelectedItem == null && comboPriceFrom.SelectedItem != null)
                {
                    priceFrom = comboPriceFrom.SelectedIndex * 50000;
                }

                // get the number of rooms
                if (comboNbRooms.SelectedItem != null)
                {
                    nbRooms = comboNbRooms.SelectedIndex;
                }

                // get the number of bathrooms
                if (comboNbBathrooms.SelectedItem != null)
                {
                    nbBathrooms = comboNbBathrooms.SelectedIndex;
                }

                // get values from the buttons
                if (radioGarage.Checked)
                {
                    garage = true;
                }
                if (radioPool.Checked)
                {
                    pool = true;
                }
                if (radioFireplace.Checked)
                {
                    fireplace = true;
                }

                IQueryable<House> hsList = null;
                //linq
                if (priceFrom > priceTo)
                {
                    hsList = from hs in context.Houses
                             where hs.CT_code == ctCode && hs.HS_price >= priceFrom && hs.HS_nb_rooms >= nbRooms && hs.HS_nb_bathrooms >= nbBathrooms
                             && hs.HS_garage == garage && hs.HS_pool == pool && hs.HS_fireplace == fireplace
                             select hs;
                }
                else if (priceFrom < priceTo)
                {
                    hsList = from hs in context.Houses
                             where hs.CT_code == ctCode && hs.HS_price >= priceFrom && hs.HS_price <= priceTo && hs.HS_nb_rooms >= nbRooms
                             && hs.HS_nb_bathrooms >= nbBathrooms && hs.HS_garage == garage && hs.HS_pool == pool && hs.HS_fireplace == fireplace
                             select hs;
                }
                else if (priceFrom == priceTo)
                {
                    hsList = from hs in context.Houses
                             where hs.CT_code == ctCode && hs.HS_price >= priceFrom && hs.HS_nb_rooms >= nbRooms && hs.HS_nb_bathrooms >= nbBathrooms
                             && hs.HS_garage == garage && hs.HS_pool == pool && hs.HS_fireplace == fireplace
                             select hs;
                }
                if (hsList.Count() == 0)
                {
                    MessageBox.Show("No house found.", "No house", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ResultCustomer result = new ResultCustomer();
                    result.HsList = hsList;
                    result.Show();
                    this.Hide();
                }
                EmptyAll();
            }
        }

        
    }
}

        
    
