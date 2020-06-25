using MetroFramework;
using MetroFramework.Forms;
using System.Windows.Forms;

namespace BookRentalShop20
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {


            if (MetroMessageBox.Show(this, "정말 종료하시겠습니까?", "종료",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (Form item in this.MdiChildren)
                {
                    item.Close();
                }
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void MnuItemDivMng_Click(object sender, System.EventArgs e)
        {
            DivForm form = new DivForm();
            InitChildForm(form, "구분코드 관리");
        }

        private void MnuItemUserMng_Click(object sender, System.EventArgs e)
        {
            UserForm form = new UserForm();
            InitChildForm(form, "사용자 관리");
        }

        private void InitChildForm(Form form, string strFormTitle)
        {
            form.Text = strFormTitle;
            form.Dock = DockStyle.Fill;
            form.MdiParent = this;
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void 회원관리MToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            MemberFrom form = new MemberFrom();
            InitChildForm(form, "회원관리");
        }

        private void 책관리BToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            BookForm form = new BookForm();
            InitChildForm(form, "책관리");
        }

        private void MainForm_Activated(object sender, System.EventArgs e)
        {
            LblUserID.Text = Commons.LOGINUSERID;
        }

        private void 책대여관리RToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            RentalForm form = new RentalForm();
            InitChildForm(form, "책 대여관리");
        }
    }
}
