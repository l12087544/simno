using Simno.BaseData.Base;
using Simno.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simno
{
    public class MenuCommand : AbstractCommand
    {
        public MenuCommand()
        {
        }

        public MenuCommand(IUserContext userContext) : this(userContext, null)
        {
        }

        public MenuCommand(IUserContext userContext, object[] args)
        {
            base._userContext = userContext;
            base.args = args;
        }

        public override bool Execute(object sender)
        {
            using (MenuFrm frm = new MenuFrm())
            {
                frm.ShowDialog();
            }
            return base.Execute(sender);
        }
    }
}

