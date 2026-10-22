
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    partial class FormCustomerManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            grpCustomerList = new GroupBox();
            dgvCustomers = new DataGridView();
            grpCustomerInfo = new GroupBox();
            lblCustomerId = new Label();
            txtCustomerId = new TextBox();
            lblCustomerName = new Label();
            txtCustomerName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            grpCustomerSearch = new GroupBox();
            txtKeyword = new TextBox();
            btnSearch = new Button();
            btnLoad = new Button();
            grpCustomerList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
            grpCustomerInfo.SuspendLayout();
            grpCustomerSearch.SuspendLayout();
            SuspendLayout();
            // 
            // grpCustomerList
            // 
            grpCustomerList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpCustomerList.Controls.Add(dgvCustomers);
            grpCustomerList.Location = new Point(20, 110);
            grpCustomerList.Name = "grpCustomerList";
            grpCustomerList.Size = new Size(928, 530);
            grpCustomerList.TabIndex = 0;
            grpCustomerList.TabStop = false;
            grpCustomerList.Text = "Danh sách Khách hàng";
            // 
            // dgvCustomers
            // 
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCustomers.Location = new Point(10, 30);
            dgvCustomers.MultiSelect = false;
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.ReadOnly = true;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.RowHeadersWidth = 51;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.Size = new Size(908, 485);
            dgvCustomers.TabIndex = 0;
            // 
            // grpCustomerInfo
            // 
            grpCustomerInfo.Controls.Add(lblCustomerId);
            grpCustomerInfo.Controls.Add(txtCustomerId);
            grpCustomerInfo.Controls.Add(lblCustomerName);
            grpCustomerInfo.Controls.Add(txtCustomerName);
            grpCustomerInfo.Controls.Add(lblPhone);
            grpCustomerInfo.Controls.Add(txtPhone);
            grpCustomerInfo.Controls.Add(lblEmail);
            grpCustomerInfo.Controls.Add(txtEmail);
            grpCustomerInfo.Controls.Add(lblAddress);
            grpCustomerInfo.Controls.Add(txtAddress);
            grpCustomerInfo.Controls.Add(btnAdd);
            grpCustomerInfo.Controls.Add(btnUpdate);
            grpCustomerInfo.Controls.Add(btnDelete);
            grpCustomerInfo.Location = new Point(954, 15);
            grpCustomerInfo.Name = "grpCustomerInfo";
            grpCustomerInfo.Size = new Size(329, 625);
            grpCustomerInfo.TabIndex = 1;
            grpCustomerInfo.TabStop = false;
            grpCustomerInfo.Text = "Thông tin Khách hàng";
            // 
            // lblCustomerId
            // 
            lblCustomerId.AutoSize = true;
            lblCustomerId.BackColor = Color.Transparent;
            lblCustomerId.ForeColor = Color.Black;
            lblCustomerId.Location = new Point(20, 40);
            lblCustomerId.Name = "lblCustomerId";
            lblCustomerId.Size = new Size(128, 23);
            lblCustomerId.TabIndex = 0;
            lblCustomerId.Text = "Mã khách hàng";
            lblCustomerId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCustomerId
            // 
            txtCustomerId.BackColor = Color.WhiteSmoke;
            txtCustomerId.BorderStyle = BorderStyle.FixedSingle;
            txtCustomerId.ForeColor = Color.Black;
            txtCustomerId.Location = new Point(20, 65);
            txtCustomerId.Name = "txtCustomerId";
            txtCustomerId.ReadOnly = true;
            txtCustomerId.Size = new Size(290, 30);
            txtCustomerId.TabIndex = 1;
            // 
            // lblCustomerName
            // 
            lblCustomerName.AutoSize = true;
            lblCustomerName.BackColor = Color.Transparent;
            lblCustomerName.ForeColor = Color.Black;
            lblCustomerName.Location = new Point(20, 120);
            lblCustomerName.Name = "lblCustomerName";
            lblCustomerName.Size = new Size(130, 23);
            lblCustomerName.TabIndex = 2;
            lblCustomerName.Text = "Tên khách hàng";
            lblCustomerName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCustomerName
            // 
            txtCustomerName.BackColor = Color.White;
            txtCustomerName.BorderStyle = BorderStyle.FixedSingle;
            txtCustomerName.ForeColor = Color.Black;
            txtCustomerName.Location = new Point(20, 145);
            txtCustomerName.MaxLength = 100;
            txtCustomerName.Name = "txtCustomerName";
            txtCustomerName.Size = new Size(290, 30);
            txtCustomerName.TabIndex = 3;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.BackColor = Color.Transparent;
            lblPhone.ForeColor = Color.Black;
            lblPhone.Location = new Point(20, 200);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(111, 23);
            lblPhone.TabIndex = 4;
            lblPhone.Text = "Số điện thoại";
            lblPhone.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPhone
            // 
            txtPhone.BackColor = Color.White;
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.ForeColor = Color.Black;
            txtPhone.Location = new Point(20, 225);
            txtPhone.MaxLength = 15;
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(290, 30);
            txtPhone.TabIndex = 5;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.BackColor = Color.Transparent;
            lblEmail.ForeColor = Color.Black;
            lblEmail.Location = new Point(20, 280);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(51, 23);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email";
            lblEmail.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.ForeColor = Color.Black;
            txtEmail.Location = new Point(20, 305);
            txtEmail.MaxLength = 100;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(290, 30);
            txtEmail.TabIndex = 7;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.BackColor = Color.Transparent;
            lblAddress.ForeColor = Color.Black;
            lblAddress.Location = new Point(20, 360);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(62, 23);
            lblAddress.TabIndex = 8;
            lblAddress.Text = "Địa chỉ";
            lblAddress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtAddress
            // 
            txtAddress.BackColor = Color.White;
            txtAddress.BorderStyle = BorderStyle.FixedSingle;
            txtAddress.ForeColor = Color.Black;
            txtAddress.Location = new Point(20, 385);
            txtAddress.MaxLength = 500;
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(290, 30);
            txtAddress.TabIndex = 9;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.SeaGreen;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(6, 455);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 40);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.SteelBlue;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(112, 455);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(106, 40);
            btnUpdate.TabIndex = 11;
            btnUpdate.Text = "Cập nhật";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += button3_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.IndianRed;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(224, 455);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 40);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // grpCustomerSearch
            // 
            grpCustomerSearch.Controls.Add(txtKeyword);
            grpCustomerSearch.Controls.Add(btnSearch);
            grpCustomerSearch.Controls.Add(btnLoad);
            grpCustomerSearch.Location = new Point(20, 15);
            grpCustomerSearch.Name = "grpCustomerSearch";
            grpCustomerSearch.Size = new Size(928, 85);
            grpCustomerSearch.TabIndex = 2;
            grpCustomerSearch.TabStop = false;
            grpCustomerSearch.Text = "Tìm kiếm Khách hàng";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(15, 32);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(644, 30);
            txtKeyword.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.SteelBlue;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(693, 30);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(100, 35);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.SeaGreen;
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLoad.ForeColor = Color.White;
            btnLoad.Location = new Point(809, 30);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(100, 35);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Tải lại";
            btnLoad.UseVisualStyleBackColor = false;
            // 
            // FormCustomerManagement
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1320, 700);
            Controls.Add(grpCustomerSearch);
            Controls.Add(grpCustomerInfo);
            Controls.Add(grpCustomerList);
            Font = new Font("Segoe UI", 10F);
            Name = "FormCustomerManagement";
            Text = "Quản lý Khách hàng";
            Load += FormCustomerManagement_Load;
            grpCustomerList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            grpCustomerInfo.ResumeLayout(false);
            grpCustomerInfo.PerformLayout();
            grpCustomerSearch.ResumeLayout(false);
            grpCustomerSearch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpCustomerList;
        private DataGridView dgvCustomers;

        private GroupBox grpCustomerInfo;

        private Label lblCustomerId;
        private TextBox txtCustomerId;

        private Label lblCustomerName;
        private TextBox txtCustomerName;

        private Label lblPhone;
        private TextBox txtPhone;

        private Label lblEmail;
        private TextBox txtEmail;

        private Label lblAddress;
        private TextBox txtAddress;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;

        private GroupBox grpCustomerSearch;
        private TextBox txtKeyword;
        private Button btnSearch;
        private Button btnLoad;
    }
}
