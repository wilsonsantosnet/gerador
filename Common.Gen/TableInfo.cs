using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{

    public class HtmlCtrl
    {

        public string htmlField { get; set; }
        public string htmlFilter { get; set; }

    }

    public static class FieldsHtml
    {

        public static HtmlCtrl radio(Dictionary<string, string> dataItem, string name)
        {
            var htmlField = radioMake(dataItem, name, string.Format("vm.Model.{0}", name));
            var htmlFilter = radioMake(dataItem, name, string.Format("vm.crud.ModelFilter.{0}", name));

            return new HtmlCtrl
            {
                htmlField = htmlField,
                htmlFilter = htmlFilter
            };

        }

        private static string radioMake(Dictionary<string, string> dataItem, string name, string model)
        {
            var html = string.Empty;
            foreach (var item in dataItem)
            {
                html += "<div class='radio'><label><input type='radio' name='" + name + "' value='" + item.Key + "' ng-model='" + model + "'>" + item.Value + "</label></div>";
            }

            return html;
        }

    }
    public enum TypeCtrl
    {
        Radio
    }

    public class FieldConfig
    {
        public FieldConfig init(TypeCtrl type)
        {
            this.TypeCtrl = type;
            switch (this.TypeCtrl)
            {
                case TypeCtrl.Radio:
                    this.HTML = FieldsHtml.radio(this.DataItem, this.Name);
                    break;
                default:
                    break;
            }

            return this;
        }
        public FieldConfig()
        {
            this.Create = true;
            this.Edit = true;
            this.List = true;
            this.Details = true;
            this.Filter = true;
            this.DataItem = new Dictionary<string, string>();
            this.upload = false;
        }

        public TypeCtrl TypeCtrl { get; set; }
        public string Name { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool List { get; set; }
        public bool Details { get; set; }
        public bool Filter { get; set; }
        public int Order { get; set; }
        public HtmlCtrl HTML { get; set; }
        public Dictionary<string, string> DataItem { get; set; }
        public bool upload { get; set; }
        public bool SelectSearch { get; set; }
        public bool TextEditor { get; set; }


    }

    public class TableInfo
    {
        public TableInfo()
        {
            this.CodeCustomImplemented = false;
            this.MakeCrud = false;
            this.MakeApp = false;
            this.MakeApi = false;
            this.MakeDomain = false;
            this.MakeTest = false;
            this.MakeFront = false;
            this.Scaffold = true;
            this.twoCols = false;
            this.Authorize = true;

        }

        public bool Authorize { get; set; }

        public List<FieldConfig> FieldsConfig { get; set; }

        public bool InheritQuery { get; set; }

        public bool ModelBase { get; set; }

        public bool ModelBaseWithoutGets { get; set; }

        private string _InheritClassName;

        private string _tableName;

        public string InheritClassName
        {
            get
            {
                return _InheritClassName.IsSent() ? _InheritClassName : this.ClassName;
            }
            set { _InheritClassName = value; }
        }

        public string BoundedContext { get; set; }

        public bool MakeFront { get; set; }

        public bool Scaffold { get; set; }

        public string TableName
        {
            get
            {
                return this._tableName.IsNull() ? this.ClassName : this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }

        public string ClassNameFormated { get; set; }

        public string ClassName { get; set; }

        public string ToolsName { get; set; }

        public bool IsCompositeKey { get { return Keys != null ? Keys.Count() > 1 : false; } }

        public IEnumerable<string> Keys { get; set; }

        public IEnumerable<string> KeysTypes { get; set; }

        public bool MakeCrud { get; set; }

        public bool MakeTest { get; set; }

        public bool MakeApp { get; set; }

        public bool MakeApi { get; set; }

        public bool MakeDomain { get; set; }

        public bool MakeSummary { get; set; }

        public bool MakeDto { get; set; }

        public bool CodeCustomImplemented { get; set; }

        public IEnumerable<Info> ReletedClasss { get; set; }

        public bool twoCols { get; set; }


        #region Obsolet

        public string ClassNameRigth { get; set; }
        public string TableHelper { get; set; }
        public string LeftKey { get; set; }
        public string RightKey { get; set; }


        #endregion

    }
}
