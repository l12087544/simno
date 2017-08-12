using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Simno.BaseData.Base;
using Simno.Component.InterFace;
using DevExpress.XtraTab;
using Simno.Component.Controller;
using Simno.DataBase;
using DevExpress.XtraBars;
using Simno.BaseData.Data;
using static DevExpress.Xpo.Logger.LogManager;
using Simno.Common;
using Simno.BaseData;
using System.Xml;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace Run
{
    public partial class MainFrm : DevExpress.XtraEditors.XtraForm, IFormView
    {
        private IMainController _currentController = null;
        private static string _currentDeptId = string.Empty;
        private IDesktop _currentDesktop = null;
        private EventHandler _currentEventHandler = null;
        private IMainControl _currentMainControl = null;
        private TabControlController _currentTabController = null;
        private IDictionary<string, TabControlController> _dictTabCtroller = null;
   //     private EverydayMessageFrm _everydayMessageFrm = null;
        private FormController _formController = null;
        private XtraTabPage _hotTabPage = null;
        private bool _isLogout = true;
        private BaseLoginLogData _loginLoginData = null;
        private string _moduleid = string.Empty;
        private IDictionary<string, BarLargeButtonItem> _quickChanelDictionary = new Dictionary<string, BarLargeButtonItem>();
        private XtraTabControl _tabMainControl = null;
        private IList<string> _toolbarList = new List<string>();
        private IUserContext _userContext = null;
        private string appUpdateConfig = string.Empty;
        private int Hotkey1;
       // private MessageHandler messageHandler = null;
        private IDictionary<string, BarSubItem> moduleTypeList = new Dictionary<string, BarSubItem>();
        //private RepositoryItemMarqueeProgressBar repositoryItemMarqueeProgressBar1;
        //private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        //private RepositoryItemProgressBar repositoryItemProgressBar1;
        private bool s_InterSetTime = false;
        private Timer timerGarbageCollect;
      //  private EventWrapper wrapper = null;
        private XmlDocument xmlDocument = null;
        public MainFrm(IUserContext userContext)
        {
            InitializeComponent();
            this._userContext = userContext;
            this._formController = new MainFormController(this, this._userContext);
            ((MainFormController)this._formController).OnModuleClicked += new EventHandler(this.MainFrm_OnModuleClicked);
            this.InitAppRunModule();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this._formController.Initialize();
            this._currentController = ((MainFormController)this._formController).Controller;
            this.InitControls();
            Image image = this.LoadImage(this.barItem_quickChanel.Caption);
            if (image != null)
            {
                this.barItem_quickChanel.Glyph = image;
            }
            Image image2 = this.LoadImage(this.barItem_Home.Caption);
            if (image2 != null)
            {
                this.barItem_Home.CaptionAlignment = BarItemCaptionAlignment.Right;
                this.barItem_Home.Glyph = image2;
                this.barItem_Home.LargeGlyph = image2;
            }
            Image image3 = this.LoadImage(this.barItem_Logout.Caption);
            if (image3 != null)
            {
                this.barItem_Logout.CaptionAlignment = BarItemCaptionAlignment.Right;
                this.barItem_Logout.Glyph = image3;
                this.barItem_Logout.LargeGlyph = image3;
            }
            Image image4 = this.LoadImage(this.barItem_Message.Caption);
            if (image4 != null)
            {
                this.barItem_Message.CaptionAlignment = BarItemCaptionAlignment.Right;
                this.barItem_Message.Glyph = image4;
                this.barItem_Message.LargeGlyph = image4;
            }
            Image image5 = this.LoadImage(this.barItem_Parameter.Caption);
            if (image5 != null)
            {
                this.barItem_Parameter.CaptionAlignment = BarItemCaptionAlignment.Right;
                this.barItem_Parameter.Glyph = image5;
                this.barItem_Parameter.LargeGlyph = image5;
            }
            //if (!(RemotingObject.Instance.IsLocalDatabase || ServiceManager.Instance.IsWebService))
            //{
            //    this.InitRemotingCalls();
            //    this.RegisterBroadCast();
            //}
            this.RegisterHotKey();
        }
        private void InitControls()
        {
            this.InitApp();
            this.BindEvents();
        }
        private void InitApp()
        {
            //ForceSynchLocalSystemTime();
           // SystemEvents.TimeChanged += new EventHandler(this.SystemTimeChanged);
        }
        private void BindEvents()
        {
            this.barItem_Logout.ItemClick += delegate (object sender, ItemClickEventArgs e)
            {
                _isLogout = false;
                if (MessageBox.Show("是否注销当前用户？", "系统提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string str = "run.exe";

                    string path = Path.Combine(Application.StartupPath, str);
                    if (File.Exists(path))
                    {
                        Process.Start(path);
                    }

                    Application.Exit();
                }
                _isLogout = true;

            };
            this.barItem_Home.ItemClick += delegate (object sender, ItemClickEventArgs e) {
                MainFormController controller = (MainFormController)this._formController;
                if (this._currentEventHandler == null)
                {
                    this._currentEventHandler = controller.quickChanelEventHandler;
                }
                object currentHomeMain = controller.CurrentHomeMain;
                if (currentHomeMain == null)
                {
                    currentHomeMain = controller.GenerateMainControl();
                }
                e.Item.Tag = currentHomeMain;
                this._currentEventHandler(e.Item.Tag, e);
            };
            //this.barItem_Message.ItemClick += delegate (object sender, ItemClickEventArgs e) {
            //    using (MailMessageMainFrm frm = new MailMessageMainFrm())
            //    {
            //        if (frm.ShowDialog() == DialogResult.Yes)
            //        {
            //        }
            //    }
            //};
            //this.barItem_Parameter.ItemClick += delegate (object sender, ItemClickEventArgs e) {
            //    using (BaseParameterSettingFrm frm = new BaseParameterSettingFrm())
            //    {
            //        if (frm.ShowDialog() == DialogResult.Yes)
            //        {
            //        }
            //    }
            //};
        }

      
        private void _tabMainControl_CloseButtonClick(object sender, EventArgs e)
        {
            if (this._tabMainControl.TabPages.Count != 1)
            {
                XtraTabPage page = this._hotTabPage;
                if (page.Tag is IMainControl)
                {
                    IMainControl tag = page.Tag as IMainControl;
                    ((Control)tag).Dispose();
                }
                if (this._dictTabCtroller.ContainsKey(page.Text))
                {
                    this._dictTabCtroller.Remove(page.Text);
                }
                this._tabMainControl.TabPages.Remove(page);
            }
        }
        private void hotkey_OnHotkey(int HotKeyID)
        {
            //FormCSharpWinDemo.get_Instance().Show();
        }
        private void RegisterHotKey()
        {
            HotkeyHelper helper = new HotkeyHelper(base.Handle);
            this.Hotkey1 = helper.RegisterHotkey(Keys.F2, HotkeyHelper.KeyFlags.MOD_CONTROL);
            helper.OnHotkey += new HotkeyEventHandler(this.hotkey_OnHotkey);
        }

        private Image LoadImage(string name)
        {
            Image image = null;
            try
            {
                string path = Path.Combine(Application.StartupPath, string.Format(@"Images\toolbarImages\{0}.png", name));
                if (File.Exists(path))
                {
                    return Image.FromFile(path);
                }
                path = Path.Combine(Application.StartupPath, string.Format(@"Images\toolbarImages\{0}.png", "帮助"));
                if (File.Exists(path))
                {
                    image = Image.FromFile(path);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            return image;
        }

        private void MainFrm_OnModuleClicked(object sender, EventArgs e)
        {
            if (sender is SystemModuleData)
            {
                SystemModuleData data = sender as SystemModuleData;
                IMainController mainController = null;
                BarItem item = new BarLargeButtonItem
                {
                    Caption = data.moduleName
                };
                ItemClickEventArgs args = new ItemClickEventArgs(item, null);
                if (this._currentTabController != null)
                {
                    this._currentMainControl = this._currentTabController.MainControl;
                    mainController = this._currentTabController.MainController;
                }
                else
                {
                    mainController = ((MainFormController)this._formController).GenerateController(data.dllName, data.nameSpace);
                    _currentDeptId = this.GenerateCurrentWstationDeptId(data.deptId);
                    this._moduleid = data.moduleId;
                    UserContextManager.userContext.currentWorkstationModule = this.GenerateSystemModule(data.moduleId);
                    this._currentController = mainController;
                    this._currentEventHandler = ((MainFormController)this._formController).quickChanelEventHandler;
                }
                if (!((this._dictTabCtroller == null) || this._dictTabCtroller.ContainsKey(data.moduleName)))
                {
                    TabControlController controller2 = new TabControlController
                    {
                        eventHandler = this._currentEventHandler,
                        CurrentDeptid = _currentDeptId,
                        ModuleId = data.moduleId,
                        CurrentModule = UserContextManager.userContext.currentWorkstationModule,
                        MainController = mainController
                    };
                    this._dictTabCtroller.Add(data.moduleName, controller2);
                }
                ((MainFormController)this._formController).QuickChanelEventHandler(mainController, args);
            }
        }
        private object GenerateSystemModule(string moduleid)
        {
            IHSystemModule module = new HSystemModuleImp();
            return module.GenerateById(moduleid);
        }

        private string GenernateXmlDocument()
        {
            string xpath = "/AppUpdateConfig/Style";
            XmlNode node = this.xmlDocument.SelectSingleNode(xpath);
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }

        private void InitAppRunModule()
        {
            //if ((this._userContext.MachineConfig != null) && this._userContext.MachineConfig.IsMultiAppModule)
            //{
                this._tabMainControl = new XtraTabControl();
                this._tabMainControl.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders;
                this._tabMainControl.HotTrackedPageChanged += new TabPageChangedEventHandler(this._tabMainControl_HotTrackedPageChanged);
                this._tabMainControl.CloseButtonClick += new EventHandler(this._tabMainControl_CloseButtonClick);
                this._tabMainControl.SelectedPageChanged += new TabPageChangedEventHandler(this._tabMainControl_SelectedPageChanged);
                this._dictTabCtroller = new Dictionary<string, TabControlController>();
                this._tabMainControl.Dock = DockStyle.Fill;
                this.panel_main.Controls.Add(this._tabMainControl);
                MyControlHelper.DockControl<XtraTabControl>(this._tabMainControl, this.panel_main, DockStyle.Fill);
            //}
        }
        private void _tabMainControl_HotTrackedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (e.Page != null)
            {
                this._hotTabPage = e.Page;
            }
        }

        private void _tabMainControl_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (e.Page != null)
            {
                this._currentTabController = this._dictTabCtroller[e.Page.Text];
                this._currentMainControl = this._dictTabCtroller[e.Page.Text].MainControl;
                if (this._currentMainControl.LeftControl != null)
                {
                }
                this._currentEventHandler = this._dictTabCtroller[e.Page.Text].eventHandler;
                _currentDeptId = this._dictTabCtroller[e.Page.Text].CurrentDeptid;
                this._moduleid = this._dictTabCtroller[e.Page.Text].ModuleId;
                UserContextManager.userContext.currentWorkstationModule = this._dictTabCtroller[e.Page.Text].CurrentModule;
                this._currentController = this._dictTabCtroller[e.Page.Text].MainController;
                string str = "HIS";//FileHelper.LicensesData.Rows[0]["useCompanyName"].ToString();
                this.Text = string.Format("{0}-管理系统主程序【{1}】", str, e.Page.Text);
                this.ResetToolbar();
                if (this._dictTabCtroller[e.Page.Text].MainController is BaseManagerLeftMainController)
                {
                    (this._dictTabCtroller[e.Page.Text].MainController as BaseManagerLeftMainController).CreateToolBarButtons();
                }
                else
                {
                    if (this._dictTabCtroller[e.Page.Text].MainController != null)
                    {
                        this._dictTabCtroller[e.Page.Text].MainController.CreateToolBarButtons();
                    }
                }
            }
        }
        private BarLargeButtonItem SetBarLargeButtonItem(SystemModuleData moduleData, IMainController controller, EventHandler eventHandler)
        {
            BarLargeButtonItem item = new BarLargeButtonItem(this.barManager, moduleData.moduleName)
            {
                Manager = { ShowFullMenus = true }
            };
            if (moduleData.toolbarIcon.Length > 1)
            {
                Image image = ImageHelper.CovertBytesToImage(moduleData.toolbarIcon, new Size(0x10, 0x10));
                item.Glyph = image;
            }
            item.Tag = controller;
            if (!this._quickChanelDictionary.ContainsKey(moduleData.moduleId))
            {
                this._quickChanelDictionary.Add(moduleData.moduleId, item);
            }
            item.ItemClick += delegate (object sender, ItemClickEventArgs e) {
                if (eventHandler != null)
                {
                    this._currentEventHandler = eventHandler;
                    _currentDeptId = this.GenerateCurrentWstationDeptId(moduleData.deptId);
                    this._moduleid = moduleData.moduleId;
                    UserContextManager.userContext.currentWorkstationModule = this.GenerateSystemModule(moduleData.moduleId);
                    this._currentController = controller;
                    if (!((this._dictTabCtroller == null) || this._dictTabCtroller.ContainsKey(moduleData.moduleName)))
                    {
                        TabControlController controller1 = new TabControlController
                        {
                            eventHandler = eventHandler,
                            CurrentDeptid = _currentDeptId,
                            ModuleId = moduleData.moduleId,
                            CurrentModule = UserContextManager.userContext.currentWorkstationModule,
                            MainController = controller
                        };
                        this._dictTabCtroller.Add(moduleData.moduleName, controller1);
                    }
                    eventHandler(e.Item.Tag, e);
                }
            };
            return item;
        }

        public void AddQuickChanelButton(SystemModuleData moduleData, IMainController controller, EventHandler eventHandler)
        {
            BarLargeButtonItem item;
            if (!string.IsNullOrEmpty(moduleData.moduleType))
            {
                if (this.moduleTypeList.ContainsKey(moduleData.moduleType))
                {
                    item = this.SetBarLargeButtonItem(moduleData, controller, eventHandler);
                    this.moduleTypeList[moduleData.moduleType].ItemLinks.Add(item);
                }
                else
                {
                    BarSubItem item2 = new BarSubItem
                    {
                        Caption = moduleData.moduleType
                    };
                    this.moduleTypeList.Add(moduleData.moduleType, item2);
                    item = this.SetBarLargeButtonItem(moduleData, controller, eventHandler);
                    this.moduleTypeList[moduleData.moduleType].ItemLinks.Add(item);
                    this.barItem_quickChanel.ItemLinks.Add(item2);
                }
            }
            else
            {
                item = this.SetBarLargeButtonItem(moduleData, controller, eventHandler);
                this.barItem_quickChanel.ItemLinks.Add(item);
            }
            MemoryUtil.FlushMemory();
        }
        private IList<DeptData> GenerateChildDept(string deptid)
        {
            IDept dept = new DeptImp();
            return dept.LoadDeptListByParentId(deptid);
        }

        private string GenerateCurrentWstationDeptId(string deptid)
        {
            IList<DeptData> deptList = this.GenerateChildDept(deptid);
            if ((deptList != null) && (deptList.Count != 0))
            {
                //using (ChildDeptListFrm frm = new ChildDeptListFrm(deptList))
                //{
                //    if (frm.ShowDialog() == DialogResult.OK)
                //    {
                //        UserContextManager.userContext.childDeptid = frm.DeptId;
                //        return this._userContext.currentModule.deptId;
                //    }
                //}
            }
            UserContextManager.userContext.childDeptid = string.Empty;
            return deptid;
        }
        public void AddQuickChanelButton(string controllerName, IMainController controller, EventHandler eventHandler, int imageIndex)
        {
        }

        public void AddQuickChanelButton(string controllerName, IMainController controller, EventHandler eventHandler, int imageIndex, string deptid, string moduleid)
        {
            BarLargeButtonItem item = new BarLargeButtonItem(this.barManager, controllerName)
            {
                Tag = controller
            };
            if (!this._quickChanelDictionary.ContainsKey(moduleid))
            {
                this._quickChanelDictionary.Add(moduleid, item);
            }
            item.ItemClick += delegate (object sender, ItemClickEventArgs e) {
                if (eventHandler != null)
                {
                    this._currentEventHandler = eventHandler;
                    _currentDeptId = this.GenerateCurrentWstationDeptId(deptid);
                    this._moduleid = moduleid;
                    UserContextManager.userContext.currentWorkstationModule = this.GenerateSystemModule(moduleid);
                    eventHandler(e.Item.Tag, e);
                    this._currentController = controller;
                }
            };
            this.barItem_quickChanel.ItemLinks.Add(item);
            MemoryUtil.FlushMemory();
        }
        public void CreateMenu(IList<MenuData> menuList)
        {
            MainAppearanceMenu menu = new MainAppearanceMenu(this.barManager, this.defaultLookAndFeel, "关于", null, menuList);
        }

        public void CreateStatusBar(FormController.StatusBarData statusBarData)
        {
            //this.barStaticItem_strCompany.Caption = statusBarData.strCompany;
            //this.barStaticItem_strDeptAndUser.Caption = statusBarData.strDeptAndUser;
            //this.barStaticItem_strNetWork.Caption = statusBarData.strNetWork;
            this.barStaticItem_Regorgan.Caption = string.Format("登录机构：{0}", this._userContext.currentRegorgan.jg_mc);
        }

        public void CreateToolBar(FormController.ToolBarData toolBarData)
        {
        }

        public void CreateToolBarButton(string name)
        {
            if (!this._toolbarList.Contains(name))
            {
                this._toolbarList.Add(name);
                BarLargeButtonItem item = new BarLargeButtonItem(this.barManager, name);
                EventHandler eventHandler = (sender, e) => MessageBox.Show(name);
                this.CreateToolBarButton(name, eventHandler);
            }
        }

        public void CreateToolBarButton(string name, EventHandler eventHandler)
        {
            this.CreateToolBarButton(name, eventHandler, string.Empty, false);
        }

        public void CreateToolBarButton(string name, EventHandler eventHandler, string imageName, bool isBeginGroup)
        {
            Image image = this.LoadImage(name);
            this.CreateToolBarButton(name, eventHandler, image, 0x63, false);
        }

        public void CreateToolBarButton(string name, EventHandler eventHandler, Image image, int linkIndex, bool isBeginGroup)
        {
            try
            {
                BarLargeButtonItem barLargeButtonItem;
                if (!this._toolbarList.Contains(name))
                {
                    this._toolbarList.Add(name);
                    linkIndex += 4;
                    barLargeButtonItem = new BarLargeButtonItem(this.barManager, name)
                    {
                        CaptionAlignment = BarItemCaptionAlignment.Right,
                        Name = string.Format("temp_{0}", name)
                    };
                    if (image != null)
                    {
                        barLargeButtonItem.LargeGlyph = image;
                        barLargeButtonItem.Glyph = image;
                    }
                    barLargeButtonItem.ItemClick += delegate (object sender, ItemClickEventArgs e) {
                        if (eventHandler != null)
                        {
                            eventHandler(barLargeButtonItem.Caption, e);
                        }
                    };
                    if (this.bar_toolBar.ItemLinks.Count <= linkIndex)
                    {
                        BarItemLink link = this.bar_toolBar.AddItem(barLargeButtonItem);
                        if (isBeginGroup)
                        {
                            link.BeginGroup = true;
                        }
                    }
                    else
                    {
                        BarItemLink beforeLink = this.bar_toolBar.ItemLinks[linkIndex];
                        if (beforeLink == null)
                        {
                            this.bar_toolBar.AddItem(barLargeButtonItem);
                        }
                        else
                        {
                            this.bar_toolBar.InsertItem(beforeLink, barLargeButtonItem);
                        }
                        if (isBeginGroup)
                        {
                            beforeLink.BeginGroup = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("创建工具条异常：" + exception.Message);
            }
        }
        public void DisplayTimer(string strTime)
        {
            //this.barStaticItem_strTime.Caption = strTime;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        public IMainControl CurrentMainControl
        {
            get
            {
                return this._currentMainControl;
            }
            set
            {
                this._currentMainControl = value;
            }
        }

        public string windowText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public void DockMainControl(IMainControl mainControl)
        {
            this.ResetToolbar();
            if (mainControl is Control)
            {
                Control control = mainControl as Control;
                if (!((this._dictTabCtroller == null) || this._dictTabCtroller.ContainsKey("首页")) && mainControl is IDesktop )
                {
                    TabControlController controller2 = new TabControlController
                    {
                        eventHandler = this._currentEventHandler,
                        CurrentDeptid = _currentDeptId,
                        ModuleId = "00001",
                        CurrentModule = UserContextManager.userContext.currentWorkstationModule,
                      //  MainController = mainController
                    };
                    control.Text = "首页";
                    this._dictTabCtroller.Add("首页", controller2);
                }
                
                string text = control.Text;
                if ((this._userContext.MachineConfig.IsMultiAppModule && (this._tabMainControl != null)) && !string.IsNullOrEmpty(text))
                {
                    this._tabMainControl.Visible = true;
                    control.Dock = DockStyle.Fill;
                    Control control2 = this.panel_main;
                    XtraTabPage page = new XtraTabPage
                    {
                        Text = text
                    };
                    if (this._dictTabCtroller.ContainsKey(text))
                    {
                        if (this._dictTabCtroller[text].MainTabPage == null)
                        {
                            this._dictTabCtroller[text].MainTabPage = page;
                            this._dictTabCtroller[text].MainControl = mainControl;
                            this._tabMainControl.TabPages.Add(page);
                            page.Tag = control;
                            page.Controls.Add(control);
                            this._tabMainControl.SelectedTabPage = page;
                        }
                        else
                        {
                            if (this._tabMainControl.SelectedTabPage.Equals(this._dictTabCtroller[text].MainTabPage))
                            {
                                TabPageChangedEventArgs e = new TabPageChangedEventArgs(null, this._dictTabCtroller[text].MainTabPage);
                                this._tabMainControl_SelectedPageChanged(this._dictTabCtroller[text].MainTabPage, e);
                            }
                            this._tabMainControl.SelectedTabPage = this._dictTabCtroller[text].MainTabPage;
                        }
                    }
                }
                else
                {
                    //if (this._tabMainControl != null)
                    //{
                    //    this._tabMainControl.Visible = false;
                    //}
                    if (this._currentMainControl != mainControl)
                    {
                        if (this._currentMainControl != null)
                        {
                            Control control3;
                            this._currentMainControl.ClearToolBarButton();
                            control.Visible = false;
                            control.Dock = DockStyle.Fill;
                            this.panel_main.Controls.Add(control);
                            control.Visible = true;
                            if (!this._userContext.MachineConfig.IsMultiAppModule)
                            {
                                control3 = this._currentMainControl as Control;
                                this.panel_main.Controls.Remove(control3);
                                control3.Dispose();
                            }
                            else if (mainControl is IDesktop)
                            {
                                if (this._currentDesktop != null)
                                {
                                    control3 = this._currentDesktop as Control;
                                    this.panel_main.Controls.Remove(control3);
                                    control3.Dispose();
                                }
                                this._currentDesktop = mainControl as IDesktop;
                            }
                        }
                        else
                        {
                            MyControlHelper.DockControl<IMainControl>(mainControl, this.panel_main, DockStyle.Fill);
                            if (mainControl is IDesktop)
                            {
                                this._currentDesktop = mainControl as IDesktop;
                            }
                        }
                        this._currentMainControl = mainControl;
                    }
                }
            }
        }
        private void ResetToolbar()
        {
            IList<BarItem> list = new List<BarItem>();
            foreach (BarItemLink link in this.bar_toolBar.ItemLinks)
            {
                if (link.Item.Name.StartsWith("temp_"))
                {
                    list.Add(link.Item);
                }
            }
            foreach (BarItem item in list)
            {
                this.barManager.Items.Remove(item);
            }
            this._toolbarList.Clear();
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isLogout)
            {
                if (MessageBox.Show("是否退出系统？", "系统提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
           
        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}