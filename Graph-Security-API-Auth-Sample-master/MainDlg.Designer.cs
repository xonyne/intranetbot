namespace GraphAuthSample
{
    partial class MainDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.sendRequestFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.urlInput = new System.Windows.Forms.ComboBox();
            this.requestButton = new System.Windows.Forms.Button();
            this.signOutButton = new System.Windows.Forms.Button();
            this.authMethodTabControl = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.userDelegatedLabel = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.appOnlyTable = new System.Windows.Forms.TableLayoutPanel();
            this.appOnlyDescription = new System.Windows.Forms.RichTextBox();
            this.tenantIdInput = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.appKeyInput = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.helpContent = new System.Windows.Forms.RichTextBox();
            this.appIdFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.appIdInput = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.statusOutput = new System.Windows.Forms.TextBox();
            this.resultSplitContainer = new System.Windows.Forms.SplitContainer();
            this.authTokenTable = new System.Windows.Forms.TableLayoutPanel();
            this.tokenOutput = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.responseTable = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.responseContentOutput = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.responseHeaderOutput = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.mainTable.SuspendLayout();
            this.sendRequestFlowLayoutPanel.SuspendLayout();
            this.authMethodTabControl.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.appOnlyTable.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.appIdFlowLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultSplitContainer)).BeginInit();
            this.resultSplitContainer.Panel1.SuspendLayout();
            this.resultSplitContainer.Panel2.SuspendLayout();
            this.resultSplitContainer.SuspendLayout();
            this.authTokenTable.SuspendLayout();
            this.responseTable.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.mainTable);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.mainSplitContainer.Size = new System.Drawing.Size(1346, 809);
            this.mainSplitContainer.SplitterDistance = 264;
            this.mainSplitContainer.SplitterWidth = 5;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // mainTable
            // 
            this.mainTable.ColumnCount = 1;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainTable.Controls.Add(this.sendRequestFlowLayoutPanel, 0, 2);
            this.mainTable.Controls.Add(this.authMethodTabControl, 0, 1);
            this.mainTable.Controls.Add(this.appIdFlowLayoutPanel, 0, 0);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.Location = new System.Drawing.Point(0, 0);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 3;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.mainTable.Size = new System.Drawing.Size(1346, 264);
            this.mainTable.TabIndex = 0;
            // 
            // sendRequestFlowLayoutPanel
            // 
            this.sendRequestFlowLayoutPanel.Controls.Add(this.label4);
            this.sendRequestFlowLayoutPanel.Controls.Add(this.urlInput);
            this.sendRequestFlowLayoutPanel.Controls.Add(this.requestButton);
            this.sendRequestFlowLayoutPanel.Controls.Add(this.signOutButton);
            this.sendRequestFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sendRequestFlowLayoutPanel.Location = new System.Drawing.Point(2, 203);
            this.sendRequestFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.sendRequestFlowLayoutPanel.Name = "sendRequestFlowLayoutPanel";
            this.sendRequestFlowLayoutPanel.Size = new System.Drawing.Size(1342, 59);
            this.sendRequestFlowLayoutPanel.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Graph URL:";
            // 
            // urlInput
            // 
            this.urlInput.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.urlInput.FormattingEnabled = true;
            this.urlInput.Location = new System.Drawing.Point(78, 5);
            this.urlInput.Margin = new System.Windows.Forms.Padding(2);
            this.urlInput.Name = "urlInput";
            this.urlInput.Size = new System.Drawing.Size(706, 23);
            this.urlInput.TabIndex = 7;
            this.urlInput.Text = "https://graph.microsoft.com/beta/security/alerts?$top=1&$select=vendorInformation" +
    "/provider";
            // 
            // requestButton
            // 
            this.requestButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.requestButton.Location = new System.Drawing.Point(812, 2);
            this.requestButton.Margin = new System.Windows.Forms.Padding(26, 2, 26, 2);
            this.requestButton.Name = "requestButton";
            this.requestButton.Size = new System.Drawing.Size(99, 27);
            this.requestButton.TabIndex = 10;
            this.requestButton.Text = "Send Request";
            this.requestButton.UseVisualStyleBackColor = true;
            this.requestButton.Click += new System.EventHandler(this.OnSendRequest);
            // 
            // signOutButton
            // 
            this.signOutButton.Location = new System.Drawing.Point(940, 3);
            this.signOutButton.Name = "signOutButton";
            this.signOutButton.Size = new System.Drawing.Size(75, 23);
            this.signOutButton.TabIndex = 11;
            this.signOutButton.Text = "Sign out";
            this.signOutButton.UseVisualStyleBackColor = true;
            this.signOutButton.Click += new System.EventHandler(this.OnSignOut);
            // 
            // authMethodTabControl
            // 
            this.authMethodTabControl.Controls.Add(this.tabPage3);
            this.authMethodTabControl.Controls.Add(this.tabPage4);
            this.authMethodTabControl.Controls.Add(this.tabPage5);
            this.authMethodTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authMethodTabControl.Location = new System.Drawing.Point(2, 34);
            this.authMethodTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.authMethodTabControl.Name = "authMethodTabControl";
            this.authMethodTabControl.SelectedIndex = 0;
            this.authMethodTabControl.Size = new System.Drawing.Size(1342, 165);
            this.authMethodTabControl.TabIndex = 17;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.userDelegatedLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(1334, 137);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "User delegated authorization";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // userDelegatedLabel
            // 
            this.userDelegatedLabel.BackColor = System.Drawing.SystemColors.Window;
            this.userDelegatedLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userDelegatedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDelegatedLabel.Location = new System.Drawing.Point(2, 2);
            this.userDelegatedLabel.Margin = new System.Windows.Forms.Padding(2);
            this.userDelegatedLabel.Name = "userDelegatedLabel";
            this.userDelegatedLabel.ReadOnly = true;
            this.userDelegatedLabel.Size = new System.Drawing.Size(1330, 133);
            this.userDelegatedLabel.TabIndex = 20;
            this.userDelegatedLabel.Text = resources.GetString("userDelegatedLabel.Text");
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.appOnlyTable);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage4.Size = new System.Drawing.Size(1334, 139);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Application-only authorization";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // appOnlyTable
            // 
            this.appOnlyTable.ColumnCount = 2;
            this.appOnlyTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.appOnlyTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.appOnlyTable.Controls.Add(this.appOnlyDescription, 0, 0);
            this.appOnlyTable.Controls.Add(this.tenantIdInput, 1, 2);
            this.appOnlyTable.Controls.Add(this.label7, 0, 2);
            this.appOnlyTable.Controls.Add(this.label8, 0, 1);
            this.appOnlyTable.Controls.Add(this.appKeyInput, 1, 1);
            this.appOnlyTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appOnlyTable.Location = new System.Drawing.Point(2, 2);
            this.appOnlyTable.Name = "appOnlyTable";
            this.appOnlyTable.RowCount = 3;
            this.appOnlyTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.appOnlyTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.appOnlyTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.appOnlyTable.Size = new System.Drawing.Size(1330, 135);
            this.appOnlyTable.TabIndex = 18;
            // 
            // appOnlyDescription
            // 
            this.appOnlyDescription.BackColor = System.Drawing.SystemColors.Window;
            this.appOnlyDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.appOnlyTable.SetColumnSpan(this.appOnlyDescription, 2);
            this.appOnlyDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appOnlyDescription.Location = new System.Drawing.Point(3, 3);
            this.appOnlyDescription.Name = "appOnlyDescription";
            this.appOnlyDescription.ReadOnly = true;
            this.appOnlyDescription.Size = new System.Drawing.Size(1324, 74);
            this.appOnlyDescription.TabIndex = 17;
            this.appOnlyDescription.Text = resources.GetString("appOnlyDescription.Text");
            // 
            // tenantIdInput
            // 
            this.tenantIdInput.FormattingEnabled = true;
            this.tenantIdInput.Location = new System.Drawing.Point(102, 110);
            this.tenantIdInput.Margin = new System.Windows.Forms.Padding(2);
            this.tenantIdInput.Name = "tenantIdInput";
            this.tenantIdInput.Size = new System.Drawing.Size(711, 23);
            this.tenantIdInput.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 108);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Tenant ID:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 80);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "App Key:";
            // 
            // appKeyInput
            // 
            this.appKeyInput.Location = new System.Drawing.Point(102, 82);
            this.appKeyInput.Margin = new System.Windows.Forms.Padding(2);
            this.appKeyInput.Name = "appKeyInput";
            this.appKeyInput.Size = new System.Drawing.Size(711, 21);
            this.appKeyInput.TabIndex = 15;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.helpContent);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1334, 139);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Help";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // helpContent
            // 
            this.helpContent.BackColor = System.Drawing.SystemColors.Window;
            this.helpContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.helpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpContent.Location = new System.Drawing.Point(0, 0);
            this.helpContent.Name = "helpContent";
            this.helpContent.ReadOnly = true;
            this.helpContent.Size = new System.Drawing.Size(1334, 139);
            this.helpContent.TabIndex = 0;
            this.helpContent.Text = resources.GetString("helpContent.Text");
            // 
            // appIdFlowLayoutPanel
            // 
            this.appIdFlowLayoutPanel.Controls.Add(this.label1);
            this.appIdFlowLayoutPanel.Controls.Add(this.appIdInput);
            this.appIdFlowLayoutPanel.Controls.Add(this.richTextBox1);
            this.appIdFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appIdFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.appIdFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.appIdFlowLayoutPanel.Name = "appIdFlowLayoutPanel";
            this.appIdFlowLayoutPanel.Size = new System.Drawing.Size(1346, 32);
            this.appIdFlowLayoutPanel.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "App ID:";
            // 
            // appIdInput
            // 
            this.appIdInput.FormattingEnabled = true;
            this.appIdInput.Location = new System.Drawing.Point(52, 2);
            this.appIdInput.Margin = new System.Windows.Forms.Padding(2);
            this.appIdInput.Name = "appIdInput";
            this.appIdInput.Size = new System.Drawing.Size(409, 23);
            this.appIdInput.TabIndex = 8;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(466, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(780, 22);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "Permissions are defined in the application registration (go to http://apps.dev.mi" +
    "crosoft.com to register your app)";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel2.Controls.Add(this.statusOutput, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.resultSplitContainer, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1346, 540);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // statusOutput
            // 
            this.statusOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.statusOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusOutput.Location = new System.Drawing.Point(2, 2);
            this.statusOutput.Margin = new System.Windows.Forms.Padding(2);
            this.statusOutput.Name = "statusOutput";
            this.statusOutput.Size = new System.Drawing.Size(1342, 21);
            this.statusOutput.TabIndex = 0;
            // 
            // resultSplitContainer
            // 
            this.resultSplitContainer.BackColor = System.Drawing.Color.AntiqueWhite;
            this.resultSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultSplitContainer.Location = new System.Drawing.Point(2, 29);
            this.resultSplitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.resultSplitContainer.Name = "resultSplitContainer";
            // 
            // resultSplitContainer.Panel1
            // 
            this.resultSplitContainer.Panel1.Controls.Add(this.authTokenTable);
            // 
            // resultSplitContainer.Panel2
            // 
            this.resultSplitContainer.Panel2.Controls.Add(this.responseTable);
            this.resultSplitContainer.Size = new System.Drawing.Size(1342, 639);
            this.resultSplitContainer.SplitterDistance = 425;
            this.resultSplitContainer.SplitterWidth = 3;
            this.resultSplitContainer.TabIndex = 1;
            // 
            // authTokenTable
            // 
            this.authTokenTable.BackColor = System.Drawing.Color.Linen;
            this.authTokenTable.ColumnCount = 1;
            this.authTokenTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.authTokenTable.Controls.Add(this.tokenOutput, 0, 1);
            this.authTokenTable.Controls.Add(this.label5, 0, 0);
            this.authTokenTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authTokenTable.Location = new System.Drawing.Point(0, 0);
            this.authTokenTable.Margin = new System.Windows.Forms.Padding(2);
            this.authTokenTable.Name = "authTokenTable";
            this.authTokenTable.RowCount = 2;
            this.authTokenTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.authTokenTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.authTokenTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.authTokenTable.Size = new System.Drawing.Size(425, 639);
            this.authTokenTable.TabIndex = 1;
            // 
            // tokenOutput
            // 
            this.tokenOutput.BackColor = System.Drawing.Color.Linen;
            this.tokenOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tokenOutput.Location = new System.Drawing.Point(2, 29);
            this.tokenOutput.Margin = new System.Windows.Forms.Padding(2);
            this.tokenOutput.Name = "tokenOutput";
            this.tokenOutput.ReadOnly = true;
            this.tokenOutput.Size = new System.Drawing.Size(421, 608);
            this.tokenOutput.TabIndex = 0;
            this.tokenOutput.Text = "";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 6);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Auth Token";
            // 
            // responseTable
            // 
            this.responseTable.BackColor = System.Drawing.Color.Beige;
            this.responseTable.ColumnCount = 1;
            this.responseTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.responseTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.responseTable.Controls.Add(this.label6, 0, 0);
            this.responseTable.Controls.Add(this.tabControl1, 0, 1);
            this.responseTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.responseTable.Location = new System.Drawing.Point(0, 0);
            this.responseTable.Margin = new System.Windows.Forms.Padding(2);
            this.responseTable.Name = "responseTable";
            this.responseTable.RowCount = 2;
            this.responseTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.responseTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.responseTable.Size = new System.Drawing.Size(914, 639);
            this.responseTable.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Response";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(2, 29);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(910, 608);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.responseContentOutput);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(902, 580);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Content";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // responseContentOutput
            // 
            this.responseContentOutput.BackColor = System.Drawing.Color.Beige;
            this.responseContentOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.responseContentOutput.Location = new System.Drawing.Point(2, 2);
            this.responseContentOutput.Margin = new System.Windows.Forms.Padding(2);
            this.responseContentOutput.Name = "responseContentOutput";
            this.responseContentOutput.ReadOnly = true;
            this.responseContentOutput.Size = new System.Drawing.Size(898, 576);
            this.responseContentOutput.TabIndex = 0;
            this.responseContentOutput.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.responseHeaderOutput);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(902, 582);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Header";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // responseHeaderOutput
            // 
            this.responseHeaderOutput.BackColor = System.Drawing.Color.Beige;
            this.responseHeaderOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.responseHeaderOutput.Location = new System.Drawing.Point(2, 2);
            this.responseHeaderOutput.Margin = new System.Windows.Forms.Padding(2);
            this.responseHeaderOutput.Name = "responseHeaderOutput";
            this.responseHeaderOutput.ReadOnly = true;
            this.responseHeaderOutput.Size = new System.Drawing.Size(898, 578);
            this.responseHeaderOutput.TabIndex = 1;
            this.responseHeaderOutput.Text = "";
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 809);
            this.Controls.Add(this.mainSplitContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainDlg";
            this.Text = "Graph Authentication Sample";
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.mainTable.ResumeLayout(false);
            this.sendRequestFlowLayoutPanel.ResumeLayout(false);
            this.sendRequestFlowLayoutPanel.PerformLayout();
            this.authMethodTabControl.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.appOnlyTable.ResumeLayout(false);
            this.appOnlyTable.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.appIdFlowLayoutPanel.ResumeLayout(false);
            this.appIdFlowLayoutPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.resultSplitContainer.Panel1.ResumeLayout(false);
            this.resultSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultSplitContainer)).EndInit();
            this.resultSplitContainer.ResumeLayout(false);
            this.authTokenTable.ResumeLayout(false);
            this.authTokenTable.PerformLayout();
            this.responseTable.ResumeLayout(false);
            this.responseTable.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.TableLayoutPanel mainTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox urlInput;
        private System.Windows.Forms.Button requestButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox statusOutput;
        private System.Windows.Forms.SplitContainer resultSplitContainer;
        private System.Windows.Forms.RichTextBox tokenOutput;
        private System.Windows.Forms.RichTextBox responseContentOutput;
        private System.Windows.Forms.FlowLayoutPanel sendRequestFlowLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel authTokenTable;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel responseTable;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox responseHeaderOutput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox appKeyInput;
        private System.Windows.Forms.ComboBox tenantIdInput;
        private System.Windows.Forms.TabControl authMethodTabControl;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.FlowLayoutPanel appIdFlowLayoutPanel;
        private System.Windows.Forms.ComboBox appIdInput;
        private System.Windows.Forms.RichTextBox userDelegatedLabel;
        private System.Windows.Forms.Button signOutButton;
        private System.Windows.Forms.TableLayoutPanel appOnlyTable;
        private System.Windows.Forms.RichTextBox appOnlyDescription;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.RichTextBox helpContent;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

