﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Common.Gen
{
    public class HelperSysObjectsDDD : HelperSysObjectsBaseBack
    {

        private DefineTemplateFolder _defineTemplateFolder;
        public HelperSysObjectsDDD(IEnumerable<Context> contexts)
        {
            this.Contexts = contexts;
            PathOutput.UsePathProjects = true;
            this._defineTemplateFolder = new DefineTemplateFolder();
            this._defineTemplateFolder.SetTemplatePathBase("Templates\\Back");
            base.ArquitetureType = ArquitetureType.DDD;
        }

        public override void DefineTemplateByTableInfo(Context config, TableInfo tableInfo)
        {
            this.DefineTemplateByTableInfoBack(config, tableInfo);
            this.DefineTemplateByTableInfoFront(config, tableInfo);
        }

        public override void DefineTemplateByTableInfoFields(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            this.DefineTemplateByTableInfoFieldsBack(config, tableInfo, infos);
            this.DefineTemplateByTableInfoFieldsFront(config, tableInfo, infos);

        }

        protected override void DefineTemplateByTableInfoFieldsBack(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            this.ExecuteTemplateEntitysBase(tableInfo, config, infos);
            this.ExecuteTemplateEntitysExt(tableInfo, config, infos);
            this.ExecuteTemplateEntityValidatorSpecification(tableInfo, config, infos);
            this.ExecuteTemplateEntityWarningSpecification(tableInfo, config, infos);
            this.ExecuteTemplateEntityValidatorSpecificationReposytory(tableInfo, config, infos);
            this.ExecuteTemplateEntityServiceBase(tableInfo, config, infos);
            this.ExecuteTemplateEntityServiceExt(tableInfo, config, infos);
            this.ExecuteTemplateIEntityRepository(tableInfo, config, infos);
            this.ExecuteTemplateEntityRepository(tableInfo, config, infos);
            this.ExecuteTemplateFilterBasicExtension(tableInfo, config, infos);
            this.ExecuteTemplateFilterCustomExtension(tableInfo, config, infos);
            this.ExecuteTemplateOrderByDomainExtension(tableInfo, config, infos);
            this.ExecuteTemplateIEntityService(tableInfo, config, infos);
            this.ExecuteTemplateEntityMapBase(tableInfo, config, infos);
            this.ExecuteTemplateEntityMapExtension(tableInfo, config, infos);
            this.ExecuteTemplateFilter(tableInfo, config, infos);
            this.ExecuteTemplateFilterPartial(tableInfo, config, infos);
            this.ExecuteTemplateDto(tableInfo, config, infos);
            this.ExecuteTemplateDtoSpecialized(tableInfo, config, infos);
            this.ExecuteTemplateDtoSpecializedResult(tableInfo, config, infos);
            this.ExecuteTemplateDtoSpecializedReport(tableInfo, config, infos);
            this.ExecuteTemplateDtoSpecializedDetails(tableInfo, config, infos);
            this.ExecuteTemplateIEntityApplicationService(tableInfo, config, infos);
            this.ExecuteTemplateEntityApplicationServiceBase(tableInfo, config, infos);
            this.ExecuteTemplateEntityApplicationService(tableInfo, config, infos);
            this.ExecuteTemplateApi(tableInfo, config, infos);
            this.ExecuteTemplateApiMore(tableInfo, config, infos);

        }

        protected override void DefineTemplateByTableInfoBack(Context config, TableInfo tableInfo)
        {
            this.ExecuteTemplateDbContext(tableInfo, config);
            this.ExecuteTemplateSummary(tableInfo, config);
            this.ExecuteTemplateAutoMapperProfile(tableInfo, config);
            this.ExecuteTemplateAutoMapperProfileCustom(tableInfo, config);
            this.ExecuteTemplateAutoMapper(tableInfo, config);
            this.ExecuteTemplateContainer(tableInfo, config);
            this.ExecuteTemplateContainerPartial(tableInfo, config);
            this.ExecuteTemplateApiStart(tableInfo, config);
            this.ExecuteTemplateApiCurrentUser(tableInfo, config);
            this.ExecuteTemplateApiUpload(tableInfo, config);
            this.ExecuteTemplateApiDownload(tableInfo, config);
            this.ExecuteTemplateApiHealth(tableInfo, config);
            this.ExecuteTemplateApiAppSettings(tableInfo, config);
            this.ExecuteTemplateApiProject(tableInfo, config);
            this.ExecuteTemplateDataProject(tableInfo, config);
            this.ExecuteTemplateSummaryProject(tableInfo, config);
            this.ExecuteTemplateFilterProject(tableInfo, config);
            this.ExecuteTemplateDomainProject(tableInfo, config);
            this.ExecuteTemplateDtoProject(tableInfo, config);
            this.ExecuteTemplateAppProject(tableInfo, config);
        }

        protected override void DefineTemplateByTableInfoFront(Context config, TableInfo tableInfo)
        {
            if (config.MakeFront)
            {
                var front = this.DefineFrontTemplateClass(config);
                front.SetCamelCasingExceptions(base._camelCasingExceptions);
                front.DefineTemplateByTableInfo(config, tableInfo);
            }
        }

        protected override void DefineTemplateByTableInfoFieldsFront(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            if (config.MakeFront)
            {
                var front = this.DefineFrontTemplateClass(config);
                front.SetCamelCasingExceptions(base._camelCasingExceptions);
                front.DefineTemplateByTableInfoFields(config, tableInfo, infos);
            }
        }

        public virtual HelperSysObjectsBase DefineFrontTemplateClass(Context config)
        {
            return new HelperSysObjectsAngular(config);
        }

        #region Execute Templates

        private void ExecuteTemplateEntitysBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputDomainEntitysBase(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityBase(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityProperty(tableInfo));

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplatePropertys = Read.AllText(tableInfo, pathTemplatePropertys, this._defineTemplateFolder);


            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            classBuilder = classBuilder.Replace("<#classBaseEntity#>", this.InheritEntityBase(infos));
            classBuilder = classBuilder.Replace("<#parametersRequired#>", this.ParametersRequired(infos));
            classBuilder = classBuilder.Replace("<#property#>", base.BuilderPropertys(infos, textTemplatePropertys, false));
            classBuilder = classBuilder.Replace("<#initParametersRequired#>", this.InitParametersRequired(infos));
            classBuilder = classBuilder.Replace("<#parametersRequired#>", this.ParametersRequired(infos));
            classBuilder = classBuilder.Replace("<#methodsSeters#>", this.MethodsSeters(infos, tableInfo));


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateEntitysExt(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputDomainEntitysExt(tableInfo, configContext);
            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarModelPartialExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityExt(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;



            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityProperty(tableInfo));
            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplatePropertys = Read.AllText(tableInfo, pathTemplatePropertys, this._defineTemplateFolder);


            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            classBuilder = classBuilder.Replace("<#classBaseEntity#>", this.InheritEntityBase(infos));
            classBuilder = classBuilder.Replace("<#parametersRequired#>", this.ParametersRequired(infos));
            classBuilder = classBuilder.Replace("<#parametersRequiredToBase#>", this.ParametersRequiredToBase(infos));
            classBuilder = classBuilder.Replace("<#property#>", base.BuilderPropertys(infos, textTemplatePropertys, false));
            classBuilder = classBuilder.Replace("<#initParametersRequired#>", this.InitParametersRequired(infos));
            classBuilder = classBuilder.Replace("<#methodsSeters#>", this.MethodsSeters(infos, tableInfo));


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateEntityValidatorSpecification(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputDomainEntitysValidatorSpecification(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityValidatorSpecification(tableInfo));

            if (File.Exists(pathOutput))
                return;

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateEntityWarningSpecification(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputDomainEntitysWarningSpecification(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityWarningSpecification(tableInfo));

            if (File.Exists(pathOutput))
                return;

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateEntityValidatorSpecificationReposytory(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputDomainEntitysValidatorSpecificationRepository(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityValidatorSpecificationRepository(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateEntityServiceBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputDomainEntitysServiceBase(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityServiceBase(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            textTemplateClass = textTemplateClass.Replace("<#setUserCreate#>", Audit.ExistsAuditFields(infos) ? "<#classNameLower#>.SetUserCreate(1);" : "");
            textTemplateClass = textTemplateClass.Replace("<#setUserCreateOld#>", Audit.ExistsAuditFields(infos) ? "<#classNameLower#>.SetUserCreate(<#classNameLower#>Old.UserCreateId, <#classNameLower#>Old.UserCreateDate);" : "");
            textTemplateClass = textTemplateClass.Replace("<#setUserUpdate#>", Audit.ExistsAuditFields(infos) ? "<#classNameLower#>.SetUserUpdate(1);" : "");

            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            classBuilder = classBuilder.Replace("<#GetOneFilterKeys#>", GetOneFilterKeys(tableInfo));


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private static string GetOneFilterKeys(TableInfo tableInfo)
        {
            var keys = tableInfo.Keys;
            var WhereByKeys = String.Join(", ", keys.Select(_ => string.Format("{0} = {1}.{0}", _, tableInfo.ClassName.ToLowerCase())));

            var templateWhereByKeys = string.Format("new {0}Filter {{ {1} }}", tableInfo.ClassName, WhereByKeys);

            return templateWhereByKeys;
        }

        private void ExecuteTemplateEntityServiceExt(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputDomainEntitysServiceExt(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityServiceExt(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateIEntityRepository(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputDomainIEntitysRepository(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.IEntityRepository(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateEntityRepository(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputInfraEntitysRepository(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityRepository(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            classBuilder = classBuilder.Replace("<#fieldGetByFilters#>", classBuilder);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateFilterBasicExtension(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputInfraFilterBasicExtension(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityFilterBasicExtension(tableInfo));


            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateFilters = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.FilterBasicExtension(tableInfo));
            var textTemplateFilters = Read.AllText(tableInfo, pathTemplateFilters, this._defineTemplateFolder);
            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            var classBuilderFilters = string.Empty;

            classBuilderFilters = base.BuilderSampleFilters(infos, textTemplateFilters, classBuilderFilters);

            classBuilder = classBuilder.Replace("<#filtersExpressions#>", classBuilderFilters);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateFilterCustomExtension(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputInfraFilterCustomExtension(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityFilterCustomExtension(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateOrderByDomainExtension(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputInfraOrderByDomainExtension(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityOrderByDomainExtension(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateIEntityService(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputDomainIEntitysService(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.IEntityService(tableInfo));

            if (File.Exists(pathOutput))
                return;

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateEntityMapBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (!tableInfo.MakeDomain)
                return;

            if (!tableInfo.Scaffold)
                return;

            var pathOutput = PathOutput.PathOutputEntityMapBase(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityMapBase(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateLength = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.MapsLength(tableInfo));
            var pathTemplateRequired = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.MapsRequired(tableInfo));
            var pathTemplateMapper = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.MapsMapper(tableInfo));
            var pathTemplateManyToMany = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.MapsManyToMany(tableInfo));
            var pathTemplateCompositeKey = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.MapsCompositeKey(tableInfo));

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var textTemplateLength = Read.AllText(tableInfo, pathTemplateLength, this._defineTemplateFolder);
            var textTemplateRequired = Read.AllText(tableInfo, pathTemplateRequired, this._defineTemplateFolder);
            var textTemplateMapper = Read.AllText(tableInfo, pathTemplateMapper, this._defineTemplateFolder);
            var textTemplateManyToMany = Read.AllText(tableInfo, pathTemplateManyToMany, this._defineTemplateFolder);
            var textTemplateCompositeKey = Read.AllText(tableInfo, pathTemplateCompositeKey, this._defineTemplateFolder);

            var classBuilderitemTemplateLength = string.Empty;
            var classBuilderitemTemplateRequired = string.Empty;
            var classBuilderitemplateMapper = string.Empty;
            var classBuilderitemplateMapperKey = string.Empty;
            var classBuilderitemplateManyToMany = string.Empty;
            var classBuilderitemplateCompositeKey = string.Empty;

            string classBuilder = base.MakeClassBuilderMapORM(tableInfo, configContext, infos, textTemplateClass, textTemplateLength, textTemplateRequired, textTemplateMapper, textTemplateManyToMany, textTemplateCompositeKey, ref classBuilderitemTemplateLength, ref classBuilderitemTemplateRequired, ref classBuilderitemplateMapper, ref classBuilderitemplateMapperKey, ref classBuilderitemplateManyToMany, ref classBuilderitemplateCompositeKey);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }


        }
        private void ExecuteTemplateEntityMapExtension(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputEntityMapExtension(tableInfo, configContext);

            if (!tableInfo.MakeDomain)
                return;

            if (!tableInfo.Scaffold)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarMapperPartialExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityMapExtension(tableInfo));
            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateDbContext(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputDbContext(configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.Context(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateRegister = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ContextMappers(tableInfo));


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateMappers = Read.AllText(tableInfo, pathTemplateRegister, this._defineTemplateFolder);
            textTemplateClass = textTemplateClass.Replace("<#module#>", configContext.ContextName);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            var classBuilderMappers = string.Empty;


            foreach (var item in configContext.TableInfo.Where(_ => _.Scaffold))
            {

                var itemMappaer = TextTemplateMappers.
                        Replace("<#className#>", item.ClassName);

                classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabMaps(), itemMappaer, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#mappers#>", classBuilderMappers);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateFilter(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputFilterWithFolder(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.Filter(tableInfo));
            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ModelsProperty(tableInfo));

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplatePropertys = Read.AllText(tableInfo, pathTemplatePropertys, this._defineTemplateFolder);
            if (!File.Exists(pathTemplateClass))
                return;

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            if (infos.IsAny())
            {

                foreach (var item in infos)
                {
                    classBuilderPropertys = MakeFilterDateRange(TextTemplatePropertys, classBuilderPropertys, item);

                    if (item.Type == "bool")
                        classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, item.PropertyName, "bool?");
                    else
                        classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, item.PropertyName, item.Type);

                }
            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);

            using (var stream = new StreamWriter(pathOutput))
            {

                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateFilterPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputFilterPartialWithFolder(tableInfo, configContext);

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarFiltersPartialExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.FiltersPartial(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {

                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateDto(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (!tableInfo.MakeDto)
                return;

            if (tableInfo.CodeCustomImplemented)
                return;

            var pathOutput = PathOutput.PathOutputDto(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.Dto(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplatevalidation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ModelsValidationProperty(tableInfo));
            var pathTemplatevalidationLength = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ModelsValidationLength(tableInfo));
            var pathTemplatevalidationRequired = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ModelsValidationRequired(tableInfo));
            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ModelsValidationProperty(tableInfo));

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateValidation = Read.AllText(tableInfo, pathTemplatevalidation, this._defineTemplateFolder);
            var TextTemplateValidationLength = Read.AllText(tableInfo, pathTemplatevalidationLength, this._defineTemplateFolder);
            var TextTemplateValidationRequired = Read.AllText(tableInfo, pathTemplatevalidationRequired, this._defineTemplateFolder);
            var TextTemplatePropertys = Read.AllText(tableInfo, pathTemplatePropertys, this._defineTemplateFolder);


            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;
            var classBuilderitemTemplateValidation = string.Empty;

            if (infos.IsAny())
            {

                foreach (var item in infos)
                {
                    if (Audit.GetAuditFields().Contains(item.PropertyName))
                        continue;

                    var blackList = FieldInBlackListCreateOrEdit(tableInfo, item.PropertyName);

                    if (IsIdField(item) || blackList || !IsRequired(item))
                    {
                        var itempropert = TextTemplatePropertys.
                                Replace("<#propertyName#>", item.PropertyName).
                                Replace("<#type#>", item.Type).
                                Replace("<#RequiredValidation#>", string.Empty).
                                Replace("<#MaxLengthValidation#>", string.Empty).
                                Replace("<#tab#>", Tabs.TabModels());

                        classBuilderPropertys += string.Format("{0}{1}{2}", Tabs.TabModels(), itempropert, System.Environment.NewLine);
                    }
                    else
                        classBuilderPropertys += MakeValidationsAttributes(TextTemplateValidation, TextTemplateValidationLength, TextTemplateValidationRequired, classBuilderitemTemplateValidation, item);

                }
            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private static bool IsIdField(Info item)
        {
            return item.IsKey == 1 || item.PropertyName.IsNotNull() && item.PropertyName.EndsWith("Id");
        }

        private bool FieldInBlackListCreateOrEdit(TableInfo tableInfo, string propertyName)
        {
            if (tableInfo.FieldsConfig.NotIsAny())
                return false;

            return tableInfo.FieldsConfig
                .Where(_ => _.Name.ToUpper() == propertyName.ToUpper())
                .Where(_ => _.Create == false || _.Edit == false).IsAny();
        }

        private void ExecuteTemplateDtoSpecialized(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecialized(tableInfo, configContext);

            if (!tableInfo.MakeDto)
                return;


            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarDtoEspecializadosExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoSpecialized(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateNavPropertysCollection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyCollection(tableInfo));
            var pathTemplateNavPropertysInstance = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyInstance(tableInfo));


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateNavPropertysCollections = Read.AllText(tableInfo, pathTemplateNavPropertysCollection, this._defineTemplateFolder);
            var TextTemplateNavPropertysInstance = Read.AllText(tableInfo, pathTemplateNavPropertysInstance, this._defineTemplateFolder);


            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            classBuilder = MakePropertyNavigationDto(tableInfo, configContext, TextTemplateNavPropertysCollections, TextTemplateNavPropertysInstance, classBuilder);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateDtoSpecializedResult(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecializedResult(tableInfo, configContext);
            if (!tableInfo.MakeDto)
                return;


            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarDtoEspecializadosDeResultadoExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoSpecilizedResult(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateNavPropertysCollection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyCollection(tableInfo));
            var pathTemplateNavPropertysInstance = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyInstance(tableInfo));


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateNavPropertysCollections = Read.AllText(tableInfo, pathTemplateNavPropertysCollection, this._defineTemplateFolder);
            var TextTemplateNavPropertysInstance = Read.AllText(tableInfo, pathTemplateNavPropertysInstance, this._defineTemplateFolder);


            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            classBuilder = MakePropertyNavigationDto(tableInfo, configContext, TextTemplateNavPropertysCollections, TextTemplateNavPropertysInstance, classBuilder);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateDtoSpecializedReport(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecializedReport(tableInfo, configContext);

            if (!tableInfo.MakeDto)
                return;


            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarDtoEspecializadosDeRelatorioExistentes"]) == false)
                return;


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoSpecializedReport(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateNavPropertysCollection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyCollection(tableInfo));
            var pathTemplateNavPropertysInstance = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyInstance(tableInfo));


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateNavPropertysCollections = Read.AllText(tableInfo, pathTemplateNavPropertysCollection, this._defineTemplateFolder);
            var TextTemplateNavPropertysInstance = Read.AllText(tableInfo, pathTemplateNavPropertysInstance, this._defineTemplateFolder);


            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            classBuilder = MakePropertyNavigationDto(tableInfo, configContext, TextTemplateNavPropertysCollections, TextTemplateNavPropertysInstance, classBuilder);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateDtoSpecializedDetails(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecializedDetails(tableInfo, configContext);

            if (!tableInfo.MakeDto)
                return;


            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarDtoEspecializadosDeDetalhesExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoSpecializedDetails(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateNavPropertysCollection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyCollection(tableInfo));
            var pathTemplateNavPropertysInstance = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.DtoNavPropertyInstance(tableInfo));


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateNavPropertysCollections = Read.AllText(tableInfo, pathTemplateNavPropertysCollection, this._defineTemplateFolder);
            var TextTemplateNavPropertysInstance = Read.AllText(tableInfo, pathTemplateNavPropertysInstance, this._defineTemplateFolder);


            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            classBuilder = MakePropertyNavigationDto(tableInfo, configContext, TextTemplateNavPropertysCollections, TextTemplateNavPropertysInstance, classBuilder);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateSummary(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputSummary(tableInfo, configContext);

            if (!tableInfo.MakeSummary)
                return;

            if (File.Exists(pathOutput) || tableInfo.CodeCustomImplemented)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.Summary(tableInfo));
            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateAutoMapperProfile(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeDto)
                return;

            var pathOutput = PathOutput.PathOutputAutoMapperProfile(configContext, tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.AutoMapperProfile(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateMappers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProfileRegisters(tableInfo));
            var pathTemplateMappersSpecilize = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProfileRegistersSpecilize(tableInfo));
            var pathTemplateMappersSpecilizeResult = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProfileRegistersSpecilizeResult(tableInfo));
            var pathTemplateMappersSpecilizeReport = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProfileRegistersSpecilizeReport(tableInfo));
            var pathTemplateMappersSpecilizeDetails = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProfileRegistersSpecilizeDetails(tableInfo));

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateMappers = Read.AllText(tableInfo, pathTemplateMappers, this._defineTemplateFolder);
            var TextTemplateMappersSpecilize = Read.AllText(tableInfo, pathTemplateMappersSpecilize, this._defineTemplateFolder);
            var TextTemplateMappersSpecilizeResult = Read.AllText(tableInfo, pathTemplateMappersSpecilizeResult, this._defineTemplateFolder);
            var TextTemplateMappersSpecilizeReport = Read.AllText(tableInfo, pathTemplateMappersSpecilizeReport, this._defineTemplateFolder);
            var TextTemplateMappersSpecilizeDetails = Read.AllText(tableInfo, pathTemplateMappersSpecilizeDetails, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderMappers = string.Empty;


            foreach (var item in configContext.TableInfo)
            {
                var className = item.ClassName;

                if (!string.IsNullOrEmpty(className))
                {
                    var itemMappaer = TextTemplateMappers.
                            Replace("<#className#>", className);

                    var itemMappaerSpecilize = TextTemplateMappersSpecilize.
                           Replace("<#className#>", className);

                    var itemMappaerSpecilizeResult = TextTemplateMappersSpecilizeResult.
                        Replace("<#className#>", className);

                    var itemMappaerSpecilizeReport = TextTemplateMappersSpecilizeReport.
                        Replace("<#className#>", className);

                    var itemMappaerSpecilizeDetails = TextTemplateMappersSpecilizeDetails.
                        Replace("<#className#>", className);


                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMappaer, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMappaerSpecilize, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMappaerSpecilizeResult, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMappaerSpecilizeReport, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", Tabs.TabSets(), itemMappaerSpecilizeDetails, System.Environment.NewLine);
                }
            }



            classBuilder = classBuilder.Replace("<#registers#>", classBuilderMappers);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateAutoMapper(TableInfo tableInfo, Context configContext)
        {

            if (!tableInfo.MakeApp)
                return;

            var pathOutput = PathOutput.PathOutputAutoMapper(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.Automapper(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateAutoMapperProfileCustom(TableInfo tableInfo, Context configContext)
        {

            if (!tableInfo.MakeApp)
                return;

            var pathOutput = PathOutput.PathOutputAutoMapperProfileCustom(configContext, tableInfo);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.AutoMapperProfileCustom(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateIEntityApplicationService(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;


            var pathOutput = PathOutput.PathOutputApplicationIEntityApplicationService(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.IEntityApplicationService(tableInfo));

            if (File.Exists(pathOutput))
                return;

            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateEntityApplicationService(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputApplicatioEntityApplicationService(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityApplicationService(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);



            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateEntityApplicationServiceBase(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputApplicatioEntityApplicationServiceBase(tableInfo, configContext);

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityApplicationServiceBase(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = base.GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            classBuilder = classBuilder.Replace("<#parametersRequiredApplication#>", this.ParametersRequiredApplication(infos));
            classBuilder = classBuilder.Replace("<#methodsSetersApplication#>", this.MethodsSetersApplication(infos));
            classBuilder = classBuilder.Replace("<#GetOneFilterKeys#>", GetOneFilterKeys(tableInfo));


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateContainer(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApp)
                return;

            var pathOutput = PathOutput.PathOutputContainerApi(configContext);

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.Container(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var pathTemplateInjections = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ContainerInjections(tableInfo));


            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateInjections = Read.AllText(tableInfo, pathTemplateInjections, this._defineTemplateFolder);

            if (configContext.Module.IsNullOrEmpty())
                textTemplateClass = textTemplateClass.Replace("<#domainSource#>", configContext.ProjectName);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderMappers = string.Empty;

            foreach (var item in configContext.TableInfo)
            {
                if (!string.IsNullOrEmpty(item.ClassName))
                {
                    var itemInjections = TextTemplateInjections.
                            Replace("<#namespace#>", configContext.Namespace).
                            Replace("<#module#>", configContext.Module.IsNullOrEmpty() ? configContext.ProjectName : configContext.Module).
                            Replace("<#className#>", item.ClassName).
                            Replace("<#domainSource#>", configContext.DomainSource.IsNullOrEmpty() ? configContext.ProjectName : configContext.DomainSource).
                            Replace("<#namespaceDomainSource#>", configContext.NamespaceDomainSource);


                    classBuilderMappers += string.Format("{0}{1}", itemInjections, System.Environment.NewLine);
                }
            }


            classBuilder = classBuilder.Replace("<#injections#>", classBuilderMappers);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateContainerPartial(TableInfo tableInfo, Context configContext)
        {

            var pathOutput = PathOutput.PathOutputContainerPartialApi(configContext);

            if (!tableInfo.MakeApp)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarContainerClassPartialExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ContainerPartial(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateApi(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {

            var pathOutput = PathOutput.PathOutputApi(tableInfo, configContext);
            if (!tableInfo.MakeApi)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarApiExistentes"]) == false)
                return;


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiRest(tableInfo));
            var pathTemplateApiGet = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiGet(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var TextTemplateApiGet = Read.AllText(tableInfo, pathTemplateApiGet, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            var classBuilderApiGet = string.Empty;

            if (!tableInfo.IsCompositeKey && tableInfo.Keys.IsNotNull())
            {
                classBuilderApiGet = TextTemplateApiGet;
                classBuilderApiGet = classBuilderApiGet.Replace("<#className#>", tableInfo.ClassName);
                classBuilderApiGet = classBuilderApiGet.Replace("<#namespace#>", configContext.Namespace);
                classBuilderApiGet = classBuilderApiGet.Replace("<#inheritClassName#>", tableInfo.InheritClassName);
                classBuilderApiGet = classBuilderApiGet.Replace("<#KeyName#>", tableInfo.Keys.FirstOrDefault());
                classBuilderApiGet = classBuilderApiGet.Replace("<#KeyType#>", tableInfo.KeysTypes.FirstOrDefault());
            }


            classBuilder = classBuilder.Replace("<#ApiGet#>", classBuilderApiGet);

            if (!tableInfo.Authorize)
                classBuilder = classBuilder.Replace("[Authorize]", string.Empty);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateApiMore(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {

            var pathOutput = PathOutput.PathOutputApiMore(tableInfo, configContext);
            if (!tableInfo.MakeApi)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarApiExistentes"]) == false)
                return;


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiMore(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);

            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);
            if (!tableInfo.Authorize)
                classBuilder = classBuilder.Replace("[Authorize]", string.Empty);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateApiAppSettings(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            var pathOutput = PathOutput.PathOutputAppSettings(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.Appsettings(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateApiProject(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            var pathOutput = PathOutput.PathOutputProjectApi(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProjectApi(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }


        private void ExecuteTemplateAppProject(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApp)
                return;

            var pathOutput = PathOutput.PathOutputProjectApp(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProjectApp(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateDtoProject(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApp)
                return;

            var pathOutput = PathOutput.PathOutputProjectDto(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProjectDto(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateDomainProject(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputProjectDomain(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProjectDomain(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateFilterProject(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputProjectFilter(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProjectFilter(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateSummaryProject(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputProjectSummary(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProjectSummary(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateDataProject(TableInfo tableInfo, Context configContext)
        {

            var pathOutput = PathOutput.PathOutputProjectInfra(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ProjectData(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }


        private void ExecuteTemplateApiUpload(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            var pathOutput = PathOutput.PathOutputApiUpload(configContext, tableInfo);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiUpload(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateApiHealth(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            var pathOutput = PathOutput.PathOutputApiHeath(configContext, tableInfo);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiHealth(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateApiDownload(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            var pathOutput = PathOutput.PathOutputApiDownload(configContext, tableInfo);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiDownalod(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private void ExecuteTemplateApiCurrentUser(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            var pathOutput = PathOutput.PathOutputApiCurrentUser(configContext, tableInfo);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiCurrentUser(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }
        private void ExecuteTemplateApiStart(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            var pathOutput = PathOutput.PathOutputWebApiStart(configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.ApiStart(tableInfo));
            if (!File.Exists(pathTemplateClass))
                return;

            var textTemplateClass = Read.AllText(tableInfo, pathTemplateClass, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        #endregion

        #region helpers
        private string InheritEntityBase(IEnumerable<Info> infos)
        {
            return Audit.ExistsAuditFields(infos) ? "DomainBaseWithUserCreate" : "DomainBase";
        }
        private string ParametersRequired(IEnumerable<Info> infos)
        {
            var _parametersRequired = string.Empty;
            foreach (var item in infos)
            {
                if (Audit.GetAuditFields().Contains(item.PropertyName))
                    continue;

                if (IsRequired(item))
                    _parametersRequired += string.Format("{0} {1}, ", item.Type, item.PropertyName.ToLower());
            }

            return !_parametersRequired.IsNullOrEmpaty() ? _parametersRequired.Substring(0, _parametersRequired.Length - 2) : _parametersRequired;
        }
        private string MethodsSetersApplication(IEnumerable<Info> infos)
        {
            var _methods = string.Empty;
            foreach (var item in infos)
            {
                if (Audit.GetAuditFields().Contains(item.PropertyName))
                    continue;

                if (!IsRequired(item))
                    _methods += string.Format("{0}domain.Setar{1}(_dto.{1});{2}", Tabs.TabSets(), item.PropertyName, System.Environment.NewLine);
            }

            return _methods;
        }
        private string ParametersRequiredToBase(IEnumerable<Info> infos)
        {
            var _parametersRequired = string.Empty;
            foreach (var item in infos)
            {
                if (Audit.GetAuditFields().Contains(item.PropertyName))
                    continue;

                if (IsRequired(item))
                    _parametersRequired += string.Format("{0}, ", item.PropertyName.ToLower());
            }

            return !_parametersRequired.IsNullOrEmpaty() ? _parametersRequired.Substring(0, _parametersRequired.Length - 2) : _parametersRequired;
        }
        private string InitParametersRequired(IEnumerable<Info> infos)
        {
            var _parametersRequired = string.Empty;
            foreach (var item in infos)
            {
                if (Audit.GetAuditFields().Contains(item.PropertyName))
                    continue;

                if (IsRequired(item))
                    _parametersRequired += string.Format("{0}this.{1} = {2};{3}", Tabs.TabSets(), item.PropertyName, item.PropertyName.ToLower(), System.Environment.NewLine);
            }

            return _parametersRequired;
        }
        private string ParametersRequiredApplication(IEnumerable<Info> infos)
        {
            var _parametersRequired = string.Empty;
            foreach (var item in infos)
            {
                if (Audit.GetAuditFields().Contains(item.PropertyName))
                    continue;

                if (IsRequired(item))
                    _parametersRequired += string.Format("_dto.{0},{1}                                        ", item.PropertyName, System.Environment.NewLine);
            }

            return !_parametersRequired.IsNullOrEmpaty() ? _parametersRequired.Substring(0, _parametersRequired.Length - 43) : _parametersRequired;
        }
        private string MethodsSeters(IEnumerable<Info> infos, TableInfo tableInfo)
        {

            var pathTemplateMethosSeters = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateName.EntityMethodSeters(tableInfo));
            var textTemplateMethosSeters = Read.AllText(tableInfo, pathTemplateMethosSeters, this._defineTemplateFolder);

            var _methods = string.Empty;
            foreach (var item in infos)
            {
                if (Audit.GetAuditFields().Contains(item.PropertyName))
                    continue;

                if (!IsRequired(item))
                    _methods += string.Format("{0}{1}", textTemplateMethosSeters.Replace("<#propertyName#>", item.PropertyName).Replace("<#propertyNameLower#>", item.PropertyName.ToLower()).Replace("<#type#>", item.Type), System.Environment.NewLine);
            }

            return _methods;
        }

        #endregion
    }
}
