using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BookRentalShop20
{
    public partial class BookForm : MetroForm
    {

        string mode = "";
        public BookForm()
        {
            InitializeComponent();
        }

        private void UpdateData()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                string strQuery = "SELECT Idx ,Author ,d.Names AS 'DivNames', b.Division ,b.Names " +
                                  "       , ReleaseDate, ISBN " +
                                  "       , REPLACE(CONVERT(VARCHAR, CAST(Price AS MONEY), 1), '.00', '') AS Price " +
                                  "  FROM bookstbl AS b " +
                                  " INNER JOIN divtbl AS d " +
                                  "    ON b.Division = d.Division ";
                //SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "bookstbl");
                GrdBooksTbl.DataSource = ds;
                GrdBooksTbl.DataMember = "bookstbl";


            }
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow data = GrdBooksTbl.Rows[e.RowIndex];
                TxtIdx.Text = data.Cells[0].Value.ToString();
                TxtAuthor.Text = data.Cells[1].Value.ToString();
                TxtIdx.ReadOnly = true;
                TxtIdx.BackColor = MetroColors.Green;
                // Division Names으로 값을 찾아오는 방법
                //CboDivision.SelectedIndex = CboDivision.FindString(data.Cells[2].Value.ToString());
                CboDivision.SelectedValue = data.Cells[3].Value; // Division Key로 Value를 넣는 방법
                TxtTitle.Text = data.Cells[4].Value.ToString();

                DtpReleaseDate.CustomFormat = "yyyy-MM-dd";
                DtpReleaseDate.Format = DateTimePickerFormat.Custom;

                DtpReleaseDate.Value = DateTime.Parse(data.Cells[5].Value.ToString());
                TxtISBN.Text = data.Cells[6].Value.ToString();
                TxtPrice.Text = data.Cells[7].Value.ToString();
                mode = "UPDATE";    // 수정은 UPDATE

            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearTextControls();

            mode = "INSERT";    // 신규는 INSERT
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtPrice.Text) || string.IsNullOrEmpty(TxtAuthor.Text) || string.IsNullOrEmpty(TxtISBN.Text)
                || string.IsNullOrEmpty(TxtTitle.Text))
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

        }

        private void ClearTextControls()
        {
            TxtIdx.Text = TxtAuthor.Text = TxtTitle.Text = TxtISBN.Text = TxtPrice.Text = string.Empty;
            TxtIdx.ReadOnly = true;
            CboDivision.SelectedIndex = -1;

            DtpReleaseDate.CustomFormat = " ";
            DtpReleaseDate.Format = DateTimePickerFormat.Custom;
            TxtIdx.Focus();
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
                    strQuery = " UPDATE dbo.bookstbl " +
                               "    SET Author = @Author " +
                               "      , Division = @Division " +
                               "      , Names = @Names " +
                               "      , ReleaseDate = @ReleaseDate " +
                               "      , ISBN = @ISBN " +
                               "      , Price = @Price " +
                               "  WHERE Idx = @Idx ";
                }
                else if (mode == "INSERT")
                {
                    strQuery = "INSERT INTO dbo.bookstbl "+
                               "                 (Author "+
                               "                , Division "+
                               "                , Names "+
                               "                , ReleaseDate "+
                               "                , ISBN "+
                               "                , Price) "+
                               "     VALUES "+
                               "                (@Author "+
                               "               , @Division "+
                               "               , @Names "+
                               "               , @ReleaseDate "+
                               "               , @ISBN "+
                               "               , @Price) ";
                }
                cmd.CommandText = strQuery;
                if (mode == "UPDATE")
                {
                    SqlParameter parmIdx = new SqlParameter("@Idx", SqlDbType.Int);    // DB에 저장된 스키마를 따라서
                    parmIdx.Value = TxtIdx.Text;
                    cmd.Parameters.Add(parmIdx);
                }

                SqlParameter parmAuthor = new SqlParameter("@Author", SqlDbType.VarChar, 45);    // DB에 저장된 스키마를 따라서
                parmAuthor.Value = TxtAuthor.Text;
                cmd.Parameters.Add(parmAuthor);

                SqlParameter parmDivision = new SqlParameter("@Division", SqlDbType.Char, 4);    // DB에 저장된 스키마를 따라서
                parmDivision.Value = CboDivision.SelectedValue;
                cmd.Parameters.Add(parmDivision);

                SqlParameter parmNames = new SqlParameter("@Names", SqlDbType.VarChar, 100);    // DB에 저장된 스키마를 따라서
                parmNames.Value = TxtTitle.Text;
                cmd.Parameters.Add(parmNames);

                SqlParameter parmReleaseDate = new SqlParameter("@ReleaseDate", SqlDbType.Date);    // DB에 저장된 스키마를 따라서
                parmReleaseDate.Value = DtpReleaseDate.Text;
                cmd.Parameters.Add(parmReleaseDate);

                SqlParameter parmISBN = new SqlParameter("@ISBN", SqlDbType.VarChar, 200);    // DB에 저장된 스키마를 따라서
                parmISBN.Value = TxtISBN.Text;
                cmd.Parameters.Add(parmISBN);

                SqlParameter parmPrice = new SqlParameter("@Price", SqlDbType.Decimal, 10);    // DB에 저장된 스키마를 따라서
                parmPrice.Value = TxtPrice.Text;
                cmd.Parameters.Add(parmPrice);

                cmd.ExecuteNonQuery();
            }
        }

        private void TxtNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                BtnSave_Click(sender, new EventArgs());
            }
            else if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                MetroMessageBox.Show(this, "가격은 숫자만 입력이 가능합니다.", "오류",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtIdx.Text))
            {
                MetroMessageBox.Show(this, "빈값은 삭제할 수 없습니다.", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DeleteProcess();
            UpdateData();
            ClearTextControls();
        }

        private void DeleteProcess()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM bookstbl " +
                                  " WHERE Idx = @Idx";
                SqlParameter parmIdx = new SqlParameter("@Idx", SqlDbType.Int);    // DB에 저장된 스키마를 따라서
                parmIdx.Value = TxtIdx.Text;
                cmd.Parameters.Add(parmIdx);

                cmd.ExecuteNonQuery();
            }
        }

        private void MemberFrom_Load(object sender, EventArgs e)
        {
            DtpReleaseDate.CustomFormat = "yyyy-MM-dd";
            DtpReleaseDate.Format = DateTimePickerFormat.Custom;
            // TODO: 이 코드는 데이터를 'bookRentalShopDBDataSet.divtbl' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            UpdateData();       // 데이터그리드에 DB 데이터 로딩하기
            UpdateCboDivision();
        }

        private void UpdateCboDivision()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                string strQuery = @"SELECT Division, Names
                                      FROM divtbl";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, string> temps = new Dictionary<string, string>();
                while (reader.Read())
                {
                    temps.Add(reader[0].ToString(), reader[1].ToString());
                }

                CboDivision.DataSource = new BindingSource(temps, null);
                CboDivision.DisplayMember = "Value";
                CboDivision.ValueMember = "Key";
                CboDivision.SelectedIndex = -1;
            }
        }

        private void DtpReleaseDate_ValueChanged(object sender, EventArgs e)
        {
            DtpReleaseDate.CustomFormat = "yyyy-MM-dd";
            DtpReleaseDate.Format = DateTimePickerFormat.Custom;

        }

        private void TxtPrice_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtPrice.Text))
                return;
            try
            {
                string lgsText;
                lgsText = TxtPrice.Text.Replace(",", ""); //** 숫자변환시 콤마로 발생하는 에러방지...
                TxtPrice.Text = String.Format("{0:#,###}", Convert.ToDecimal(lgsText));
                TxtPrice.SelectionStart = TxtPrice.Text.Length; //** 캐럿을 맨 뒤로 보낸다...
                TxtPrice.SelectionLength = 0;

            }
            catch (OverflowException)
            {

               // decimal 범위 초과 오류
            }
        }
    }
}
