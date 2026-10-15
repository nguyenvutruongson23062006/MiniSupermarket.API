namespace MiniSupermarket.WinForms
{
    partial class FormPOS
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            splitMain = new SplitContainer();
            leftLayout = new TableLayoutPanel();
            barcodePanel = new Panel();
            btnRemoveItem = new Button();
            txtBarcode = new TextBox();
            lblBarcode = new Label();
            dgvCart = new DataGridView();
            paymentPanel = new Panel();
            btnClearCart = new Button();
            btnCheckout = new Button();
            lblChange = new Label();
            lblChangeCaption = new Label();
            txtCashReceived = new TextBox();
            lblCashReceived = new Label();
            lblTotalAmount = new Label();
            lblTotalCaption = new Label();
            lblCustomerName = new Label();
            btnFindCustomer = new Button();
            txtCustomerPhone = new TextBox();
            lblCustomerPhone = new Label();
            lblPaymentTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            leftLayout.SuspendLayout();
            barcodePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            paymentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.FixedPanel = FixedPanel.Panel2;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(leftLayout);
            splitMain.Panel1.Padding = new Padding(12);
            splitMain.Panel1MinSize = 520;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(paymentPanel);
            splitMain.Panel2.Padding = new Padding(12);
            splitMain.Panel2MinSize = 390;
            splitMain.Size = new Size(1050, 660);
            splitMain.SplitterDistance = 680;
            splitMain.SplitterWidth = 6;
            splitMain.TabIndex = 0;
            // 
            // leftLayout
            // 
            leftLayout.ColumnCount = 1;
            leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftLayout.Controls.Add(barcodePanel, 0, 0);
            leftLayout.Controls.Add(dgvCart, 0, 1);
            leftLayout.Dock = DockStyle.Fill;
            leftLayout.Location = new Point(12, 12);
            leftLayout.Name = "leftLayout";
            leftLayout.RowCount = 2;
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            leftLayout.Size = new Size(656, 636);
            leftLayout.TabIndex = 0;
            // 
            // barcodePanel
            // 
            barcodePanel.Controls.Add(btnRemoveItem);
            barcodePanel.Controls.Add(txtBarcode);
            barcodePanel.Controls.Add(lblBarcode);
            barcodePanel.Dock = DockStyle.Fill;
            barcodePanel.Location = new Point(3, 3);
            barcodePanel.Name = "barcodePanel";
            barcodePanel.Size = new Size(650, 64);
            barcodePanel.TabIndex = 0;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemoveItem.BackColor = Color.Firebrick;
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.ForeColor = Color.White;
            btnRemoveItem.Location = new Point(536, 24);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(111, 34);
            btnRemoveItem.TabIndex = 0;
            btnRemoveItem.Text = "Bỏ 1 SP";
            btnRemoveItem.UseVisualStyleBackColor = false;
            // 
            // txtBarcode
            // 
            txtBarcode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBarcode.Font = new Font("Segoe UI", 12F);
            txtBarcode.Location = new Point(0, 24);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "Quét/nhập mã vạch rồi nhấn Enter";
            txtBarcode.Size = new Size(528, 34);
            txtBarcode.TabIndex = 1;
            // 
            // lblBarcode
            // 
            lblBarcode.AutoSize = true;
            lblBarcode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBarcode.Location = new Point(0, 0);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new Size(163, 20);
            lblBarcode.TabIndex = 2;
            lblBarcode.Text = "MÃ VẠCH SẢN PHẨM";
            // 
            // dgvCart
            // 
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.ColumnHeadersHeight = 38;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.Location = new Point(3, 73);
            dgvCart.Name = "dgvCart";
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowHeadersWidth = 51;
            dgvCart.RowTemplate.Height = 34;
            dgvCart.Size = new Size(650, 560);
            dgvCart.TabIndex = 1;
            // 
            // paymentPanel
            // 
            paymentPanel.BackColor = Color.White;
            paymentPanel.Controls.Add(btnClearCart);
            paymentPanel.Controls.Add(btnCheckout);
            paymentPanel.Controls.Add(lblChange);
            paymentPanel.Controls.Add(lblChangeCaption);
            paymentPanel.Controls.Add(txtCashReceived);
            paymentPanel.Controls.Add(lblCashReceived);
            paymentPanel.Controls.Add(lblTotalAmount);
            paymentPanel.Controls.Add(lblTotalCaption);
            paymentPanel.Controls.Add(lblCustomerName);
            paymentPanel.Controls.Add(btnFindCustomer);
            paymentPanel.Controls.Add(txtCustomerPhone);
            paymentPanel.Controls.Add(lblCustomerPhone);
            paymentPanel.Controls.Add(lblPaymentTitle);
            paymentPanel.Dock = DockStyle.Fill;
            paymentPanel.Location = new Point(12, 12);
            paymentPanel.Name = "paymentPanel";
            paymentPanel.Size = new Size(342, 636);
            paymentPanel.TabIndex = 0;
            paymentPanel.Paint += paymentPanel_Paint;
            // 
            // btnClearCart
            // 
            btnClearCart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnClearCart.BackColor = Color.Firebrick;
            btnClearCart.FlatStyle = FlatStyle.Flat;
            btnClearCart.ForeColor = Color.White;
            btnClearCart.Location = new Point(20, 575);
            btnClearCart.Name = "btnClearCart";
            btnClearCart.Size = new Size(302, 42);
            btnClearCart.TabIndex = 0;
            btnClearCart.Text = "HỦY GIỎ HÀNG";
            btnClearCart.UseVisualStyleBackColor = false;
            // 
            // btnCheckout
            // 
            btnCheckout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCheckout.BackColor = Color.SeaGreen;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Location = new Point(20, 520);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(302, 48);
            btnCheckout.TabIndex = 1;
            btnCheckout.Text = "THANH TOÁN (F9)";
            btnCheckout.UseVisualStyleBackColor = false;
            // 
            // lblChange
            // 
            lblChange.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblChange.Location = new Point(20, 400);
            lblChange.Name = "lblChange";
            lblChange.Size = new Size(316, 49);
            lblChange.TabIndex = 2;
            lblChange.Text = "0 đ";
            lblChange.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblChangeCaption
            // 
            lblChangeCaption.AutoSize = true;
            lblChangeCaption.Location = new Point(20, 376);
            lblChangeCaption.Name = "lblChangeCaption";
            lblChangeCaption.Size = new Size(135, 20);
            lblChangeCaption.TabIndex = 3;
            lblChangeCaption.Text = "Tiền thừa trả khách";
            // 
            // txtCashReceived
            // 
            txtCashReceived.Font = new Font("Segoe UI", 13F);
            txtCashReceived.Location = new Point(20, 326);
            txtCashReceived.Name = "txtCashReceived";
            txtCashReceived.Size = new Size(316, 36);
            txtCashReceived.TabIndex = 4;
            txtCashReceived.TextAlign = HorizontalAlignment.Right;
            // 
            // lblCashReceived
            // 
            lblCashReceived.AutoSize = true;
            lblCashReceived.Location = new Point(20, 299);
            lblCashReceived.Name = "lblCashReceived";
            lblCashReceived.Size = new Size(109, 20);
            lblCashReceived.TabIndex = 5;
            lblCashReceived.Text = "Tiền khách đưa";
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalAmount.ForeColor = Color.FromArgb(41, 100, 180);
            lblTotalAmount.Location = new Point(20, 235);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(316, 55);
            lblTotalAmount.TabIndex = 6;
            lblTotalAmount.Text = "0 đ";
            lblTotalAmount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalCaption.Location = new Point(20, 208);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new Size(259, 23);
            lblTotalCaption.TabIndex = 7;
            lblTotalCaption.Text = "TỔNG TIỀN CẦN THANH TOÁN";
            // 
            // lblCustomerName
            // 
            lblCustomerName.AutoEllipsis = true;
            lblCustomerName.ForeColor = Color.DarkGreen;
            lblCustomerName.Location = new Point(20, 169);
            lblCustomerName.Name = "lblCustomerName";
            lblCustomerName.Size = new Size(316, 25);
            lblCustomerName.TabIndex = 8;
            lblCustomerName.Text = "Khách vãng lai";
            // 
            // btnFindCustomer
            // 
            btnFindCustomer.Location = new Point(235, 123);
            btnFindCustomer.Name = "btnFindCustomer";
            btnFindCustomer.Size = new Size(101, 34);
            btnFindCustomer.TabIndex = 9;
            btnFindCustomer.Text = "Tìm KH";
            // 
            // txtCustomerPhone
            // 
            txtCustomerPhone.Location = new Point(20, 123);
            txtCustomerPhone.Name = "txtCustomerPhone";
            txtCustomerPhone.Size = new Size(209, 27);
            txtCustomerPhone.TabIndex = 10;
            // 
            // lblCustomerPhone
            // 
            lblCustomerPhone.AutoSize = true;
            lblCustomerPhone.Location = new Point(20, 96);
            lblCustomerPhone.Name = "lblCustomerPhone";
            lblCustomerPhone.Size = new Size(294, 20);
            lblCustomerPhone.TabIndex = 11;
            lblCustomerPhone.Text = "Số điện thoại khách hàng (không bắt buộc)";
            // 
            // lblPaymentTitle
            // 
            lblPaymentTitle.AutoSize = true;
            lblPaymentTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPaymentTitle.Location = new Point(20, 24);
            lblPaymentTitle.Name = "lblPaymentTitle";
            lblPaymentTitle.Size = new Size(315, 32);
            lblPaymentTitle.TabIndex = 12;
            lblPaymentTitle.Text = "THÔNG TIN THANH TOÁN";
            // 
            // FormPOS
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1050, 660);
            Controls.Add(splitMain);
            KeyPreview = true;
            Name = "FormPOS";
            Text = "Bán hàng POS";
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            leftLayout.ResumeLayout(false);
            barcodePanel.ResumeLayout(false);
            barcodePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            paymentPanel.ResumeLayout(false);
            paymentPanel.PerformLayout();
            ResumeLayout(false);
        }

        private SplitContainer splitMain = null!;
        private TableLayoutPanel leftLayout = null!;
        private Panel barcodePanel = null!;
        private TextBox txtBarcode = null!;
        private Label lblBarcode = null!;
        private Button btnRemoveItem = null!;
        private DataGridView dgvCart = null!;
        private Panel paymentPanel = null!;
        private Label lblPaymentTitle = null!;
        private Label lblCustomerPhone = null!;
        private TextBox txtCustomerPhone = null!;
        private Button btnFindCustomer = null!;
        private Label lblCustomerName = null!;
        private Label lblTotalCaption = null!;
        private Label lblTotalAmount = null!;
        private Label lblCashReceived = null!;
        private TextBox txtCashReceived = null!;
        private Label lblChangeCaption = null!;
        private Label lblChange = null!;
        private Button btnCheckout = null!;
        private Button btnClearCart = null!;
    }
}
