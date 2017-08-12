using DevExpress.XtraBars;

namespace Run
{
    internal class ButtonBarItem
    {
        private ItemClickEventHandler itemClickEventHandler;
        private BarManager manager;
        private string v1;
        private int v2;

        public ButtonBarItem(BarManager manager, string v1, int v2, ItemClickEventHandler itemClickEventHandler)
        {
            this.manager = manager;
            this.v1 = v1;
            this.v2 = v2;
            this.itemClickEventHandler = itemClickEventHandler;
        }
    }
}