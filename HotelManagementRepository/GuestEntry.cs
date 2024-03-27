using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HotelManagementRepository.App_Data;

namespace HotelManagementRepository
{
    public partial class GuestEntry : Form
    {
        Repository repository = new Repository();

        public int GuestID { get; set; }

        public GuestEntry()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        void ResetForm()
        {
            txtID.Text = null;
            txtName.Text = null;
            txtPhone.Text = null;
            txtAddress.Text = null;

            roomsTableBindingSource.DataSource = null;
            txtName.Focus();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                GuestTable guest = new GuestTable();

                if (txtID.Text.Length > 0)
                    guest.GuestID = Convert.ToInt32(txtID.Text);

                
                guest.GuestName = txtName.Text;
                guest.Phone = txtPhone.Text;
                guest.Address = txtAddress.Text;





                foreach (DataGridViewRow item in gridItem.Rows)
                {

                    if (item.IsNewRow) continue;

                    RoomsTable roomsDetails = new RoomsTable();

                    roomsDetails.RoomNumber = Convert.ToInt32(item.Cells[1].Value);
                    roomsDetails.RoomType = item.Cells[2].Value.ToString();
                    roomsDetails.RoomPerNight = Convert.ToDecimal(item.Cells[3].Value);
                    guest.roomsTables.Add(roomsDetails);
                }

                if (txtID.Text.Length > 0)
                {

                    int rw = repository.UpdateGuest(guest);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data updated successfully");
                    }
                }
                else
                {
                    int rw = repository.SaveGuest(guest);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data saved successfully");
                    }
                }


                ResetForm();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void GuestEntry_Load(object sender, EventArgs e)
        {
            if(GuestID > 0)
            {
                var guest = repository.GetGuestLists(GuestID);

                txtID.Text = guest.GuestID.ToString();
                txtName.Text = guest.GuestName.ToString();
                txtPhone.Text = guest.Phone;
                txtAddress.Text = guest.Address;

                roomsTableBindingSource.DataSource = guest.roomsTables;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Length > 0)
            {

                var dialog = MessageBox.Show("Delete record", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dialog == DialogResult.OK)
                {
                    int rw = repository.DeleteGuest(txtID.Text);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data deleted successfully");
                        ResetForm();
                    }
                }




            }
        }
    }
}
