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
using DevExpress.XtraBars;
using Run.Properties;
using System.IO;
using Simno.BaseData.Data;
using Simno.BaseData;
using Simno.BaseData.Base;
using Simno.Common;
using Simno.Component.InterFace;
using Simno.Component.Controller;
using DevExpress.XtraTab;

namespace Run
{
    public partial class BaseForm : XtraForm,IFormView
    {
        private IMainControl _currentMainControl = null;
        BarStaticItem _lblHospital = null;
        private string _moduleid = string.Empty;
        private IUserContext _userContext = null;
        private static string _currentDeptId = string.Empty;
        private IMainController _currentController = null;
        private EventHandler _currentEventHandler = null;
        private XtraTabPage _hotTabPage = null;
        private IDesktop _currentDesktop = null;
        private TabControlController _currentTabController = null;
        private XtraTabControl _tabMainControl = null;
        private IDictionary<string, TabControlController> _dictTabCtroller = null;
        private IList<string> _toolbarList = new List<string>();
        private IDictionary<string, BarSubItem> moduleTypeList = new Dictionary<string, BarSubItem>();
        private IDictionary<string, BarLargeButtonItem> _quickChanelDictionary = new Dictionary<string, BarLargeButtonItem>();

       

        public BaseForm()
        {
            InitializeComponent();
            InitAppRunModule();
            InitMenu();
            InitButton();
            DockMainControl();
        }
        private void InitAppRunModule()
        {
            //if ((this._userContext.MachineConfig != null) && this._userContext.MachineConfig.IsMultiAppModule)
            //{
            this._tabMainControl = new XtraTabControl();
            this._tabMainControl.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders;
            //this._tabMainControl.HotTrackedPageChanged += new TabPageChangedEventHandler(this._tabMainControl_HotTrackedPageChanged);
            //this._tabMainControl.CloseButtonClick += new EventHandler(this._tabMainControl_CloseButtonClick);
            this._tabMainControl.SelectedPageChanged += new TabPageChangedEventHandler(this._tabMainControl_SelectedPageChanged);
            this._dictTabCtroller = new Dictionary<string, TabControlController>();
            this._tabMainControl.Dock = DockStyle.Fill;
            this.panel_main.Controls.Add(this._tabMainControl);
            MyControlHelper.DockControl<XtraTabControl>(this._tabMainControl, this.panel_main, DockStyle.Fill);
            //}
        }
        private void InitButton()
        {
            IHSystemModule ism = new HSystemModuleImp();
            IList<SystemModuleData> sdatalist = ism.GenerateAllList();
            foreach (SystemModuleData moduleData in sdatalist)
            {
                AddQuickChanelButton(moduleData, null, null);
            }
        }
        #region 初始化菜单

        private void InitMenu()
        {
            IMenu menuimp = new MenuImp();
            IList<MenuData> lst = menuimp.GetTreeMenus();
            LoadMainMenus(lst);
        }
        public void DockMainControl(IMainControl mainControl)
        {
            this.ResetToolbar();
            if (mainControl is Control)
            {
                Control control = mainControl as Control;
                string text = control.Text;
                if ((this._tabMainControl != null) && !string.IsNullOrEmpty(text))
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
                    if (this._tabMainControl != null)
                    {
                        this._tabMainControl.Visible = false;
                    }
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
                string str = FileHelper.LicensesData.Rows[0]["useCompanyName"].ToString();
                this.Text = string.Format("{0}-管理系统主程序【{1}】", str, e.Page.Text);
                this.ResetToolbar();
                if (this._dictTabCtroller[e.Page.Text].MainController is BaseManagerLeftMainController)
                {
                    (this._dictTabCtroller[e.Page.Text].MainController as BaseManagerLeftMainController).CreateToolBarButtons();
                }
                else
                {
                    this._dictTabCtroller[e.Page.Text].MainController.CreateToolBarButtons();
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
                this.barManager1.Items.Remove(item);
            }
            this._toolbarList.Clear();
        }
        private void LoadMainMenus(IList<MenuData> menuList)
        {
            barManager1.BeginUpdate();
            foreach (MenuData data in menuList)
            {
                BarSubItem item = new BarSubItem(barManager1, data.menu_name)
                {
                    Tag = data
                };
                barMain.ItemLinks.Insert(0, item);
                item.Glyph = this.LoadImageFormFile(@"images\menus\mainmenu.png");
                if (data.menus != null)
                {
                    this.BuildMainMenu(item, data.menus);
                }
            }
            barManager1.EndUpdate();
        }
        private void BuildMainMenu(BarSubItem barItem, IList<MenuData> menuList)
        {
            foreach (MenuData data in menuList)
            {
                if ((data.menus != null) && (data.menus.Count > 0))
                {
                    BarSubItem item = new BarSubItem(barManager1, data.menu_name)
                    {
                        Tag = data
                    };
                    barItem.AddItem(item);
                    this.BuildMainMenu(item, data.menus);
                }
                else
                {
                    BarLargeButtonItem item3 = new BarLargeButtonItem(barManager1, data.menu_name)
                    {
                        Tag = data,
                        Glyph = this.LoadImageFormFile(data.menu_icon)
                    };
                    if (barItem != null)
                    {
                        barItem.AddItem(item3);
                    }
                    item3.ItemClick += new ItemClickEventHandler(this.barLargeButtonItem_ItemClick);
                }
            }
        }
        private void barLargeButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item.Tag is MenuData)
            {
                MenuData tag = e.Item.Tag as MenuData;
                UserContextManager.userContext.currentMenuId = tag.menu_id.ToString();
                MenuManager.Execute(tag);
            }
        }
        private Image LoadImageFormFile(string fileName)
        {
            Image image = null;
            try
            {
                string path = Path.Combine(Application.StartupPath, fileName);
                if (!File.Exists(path))
                {
                    return null;
                }
                image = Image.FromFile(path);
            }
            catch (Exception)
            {
            }
            return image;
        }

        public void CreateToolBarButton(string name)
        {
            if (!this._toolbarList.Contains(name))
            {
                this._toolbarList.Add(name);
                BarLargeButtonItem item = new BarLargeButtonItem(this.barManager1, name);
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
                //if (!this._toolbarList.Contains(name))
                //{
                this._toolbarList.Add(name);
                linkIndex += 4;
                barLargeButtonItem = new BarLargeButtonItem(this.barManager1, name)
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
                if (this.barMain.ItemLinks.Count <= linkIndex)
                {
                    BarItemLink link = this.barMain.AddItem(barLargeButtonItem);
                    if (isBeginGroup)
                    {
                        link.BeginGroup = true;
                    }
                }
                else
                {
                    BarItemLink beforeLink = this.barMain.ItemLinks[linkIndex];
                    if (beforeLink == null)
                    {
                        this.barMain.AddItem(barLargeButtonItem);
                    }
                    else
                    {
                        this.barMain.InsertItem(beforeLink, barLargeButtonItem);
                    }
                    if (isBeginGroup)
                    {
                        beforeLink.BeginGroup = true;
                    }
                }
                //}
            }
            catch (Exception exception)
            {
                MessageBox.Show("创建工具条异常：" + exception.Message);
            }
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

        public void CreateMenu(IList<MenuData> menuList)
        {
            throw new NotImplementedException();
        }


        public void CreateStatusBar(FormController.StatusBarData statusBarData)
        {
            throw new NotImplementedException();
        }

        public void CreateToolBar(FormController.ToolBarData toolBarData)
        {
            throw new NotImplementedException();
        }

        public void DisplayTimer(string strTime)
        {
            throw new NotImplementedException();
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
        private BarLargeButtonItem SetBarLargeButtonItem(SystemModuleData moduleData, IMainController controller, EventHandler eventHandler)
        {
            BarLargeButtonItem item = new BarLargeButtonItem(this.barManager1, moduleData.moduleName)
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
        public void AddQuickChanelButton(string controllerName, IMainController controller, EventHandler eventHandler, int imageIndex)
        {
        }

        public void AddQuickChanelButton(string controllerName, IMainController controller, EventHandler eventHandler, int imageIndex, string deptid, string moduleid)
        {
            BarLargeButtonItem item = new BarLargeButtonItem(this.barManager1, controllerName)
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
        private object GenerateSystemModule(string moduleid)
        {
            IHSystemModule module = new HSystemModuleImp();
            return module.GenerateById(moduleid);
        }
        private IList<DeptData> GenerateChildDept(string deptid)
        {
            IDept dept = new DeptImp();
            return dept.LoadDeptListByParentId(deptid);
        }
        #endregion

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
    }
}