namespace MiniSupermarket.WinForms
{
    partial class FormProductManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtKeyword = new TextBox();
            btnSearch = new Button();
            btnLoad = new Button();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            grpProductList1 = new GroupBox();
            dgvProducts = new DataGridView();
            grpProductList2 = new GroupBox();
            cboCategory = new ComboBox();
            numStock = new NumericUpDown();
            numPrice = new NumericUpDown();
            lblCategory = new Label();
            lblStock = new Label();
            lblPrice = new Label();
            txtProductName = new TextBox();
            txtBarcode = new TextBox();
            txtProductId = new TextBox();
            lblProductName = new Label();
            lblBarcode = new Label();
            lblProductId = new Label();
            grpProductList1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            grpProductList2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            SuspendLayout();
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(10, 55);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.PlaceholderText = "Nhập tên sản phẩm hoặc mã vạch...";
            txtKeyword.Size = new Size(637, 30);
            txtKeyword.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.SteelBlue;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(653, 53);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(110, 32);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.SeaGreen;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.ForeColor = Color.White;
            btnLoad.Location = new Point(769, 53);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(110, 32);
            btnLoad.TabIndex = 3;
            btnLoad.Text = "Tải lại";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.SeaGreen;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(359, 644);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(110, 38);
            btnAdd.TabIndex = 16;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += button1_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.SteelBlue;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(489, 644);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(110, 38);
            btnUpdate.TabIndex = 17;
            btnUpdate.Text = "Sửa";
            btnUpdate.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.IndianRed;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(619, 644);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(110, 38);
            btnDelete.TabIndex = 18;
            btnDelete.Text = " Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // grpProductList1
            // 
            grpProductList1.Controls.Add(dgvProducts);
            grpProductList1.Location = new Point(10, 91);
            grpProductList1.Name = "grpProductList1";
            grpProductList1.Size = new Size(1060, 323);
            grpProductList1.TabIndex = 19;
            grpProductList1.TabStop = false;
            grpProductList1.Text = "Danh sách sản phẩm";
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(10, 29);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowHeadersWidth = 51;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(1040, 278);
            dgvProducts.TabIndex = 1;
            // 
            // grpProductList2
            // 
            grpProductList2.Controls.Add(cboCategory);
            grpProductList2.Controls.Add(numStock);
            grpProductList2.Controls.Add(numPrice);
            grpProductList2.Controls.Add(lblCategory);
            grpProductList2.Controls.Add(lblStock);
            grpProductList2.Controls.Add(lblPrice);
            grpProductList2.Controls.Add(txtProductName);
            grpProductList2.Controls.Add(txtBarcode);
            grpProductList2.Controls.Add(txtProductId);
            grpProductList2.Controls.Add(lblProductName);
            grpProductList2.Controls.Add(lblBarcode);
            grpProductList2.Controls.Add(lblProductId);
            grpProductList2.Location = new Point(9, 420);
            grpProductList2.Name = "grpProductList2";
            grpProductList2.Size = new Size(1061, 218);
            grpProductList2.TabIndex = 20;
            grpProductList2.TabStop = false;
            grpProductList2.Text = "Thông tin sản phẩm";
            // 
            // cboCategory
            // 
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.FormattingEnabled = true;
            cboCategory.Location = new Point(690, 144);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new Size(330, 31);
            cboCategory.TabIndex = 27;
            // 
            // numStock
            // 
            numStock.Location = new Point(160, 144);
            numStock.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numStock.Name = "numStock";
            numStock.Size = new Size(330, 30);
            numStock.TabIndex = 26;
            numStock.ThousandsSeparator = true;
            // 
            // numPrice
            // 
            numPrice.DecimalPlaces = 2;
            numPrice.Location = new Point(690, 94);
            numPrice.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(330, 30);
            numPrice.TabIndex = 25;
            numPrice.ThousandsSeparator = true;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(560, 149);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(106, 23);
            lblCategory.TabIndex = 24;
            lblCategory.Text = "Nhóm hàng:";
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(30, 149);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(113, 23);
            lblStock.TabIndex = 23;
            lblStock.Text = "Số lượng tồn:";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(560, 99);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(73, 23);
            lblPrice.TabIndex = 22;
            lblPrice.Text = "Giá bán:";
            // 
            // txtProductName
            // 
            txtProductName.Location = new Point(160, 94);
            txtProductName.Name = "txtProductName";
            txtProductName.Size = new Size(330, 30);
            txtProductName.TabIndex = 21;
            // 
            // txtBarcode
            // 
            txtBarcode.Location = new Point(690, 44);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(330, 30);
            txtBarcode.TabIndex = 20;
            // 
            // txtProductId
            // 
            txtProductId.Location = new Point(160, 44);
            txtProductId.Name = "txtProductId";
            txtProductId.ReadOnly = true;
            txtProductId.Size = new Size(330, 30);
            txtProductId.TabIndex = 19;
            // 
            // lblProductName
            // 
            lblProductName.AutoSize = true;
            lblProductName.Location = new Point(30, 99);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(120, 23);
            lblProductName.TabIndex = 18;
            lblProductName.Text = "Tên sản phẩm:";
            // 
            // lblBarcode
            // 
            lblBarcode.AutoSize = true;
            lblBarcode.Location = new Point(560, 49);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new Size(78, 23);
            lblBarcode.TabIndex = 17;
            lblBarcode.Text = "Mã vạch:";
            // 
            // lblProductId
            // 
            lblProductId.AutoSize = true;
            lblProductId.Location = new Point(30, 49);
            lblProductId.Name = "lblProductId";
            lblProductId.Size = new Size(118, 23);
            lblProductId.TabIndex = 16;
            lblProductId.Text = "Mã sản phẩm:";
            // 
            // FormProductManagement
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.White;
            ClientSize = new Size(1436, 751);
            Controls.Add(grpProductList2);
            Controls.Add(grpProductList1);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(btnLoad);
            Controls.Add(btnSearch);
            Controls.Add(txtKeyword);
            Font = new Font("Segoe UI", 10F);
            Name = "FormProductManagement";
            Text = "Quản lý Sản phẩm";
            Load += FormProductManagement_Load;
            grpProductList1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            grpProductList2.ResumeLayout(false);
            grpProductList2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtKeyword;
        private Button btnSearch;
        private Button btnLoad;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private GroupBox grpProductList1;
        private DataGridView dgvProducts;
        private GroupBox grpProductList2;
        private ComboBox cboCategory;
        private NumericUpDown numStock;
        private NumericUpDown numPrice;
        private Label lblCategory;
        private Label lblStock;
        private Label lblPrice;
        private TextBox txtProductName;
        private TextBox txtBarcode;
        private TextBox txtProductId;
        private Label lblProductName;
        private Label lblBarcode;
        private Label lblProductId;
    }
}