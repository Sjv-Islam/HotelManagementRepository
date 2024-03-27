using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagementRepository.App_Data
{
    internal class Repository
    {
        string conString = $"server =(localdb)\\mssqllocaldb; AttachDbFilename = {Application.StartupPath}\\App_Data\\ProjectDB.mdf; Trusted_Connection = true;";

        public List<GuestTable> GetGuestLists()
        {


            List<GuestTable> guests = new List<GuestTable>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                
                cmd.CommandText = "select * from GuestDetails";

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                con.Open();
                sda.Fill(ds);


                if (ds.Tables.Count > 0)
                {



                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GuestTable guest = new GuestTable();
                        guest.GuestID = Convert.ToInt32(dr["GuestID"]);
                        guest.GuestName = dr["GuestName"].ToString();
                        guest.Phone = dr["Phone"].ToString();
                        guest.Address = dr["Address"]?.ToString();
                        guests.Add(guest);
                    }

                }

            }

            return guests;
        }

        public GuestTable GetGuestLists(int GuestID)
        {

            GuestTable guest = new GuestTable();
            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                cmd.CommandText = $"select * from GuestDetails where GuestID = {GuestID}; select * from RoomDetails where GuestID = {GuestID}" ;

                cmd.Parameters.Add(new SqlParameter("@invoiceId", GuestID));

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                sda.Fill(ds);


                if (ds.Tables.Count > 0)
                {

                    var dr = ds.Tables[0].Rows[0];





                    guest.GuestID = Convert.ToInt32(dr["GuestID"]);
                    guest.GuestName = dr["GuestName"].ToString();
                    guest.Phone = dr["Phone"].ToString();
                    guest.Address = dr["Address"]?.ToString();

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {

                        RoomsTable room = new RoomsTable();


                        room.GuestID = Convert.ToInt32(row["GuestID"]);
                        room.RoomNumber = Convert.ToInt32(row["RoomNumber"]);
                        room.RoomType = row["RoomType"].ToString();
                        room.RoomPerNight = Convert.ToUInt32(row["RoomPerNight"]);

                        guest.roomsTables.Add(room);
                    }



                }

            }
            return guest;
        }

        public int SaveGuest(GuestTable Guest)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;



                try
                {


                    cmd.CommandText = "select isnull(max(guestid), 0) + 1 as GuestID from GuestDetails";


                    string GuestID = cmd.ExecuteScalar()?.ToString();



                    cmd.CommandText = $"INSERT INTO [dbo].[GuestDetails]([GuestID],[GuestName],[Phone],[Address]) VALUES (  {GuestID},'{Guest.GuestName}', '{Guest.Phone}', '{Guest.Address}')";


                    rowNo = cmd.ExecuteNonQuery();


                    if (rowNo > 0)
                    {

                        foreach (var guest in Guest.roomsTables)
                        {
                            cmd.CommandText = $"INSERT INTO [dbo].[RoomDetails] ([GuestID],[RoomNumber] ,[RoomType] ,[RoomPerNight] ) VALUES ({GuestID} ,'{guest.RoomNumber}' , '{guest.RoomType}' , '{guest.RoomPerNight}')";


                            int r1 = cmd.ExecuteNonQuery();
                        }

                    }

                    tran.Commit();
                }
                catch (SqlException e)
                {

                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }


        public int UpdateGuest(GuestTable Guest)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;



                try
                {




                    cmd.CommandText = $"UPDATE [dbo].[GuestDetails]   SET  [GuestName] = '{Guest.GuestName}',[Address] = '{Guest.Address}',[Phone] = '{Guest.Phone}' where GuestID = {Guest.GuestID}";

                    rowNo = cmd.ExecuteNonQuery();


                    if (rowNo > 0)
                    {
                        cmd.CommandText = $"delete from [dbo].[RoomDetails] where GuestID = {Guest.GuestID}";


                        if (cmd.ExecuteNonQuery() >= 0)
                        {
                            foreach (var guest in Guest.roomsTables)
                            {
                                cmd.CommandText = $"INSERT INTO [dbo].[RoomDetails] ([GuestID] ,[RoomNumber] ,[RoomType] ,[RoomPerNight])  VALUES ({Guest.GuestID} ,'{guest.RoomNumber}' , '{guest.RoomType}' , '{guest.RoomPerNight}')";


                                cmd.ExecuteNonQuery();
                            }
                        }



                    }

                    tran.Commit();
                }
                catch (SqlException e)
                {

                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }

        public int DeleteGuest(string GuestID)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;




                try
                {



                    cmd.CommandText = $"delete from [dbo].[GuestDetails]   where GuestID = {GuestID}";

                    rowNo = cmd.ExecuteNonQuery();

                    tran.Commit();

                }
                catch (SqlException e)
                {
                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }

        internal List<vwGuestRoom> GetReportData()
        {
            List<vwGuestRoom> guests = new List<vwGuestRoom>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * FROM vwGuestRoom";



                DataTable dt = new DataTable();
                con.Open();



                dt.Load(cmd.ExecuteReader());




                foreach (DataRow dr in dt.Rows)
                {
                    vwGuestRoom guest = new vwGuestRoom();
                    guest.GuestID = Convert.ToInt32(dr["GuestID"]);
                    guest.GuestName = dr["GuestName"].ToString();
                    guest.Phone = dr["Phone"].ToString();
                    guest.Address = dr["Address"]?.ToString();
                    guest.RoomNumber = Convert.ToInt32(dr["RoomNumber"]);
                    guest.RoomType = dr["RoomType"].ToString();
                    guest.RoomPerNight = Convert.ToDecimal(dr["RoomPerNight"]);






                    guests.Add(guest);
                }



            }

            return guests;
        }
    }
}
