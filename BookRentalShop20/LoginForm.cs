using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BookRentalShop20
{
    public partial class LoginForm : MetroForm
    {

        public LoginForm()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            //Application.Exit();
            if (MetroMessageBox.Show(this, "종료하시겠습니까?", "확인",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Environment.Exit(0);
            }

        }
        /// <summary>
        /// 로그인 처리버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            LoginProcess();
        }

        private void TextBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)            // 엔터 
            {
                TextBoxPassword.Focus();
            }
        }

        private void TextBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)            // 엔터 
            {
                LoginProcess();
            }
        }

        private void LoginProcess()
        {
            if (String.IsNullOrEmpty(TextBoxID.Text) ||
                String.IsNullOrEmpty(TextBoxPassword.Text))
            {
                MetroMessageBox.Show(this, "아이디 / 패스워드를 입력하세요!", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string strUserId = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT userID FROM userTbl
                                     WHERE userID = @userID
                                       AND password = @password";   // SELECT
                    SqlParameter parmUserId = new SqlParameter("@userID", SqlDbType.VarChar, 12);    // DB에 저장된 스키마를 따라서
                    parmUserId.Value = TextBoxID.Text;
                    cmd.Parameters.Add(parmUserId);

                    SqlParameter parmPassword = new SqlParameter("@password", SqlDbType.VarChar, 20);    // DB에 저장된 스키마를 따라서
                    parmPassword.Value = TextBoxPassword.Text;
                    cmd.Parameters.Add(parmPassword);
                    // SqlCommand.ExecuteNonQuery 반환값이 없을 때
                    // SqlCommand.ExecuteReader   테이블을 반환
                    // SqlCommand.ExecuteScalar    하나의 값만 반환받을 때
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    strUserId = reader["userID"] != null ? reader["userID"].ToString() : "";

                    if (strUserId != "")
                    {
                        Commons.LOGINUSERID = strUserId;
                        MetroMessageBox.Show(this, "접속성공", "로그인성공");
                        this.Close();
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "접속실패", "로그인실패",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    //Debug.WriteLine("On the Debug");
                }

            }
            catch (Exception ex)
            {

                MetroMessageBox.Show(this, $"Error : {ex.StackTrace}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
