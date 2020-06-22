using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BookRentalShop20
{
    public partial class UserForm : MetroForm
    {
    
        string mode = "";
        public UserForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'bookRentalShopDBDataSet.divtbl' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            UpdateData();       // 데이터그리드에 DB 데이터 로딩하기

        }
        /// <summary>
        /// 사용자 데이터 가져오기
        /// </summary>
        private void UpdateData()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                string strQuery = "SELECT id ,userID,password,lastLoginDt,loginIpAddr " +
                                  "  FROM dbo.userTbl ";
                //SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "usertbl");

                GrdUserTbl.DataSource = ds;
                GrdUserTbl.DataMember = "usertbl";
            }
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow data = GrdUserTbl.Rows[e.RowIndex];
                TxtID.Text = data.Cells["id"].Value.ToString();
                TxtUserID.Text = data.Cells["userID"].Value.ToString();
                TxtPassword.Text = data.Cells["password"].Value.ToString();
                //TxtUserID.ReadOnly = true;
                TxtUserID.BackColor = MetroColors.Green;

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
            if (string.IsNullOrEmpty(TxtUserID.Text) || string.IsNullOrEmpty(TxtPassword.Text))
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
            TxtID.Text = TxtUserID.Text = TxtPassword.Text = string.Empty;
            TxtUserID.Focus();
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
                    strQuery = " UPDATE dbo.usertbl " +
                               "    SET Password = @Password, " +
                               "        userID = @userID " +
                               "  WHERE id = @id ";
                }
                else if (mode == "INSERT")
                {
                    strQuery = "INSERT INTO dbo.usertbl (userID, password) " +
                               "VALUES (@userID, @Password) ";
                }
                cmd.CommandText = strQuery;
                if (mode == "UPDATE")
                {
                    SqlParameter parmID = new SqlParameter("@id", SqlDbType.Int);
                    parmID.Value = TxtID.Text;
                    cmd.Parameters.Add(parmID);
                }
                SqlParameter parmUserID = new SqlParameter("@userID", SqlDbType.VarChar, 12);    // DB에 저장된 스키마를 따라서
                parmUserID.Value = TxtUserID.Text;
                cmd.Parameters.Add(parmUserID);

                SqlParameter parmPassword = new SqlParameter("@Password", SqlDbType.VarChar, 20);    // DB에 저장된 스키마를 따라서
                parmPassword.Value = TxtPassword.Text;
                cmd.Parameters.Add(parmPassword);

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

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtUserID.Text) || string.IsNullOrEmpty(TxtPassword.Text))
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
                cmd.CommandText = "DELETE FROM dbo.usertbl " +
                                  " WHERE userID = @userID ";
                SqlParameter parmUserID = new SqlParameter("@userID", SqlDbType.VarChar, 12);
                parmUserID.Value = TxtUserID.Text;
                cmd.Parameters.Add(parmUserID);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
