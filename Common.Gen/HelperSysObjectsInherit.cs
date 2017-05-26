using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class HelperSysObjectsInherit : HelperSysObjectsTableModel
    {

        private DefineTemplateFolder _defineTemplateFolder;
        public HelperSysObjectsInherit(IEnumerable<Context> contexts) : base(contexts)
        {
            this.Contexts = contexts;
            PathOutput.UsePathProjects = true;
            this._defineTemplateFolder = new DefineTemplateFolder();
        }



    }
}
