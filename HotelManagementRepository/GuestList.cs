using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using HotelManagementRepository.App_Data;

namespace HotelManagementRepository
{
    public partial class GuestList : Form
    {
        Repository repository = new Repository();
        public GuestList()
        {
            InitializeComponent();
        }
        void DataLoad()
        {

            this.guestTableBindingSource.DataSource = repository.GetGuestLists();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var id = GuestGrid.SelectedRows[0].Cells[0].Value.ToString();


            if (int.Parse(id) > 0)
            {
                GuestEntry form = new GuestEntry();

                form.GuestID = int.Parse(id);


                form.ShowDialog(this);
            }

        }

        private void GuestList_Load(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            ReportDocument report = new ReportDocument();


            report.Load($"{Application.StartupPath}\\MyReport\\GuestReport.rpt");

            if (report.IsLoaded)
            {


                report.SetDataSource(repository.GetReportData());

            }



            ReportViewerForm form = new ReportViewerForm();

            form.crystalReportViewer.ReportSource = report;



            form.ShowDialog(this);
        }
    }
}
