using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public enum ArquitetureType {

        TableModel,
        DDD,
        TransactionScript

    }
    public class Context
    {

        public Context()
        {
            this.Arquiteture = ArquitetureType.TableModel;
            this.ClearAllFiles = false;
            this.MakeNavigationPropertys = false;
            this.DeleteFilesNotFoundTable = false;
            this.MakeFront = false;
            this.ShowKeysInFront = false;
            this.LengthBigField = 150;
        }

        #region propertys



        private string _namespace;

        private string _module;

        private string _contextName;

        public bool ShowKeysInFront { get; set; }

        public int LengthBigField { get; set; }

        public ArquitetureType Arquiteture { get; set; }

        public string Company { get; set; }

        public string Namespace
        {
            get
            {

                if (!String.IsNullOrEmpty(_namespace) && !String.IsNullOrEmpty(Module))
                    return string.Format("{0}.{1}", _namespace, Module);

                return _namespace;

            }
            set { _namespace = value; }
        }

        public string NamespaceRoot
        {
            get { return _namespace; }
        }

        public string NamespaceDomainSource
        {
            get { 

                if (!String.IsNullOrEmpty(_namespace) && !String.IsNullOrEmpty(DomainSource))
                    return string.Format("{0}.{1}", _namespace, DomainSource);

                return _namespace;
            
            }
        }

        public string ProjectName { get; set; }

        public bool ClearAllFiles { get; set; }

        public bool DeleteFilesNotFoundTable { get; set; }

        public bool AlertNotFoundTable { get; set; }

        public bool MakeNavigationPropertys { get; set; }

        public string DomainSource { get; set; }

        public bool MakeFront { get; set; }

        public bool CamelCasing { get; set; }

        public string Module
        {
            get
            {
                if (_module == null)
                    return string.Empty;
                return _module;

            }
            set { _module = value; }
        }

        public string ContextName
        {
            get
            {
                if (_contextName == null)
                    return this.Module;
                return _contextName;

            }
            set { _contextName = value; }
        }

        public string OutputAngular { get; set; }

        public string ConnectionString { get; set; }

        public string OutputClassDomain { get; set; }

        public string OutputClassApp { get; set; }

        public string OutputClassUri { get; set; }

        public string OutputClassTestsApp { get; set; }

        public string OutputClassTestsApi { get; set; }

        public string OutputClassApi { get; set; }

        public string OutputClassDto { get; set; }

        public string OutputClassSummary { get; set; }
        
        public string OutputClassFilter { get; set; }

        public string OutputClassInfra { get; set; }


        public List<TableInfo> TableInfo { get; set; }




        #endregion



    }
}
