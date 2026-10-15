namespace MiniSupermarket.WinForms
{
    partial class FormCategoryManagement
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

        private void InitializeComponent()
        {
            dgvCategories = new DataGridView();
            txtKeyword = new TextBox();
            txtDescription = new TextBox();
            txtId = new TextBox();
            txtCategoryName = new TextBox();
            btnSearch = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            btnLoad = new Button();
            grpSearch = new GroupBox();
            grpCategoryList = new GroupBox();
            grpCategoryInfo = new GroupBox();
            lblId = new Label();
            lblCategoryName = new Label();
            lblDescription = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            grpSearch.SuspendLayout();
            grpCategoryList.SuspendLayout();
            grpCategoryInfo.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCategories
            // 
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AllowUserToResizeRows = false;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.BorderStyle = BorderStyle.Fixed3D;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategories.Location = new Point(11, 33);
            dgvCategories.Margin = new Padding(3, 4, 3, 4);
            dgvCategories.MultiSelect = false;
            dgvCategories.Name = "dgvCategories";
            dgvCategories.ReadOnly = true;
            dgvCategories.RowHeadersWidth = 40;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.Size = new Size(858, 407);
            dgvCategories.TabIndex = 0;
            dgvCategories.CellClick += dgvCategories_CellClick;
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(17, 29);
            txtKeyword.Margin = new Padding(3, 4, 3, 4);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.PlaceholderText = "Nhập từ khóa...";
            txtKeyword.Size = new Size(580, 27);
            txtKeyword.TabIndex = 1;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(17, 227);
            txtDescription.Margin = new Padding(3, 4, 3, 4);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(305, 245);
            txtDescription.TabIndex = 5;
            // 
            // txtId
            // 
            txtId.Location = new Point(17, 67);
            txtId.Margin = new Padding(3, 4, 3, 4);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(305, 27);
            txtId.TabIndex = 1;
            // 
            // txtCategoryName
            // 
            txtCategoryName.Location = new Point(17, 147);
            txtCategoryName.Margin = new Padding(3, 4, 3, 4);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(305, 27);
            txtCategoryName.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.SteelBlue;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(603, 28);
            btnSearch.Margin = new Padding(3, 4, 3, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(120, 33);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.IndianRed;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(253, 480);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(69, 40);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.SteelBlue;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(131, 480);
            btnUpdate.Margin = new Padding(3, 4, 3, 4);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(116, 40);
            btnUpdate.TabIndex = 7;
            btnUpdate.Text = "Cập nhật";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.SeaGreen;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(13, 480);
            btnAdd.Margin = new Padding(3, 4, 3, 4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(112, 40);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.SeaGreen;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLoad.ForeColor = Color.White;
            btnLoad.Location = new Point(724, 28);
            btnLoad.Margin = new Padding(3, 4, 3, 4);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(120, 33);
            btnLoad.TabIndex = 3;
            btnLoad.Text = "Tải lại";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
            // 
            // grpSearch
            // 
            grpSearch.Controls.Add(txtKeyword);
            grpSearch.Controls.Add(btnSearch);
            grpSearch.Controls.Add(btnLoad);
            grpSearch.Location = new Point(23, 20);
            grpSearch.Margin = new Padding(3, 4, 3, 4);
            grpSearch.Name = "grpSearch";
            grpSearch.Padding = new Padding(3, 4, 3, 4);
            grpSearch.Size = new Size(880, 80);
            grpSearch.TabIndex = 0;
            grpSearch.TabStop = false;
            grpSearch.Text = "Tìm kiếm";
            // 
            // grpCategoryList
            // 
            grpCategoryList.Controls.Add(dgvCategories);
            grpCategoryList.Location = new Point(23, 113);
            grpCategoryList.Margin = new Padding(3, 4, 3, 4);
            grpCategoryList.Name = "grpCategoryList";
            grpCategoryList.Padding = new Padding(3, 4, 3, 4);
            grpCategoryList.Size = new Size(880, 460);
            grpCategoryList.TabIndex = 4;
            grpCategoryList.TabStop = false;
            grpCategoryList.Text = "Danh sách Nhóm hàng";
            grpCategoryList.Enter += grpCategoryList_Enter;
            // 
            // grpCategoryInfo
            // 
            grpCategoryInfo.Controls.Add(lblId);
            grpCategoryInfo.Controls.Add(txtId);
            grpCategoryInfo.Controls.Add(lblCategoryName);
            grpCategoryInfo.Controls.Add(txtCategoryName);
            grpCategoryInfo.Controls.Add(lblDescription);
            grpCategoryInfo.Controls.Add(txtDescription);
            grpCategoryInfo.Controls.Add(btnAdd);
            grpCategoryInfo.Controls.Add(btnUpdate);
            grpCategoryInfo.Controls.Add(btnDelete);
            grpCategoryInfo.Location = new Point(931, 20);
            grpCategoryInfo.Margin = new Padding(3, 4, 3, 4);
            grpCategoryInfo.Name = "grpCategoryInfo";
            grpCategoryInfo.Padding = new Padding(3, 4, 3, 4);
            grpCategoryInfo.Size = new Size(338, 553);
            grpCategoryInfo.TabIndex = 5;
            grpCategoryInfo.TabStop = false;
            grpCategoryInfo.Text = "Thông tin Nhóm hàng";
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Location = new Point(17, 40);
            lblId.Name = "lblId";
            lblId.Size = new Size(49, 20);
            lblId.TabIndex = 0;
            lblId.Text = "Mã ID";
            // 
            // lblCategoryName
            // 
            lblCategoryName.AutoSize = true;
            lblCategoryName.Location = new Point(17, 120);
            lblCategoryName.Name = "lblCategoryName";
            lblCategoryName.Size = new Size(230, 20);
            lblCategoryName.TabIndex = 2;
            lblCategoryName.Text = "Tên Nhóm hàng (Ví dụ: Bánh kẹo)";
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(17, 200);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(162, 20);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "Mô Tả (Mô tả chi tiết...)";
            // 
            // FormCategoryManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1310, 751);
            Controls.Add(grpSearch);
            Controls.Add(grpCategoryList);
            Controls.Add(grpCategoryInfo);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormCategoryManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Danh mục Nhóm hàng - FormCategoryManagement";
            Load += FormCategoryManagement_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            grpSearch.ResumeLayout(false);
            grpSearch.PerformLayout();
            grpCategoryList.ResumeLayout(false);
            grpCategoryInfo.ResumeLayout(false);
            grpCategoryInfo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // =========================================================
        // DATA GRID VIEW
        // =========================================================

        private DataGridView dgvCategories;

        // =========================================================
        // TEXTBOX
        // =========================================================

        private TextBox txtKeyword;
        private TextBox txtDescription;
        private TextBox txtId;
        private TextBox txtCategoryName;

        // =========================================================
        // BUTTON
        // =========================================================

        private Button btnSearch;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private Button btnLoad;

        // =========================================================
        // GROUPBOX
        // =========================================================

        private GroupBox grpSearch;
        private GroupBox grpCategoryList;
        private GroupBox grpCategoryInfo;

        // =========================================================
        // LABEL
        // =========================================================

        private Label lblId;
        private Label lblCategoryName;
        private Label lblDescription;
    }
}