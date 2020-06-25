using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BookRentalShop20
{
    public partial class RentalForm : MetroForm
    {

        string mode = "";
        public RentalForm()
        {
            InitializeComponent();
        }

        private void UpdateData()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                string strQuery = "SELECT r.idx AS '대여번호', m.Idx AS '회원번호'," +
                                  "       m.Names AS '대여회원', b.Idx AS '책등록번호', " +
                                  "       t.Names AS '장르', " +
                                  "       b.Names AS '대여책제목', b.ISBN, " +
                                  "       r.rentalDate AS '대여일', " +
                                  "       r.returnDate AS '반납일' " +
                                  "       FROM rentaltbl AS r " +
                                  " INNER JOIN membertbl AS m " +
                                  "    ON r.memberIdx = m.Idx " +
                                  " INNER JOIN bookstbl AS b " +
                                  "    ON r.bookIdx = b.Idx " +
                                  " INNER JOIN divtbl AS t " +
                                  "    ON b.division = t.division; ";
                //SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "rentaltbl");
                GrdRentalsTbl.DataSource = ds;
                GrdRentalsTbl.DataMember = "rentaltbl";


            }
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow data = GrdRentalsTbl.Rows[e.RowIndex];
                TxtIdx.Text = data.Cells[0].Value.ToString();
                TxtIdx.ReadOnly = true;
                TxtIdx.BackColor = MetroColors.Green;
                //CboMember.SelectedValue = CboMember.FindString(data.Cells[1].Value.ToString());
                CboMember.SelectedValue = data.Cells[1].Value.ToString();
                // Division Names으로 값을 찾아오는 방법
                //CboBook.SelectedIndex = CboBook.FindString(data.Cells[3].Value.ToString());
                CboBook.SelectedValue = data.Cells[3].Value.ToString();
                DtpFormat();
                DtpRentalDate.Value = DateTime.Parse(data.Cells[7].Value.ToString());
                if (!string.IsNullOrEmpty(data.Cells[8].Value.ToString()))
                    DtpReturnDate.Value = DateTime.Parse(data.Cells[8].Value.ToString());
                else
                {
                    DtpReturnDate.Format = DateTimePickerFormat.Custom;
                    DtpReturnDate.CustomFormat = " ";
                }
                mode = "UPDATE";    // 수정은 UPDATE

            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearTextControls();
            DtpFormat();
            mode = "INSERT";    // 신규는 INSERT
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (CboMember.SelectedIndex == -1 || CboBook.SelectedIndex == -1 || string.IsNullOrEmpty(DtpRentalDate.Value.ToString()))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다.", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // DB 저장 Process
            SaveProcess();
            // Data Load to WinformDataGrid
            UpdateData();

            // textbox clear
            ClearTextControls();
            DtpFormat();

        }

        private void ClearTextControls()
        {
            TxtIdx.Text = "";
            TxtIdx.ReadOnly = true;
            CboBook.SelectedIndex = -1;
            CboMember.SelectedIndex = -1;
            DtpRentalDate.Format = DateTimePickerFormat.Custom;
            DtpRentalDate.CustomFormat = " ";
            DtpReturnDate.Format = DateTimePickerFormat.Custom;
            DtpReturnDate.CustomFormat = " ";
            CboMember.Focus();
        }

        private void SaveProcess()
        {
            if (string.IsNullOrEmpty(mode))
            {
                MetroMessageBox.Show(this, "신규버튼을 누르고 데이터를 저장하십시오.", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string strQuery = "";

                if (mode == "UPDATE")
                {
                    strQuery = " UPDATE dbo.rentaltbl " +
                               "    SET memberIdx = @memberIdx " +
                               "        , bookIdx = @bookIdx " +
                               "        , rentalDate = @rentalDate " +
                               "        , returnDate = @returnDate " +
                               "  WHERE Idx = @Idx ";
                }
                else if (mode == "INSERT")
                {
                    strQuery = "INSERT INTO dbo.rentaltbl " +
                               "            (memberIdx " +
                               "             , bookIdx " +
                               "             , rentalDate " +
                               "             , returnDate) " +
                               "VALUES " +
                               "            (@memberIdx " +
                               "             , @bookIdx " +
                               "             , @rentalDate " +
                               "             , @returnDate) ";
                }
                cmd.CommandText = strQuery;
                if (mode == "UPDATE")
                {
                    SqlParameter parmIdx = new SqlParameter("@Idx", SqlDbType.Int);    // DB에 저장된 스키마를 따라서
                    parmIdx.Value = TxtIdx.Text;
                    cmd.Parameters.Add(parmIdx);
                }
                SqlParameter parmMember = new SqlParameter("@memberIdx", SqlDbType.Int);    // DB에 저장된 스키마를 따라서
                parmMember.Value = CboMember.SelectedValue;
                cmd.Parameters.Add(parmMember);
                SqlParameter parmBook = new SqlParameter("@bookIdx", SqlDbType.Int);    // DB에 저장된 스키마를 따라서
                parmBook.Value = CboBook.SelectedValue;
                cmd.Parameters.Add(parmBook);
                SqlParameter parmRental = new SqlParameter("@rentalDate", SqlDbType.Date);    // DB에 저장된 스키마를 따라서
                parmRental.Value = DtpRentalDate.Text;
                cmd.Parameters.Add(parmRental);
                SqlParameter parmReturn = new SqlParameter("@returnDate", SqlDbType.Date);    // DB에 저장된 스키마를 따라서
                parmReturn.Value = DtpReturnDate.Text;
                cmd.Parameters.Add(parmReturn);



                cmd.ExecuteNonQuery();
            }
        }

        private void TxtNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                BtnSave_Click(sender, new EventArgs());
            }



        }

        private void RentalFrom_Load(object sender, EventArgs e)
        {
            DtpFormat();

            // TODO: 이 코드는 데이터를 'bookRentalShopDBDataSet.divtbl' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            UpdateData();       // 데이터그리드에 DB 데이터 로딩하기
            UpdateCboBook();
            UpdateCboMember();
        }

        private void UpdateCboMember()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                string strQuery = "SELECT idx, Names " +
                                   "  FROM membertbl ";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, string> temps = new Dictionary<string, string>();
                while (reader.Read())
                {
                    temps.Add(reader[0].ToString(), reader[0].ToString() + "[" + reader[1].ToString() + "]");
                }

                CboMember.DisplayMember = "Value";
                CboMember.ValueMember = "Key";
                CboMember.DataSource = new BindingSource(temps, null);
                CboMember.SelectedIndex = -1;
            }
        }

        private void UpdateCboBook()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                string strQuery = "SELECT b.idx, b.Names, d.Names " +
                                  "  FROM bookstbl AS b " +
                                  " INNER JOIN divtbl AS d " +
                                  "    ON b.Division = d.Division ";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, string> temps = new Dictionary<string, string>();
                while (reader.Read())
                {
                    temps.Add(reader[0].ToString(), reader[1].ToString() + "[" + reader[2].ToString() + "]");
                }

                CboBook.DisplayMember = "Value";
                CboBook.ValueMember = "Key";
                CboBook.DataSource = new BindingSource(temps, null);
                CboBook.SelectedIndex = -1;

            }
        }

        private void DtpFormat()
        {
            DtpRentalDate.CustomFormat = "yyyy-MM-dd";
            DtpRentalDate.Format = DateTimePickerFormat.Custom;
            DtpReturnDate.CustomFormat = "yyyy-MM-dd";
            DtpReturnDate.Format = DateTimePickerFormat.Custom;
        }


    }
}
