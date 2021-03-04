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
    public partial class Admin : Form
    {
        projectEntities context = new projectEntities();
        public Admin()
        {
            InitializeComponent();
        }

        private void EmptyAll()
        {
            comboRegion.Text = "";
            txtAddress.Text = "";
            comboType.Text = "";
            comboCity.Text = "";
            comboNbBathrooms.Text = "";
            comboNbRooms.Text = "";
            comboPriceFrom.Text = "";
            comboPriceTo.Text = "";
            txtHouseCode.Text = "";
            radioGarage.Checked = false;
            radioFireplace.Checked = false;
            radioPool.Checked = false;
        }

        private void InitializeBoxes()
        {
            var regList = from reg in context.Regions
                          select reg;
            foreach(var reg in regList)
            {
                comboRegion.Items.Add(reg.REG_name);
            }

            
            for(int i = 0; i < 5; i++)
            {
                comboNbRooms.Items.Add(String.Format("{0}+", i));
                comboNbBathrooms.Items.Add(String.Format("{0}+", i));
            }

            for(int i = 0; i < 10; i++)
            {
                comboPriceFrom.Items.Add(String.Format("{0:C0}", i * 50000));
                comboPriceTo.Items.Add(String.Format("{0:C0}", i * 50000));
            }
        }

        private void ComboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboCity.Items.Clear();
            comboCity.Text = "";
            int i = comboRegion.SelectedIndex + 1;
            var cityList = from city in context.Cities
                           where city.REG_code == i
                           select city;
            foreach(var city in cityList)
            {
                comboCity.Items.Add(city.CT_name);
            }
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            InitializeBoxes();
            listView1.FullRowSelect = true;
            txtHouseCode.Enabled = false;
        }

        private void PopulateListView()
        {
            var hsList = from house in context.Houses
                         select house;

            listView1.Items.Clear();
            foreach (var hs in hsList)
            {
                ListViewItem item = new ListViewItem(hs.HS_code.ToString());
                item.SubItems.Add(hs.HS_address.ToString());
                item.SubItems.Add(hs.HS_type.ToString());
                item.SubItems.Add("$ " + hs.HS_price.ToString());
                item.SubItems.Add(hs.HS_nb_rooms.ToString());
                item.SubItems.Add(hs.HS_nb_bathrooms.ToString());
                item.SubItems.Add(hs.HS_garage.ToString());
                item.SubItems.Add(hs.HS_pool.ToString());
                item.SubItems.Add(hs.HS_fireplace.ToString());
                item.SubItems.Add(hs.CT_code.ToString());
                listView1.Items.Add(item);
            }
        }

        private void BtnList_Click(object sender, EventArgs e)
        {
            PopulateListView();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            House hs = new House();
            if(txtAddress.Text == "")
            {
                MessageBox.Show("Please Enter the address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(comboType.SelectedItem == null)
                {
                    MessageBox.Show("Please select the type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (comboCity.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a city", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        hs.HS_address = txtAddress.Text;
                        hs.HS_type = comboType.SelectedItem.ToString();
                        string ct = comboCity.SelectedItem.ToString();
                        int i = 0;
                        var cities = from city in context.Cities
                                     where city.CT_name == ct
                                     select city;
                        foreach (var c in cities)
                        {
                            i = c.CT_code;
                        }
                        hs.CT_code = i;
                        if(comboPriceTo.SelectedItem == null && comboPriceFrom.SelectedItem == null)
                            MessageBox.Show("Please select the price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            if (comboPriceFrom.SelectedItem == null)
                                hs.HS_price = comboPriceTo.SelectedIndex * 50000;
                            else if (comboPriceTo.SelectedItem == null)
                                hs.HS_price = comboPriceFrom.SelectedIndex * 50000;
                            if (comboNbRooms.SelectedItem != null)
                                hs.HS_nb_rooms = Convert.ToByte(comboNbRooms.SelectedIndex);
                            if (comboNbBathrooms.SelectedItem != null)
                                hs.HS_nb_bathrooms = Convert.ToByte(comboNbBathrooms.SelectedIndex);
                            if (radioGarage.Checked)
                                hs.HS_garage = true;
                            else
                                hs.HS_garage = false;
                            if (radioPool.Checked)
                                hs.HS_pool = true;
                            else
                                hs.HS_pool = false;
                            if (radioFireplace.Checked)
                                hs.HS_fireplace = true;
                            else
                                hs.HS_fireplace = false;
                            context.Houses.Add(hs);
                            context.SaveChanges();
                            EmptyAll();
                            MessageBox.Show("Save succeeded.", "Saved");
                        }
                    }
                }
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (txtHouseCode.Text == "")
            {
                MessageBox.Show("Please select a house to be updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                House hs = context.Houses.Find(Convert.ToInt32(txtHouseCode.Text));
                if (hs == null)
                {
                    MessageBox.Show("No house found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (txtAddress.Text == "")
                    {
                        MessageBox.Show("Please Enter the address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (comboType.SelectedItem == null)
                        {
                            MessageBox.Show("Please select the type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (comboCity.SelectedItem == null)
                            {
                                MessageBox.Show("Please select a city", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                hs.HS_address = txtAddress.Text;
                                hs.HS_type = comboType.SelectedItem.ToString();
                                string ct = comboCity.SelectedItem.ToString();
                                int i = 0;
                                var cities = from city in context.Cities
                                             where city.CT_name == ct
                                             select city;
                                foreach (var c in cities)
                                {
                                    i = c.CT_code;
                                }
                                hs.CT_code = i;
                                if (comboPriceTo.SelectedItem == null && comboPriceFrom.SelectedItem == null)
                                    MessageBox.Show("Please select the price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                else
                                {
                                    if (comboPriceFrom.SelectedItem == null)
                                        hs.HS_price = comboPriceTo.SelectedIndex * 50000;
                                    else if (comboPriceTo.SelectedItem == null)
                                        hs.HS_price = comboPriceFrom.SelectedIndex * 50000;
                                    if (comboNbRooms.SelectedItem != null)
                                        hs.HS_nb_rooms = Convert.ToByte(comboNbRooms.SelectedIndex);
                                    if (comboNbBathrooms.SelectedItem != null)
                                        hs.HS_nb_bathrooms = Convert.ToByte(comboNbBathrooms.SelectedIndex);
                                    if (radioGarage.Checked)
                                        hs.HS_garage = true;
                                    else
                                        hs.HS_garage = false;
                                    if (radioPool.Checked)
                                        hs.HS_pool = true;
                                    else
                                        hs.HS_pool = false;
                                    if (radioFireplace.Checked)
                                        hs.HS_fireplace = true;
                                    else
                                        hs.HS_fireplace = false;
                                    context.SaveChanges();
                                    EmptyAll();
                                    MessageBox.Show("Update succeeded.", "Saved");
                                }
                            }
                        }
                    }
                }
            }
        }
    
        private void Delete_Click(object sender, EventArgs e)
        {
            if (txtHouseCode.Text == "")
            {
                MessageBox.Show("Please select a house to be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                House hs = context.Houses.Find(Convert.ToInt32(txtHouseCode.Text));
                context.Houses.Remove(hs);
                context.SaveChanges();
                MessageBox.Show("The House wite house code " + txtHouseCode.Text + " is deleted.", "Delete");
                EmptyAll();
            }

        }

        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            txtHouseCode.Text = item.SubItems[0].Text;
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
                    ResultAdmin result = new ResultAdmin();
                    result.HsList = hsList;
                    result.Show();
                    this.Hide();
                }
                EmptyAll();   
            }
        }   
    }
}

