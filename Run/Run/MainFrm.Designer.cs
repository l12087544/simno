namespace Run
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
     
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar_toolBar = new DevExpress.XtraBars.Bar();
            this.barItem_Home = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barItem_Logout = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barItem_Message = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barItem_Parameter = new DevExpress.XtraBars.BarLargeButtonItem();
            this.bar_mainMenu = new DevExpress.XtraBars.Bar();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barItem_quickChanel = new DevExpress.XtraBars.BarSubItem();
            this.barStaticItem_Regorgan = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panel_main = new DevExpress.XtraEditors.PanelControl();
            this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_main)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar_toolBar,
            this.bar_mainMenu,
            this.bar3});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barItem_Home,
            this.barItem_Logout,
            this.barItem_Message,
            this.barItem_Parameter,
            this.barItem_quickChanel,
            this.barStaticItem_Regorgan});
            this.barManager.MainMenu = this.bar_mainMenu;
            this.barManager.MaxItemId = 6;
            this.barManager.StatusBar = this.bar3;
            // 
            // bar_toolBar
            // 
            this.bar_toolBar.BarName = "Tools";
            this.bar_toolBar.DockCol = 0;
            this.bar_toolBar.DockRow = 1;
            this.bar_toolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar_toolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItem_Home),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItem_Logout),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItem_Message),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItem_Parameter)});
            this.bar_toolBar.OptionsBar.AllowQuickCustomization = false;
            this.bar_toolBar.OptionsBar.DrawBorder = false;
            this.bar_toolBar.OptionsBar.DrawDragBorder = false;
            this.bar_toolBar.OptionsBar.UseWholeRow = true;
            this.bar_toolBar.Text = "Tools";
            // 
            // barItem_Home
            // 
            this.barItem_Home.Caption = "首页";
            this.barItem_Home.Id = 0;
            this.barItem_Home.Name = "barItem_Home";
            // 
            // barItem_Logout
            // 
            this.barItem_Logout.Caption = "注销";
            this.barItem_Logout.Id = 1;
            this.barItem_Logout.Name = "barItem_Logout";
            // 
            // barItem_Message
            // 
            this.barItem_Message.Caption = "邮件";
            this.barItem_Message.Id = 2;
            this.barItem_Message.Name = "barItem_Message";
            // 
            // barItem_Parameter
            // 
            this.barItem_Parameter.Caption = "参数";
            this.barItem_Parameter.Id = 3;
            this.barItem_Parameter.Name = "barItem_Parameter";
            // 
            // bar_mainMenu
            // 
            this.bar_mainMenu.BarName = "Main Menu";
            this.bar_mainMenu.DockCol = 0;
            this.bar_mainMenu.DockRow = 0;
            this.bar_mainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar_mainMenu.OptionsBar.AllowQuickCustomization = false;
            this.bar_mainMenu.OptionsBar.DrawDragBorder = false;
            this.bar_mainMenu.OptionsBar.UseWholeRow = true;
            this.bar_mainMenu.Text = "Main menu";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItem_quickChanel),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem_Regorgan)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barItem_quickChanel
            // 
            this.barItem_quickChanel.Caption = "快速通道";
            this.barItem_quickChanel.Id = 4;
            this.barItem_quickChanel.Name = "barItem_quickChanel";
            // 
            // barStaticItem_Regorgan
            // 
            this.barStaticItem_Regorgan.Caption = "当前科室";
            this.barStaticItem_Regorgan.Id = 5;
            this.barStaticItem_Regorgan.Name = "barStaticItem_Regorgan";
            this.barStaticItem_Regorgan.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(860, 49);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 377);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(860, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 49);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 328);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(860, 49);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 328);
            // 
            // panel_main
            // 
            this.panel_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_main.Location = new System.Drawing.Point(0, 49);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(860, 328);
            this.panel_main.TabIndex = 4;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 404);
            this.Controls.Add(this.panel_main);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "MainFrm";
            this.Text = "MainFrm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar_toolBar;
        private DevExpress.XtraBars.Bar bar_mainMenu;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarLargeButtonItem barItem_Home;
        private DevExpress.XtraBars.BarLargeButtonItem barItem_Logout;
        private DevExpress.XtraBars.BarLargeButtonItem barItem_Message;
        private DevExpress.XtraBars.BarLargeButtonItem barItem_Parameter;
        private DevExpress.XtraBars.BarSubItem barItem_quickChanel;
        private DevExpress.XtraBars.BarStaticItem barStaticItem_Regorgan;
        private DevExpress.XtraEditors.PanelControl panel_main;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
    }
}