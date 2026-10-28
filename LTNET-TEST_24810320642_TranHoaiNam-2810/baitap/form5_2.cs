using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Dùng thư viện SQL mới nhất để hết cảnh báo Obsolete

namespace bai1
{
    public partial class Form5_2 : Form
    {
        private readonly string connectionString = @"Data Source=namem;Initial Catalog=QuanLySinhVien;Integrated Security=True;TrustServerCertificate=True";

        public Form5_2()
        {
            InitializeComponent();
            SetupControls();
            LoadCoursesToComboBox();
        }

        private void SetupControls()
        {
            mtxtPhone.Mask = "(000) 000-0000";
            dtpBirthDate.Format = DateTimePickerFormat.Custom;
            dtpBirthDate.CustomFormat = "dd/MM/yyyy";
        }

        private void LoadCoursesToComboBox()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CourseID, CourseName FROM Courses";
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);

                        cboCourse.DataSource = dt;
                        cboCourse.DisplayMember = "CourseName";
                        cboCourse.ValueMember = "CourseID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách khóa học: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ProcessRegister()
        {
            string fullName = txtFullName.Text.Trim();
            string phone = mtxtPhone.Text;
            string birthDate = dtpBirthDate.Value.ToString("dd/MM/yyyy");
            string gender = rdoMale.Checked ? "Nam" : (rdoFemale.Checked ? "Nữ" : "Khác");

            if (cboCourse.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khóa học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int courseId = (int)cboCourse.SelectedValue;
            string courseName = cboCourse.Text;

            if (!chkAgree.Checked)
            {
                MessageBox.Show("Bạn cần đồng ý với điều khoản đăng ký!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO Registrations (FullName, Phone, BirthDate, Gender, CourseID, IsAgreed) 
                                         VALUES (@FullName, @Phone, @BirthDate, @Gender, @CourseID, @IsAgreed)";

                    using (var cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@BirthDate", dtpBirthDate.Value);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@CourseID", courseId);
                        cmd.Parameters.AddWithValue("@IsAgreed", chkAgree.Checked);

                        cmd.ExecuteNonQuery();
                    }

                    string summaryInfo = $"--- XÁC NHẬN ĐĂNG KÝ THÀNH CÔNG ---\n\n" +
                                         $"Họ và tên: {fullName}\n" +
                                         $"Số điện thoại: {phone}\n" +
                                         $"Ngày sinh: {birthDate}\n" +
                                         $"Giới tính: {gender}\n" +
                                         $"Khóa học: {courseName}\n\n" +
                                         $"Trạng thái: Đã lưu dữ liệu vào CSDL SQL Server!";

                    MessageBox.Show(summaryInfo, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            ProcessRegister();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Sự kiện đăng nhập nếu có
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Sự kiện hiện ẩn mật khẩu nếu có
        }
    }
}