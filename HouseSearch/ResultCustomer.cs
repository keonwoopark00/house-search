using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HouseSearch
{
    public partial class ResultCustomer : Form
    {
        public ResultCustomer()
        {
            InitializeComponent();
        }

        private IQueryable<House> hsList;
        public IQueryable<House> HsList
        {
            get { return this.hsList; }
            set { this.hsList = value; }
        }

        private void ResultCustomer_Load(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"../../img");
            filePath = Path.GetFullPath(filePath);
            int i = 0;
            foreach (var hs in hsList)
            {
                PictureBox pb = new PictureBox();
                pb.Margin = new Padding(0, 0, 50, 0);
                pb.Top = 90;
                pb.Left = 50 + 350 * i;
                pb.Width = 300;
                pb.Height = 300;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                Bitmap bmp = new Bitmap(filePath + @"/" + hs.HS_code + ".jpg");
                pb.Image = (Image)bmp;
                this.Controls.Add(pb);

                Label lbAddress = new Label();
                Label lbPrice = new Label();
                lbAddress.Top = 400;
                lbAddress.Left = 50 + 350 * i;
                lbAddress.Text = hs.HS_address.ToString();
                lbAddress.Width = 300;
                lbAddress.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                lbPrice.Top = 430;
                lbPrice.Left = 50 + 350 * i;
                lbPrice.Text = String.Format("{0:C2}", hs.HS_price);
                lbPrice.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                lbPrice.Margin = new Padding(0, 0, 0, 50);
                this.Controls.Add(lbAddress);
                this.Controls.Add(lbPrice);
                i++;
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.Show();
            this.Hide();
        }
    }
}
