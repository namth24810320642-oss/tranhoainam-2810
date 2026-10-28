using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace bai1
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối SQL Server
        private readonly string connectionString = @"Data Source=namem;Initial Catalog=QuanLySinhVien;Integrated Security=True;TrustServerCertificate=True";

        public Form1()
        {
            InitializeComponent();
            SetupFormDefaults();
        }

        private void SetupFormDefaults()
        {
            // Cấu hình phím Enter / Esc và ẩn mật khẩu
            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;
            txtPassword.UseSystemPasswordChar = true;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Ẩn / Hiện mật khẩu
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool isValid = true;

            // Lấy dữ liệu và tự động xóa khoảng trắng thừa 2 đầu
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validation kiểm tra trống
            if (string.IsNullOrWhiteSpace(username))
            {
                errorProvider.SetError(txtUsername, "Tên đăng nhập không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorProvider.SetError(txtPassword, "Mật khẩu không được để trống!");
                isValid = false;
            }

            if (!isValid) return;

            // Kiểm tra CSDL SQL Server
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Dùng LOWER() và RTRIM() để tránh lệch chữ hoa/thường hoặc khoảng trắng trong DB
                    string query = "SELECT COUNT(*) FROM Users WHERE LOWER(RTRIM(Username)) = LOWER(@User) AND RTRIM(Password) = @Pass";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@User", username);
                        cmd.Parameters.AddWithValue("@Pass", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Mở Form Bài 5.2 (Form Đăng ký khóa học)
                            Form5_2 formRegistration = new Form5_2();
                            formRegistration.Show();

                            // Ẩn Form Đăng nhập hiện tại đi
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}