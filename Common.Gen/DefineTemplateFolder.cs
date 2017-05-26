using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class DefineTemplateFolder
    {
        private string _templatePathBase;

        public DefineTemplateFolder()
        {
            _templatePathBase = GetDefaultTemplateFolder();
        }

        public string GetDefaultTemplateFolder()
        {
            return "Templates";
        }

        public void SetTemplatePathBase(string templatePathBase)
        {
            _templatePathBase = templatePathBase;
        }

        public string Define()
        {
            return _templatePathBase;
        }

        public string Define(TableInfo tableInfo)
        {

            if (tableInfo.ModelBase)
                return "Templates\\Base";

            if (tableInfo.InheritQuery)
                return "Templates\\inherit";

            if (tableInfo.ModelBaseWithoutGets)
                return "Templates\\withoutgets";

            if (!tableInfo.Scaffold)
                return "Templates\\withoutScaffold";


            return Define();

        }

    }
}
