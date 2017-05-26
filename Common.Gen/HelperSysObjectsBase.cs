using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using Common.Infrastructure.Log;
using System.Reflection;
using System.Data.Entity;

namespace Common.Gen
{
    public abstract class HelperSysObjectsBase
    {

        public HelperSysObjectsBase()
        {
            this._camelCasingExceptions = new List<string> { "cpfcnpj", "cnpj", "cpf", "cep", "cpf_cnpj" };
        }
        protected IEnumerable<string> _camelCasingExceptions;
        protected SqlConnection conn { get; set; }
        public IEnumerable<Context> Contexts { get; set; }

        public abstract void DefineTemplateByTableInfoFields(Context config, TableInfo tableInfo, UniqueListInfo infos);
        public abstract void DefineTemplateByTableInfo(Context config, TableInfo tableInfo);

        protected ArquitetureType ArquitetureType;
        protected virtual ArquitetureType getArquitetureType()
        {
            return this.ArquitetureType;
        }

        public virtual void MakeClass(Context config)
        {
            MakeClass(config, string.Empty, true);
        }
        public virtual void MakeClass(Context config, bool UsePathProjects)
        {
            MakeClass(config, string.Empty, UsePathProjects);
        }
        public virtual void MakeClass(Context config, string RunOnlyThisClass, bool UsePathProjects)
        {
            PathOutput.UsePathProjects = UsePathProjects;
            ExecuteTemplateByTableInfoFields(config, RunOnlyThisClass);
            ExecuteTemplatesByTableleInfo(config);
            this.Dispose();

        }
        protected virtual string MakeClassName(TableInfo tableInfo)
        {
            return tableInfo.TableName;
        }
        protected virtual string MakePropertyName(string column, string className, int key)
        {

            if (column.ToLower() == "id")
                return string.Format("{0}Id", className);


            if (column.ToString().ToLower().StartsWith("id"))
            {
                var keyname = column.ToString().Replace("Id", "");
                return string.Format("{0}Id", keyname);
            }

            return column;
        }
        public void SetCamelCasingExceptions(IEnumerable<string> list)
        {
            this._camelCasingExceptions = list;
        }
        public void AddCamelCasingExceptions(string value)
        {
            var newList = new List<string>();
            var defaultList = this._camelCasingExceptions;
            newList.AddRange(defaultList);
            newList.Add(value);
            this._camelCasingExceptions = newList;
        }

        protected virtual string CamelCasing(string str)
        {
            var ex = this._camelCasingExceptions.Where(_ => _.ToUpper() == str.ToUpper()).SingleOrDefault();
            if (ex.IsNotNull())
                return ex;

            return str.FisrtCharToLower();

        }


        protected virtual string GenericTagsTransformer(TableInfo tableInfo, Context configContext, string classBuilder)
        {
            var IDomain = tableInfo.MakeCrud ? "IDomainCrud" : "IDomain";
            var keyName = tableInfo.Keys != null ? tableInfo.Keys.FirstOrDefault() : string.Empty;

            classBuilder = classBuilder.Replace("<#namespaceRoot#>", configContext.NamespaceRoot);
            classBuilder = classBuilder.Replace("<#namespace#>", configContext.Namespace);
            classBuilder = classBuilder.Replace("<#domainSource#>", configContext.DomainSource);
            classBuilder = classBuilder.Replace("<#namespaceDomainSource#>", configContext.NamespaceDomainSource);
            classBuilder = classBuilder.Replace("<#className#>", tableInfo.ClassName);
            classBuilder = classBuilder.Replace("<#classNameLower#>", tableInfo.ClassName.ToLowerCase());
            classBuilder = classBuilder.Replace("<#classNameFormated#>", tableInfo.ClassNameFormated);
            classBuilder = classBuilder.Replace("<#inheritClassName#>", tableInfo.InheritClassName);
            classBuilder = classBuilder.Replace("<#boundedContext#>", tableInfo.BoundedContext);
            classBuilder = classBuilder.Replace("<#KeyName#>", keyName);
            classBuilder = classBuilder.Replace("<#KeyType#>", tableInfo.KeysTypes != null ? tableInfo.KeysTypes.FirstOrDefault() : string.Empty);
            classBuilder = classBuilder.Replace("<#module#>", configContext.Module);
            classBuilder = classBuilder.Replace("<#IDomain#>", IDomain);
            classBuilder = classBuilder.Replace("<#KeyNames#>", MakeKeysFromGetContext(tableInfo));
            classBuilder = classBuilder.Replace("<#tablename#>", tableInfo.TableName);
            classBuilder = classBuilder.Replace("<#contextName#>", configContext.ContextName);
            classBuilder = classBuilder.Replace("<#contextNameLower#>", configContext.ContextName.ToLower());
            classBuilder = classBuilder.Replace("<#WhereSingle#>", MakeKeysFromGet(tableInfo));
            classBuilder = classBuilder.Replace("<#orderByKeys#>", OrderyByKeys(tableInfo));
            classBuilder = classBuilder.Replace("<#toolName#>", ToolName(tableInfo));
            classBuilder = classBuilder.Replace("<#company#>", configContext.Company);

            classBuilder = MakeReletedNamespace(tableInfo, configContext, classBuilder);

            return classBuilder;
        }
        protected string BuilderPropertys(IEnumerable<Info> infos, string textTemplatePropertys, bool includeUserCreateDate)
        {
            var classBuilderPropertys = string.Empty;
            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (Audit.GetAuditFields().Contains(item.PropertyName) && !includeUserCreateDate)
                        continue;

                    var itempropert = textTemplatePropertys.
                            Replace("<#type#>", item.Type).
                            Replace("<#propertyName#>", item.PropertyName);
                    classBuilderPropertys += string.Format("{0}{1}{2}", Tabs.TabModels(), itempropert, System.Environment.NewLine);

                }
            }

            return classBuilderPropertys;
        }
        protected string BuilderSampleFilters(IEnumerable<Info> infos, string textTemplateFilters, string classBuilderFilters)
        {
            if (infos.IsAny())
            {

                foreach (var item in infos)
                {


                    var itemFilters = string.Empty;

                    if (item.Type == "string")
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0}.Contains(filters.{0})", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }
                    else if (item.Type == "DateTime")
                    {
                        var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_=>_.{0} >= filters.{0}Start ", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                        var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_=>_.{0}  <= filters.{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                        itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, Tabs.TabSets(), itemFiltersEnd, System.Environment.NewLine);

                    }
                    else if (item.Type == "DateTime?")
                    {
                        var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value >= filters.{0}Start.Value", item.PropertyName));
                        itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                        var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_=>_.{0} != null &&  _.{0}.Value <= filters.{0}End", item.PropertyName));
                        itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.Value.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                        itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, Tabs.TabSets(), itemFiltersEnd, System.Environment.NewLine);

                    }
                    else if (item.Type == "bool?")
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }
                    else if (item.Type == "int?" || item.Type == "Int64?" || item.Type == "Int16?" || item.Type == "decimal?" || item.Type == "float?")
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }
                    else
                    {
                        itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                        itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} == filters.{0}", item.PropertyName));
                        itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                    }


                    classBuilderFilters += string.Format("{0}{1}{2}", Tabs.TabSets(), itemFilters, System.Environment.NewLine);

                }
            }

            return classBuilderFilters;
        }
        protected string ToolName(TableInfo tableInfo)
        {
            return !string.IsNullOrEmpty(tableInfo.ToolsName) ? string.Format("base.toolName = {0};", tableInfo.ToolsName) : string.Format("base.toolName = {0};", "string.Empty");
        }
        protected virtual string MakeKeysFromGet(TableInfo tableInfo, string classFilter = "model")
        {
            var keys = string.Empty;
            if (tableInfo.Keys.IsAny())
            {
                foreach (var item in tableInfo.Keys)
                    keys += string.Format(".Where(_=>_.{0} == {1}.{2})", item, classFilter, item);
            }
            return keys;
        }

        protected virtual string OrderyByKeys(TableInfo tableInfo, string classFilter = "model")
        {
            var keys = string.Empty;
            if (tableInfo.Keys.IsAny())
            {
                var index = 0;
                foreach (var item in tableInfo.Keys)
                {
                    var orderClause = "OrderBy";
                    if (index > 0)
                        orderClause = "ThenBy";

                    keys += string.Format(".{0}(_ => _.{1})", orderClause, item);
                    index++;
                }
            }
            return keys;
        }
        protected string MakeClassBuilderMapORM(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos, string textTemplateClass, string textTemplateLength, string textTemplateRequired, string textTemplateMapper, string textTemplateManyToMany, string textTemplateCompositeKey, ref string classBuilderitemTemplateLength, ref string classBuilderitemTemplateRequired, ref string classBuilderitemplateMapper, ref string classBuilderitemplateMapperKey, ref string classBuilderitemplateManyToMany, ref string classBuilderitemplateCompositeKey)
        {
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            classBuilderitemplateCompositeKey = MakeKey(infos, textTemplateCompositeKey, classBuilderitemplateCompositeKey);

            if (infos.IsAny())
            {

                foreach (var item in infos)
                {

                    if (IsString(item) && IsNotVarcharMax(item))
                    {
                        var itemTemplateLength = textTemplateLength
                            .Replace("<#propertyName#>", item.PropertyName)
                            .Replace("<#length#>", item.Length);

                        classBuilderitemTemplateLength += string.Format("{0}{1}{2}", Tabs.TabMaps(), itemTemplateLength, System.Environment.NewLine);
                    }

                    if (item.isNullable == 0)
                    {
                        var itemTemplateRequired = textTemplateRequired
                           .Replace("<#propertyName#>", item.PropertyName);

                        classBuilderitemTemplateRequired += string.Format("{0}{1}{2}", Tabs.TabMaps(), itemTemplateRequired, System.Environment.NewLine);
                    }

                    if (item.IsKey == 1)
                    {
                        var itemplateMapper = textTemplateMapper
                            .Replace("<#propertyName#>", item.PropertyName)
                            .Replace("<#columnName#>", item.ColumnName);

                        classBuilderitemplateMapperKey += string.Format("{0}{1}{2}", Tabs.TabMaps(), itemplateMapper, System.Environment.NewLine);

                    }
                    else
                    {

                        var itemplateMapper = textTemplateMapper
                           .Replace("<#propertyName#>", item.PropertyName)
                           .Replace("<#columnName#>", item.ColumnName);

                        classBuilderitemplateMapper += string.Format("{0}{1}{2}", Tabs.TabMaps(), itemplateMapper, System.Environment.NewLine);
                    }

                }

            }

            if (!string.IsNullOrEmpty(tableInfo.TableHelper))
            {

                var itemTemplateManyToMany = textTemplateManyToMany
                      .Replace("<#propertyNavigationLeft#>", tableInfo.ClassNameRigth)
                      .Replace("<#propertyNavigationRigth#>", tableInfo.ClassName)
                      .Replace("<#MapLeftKey#>", tableInfo.LeftKey)
                      .Replace("<#MapRightKey#>", tableInfo.RightKey)
                      .Replace("<#TableHelper#>", tableInfo.TableHelper);

                classBuilderitemplateManyToMany += string.Format("{0}{1}{2}", Tabs.TabMaps(), itemTemplateManyToMany, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#IsRequired#>", classBuilderitemTemplateRequired);
            classBuilder = classBuilder.Replace("<#HasMaxLength#>", classBuilderitemTemplateLength);
            classBuilder = classBuilder.Replace("<#Mapper#>", classBuilderitemplateMapper);
            classBuilder = classBuilder.Replace("<#keyName#>", classBuilderitemplateMapperKey);
            classBuilder = classBuilder.Replace("<#ManyToMany#>", classBuilderitemplateManyToMany);
            classBuilder = classBuilder.Replace("<#CompositeKey#>", classBuilderitemplateCompositeKey);
            return classBuilder;
        }
        protected bool IsRequired(Info item)
        {
            return item.isNullable == 0;
        }
        protected bool IsPropertyInstance(TableInfo tableInfo, string propertyName)
        {
            var classBuilderNavPropertys = string.Empty;
            if (tableInfo.ReletedClasss.IsNotNull())
            {
                foreach (var item in tableInfo.ReletedClasss)
                {
                    if (item.NavigationType == NavigationType.Instance && item.PropertyNameFk == propertyName)
                        return true;
                }
            }

            return false;
        }

        protected string PropertyInstance(TableInfo tableInfo, string propertyName)
        {
            var classBuilderNavPropertys = string.Empty;
            if (tableInfo.ReletedClasss.IsNotNull())
            {
                foreach (var item in tableInfo.ReletedClasss)
                {
                    if (item.NavigationType == NavigationType.Instance && item.PropertyNameFk == propertyName)
                        return item.ClassName;
                }
            }

            return string.Empty;
        }
        protected string MakePropertyNavigationDto(TableInfo tableInfo, Context configContext, string TextTemplateNavPropertysCollection, string TextTemplateNavPropertysInstace, string classBuilder)
        {
            var classBuilderNavPropertys = string.Empty;

            if (tableInfo.ReletedClasss.IsNotNull())
            {
                foreach (var item in tableInfo.ReletedClasss)
                {

                    var TextTemplateNavPropertys = item.NavigationType == NavigationType.Instance ? TextTemplateNavPropertysInstace : TextTemplateNavPropertysCollection;


                    if (item.ClassName == tableInfo.ClassName)
                        classBuilderNavPropertys += String.Format("{0}{1}", Tabs.TabModels(), TextTemplateNavPropertys
                            .Replace("<#className#>", item.ClassName)
                            .Replace("<#classNameNav#>", String.Format("{0}Self", item.ClassName)));

                    if (item.ClassName != tableInfo.ClassName)
                        classBuilderNavPropertys += String.Format("{0}{1}", Tabs.TabModels(), TextTemplateNavPropertys
                            .Replace("<#className#>", item.ClassName)
                            .Replace("<#classNameNav#>", item.ClassName));


                }
            }

            classBuilder = classBuilder
                .Replace("<#propertysNav#>", classBuilderNavPropertys);

            return classBuilder;
        }
        protected string MakePropertyNavigationModels(TableInfo tableInfo, Context configContext, string TextTemplateNavPropertysCollection, string TextTemplateNavPropertysInstace, string classBuilder)
        {
            if (!configContext.MakeNavigationPropertys)
                return classBuilder
                .Replace("<#propertysNav#>", string.Empty);

            var classBuilderNavPropertys = string.Empty;
            var usingPropertysNav = string.Empty;

            foreach (var item in tableInfo.ReletedClasss)
            {
                var TextTemplateNavPropertys = item.NavigationType == NavigationType.Instance ? TextTemplateNavPropertysInstace : TextTemplateNavPropertysCollection;

                if (item.ClassName == tableInfo.ClassName)
                    classBuilderNavPropertys += String.Format("{0}{1}", Tabs.TabModels(), TextTemplateNavPropertys
                        .Replace("<#className#>", item.ClassName)
                        .Replace("<#classNameNav#>", String.Format("{0}Self", item.ClassName)));

                if (item.ClassName != tableInfo.ClassName)
                    classBuilderNavPropertys += String.Format("{0}{1}", Tabs.TabModels(), TextTemplateNavPropertys
                        .Replace("<#className#>", item.ClassName)
                        .Replace("<#classNameNav#>", item.ClassName));


                if (item.Module != configContext.Module)
                {
                    var usingReference = String.Format("using {0}.Domain;{1}", item.Namespace, System.Environment.NewLine);
                    if (!usingPropertysNav.Contains(usingReference))
                        usingPropertysNav += usingReference;
                }

            }

            classBuilder = classBuilder
                .Replace("<#propertysNav#>", classBuilderNavPropertys);

            return classBuilder;

        }
        protected string MakeFilterDateRange(string TextTemplatePropertys, string classBuilderPropertys, Info item)
        {
            if (item.Type == "DateTime" || item.Type == "DateTime?")
            {
                classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, string.Format("{0}Start", item.PropertyName), item.Type);
                classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, string.Format("{0}End", item.PropertyName), item.Type);
            }
            return classBuilderPropertys;
        }
        protected string AddPropertyFilter(string TextTemplatePropertys, string classBuilderPropertys, Info item, string propertyName, string type)
        {
            var itempropert = TextTemplatePropertys.
                    Replace("<#type#>", type).
                    Replace("<#propertyName#>", propertyName);

            classBuilderPropertys += string.Format("{0}{1}{2}", Tabs.TabModels(), itempropert, System.Environment.NewLine);
            return classBuilderPropertys;
        }
        protected virtual string MakeValidationsAttributes(string TextTemplateValidation, string TextTemplateValidationLength, string TextTemplateValidationRequired, string classBuilderitemTemplateValidation, Info item)
        {
            if (IsRequired(item) && IsString(item) && IsNotVarcharMax(item))
            {
                var itemTemplateValidationLegth = TextTemplateValidationLength
                    .Replace("<#Length#>", item.Length)
                    .Replace("<#className#>", item.ClassName)
                    .Replace("<#propertyName#>", item.PropertyName);

                var itemTemplateValidationRequired = TextTemplateValidationRequired
                   .Replace("<#propertyName#>", item.PropertyName)
                   .Replace("<#className#>", item.ClassName);

                var itemTemplateValidation = TextTemplateValidation.Replace("<#propertyName#>", item.PropertyName).Replace("<#type#>", item.Type).Replace("<#tab#>", Tabs.TabModels());
                itemTemplateValidation = itemTemplateValidation.Replace("<#RequiredValidation#>", Tabs.TabModels(itemTemplateValidationRequired));
                itemTemplateValidation = itemTemplateValidation.Replace("<#MaxLengthValidation#>", Tabs.TabModels(itemTemplateValidationLegth));

                classBuilderitemTemplateValidation += string.Format("{0}{1}", itemTemplateValidation, System.Environment.NewLine);
                return classBuilderitemTemplateValidation;
            }

            if (IsRequired(item) && IsString(item) && IsVarcharMax(item))
            {

                var itemTemplateValidationRequired = TextTemplateValidationRequired
                   .Replace("<#propertyName#>", item.PropertyName)
                   .Replace("<#className#>", item.ClassName);

                var itemTemplateValidation = TextTemplateValidation.Replace("<#propertyName#>", item.PropertyName).Replace("<#type#>", item.Type).Replace("<#tab#>", Tabs.TabModels());
                itemTemplateValidation = itemTemplateValidation.Replace("<#RequiredValidation#>", Tabs.TabModels(itemTemplateValidationRequired));
                itemTemplateValidation = RemoveLine(itemTemplateValidation, "<#MaxLengthValidation#>");


                classBuilderitemTemplateValidation += string.Format("{0}{1}", itemTemplateValidation, System.Environment.NewLine);
                return classBuilderitemTemplateValidation;
            }

            if (IsRequired(item) && IsNotString(item))
            {
                var itemTemplateValidationLegth = TextTemplateValidationLength
                    .Replace("<#Length#>", item.Length)
                    .Replace("<#propertyName#>", item.PropertyName)
                    .Replace("<#className#>", item.ClassName);


                var itemTemplateValidationRequired = TextTemplateValidationRequired
                   .Replace("<#propertyName#>", item.PropertyName)
                   .Replace("<#className#>", item.ClassName);

                var itemTemplateValidation = TextTemplateValidation.Replace("<#propertyName#>", item.PropertyName).Replace("<#type#>", item.Type).Replace("<#tab#>", Tabs.TabModels());
                itemTemplateValidation = itemTemplateValidation.Replace("<#RequiredValidation#>", Tabs.TabModels(itemTemplateValidationRequired));
                itemTemplateValidation = RemoveLine(itemTemplateValidation, "<#MaxLengthValidation#>");

                classBuilderitemTemplateValidation += string.Format("{0}{1}", itemTemplateValidation, System.Environment.NewLine);
                return classBuilderitemTemplateValidation;
            }

            if (!IsRequired(item) && IsString(item) && IsNotVarcharMax(item))
            {
                var itemTemplateValidationLegth = TextTemplateValidationLength
                    .Replace("<#Length#>", item.Length)
                    .Replace("<#propertyName#>", item.PropertyName)
                    .Replace("<#className#>", item.ClassName);

                var itemTemplateValidation = TextTemplateValidation.Replace("<#propertyName#>", item.PropertyName).Replace("<#type#>", item.Type).Replace("<#tab#>", Tabs.TabModels());
                itemTemplateValidation = RemoveLine(itemTemplateValidation, "<#RequiredValidation#>");
                itemTemplateValidation = itemTemplateValidation.Replace("<#MaxLengthValidation#>", Tabs.TabModels(itemTemplateValidationLegth));

                classBuilderitemTemplateValidation += string.Format("{0}{1}", itemTemplateValidation, System.Environment.NewLine);
                return classBuilderitemTemplateValidation;
            }

            return classBuilderitemTemplateValidation;
        }
        protected bool IsString(Info item)
        {
            return item.Type == "string";
        }
        protected bool IsNotVarcharMax(Info item)
        {
            return !IsVarcharMax(item);
        }
        protected bool IsVarcharMax(Info item)
        {
            return item.Length.Contains("-1");
        }
        protected bool IsStringLengthBig(Info info, Context configContext)
        {
            return Convert.ToInt32(info.Length) > configContext.LengthBigField || Convert.ToInt16(info.Length) == -1;
        }
        protected string DefineMoqMethd(string type)
        {

            switch (type.ToLower())
            {
                case "string":
                case "nchar":
                    return "MakeStringValueSuccess";
                case "int":
                case "int?":
                    return "MakeIntValueSuccess";
                case "decimal":
                case "decimal?":
                case "money":
                    return "MakeDecimalValueSuccess";
                case "float?":
                case "float":
                    return "MakeFloatValueSuccess";
                case "datetime":
                case "datetime?":
                    return "MakeDateTimeValueSuccess";
                case "bool":
                case "bool?":
                    return "MakeBoolValueSuccess";
                default:
                    break;
            }


            throw new InvalidOperationException("tipo não implementado");

        }
        protected string MakeReletedIntanceValues(TableInfo tableInfo, Context configContext, string TextTemplateReletedValues, string classBuilder)
        {
            var classBuilderReletedValues = string.Empty;

            foreach (var item in tableInfo.ReletedClasss.Where(_ => _.NavigationType == NavigationType.Instance))
            {
                var itemvalue = TextTemplateReletedValues.
                       Replace("<#className#>", item.Table).
                       Replace("<#FKeyName#>", item.PropertyNameFk).
                       Replace("<#KeyName#>", item.PropertyNamePk);

                classBuilderReletedValues += string.Format("{0}{1}", itemvalue, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#reletedValues#>", classBuilderReletedValues);
            classBuilder = MakeReletedNamespace(tableInfo, configContext, classBuilder);

            return classBuilder;
        }
        protected string MakeKFilterByModel(TableInfo tableInfo)
        {
            var keys = string.Empty;
            if (tableInfo.Keys.Count() > 0)
            {
                foreach (var item in tableInfo.Keys)
                    keys += string.Format("{0} = first.{1},", item, item);
            }
            return keys;
        }

        protected string MakeComposedKeysFilter(TableInfo tableInfo, bool camelCasing)
        {
            var filter = "{";
            if (tableInfo.Keys.Count() > 0)
            {
                foreach (var item in tableInfo.Keys)
                {
                    var key = camelCasing ? CamelCasing(item) : item;
                    filter += string.Format(" {0}: item.{1},", key, key);
                }
            }
            filter = filter.Substring(0, filter.Length - 1) + " }";
            return filter;
        }

        #region Helpers

        private string MakePropertyName(string column, string className)
        {
            return MakePropertyName(column, className, 0);
        }
        private string MakeClassNameTemplateDefault(string tableName)
        {
            var result = tableName.Substring(7);
            result = CamelCaseTransform(result);
            result = ClearEnd(result);
            return result;
        }
        private bool Open(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.conn.Open();
            return true;
        }
        private string MakePropertyNameForId(TableInfo tableConfig)
        {
            var propertyName = string.Format("{0}Id", tableConfig.ClassName);
            return propertyName;
        }
        private string MakePropertyNameDefault(string columnName)
        {

            var newcolumnName = columnName.Substring(4);

            newcolumnName = TranslateNames(newcolumnName);

            newcolumnName = CamelCaseTransform(newcolumnName);

            newcolumnName = ClearEnd(newcolumnName);

            return newcolumnName;

        }
        private string ClearEnd(string value)
        {
            value = value.Replace("_X_", "");
            value = value.Replace("_", "");
            value = value.Replace("-", "");
            return value;
        }
        private string TranslateNames(string newcolumnName)
        {

            newcolumnName = newcolumnName.Contains("_cd_") ? String.Concat(newcolumnName.Replace("_cd_", "_"), "_Id_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_nm_") ? String.Concat(newcolumnName.Replace("_nm_", "_"), "_Nome_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_ds_") ? String.Concat(newcolumnName.Replace("_ds_", "_"), "_Descricao_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_nr_") ? String.Concat(newcolumnName.Replace("_nr_", "_"), "_Numero_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_pc_") ? String.Concat(newcolumnName.Replace("_pc_", "_"), "_Porcentagem_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_qt_") ? String.Concat(newcolumnName.Replace("_qt_", "_"), "_Quantidade_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_dt_") ? String.Concat(newcolumnName.Replace("_dt_", "_"), "_Data_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_fl_") ? String.Concat(newcolumnName.Replace("_fl_", "_"), "_Flag_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_vl_") ? String.Concat(newcolumnName.Replace("_vl_", "_"), "_Valor_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_in_") ? String.Concat(newcolumnName.Replace("_in_", "_"), "") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_sg_") ? String.Concat(newcolumnName.Replace("_sg_", "_"), "_Sigla_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_tp_") ? String.Concat(newcolumnName.Replace("_tp_", "_"), "_Tipo_") : newcolumnName;

            return newcolumnName;
        }
        private string TranslateNamesPatnerX(string newcolumnName)
        {


            newcolumnName = newcolumnName.Contains("_X_") ? String.Concat(newcolumnName.Replace("_X_", ""), "") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_cd") ? String.Concat(newcolumnName.Replace("_cd", ""), "_Id") : newcolumnName;

            return newcolumnName;
        }
        private string CamelCaseTransform(string newcolumnName)
        {

            newcolumnName = string.Concat(newcolumnName
                           .Split('_')
                           .Where(_ => !string.IsNullOrEmpty(_))
                           .Select(y => y.Substring(0, 1).ToUpper() + y.Substring(1) + "_"));



            return newcolumnName;
        }
        private string makePrefixTable(TableInfo tableConfig)
        {
            if (!tableConfig.TableName.Contains("_X_"))
            {
                var prefix = tableConfig.TableName.Split('_')[1];
                return prefix;
            }
            return string.Empty;
        }
        private string makePrefixField(string columnName)
        {
            return columnName.Split('_')[0];
        }
        protected string RemoveLine(string itemTemplateValidation, string targetField)
        {
            return itemTemplateValidation.Replace(targetField + System.Environment.NewLine, string.Empty);
        }
        private string GetModuleFromContextByTableName(string tableName, string module)
        {
            var result = this.Contexts
                .Where(_ => _.Module == module)
                .Where(_ => _.TableInfo
                    .Where(__ => __.TableName.Equals(tableName)).Any())
                .Select(_ => _.Module).FirstOrDefault();
            return result;
        }
        private string GetNameSpaceFromContextByTableName(string tableName, string module)
        {
            var _namespace = this.Contexts
                .Where(_ => _.Module == module)
                .Where(_ => _.TableInfo
                    .Where(__ => __.TableName.Equals(tableName)).Any())
                .Select(_ => _.Namespace).FirstOrDefault();
            return _namespace;
        }
        private string GetNameSpaceFromContextWitExposeParametersByTableName(string tableName, string module)
        {
            var namespaceApp = this.Contexts
                .Where(_ => _.Module == module)
                .Where(_ => _.TableInfo
                    .Where(__ => __.TableName.Equals(tableName))
                    .Where(___ => ___.MakeApp || ___.MakeFront || ___.MakeFront)
                    .Any())
                .Select(_ => _.Namespace).FirstOrDefault();
            return namespaceApp;
        }
        private string GetNameSpaceFromContextWithMakeDtoByTableName(string tableName, string module)
        {
            var namespaceDto = this.Contexts
               .Where(_ => _.Module == module)
               .Where(_ => _.TableInfo
                   .Where(__ => __.TableName.Equals(tableName))
                   .Where(___ => ___.MakeDto)
                   .Any())
               .Select(_ => _.Namespace).FirstOrDefault();
            return namespaceDto;
        }
        private bool AppExpose(string namespaceApp)
        {
            return !string.IsNullOrEmpty(namespaceApp);
        }
        private IEnumerable<Info> GetReletaedIntancesComplementedClasses(Context config, string currentTableName)
        {

            var commandText = new StringBuilder();


            commandText.Append("SELECT ");
            commandText.Append("KCU1.CONSTRAINT_NAME AS 'FK_Nome_Constraint' ");
            commandText.Append(",KCU1.TABLE_NAME AS 'FK_Nome_Tabela' ");
            commandText.Append(",KCU1.COLUMN_NAME AS 'FK_Nome_Coluna' ");
            commandText.Append(",FK.is_disabled AS 'FK_Esta_Desativada' ");
            commandText.Append(",KCU2.CONSTRAINT_NAME AS 'PK_Nome_Constraint_Referenciada' ");
            commandText.Append(",KCU2.TABLE_NAME AS 'PK_Nome_Tabela_Referenciada' ");
            commandText.Append(",KCU2.COLUMN_NAME AS 'PK_Nome_Coluna_Referenciada' ");
            commandText.Append("FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC ");
            commandText.Append("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1 ");
            commandText.Append("ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG ");
            commandText.Append("AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA ");
            commandText.Append("AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME ");
            commandText.Append("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2 ");
            commandText.Append("ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG  ");
            commandText.Append("AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA ");
            commandText.Append("AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME ");
            commandText.Append("AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION ");
            commandText.Append("JOIN sys.foreign_keys FK on FK.name = KCU1.CONSTRAINT_NAME ");
            commandText.Append("Where ");
            commandText.Append(string.Format("KCU1.TABLE_NAME = '{0}' ", currentTableName));
            commandText.Append("Order by  ");
            commandText.Append("KCU1.TABLE_NAME ");

            var comando = new SqlCommand(commandText.ToString(), this.conn);
            var reader = comando.ExecuteReader();



            var reletedClasses = new UniqueListInfo();

            while (reader.Read())
            {
                var tableNamePk = reader["PK_Nome_Tabela_Referenciada"].ToString();
                var classNamePk = reader["PK_Nome_Tabela_Referenciada"].ToString();


                //var _module = GetModuleFromContextByTableName(tableNamePk, config.Module);
                //var _namespace = GetNameSpaceFromContextByTableName(tableNamePk, config.Module);
                //var _namespaceDto = GetNameSpaceFromContextWithMakeDtoByTableName(tableNamePk, config.Module);

                //var namespaceApp = GetNameSpaceFromContextWitExposeParametersByTableName(tableNamePk, config.Module);
                //if (AppExpose(namespaceApp))
                //{
                //    reletedClasses.Add(new Info
                //    {
                //        Table = tableNamePk,
                //        ClassName = MakeClassName(classNamePk, tableNamePk),
                //        Module = _module,
                //        Namespace = _namespace,
                //        NamespaceApp = namespaceApp,
                //        NamespaceDto = _namespaceDto,
                //        PropertyNamePk = MakePropertyName(reader["PK_Nome_Coluna_Referenciada"].ToString(), MakeClassName(classNamePk, tableNamePk)),
                //        PropertyNameFk = MakePropertyName(reader["FK_Nome_Coluna"].ToString(), currentTableName),
                //        NavigationType = NavigationType.Instance
                //    });
                //}

                MakeReletedClass(config, reader, reletedClasses, currentTableName, tableNamePk, classNamePk, "PK_Nome_Coluna_Referenciada", "FK_Nome_Coluna", NavigationType.Instance);

            }



            comando.Dispose();
            reader.Close();


            return reletedClasses;

        }
        private IEnumerable<Info> GetReletaedClasses(Context config, string currentTableName)
        {

            var commandText = new StringBuilder().Append(string.Format("EXEC sp_fkeys '{0}'", currentTableName));

            var comando = new SqlCommand(commandText.ToString(), this.conn);
            var reader = comando.ExecuteReader();


            var reletedClasses = new UniqueListInfo();
            while (reader.Read())
            {
                var tableNameFK = reader["FKTABLE_NAME"].ToString();
                var classNameFK = reader["FKTABLE_NAME"].ToString();


                //var _module = GetModuleFromContextByTableName(tableName, config.Module);
                //var _namespace = GetNameSpaceFromContextByTableName(tableName, config.Module);
                //var _namespaceDto = GetNameSpaceFromContextWithMakeDtoByTableName(tableName, config.Module);

                //var _namespaceApp = GetNameSpaceFromContextWitExposeParametersByTableName(tableName, config.Module);
                //if (AppExpose(_namespaceApp))
                //{
                //    reletedClasses.Add(new Info
                //    {
                //        Table = tableName,
                //        ClassName = MakeClassName(className, tableName),
                //        Module = _module,
                //        Namespace = _namespace,
                //        NamespaceApp = _namespaceApp,
                //        NamespaceDto = _namespaceDto,
                //        PropertyNameFk = MakePropertyName(reader["FKCOLUMN_NAME"].ToString(), MakeClassName(className, tableName)),
                //        PropertyNamePk = MakePropertyName(reader["PKCOLUMN_NAME"].ToString(), MakeClassName(className, tableName)),
                //        NavigationType = reader["FKCOLUMN_NAME"].ToString().Equals("Id") ? NavigationType.Instance : NavigationType.Collettion
                //    });
                //}

                var navigationType = reader["FKCOLUMN_NAME"].ToString().Equals("Id") ? NavigationType.Instance : NavigationType.Collettion;
                MakeReletedClass(config, reader, reletedClasses, tableNameFK, tableNameFK, classNameFK, "PKCOLUMN_NAME", "FKCOLUMN_NAME", navigationType);

            }



            comando.Dispose();
            reader.Close();



            return reletedClasses;

        }

        private void MakeReletedClass(Context config, SqlDataReader reader, UniqueListInfo reletedClasses, string currentTableName, string tableNamePK, string className, string PK_FieldName, string FK_FieldName, NavigationType navigationType)
        {
            var _module = GetModuleFromContextByTableName(tableNamePK, config.Module);
            var _namespace = GetNameSpaceFromContextByTableName(tableNamePK, config.Module);
            var _namespaceDto = GetNameSpaceFromContextWithMakeDtoByTableName(tableNamePK, config.Module);

            var _namespaceApp = GetNameSpaceFromContextWitExposeParametersByTableName(tableNamePK, config.Module);
            if (AppExpose(_namespaceApp))
            {
                var classNameNew = MakeClassName(new TableInfo { ClassName = className });

                reletedClasses.Add(new Info
                {
                    Table = tableNamePK,
                    ClassName = classNameNew,
                    Module = _module,
                    Namespace = _namespace,
                    NamespaceApp = _namespaceApp,
                    NamespaceDto = _namespaceDto,
                    PropertyNamePk = MakePropertyName(reader[PK_FieldName].ToString(), classNameNew),
                    PropertyNameFk = MakePropertyName(reader[FK_FieldName].ToString(), classNameNew),
                    NavigationType = navigationType
                });
            }
        }
       
        private string makeClassName(string tableName)
        {
            return MakeClassNameTemplateDefault(tableName);
        }
        private void IsValid(TableInfo tableInfo)
        {
            if (!tableInfo.Scaffold)
            {
                if (tableInfo.ClassName.IsNull())
                    throw new InvalidOperationException("Para gerar Classes com Scaffold false é preciso setar a propriedade className");
            }
        }
        private void ExecuteTemplatesByTableleInfo(Context config)
        {
            foreach (var tableInfo in config.TableInfo)
            {
                DefineTemplateByTableInfo(config, tableInfo);
            }
        }
        private void DefineInfoKey(TableInfo tableInfo, List<Info> infos)
        {
            var keys = infos.Where(_ => _.IsKey == 1);

            var Keys = new List<string>();
            var KeysTypes = new List<string>();

            foreach (var item in keys)
            {
                Keys.Add(item.PropertyName);
                KeysTypes.Add(item.Type);
            }

            tableInfo.Keys = Keys;
            tableInfo.KeysTypes = KeysTypes;

        }
        private void DeleteFilesNotFoudTable(Context config, TableInfo tableInfo)
        {
            var PathOutputMaps = PathOutput.PathOutputMaps(tableInfo, config);
            var PathOutputDomainModelsValidation = PathOutput.PathOutputDomainModelsValidation(tableInfo, config);
            var PathOutputDomainModels = PathOutput.PathOutputDomainModels(tableInfo, config);
            var PathOutputApp = PathOutput.PathOutputApp(tableInfo, config);
            var PathOutputUri = PathOutput.PathOutputUri(tableInfo, config);
            var PathOutputDto = PathOutput.PathOutputDto(tableInfo, config);
            var PathOutputApi = PathOutput.PathOutputApi(tableInfo, config);
            var PathOutputApplicationTest = PathOutput.PathOutputApplicationTest(tableInfo, config);
            var PathOutputApplicationTestMoq = PathOutput.PathOutputApplicationTestMoq(tableInfo, config);
            var PathOutputApiTest = PathOutput.PathOutputApiTest(tableInfo, config);

            File.Delete(PathOutputMaps);
            File.Delete(PathOutputDomainModelsValidation);
            File.Delete(PathOutputDomainModels);
            File.Delete(PathOutputApp);
            File.Delete(PathOutputUri);
            File.Delete(PathOutputDto);
            File.Delete(PathOutputApi);
            File.Delete(PathOutputApplicationTest);
            File.Delete(PathOutputApplicationTestMoq);
            File.Delete(PathOutputApiTest);

        }
        private void ExecuteTemplateByTableInfoFields(Context config, string RunOnlyThisClass)
        {
            var qtd = 0;
            foreach (var tableInfo in config.TableInfo)
            {

                qtd++;
                var infos = new UniqueListInfo();

                IsValid(tableInfo);


                if (tableInfo.Scaffold)
                {
                    this.Open(config.ConnectionString);
                    var commandText = new StringBuilder();

                    commandText.Append("SELECT ");
                    commandText.Append(" dbo.sysobjects.name AS Tabela,");
                    commandText.Append(" dbo.syscolumns.name AS NomeColuna,");
                    commandText.Append(" dbo.syscolumns.length AS Tamanho,");
                    commandText.Append(" isnull(pk.is_primary_key,0) AS Chave,");
                    commandText.Append(" dbo.syscolumns.isnullable AS Nulo,");
                    commandText.Append(" dbo.systypes.name AS Tipo");
                    commandText.Append(" FROM ");
                    commandText.Append(" dbo.syscolumns INNER JOIN");
                    commandText.Append(" dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN");
                    commandText.Append(" dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype ");
                    commandText.Append(" LEFT JOIN (");
                    commandText.Append(" Select Tablename, is_primary_key,ColumnName from (SELECT  i.name AS IndexName,");
                    commandText.Append(" OBJECT_NAME(ic.OBJECT_ID) AS TableName,");
                    commandText.Append(" COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName,");
                    commandText.Append(" i.is_primary_key ");
                    commandText.Append(" FROM sys.indexes AS i INNER JOIN ");
                    commandText.Append(" sys.index_columns AS ic ON  i.OBJECT_ID = ic.OBJECT_ID");
                    commandText.Append(" AND i.index_id = ic.index_id");
                    commandText.Append(" WHERE   i.is_primary_key = 1) as TB_PRIMARYS) as pk");
                    commandText.Append(" ON pk.tablename =  dbo.sysobjects.name and pk.ColumnName = dbo.syscolumns.name");
                    commandText.Append(" WHERE ");
                    commandText.Append(" (dbo.sysobjects.name = '" + tableInfo.TableName + "') ");
                    commandText.Append(" AND ");
                    commandText.Append(" (dbo.systypes.status <> 1) ");
                    commandText.Append(" ORDER BY ");
                    commandText.Append(" dbo.sysobjects.name, ");
                    commandText.Append(" dbo.syscolumns.colorder ");

                    var comando = new SqlCommand(commandText.ToString(), this.conn);
                    var reader = comando.ExecuteReader();

                    tableInfo.ClassName = MakeClassName(tableInfo);
                    var reletedClass = new UniqueListInfo();
                    reletedClass.AddRange(GetReletaedClasses(config, tableInfo.TableName));
                    reletedClass.AddRange(GetReletaedIntancesComplementedClasses(config, tableInfo.TableName));
                    tableInfo.ReletedClasss = reletedClass;


                    while (reader.Read())
                    {

                        var order = 0;

                        if (tableInfo.FieldsConfig.IsAny())
                        {
                            var ordersFileds = tableInfo.FieldsConfig
                                .Where(_ => _.Name.ToLower() == reader["NomeColuna"].ToString().ToLower());

                            if (ordersFileds.IsAny())
                                order = ordersFileds.SingleOrDefault().Order;
                        }

                        infos.Add(new Info
                        {
                            Table = reader["Tabela"].ToString(),
                            ClassName = tableInfo.ClassName,
                            ColumnName = reader["NomeColuna"].ToString(),
                            PropertyName = MakePropertyName(reader["NomeColuna"].ToString(), tableInfo.ClassName, Convert.ToInt32(reader["Chave"])),
                            Length = reader["Tamanho"].ToString(),
                            IsKey = Convert.ToInt32(reader["Chave"]),
                            isNullable = Convert.ToInt32(reader["Nulo"]),
                            Type = TypeConvertCSharp.Convert(reader["tipo"].ToString(), Convert.ToInt32(reader["Nulo"])),
                            TypeOriginal = reader["tipo"].ToString(),
                            Module = config.Module,
                            Namespace = config.Namespace,
                            Order = order
                        });


                    }
                    reader.Close();

                    DefineInfoKey(tableInfo, infos);


                    if (infos.Count == 0)
                    {
                        if (config.DeleteFilesNotFoundTable)
                            DeleteFilesNotFoudTable(config, tableInfo);

                        if (config.AlertNotFoundTable)
                            throw new Exception("Tabela " + tableInfo.TableName + " Não foi econtrada");

                        continue;
                    }

                }


                if (!string.IsNullOrEmpty(RunOnlyThisClass))
                {
                    if (tableInfo.TableName != RunOnlyThisClass)
                        continue;
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.CursorLeft = 10;
                PrinstScn.WriteLine(string.Format("{0} [{1}]", tableInfo.TableName, qtd));

                DefineTemplateByTableInfoFields(config, tableInfo, CastOrdenabledToUniqueListInfo(infos));

            }
        }

        private static UniqueListInfo CastOrdenabledToUniqueListInfo(UniqueListInfo infos)
        {
            var infosOrder = new UniqueListInfo();
            infosOrder.AddRange(infos.OrderBy(_ => _.Order));
            return infosOrder;
        }

        private string MakeKeysFromGetContext(TableInfo tableInfo)
        {
            var keys = string.Empty;
            if (tableInfo.Keys != null)
            {
                if (tableInfo.Keys.IsAny())
                {
                    foreach (var item in tableInfo.Keys)
                        keys += string.Format("model.{0},", item);

                    keys = keys.Substring(0, keys.Length - 1);
                }
            }
            return keys;
        }
        private bool IsNotString(Info item)
        {
            return item.Type != "string";
        }
        private bool IsNotDataRage(string propertyName)
        {
            return !propertyName.Contains("Inicio") &&
                                    !propertyName.Contains("Start") &&
                                    !propertyName.Contains("End") &&
                                    !propertyName.Contains("Fim");
        }
        private string MakeReletedNamespace(TableInfo tableInfo, Context configContext, string classBuilder)
        {
            var namespaceReletedApp = string.Empty;
            var namespaceReletedAppTest = string.Empty;
            var reletedClass = tableInfo.ReletedClasss;

            if (reletedClass != null)
            {
                foreach (var item in reletedClass.Where(_ => _.NavigationType == NavigationType.Instance))
                {

                    if (item.NamespaceApp != configContext.Namespace)
                        namespaceReletedApp = !string.IsNullOrEmpty(item.NamespaceApp) ? string.Format("using {0}.Application;", item.NamespaceApp) : string.Empty;

                    if (item.NamespaceApp != configContext.Namespace)
                        namespaceReletedAppTest = !string.IsNullOrEmpty(item.NamespaceApp) ? string.Format("using {0}.Application.Test;", item.NamespaceApp) : string.Empty;

                }
            }

            classBuilder = classBuilder.Replace("<#namespaceReleted#>", namespaceReletedApp);
            classBuilder = classBuilder.Replace("<#namespaceReletedTest#>", namespaceReletedAppTest);

            return classBuilder;
        }
        private string MakeKey(IEnumerable<Info> infos, string textTemplateCompositeKey, string classBuilderitemplateCompositeKey)
        {
            var compositeKey = infos.Where(_ => _.IsKey == 1);
            if (compositeKey.Count() > 0)
            {
                var CompositeKeys = string.Empty;
                foreach (var item in compositeKey)
                    CompositeKeys += string.Format("d.{0},", item.PropertyName);


                var itemTemplateCompositeKey = textTemplateCompositeKey
                          .Replace("<#Keys#>", CompositeKeys);

                classBuilderitemplateCompositeKey = string.Format("{0}{1}", Tabs.TabMaps(), itemTemplateCompositeKey);
            }
            return classBuilderitemplateCompositeKey;
        }

        #endregion

        protected void Dispose()
        {
            if (this.conn != null)
            {
                this.conn.Close();
                this.conn.Dispose();
            }
        }
    }
}
