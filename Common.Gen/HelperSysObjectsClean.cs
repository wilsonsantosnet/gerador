using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    [Obsolete("Utilize a classe HelperSysObjectsTableModel, essa classe será eliminada")]
    public class HelperSysObjectsClean : HelperSysObjectsBase
    {
        public HelperSysObjectsClean(IEnumerable<Context> contexts)
        {
            this.Contexts = contexts;
            PathOutput.UsePathProjects = true;
        }

        public override void DefineTemplateByTableInfoFields(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            new HelperSysObjectsTableModel(config, "Templates").DefineTemplateByTableInfoFields(config, tableInfo, infos);
        }

        public override void DefineTemplateByTableInfo(Context config, TableInfo tableInfo)
        {
        }

    }
}
