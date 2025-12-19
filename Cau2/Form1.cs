using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cau2
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:52098/api/") };
        public Form1()
        {
            InitializeComponent();
            load();
        }

        public void load()
        {
            dgvNhanVien.DataSource = GetNhanViens().ToList();
            dgvNhanVien.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dgvNhanVien.Columns["HoTen"].HeaderText = "Họ tên";
            dgvNhanVien.Columns["TrinhDo"].HeaderText = "Trình độ";
            dgvNhanVien.Columns["Luong"].HeaderText = "Lương";
        }
        public List<NhanVien> GetNhanViens()
        {
            var response = client.GetAsync("NhanVien").Result;
            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<List<NhanVien>>().Result : null;
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if(index >= 0)
            {
                txtMaNV.Text = dgvNhanVien.Rows[index].Cells[0].Value.ToString();
                txtHoTen.Text = dgvNhanVien.Rows[index].Cells[1].Value.ToString();
                txtTrinhDo.Text = dgvNhanVien.Rows[index].Cells[2].Value.ToString();
                txtLuong.Text = dgvNhanVien.Rows[index].Cells[3].Value.ToString();
            }
        }

        public NhanVien GetNhanVien()
        {
            return new NhanVien() { MaNV = int.Parse(txtMaNV.Text), HoTen = txtHoTen.Text, TrinhDo = txtTrinhDo.Text, Luong = double.Parse(txtLuong.Text) };
        }

        public bool Add(NhanVien nv)
        {
            var response = client.PostAsJsonAsync("NhanVien", nv).Result;
            return response.IsSuccessStatusCode;
        }

        public bool Edit(NhanVien nv)
        {
            var response = client.PutAsJsonAsync("NhanVien", nv).Result;
            return response.IsSuccessStatusCode;
        }
        public bool Delete(string ma)
        {
            var response = client.DeleteAsync("NhanVien?ma=" + ma).Result;
            return response.IsSuccessStatusCode;
        }

        public NhanVien SearchNV(string ma)
        {
            var response = client.GetAsync("NhanVien?ma=" + ma).Result;
            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<NhanVien>().Result : null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if(!Add(GetNhanVien()))
            {
                MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            load();
            MessageBox.Show("Thêm thành công!");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!Edit(GetNhanVien()))
            {
                MessageBox.Show("Sửa nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            load();
            MessageBox.Show("Sửa thành công!");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn chắc chắn muốn xóa nhân viên này không !", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            if(!Delete(txtMaNV.Text))
            {
                MessageBox.Show("Xoa nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            load();
            MessageBox.Show("Xoa thành công!");
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            List<NhanVien> nhanViens = new List<NhanVien>();
            nhanViens.Add(SearchNV(txtMaNV.Text));
            dgvNhanVien.DataSource = nhanViens.ToList();
            if(SearchNV(txtMaNV.Text) == null)
            {
                MessageBox.Show("Khong tim thay nhan vien!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
