
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bai52_TinhTienDichVu
{
    public partial class Form1 : Form
    {
        ComboBox cboCategory = new ComboBox();
        ListBox lstAvailableServices = new ListBox();
        ListBox lstSelectedServices = new ListBox();

        Button btnSelect = new Button();
        Button btnRemove = new Button();
        Button btnClearAll = new Button();

        Label lblSubtotal = new Label();
        Label lblDiscount = new Label();
        Label lblTotal = new Label();

        NumericUpDown nudDiscount = new NumericUpDown();

        Dictionary<string, List<Service>> services =
            new Dictionary<string, List<Service>>();

        public Form1()
        {
            InitializeComponent();

            Text = "Bài 5.2 - Tính tiền dịch vụ";
            Size = new Size(850, 570);
            StartPosition = FormStartPosition.CenterScreen;

            TaoDuLieu();
            TaoGiaoDien();
            LoadDichVu();
            CapNhatTien();
        }

        class Service
        {
            public string Name { get; set; }
            public decimal Price { get; set; }

            public Service(string name, decimal price)
            {
                Name = name;
                Price = price;
            }

            public override string ToString()
            {
                return Name + " - " + Price.ToString("N0") + " VNĐ";
            }
        }

        void TaoDuLieu()
        {
            services["Khám bệnh"] = new List<Service>
            {
                new Service("Khám tổng quát", 200000),
                new Service("Khám chuyên khoa", 300000),
                new Service("Khám sức khỏe", 150000)
            };

            services["Xét nghiệm"] = new List<Service>
            {
                new Service("Xét nghiệm máu", 120000),
                new Service("Xét nghiệm nước tiểu", 80000),
                new Service("Xét nghiệm đường huyết", 100000)
            };

            services["Chụp X-Quang"] = new List<Service>
            {
                new Service("X-Quang ngực", 180000),
                new Service("X-Quang tay", 100000),
                new Service("X-Quang chân", 120000)
            };

            services["Vắc-xin"] = new List<Service>
            {
                new Service("Vắc-xin cúm", 350000),
                new Service("Vắc-xin viêm gan B", 250000),
                new Service("Vắc-xin uốn ván", 200000)
            };
        }

        void TaoGiaoDien()
        {
            Label title = new Label
            {
                Text = "BẢNG TÍNH TIỀN DỊCH VỤ",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(250, 15)
            };
            Controls.Add(title);

            Label lblCategory = new Label
            {
                Text = "Loại dịch vụ:",
                AutoSize = true,
                Location = new Point(30, 65)
            };
            Controls.Add(lblCategory);

            cboCategory.SetBounds(130, 60, 250, 30);
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Items.AddRange(new object[]
            {
                "Khám bệnh",
                "Xét nghiệm",
                "Chụp X-Quang",
                "Vắc-xin"
            });
            cboCategory.SelectedIndexChanged += (s, e) => LoadDichVu();
            Controls.Add(cboCategory);
            cboCategory.SelectedIndex = 0;

            Label lblAvailable = new Label
            {
                Text = "Dịch vụ có sẵn",
                AutoSize = true,
                Location = new Point(30, 110)
            };
            Controls.Add(lblAvailable);

            Label lblSelected = new Label
            {
                Text = "Dịch vụ đã chọn",
                AutoSize = true,
                Location = new Point(490, 110)
            };
            Controls.Add(lblSelected);

            lstAvailableServices.SetBounds(30, 135, 330, 230);
            lstAvailableServices.DoubleClick += (s, e) => ChonDichVu();
            Controls.Add(lstAvailableServices);

            lstSelectedServices.SetBounds(490, 135, 320, 230);
            Controls.Add(lstSelectedServices);

            btnSelect.Text = ">";
            btnSelect.SetBounds(385, 175, 75, 35);
            btnSelect.Click += (s, e) => ChonDichVu();
            Controls.Add(btnSelect);

            btnRemove.Text = "<";
            btnRemove.SetBounds(385, 225, 75, 35);
            btnRemove.Click += (s, e) => XoaDichVu();
            Controls.Add(btnRemove);

            btnClearAll.Text = "<<";
            btnClearAll.SetBounds(385, 275, 75, 35);
            btnClearAll.Click += (s, e) =>
            {
                lstSelectedServices.Items.Clear();
                CapNhatTien();
            };
            Controls.Add(btnClearAll);

            GroupBox grpMoney = new GroupBox
            {
                Text = "Thông tin thanh toán",
                Location = new Point(30, 385),
                Size = new Size(780, 115)
            };
            Controls.Add(grpMoney);

            Label lbl1 = new Label
            {
                Text = "Tổng tiền chưa giảm:",
                AutoSize = true,
                Location = new Point(15, 30)
            };
            grpMoney.Controls.Add(lbl1);

            lblSubtotal.SetBounds(155, 30, 170, 25);
            grpMoney.Controls.Add(lblSubtotal);

            Label lbl2 = new Label
            {
                Text = "Chiết khấu (%):",
                AutoSize = true,
                Location = new Point(350, 30)
            };
            grpMoney.Controls.Add(lbl2);

            nudDiscount.SetBounds(470, 25, 100, 30);
            nudDiscount.Minimum = 0;
            nudDiscount.Maximum = 100;
            nudDiscount.ValueChanged += (s, e) => CapNhatTien();
            grpMoney.Controls.Add(nudDiscount);

            Label lbl3 = new Label
            {
                Text = "Thành tiền:",
                AutoSize = true,
                Location = new Point(15, 70)
            };
            grpMoney.Controls.Add(lbl3);

            lblTotal.SetBounds(155, 70, 250, 25);
            lblTotal.Font = new Font("Arial", 11, FontStyle.Bold);
            grpMoney.Controls.Add(lblTotal);

            lblDiscount.SetBounds(590, 30, 170, 25);
            grpMoney.Controls.Add(lblDiscount);
        }

        void LoadDichVu()
        {
            if (cboCategory.SelectedItem == null)
                return;

            string category = cboCategory.SelectedItem.ToString();

            lstAvailableServices.Items.Clear();

            foreach (Service service in services[category])
            {
                lstAvailableServices.Items.Add(service);
            }
        }

        void ChonDichVu()
        {
            if (lstAvailableServices.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ!");
                return;
            }

            Service service = (Service)lstAvailableServices.SelectedItem;
            lstSelectedServices.Items.Add(service);

            CapNhatTien();
        }

        void XoaDichVu()
        {
            if (lstSelectedServices.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!");
                return;
            }

            lstSelectedServices.Items.Remove(
                lstSelectedServices.SelectedItem);

            CapNhatTien();
        }

        void CapNhatTien()
        {
            decimal subtotal = 0;

            foreach (Service service in lstSelectedServices.Items)
            {
                subtotal += service.Price;
            }

            decimal discount = subtotal * nudDiscount.Value / 100;
            decimal total = subtotal - discount;

            lblSubtotal.Text = subtotal.ToString("N0") + " VNĐ";
            lblDiscount.Text = "Giảm: " + discount.ToString("N0") + " VNĐ";
            lblTotal.Text = total.ToString("N0") + " VNĐ";
        }
    }
}
