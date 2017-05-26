﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class HelperSysObjectsAngular : HelperSysObjectsBase
    {

        private DefineTemplateFolder _defineTemplateFolder;
        public HelperSysObjectsAngular(Context context)
        {
            var _contexts = new List<Context> {
                context
            };

            this.Contexts = _contexts;
            PathOutput.UsePathProjects = true;
            this._defineTemplateFolder = new DefineTemplateFolder();
            this._defineTemplateFolder.SetTemplatePathBase("Templates\\Front");
        }

        public HelperSysObjectsAngular(IEnumerable<Context> contexts)
        {
            this.Contexts = contexts;
            PathOutput.UsePathProjects = true;
        }

        public override void DefineTemplateByTableInfoFields(Context config, TableInfo tableInfo, UniqueListInfo infos)
        {
            ExecuteTemplateAngularSaveGeneratedComponentHtml(tableInfo, config, infos);
            ExecuteTemplateAngularSaveComponentHtml(tableInfo, config, infos);
            ExecuteTemplateAngularGridGeneratedComponentHtml(tableInfo, config, infos);
            ExecuteTemplateAngularGridComponentHtml(tableInfo, config, infos);
            ExecuteTemplateAngularFilterGeneratedComponentHtml(tableInfo, config, infos);
            ExecuteTemplateAngularFilterComponentHtml(tableInfo, config, infos);
            ExecuteTemplateAngularModelGenerated(tableInfo, config, infos);
            ExecuteTemplateAngularModelFilterGenerated(tableInfo, config, infos);
        }

        public override void DefineTemplateByTableInfo(Context config, TableInfo tableInfo)
        {
            ExecuteTemplateAngularComponent(tableInfo, config);
            ExecuteTemplateAngularComponentHtml(tableInfo, config);
            ExecuteTemplateAngularModule(tableInfo, config);

            ExecuteTemplateAngularRouting(tableInfo, config);

            ExecuteTemplateAngularSaveComponent(tableInfo, config);
            ExecuteTemplateAngularSaveGeneratedComponent(tableInfo, config);

            ExecuteTemplateAngularGridComponent(tableInfo, config);
            ExecuteTemplateAngularGridGeneratedComponent(tableInfo, config);

            ExecuteTemplateAngularDeleteComponent(tableInfo, config);
            ExecuteTemplateAngularDeleteGeneratedComponent(tableInfo, config);
            ExecuteTemplateAngularDeleteGeneratedComponentHtml(tableInfo, config);

            ExecuteTemplateAngularFilterComponent(tableInfo, config);
            ExecuteTemplateAngularFilterGeneratedComponent(tableInfo, config);

            ExecuteTemplateAngularModel(tableInfo, config);
            ExecuteTemplateAngularModelFilter(tableInfo, config);

            ExecuteTemplateAngularRoutingGenerated(tableInfo, config);


        }

        #region models generated

        private void ExecuteTemplateAngularModelFilterGenerated(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutputModelGenerated = PathOutputAngular.PathOutputAngularModelFilterGenerated(tableInfo, configContext);

            var pathTemplateModelGenerated = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularModelFilterGenerated(tableInfo));
            var textTemplateModelGenerated = Read.AllText(tableInfo, pathTemplateModelGenerated, this._defineTemplateFolder);
            var classBuilderModelGenerated = GenericTagsTransformer(tableInfo, configContext, textTemplateModelGenerated);

            var classBuilderForm = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    var itemForm = string.Empty;
                    var pathTemplateField = string.Empty;
                    var textTemplateField = string.Empty;

                    if (item.Type == "string" || item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        if (item.Type == "DateTime" || item.Type == "DateTime?")
                        {
                            itemForm = string.Format(@"    public {0}Start: string;{1}", item.PropertyName, System.Environment.NewLine);
                            itemForm += string.Format(@"    public {0}End: string;", item.PropertyName);
                        }
                        else
                        {
                            itemForm = string.Format(@"    public {0}: string;", item.PropertyName);
                        }
                    }

                    else if (item.Type == "bool")
                    {
                        itemForm = string.Format(@"    public {0}: boolean;", item.PropertyName);
                    }

                    else if (item.Type == "bool?")
                    {
                        itemForm = string.Format(@"    public {0}?: boolean;", item.PropertyName);
                    }

                    else if (item.Type == "int" || item.Type == "float" || item.Type == "decimal")
                    {
                        itemForm = string.Format(@"    public {0}: number;", item.PropertyName);
                    }

                    else if (item.Type == "int?" || item.Type == "float?" || item.Type == "decimal?")
                    {
                        itemForm = string.Format(@"    public {0}?: number;", item.PropertyName);
                    }

                    classBuilderForm += string.Format("{0}{1}", itemForm, System.Environment.NewLine);
                }
            }

            classBuilderModelGenerated = classBuilderModelGenerated.Replace("<#formFields#>", classBuilderForm);

            using (var stream = new StreamWriter(pathOutputModelGenerated)) { stream.Write(classBuilderModelGenerated); }
        }

        private void ExecuteTemplateAngularModelGenerated(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutputModelGenerated = PathOutputAngular.PathOutputAngularModelGenerated(tableInfo, configContext);

            var pathTemplateModelGenerated = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularModelGenerated(tableInfo));
            var textTemplateModelGenerated = Read.AllText(tableInfo, pathTemplateModelGenerated, this._defineTemplateFolder);
            var classBuilderModelGenerated = GenericTagsTransformer(tableInfo, configContext, textTemplateModelGenerated);

            var classBuilderForm = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    var itemForm = string.Empty;
                    var pathTemplateField = string.Empty;
                    var textTemplateField = string.Empty;

                    if (item.Type == "string" || item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        itemForm = string.Format(@"    public {0}: string;", item.PropertyName);
                    }

                    else if (item.Type == "bool")
                    {
                        itemForm = string.Format(@"    public {0}: boolean;", item.PropertyName);
                    }

                    else if (item.Type == "bool?")
                    {
                        itemForm = string.Format(@"    public {0}?: boolean;", item.PropertyName);
                    }

                    else if (item.Type == "int" || item.Type == "float" || item.Type == "decimal")
                    {
                        itemForm = string.Format(@"    public {0}: number;", item.PropertyName);
                    }

                    else if (item.Type == "int?" || item.Type == "float?" || item.Type == "decimal?")
                    {
                        itemForm = string.Format(@"    public {0}?: number;", item.PropertyName);
                    }

                    classBuilderForm += string.Format("{0}{1}", itemForm, System.Environment.NewLine);
                }
            }

            classBuilderModelGenerated = classBuilderModelGenerated.Replace("<#formFields#>", classBuilderForm);

            using (var stream = new StreamWriter(pathOutputModelGenerated)) { stream.Write(classBuilderModelGenerated); }
        }
        #endregion

        #region save

        private void ExecuteTemplateAngularSaveComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularSaveComponent(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateController = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularSaveComponent(tableInfo));
            var textTemplateController = Read.AllText(tableInfo, pathTemplateController, this._defineTemplateFolder);
            var classBuilderController = GenericTagsTransformer(tableInfo, configContext, textTemplateController);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilderController); }
        }

        private void ExecuteTemplateAngularSaveGeneratedComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutputController = PathOutputAngular.PathOutputAngularSaveGeneratedComponent(tableInfo, configContext);

            var pathTemplateController = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularSaveGeneratedComponent(tableInfo));
            var textTemplateController = Read.AllText(tableInfo, pathTemplateController, this._defineTemplateFolder);
            var classBuilderController = GenericTagsTransformer(tableInfo, configContext, textTemplateController);

            using (var stream = new StreamWriter(pathOutputController)) { stream.Write(classBuilderController); }
        }


        private void ExecuteTemplateAngularSaveGeneratedComponentHtml(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularSaveGeneratedComponentHtml(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularSaveGeneratedComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeSaveClassBuilder(tableInfo, configContext, infos, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularSaveComponentHtml(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularSaveComponentHtml(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularSaveGeneratedComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeSaveClassBuilder(tableInfo, configContext, infos, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private string MakeSaveClassBuilder(TableInfo tableInfo, Context configContext, UniqueListInfo infos, string textTemplate)
        {
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var classBuilderForm = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (Audit.IsAuditField(item.PropertyName))
                        continue;

                    var itemForm = string.Empty;
                    var pathTemplateField = string.Empty;
                    var textTemplateField = string.Empty;

                    var fieldInBlackListCreate = FieldInBlackListSave(tableInfo, item.PropertyName);
                    if (fieldInBlackListCreate)
                        continue;

                    if (item.IsKey == 1 && !IsPropertyInstance(tableInfo, item.PropertyName))
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldHidden(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else if (item.Type == "string")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldInput(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else if (item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldDate(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }
                    else if (item.Type == "bool")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldCheckbox(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }
                    else if (item.Type == "bool?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldRadio(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else
                    {
                        if (IsPropertyInstance(tableInfo, item.PropertyName))
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldSelect(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                        else
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldInput(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                    }

                    itemForm = FormFieldSize(configContext, tableInfo, item, textTemplateField, tableInfo.twoCols);
                    itemForm = itemForm
                        .Replace("<#formType#>", "save")
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#isRequired#>", item.isNullable == 0 ? "required" : "")
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName));

                    classBuilderForm += string.Format("{0}{1}", itemForm, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#formFields#>", classBuilderForm);
            return classBuilder;
        }
        #endregion

        #region grid

        private void ExecuteTemplateAngularGridComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularGridComponent(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplateController = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularGridComponent(tableInfo));
            var textTemplateController = Read.AllText(tableInfo, pathTemplateController, this._defineTemplateFolder);
            var classBuilderController = GenericTagsTransformer(tableInfo, configContext, textTemplateController);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilderController); }
        }

        private void ExecuteTemplateAngularGridGeneratedComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutputController = PathOutputAngular.PathOutputAngularGridGeneratedComponent(tableInfo, configContext);

            var pathTemplateController = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularGridGeneratedComponent(tableInfo));
            var textTemplateController = Read.AllText(tableInfo, pathTemplateController, this._defineTemplateFolder);
            var classBuilderController = GenericTagsTransformer(tableInfo, configContext, textTemplateController);

            using (var stream = new StreamWriter(pathOutputController)) { stream.Write(classBuilderController); }
        }

        private void ExecuteTemplateAngularGridGeneratedComponentHtml(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularGridGeneratedComponentHtml(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularGridGeneratedComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeGridClassBuilder(tableInfo, configContext, infos, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularGridComponentHtml(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularGridComponentHtml(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularGridGeneratedComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeGridClassBuilder(tableInfo, configContext, infos, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private string MakeGridClassBuilder(TableInfo tableInfo, Context configContext, UniqueListInfo infos, string textTemplate)
        {
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var classBuilderThead = string.Empty;
            var classBuilderTbody = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (Audit.IsAuditField(item.PropertyName))
                        continue;

                    var itemForm = string.Empty;
                    var textTemplateThead = string.Empty;
                    var textTemplateTbody = string.Empty;

                    var fieldInBlackListCreate = FieldInBlackListSave(tableInfo, item.PropertyName);
                    if (fieldInBlackListCreate)
                        continue;

                    if (item.IsKey == 1 && !IsPropertyInstance(tableInfo, item.PropertyName))
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTheadId(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTbodyString(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else if (item.Type == "string")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTbodyString(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else if (item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTbodyDate(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }
                    else if (item.Type == "bool" || item.Type == "bool?")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTbodyBoolean(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else if (item.Type == "decimal" || item.Type == "decimal?" || item.Type == "float" || item.Type == "float?")
                    {
                        var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTheadFields(tableInfo));
                        textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                        var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTbodyNumber(tableInfo));
                        textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                    }

                    else
                    {
                        if (!IsPropertyInstance(tableInfo, item.PropertyName))
                        {
                            var pathTemplateThead = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTheadId(tableInfo));
                            textTemplateThead = Read.AllText(tableInfo, pathTemplateThead, this._defineTemplateFolder);
                            var pathTemplateTbody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularTbodyString(tableInfo));
                            textTemplateTbody = Read.AllText(tableInfo, pathTemplateTbody, this._defineTemplateFolder);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    textTemplateThead = textTemplateThead
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#isRequired#>", item.isNullable == 0 ? "required" : "")
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName));

                    textTemplateTbody = textTemplateTbody
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#isRequired#>", item.isNullable == 0 ? "required" : "")
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName));

                    classBuilderThead += string.Format("{0}{1}", textTemplateThead, System.Environment.NewLine);
                    classBuilderTbody += string.Format("{0}{1}", textTemplateTbody, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder
                .Replace("<#theadFields#>", classBuilderThead)
                .Replace("<#tbodyFields#>", classBuilderTbody);
            return classBuilder;
        }

        #endregion

        #region delete

        private void ExecuteTemplateAngularDeleteComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularDeleteComponent(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularDeleteComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularDeleteGeneratedComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutputController = PathOutputAngular.PathOutputAngularDeleteGeneratedComponent(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularDeleteGeneratedComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutputController)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularDeleteGeneratedComponentHtml(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularDeleteGeneratedComponentHtml(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularDeleteGeneratedComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }
        #endregion

        #region filter

        private void ExecuteTemplateAngularFilterComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularFilterComponent(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFilterComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularFilterGeneratedComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularFilterGeneratedComponent(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFilterGeneratedComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularFilterGeneratedComponentHtml(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularFilterGeneratedComponentHtml(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFilterGeneratedComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeFilterClassBuilder(tableInfo, configContext, infos, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularFilterComponentHtml(TableInfo tableInfo, Context configContext, UniqueListInfo infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularFilterComponentHtml(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFilterGeneratedComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = this.MakeFilterClassBuilder(tableInfo, configContext, infos, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private string MakeFilterClassBuilder(TableInfo tableInfo, Context configContext, UniqueListInfo infos, string textTemplate)
        {
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var classBuilderForm = string.Empty;

            if (infos.IsAny())
            {
                foreach (var item in infos)
                {
                    if (Audit.IsAuditField(item.PropertyName))
                        continue;

                    if (item.IsKey == 1 && !IsPropertyInstance(tableInfo, item.PropertyName))
                        continue;

                    var itemForm = string.Empty;
                    var pathTemplateField = string.Empty;
                    var textTemplateField = string.Empty;

                    var fieldInBlackListFilter = FieldInBlackListFilter(tableInfo, item.PropertyName);
                    if (fieldInBlackListFilter)
                        continue;

                    if (item.Type == "string")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldInput(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else if (item.Type == "DateTime" || item.Type == "DateTime?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldDate(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);

                        itemForm.Replace("<#propertyName#>", string.Format("{0}{1}{2}", item.PropertyName + "Start", System.Environment.NewLine, "<#propertyName#>"));
                        itemForm.Replace("<#propertyName#>", string.Format("{0}", item.PropertyName + "End"));
                    }
                    else if (item.Type == "bool" || item.Type == "bool?")
                    {
                        pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldRadio(tableInfo));
                        textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                    }

                    else
                    {
                        if (IsPropertyInstance(tableInfo, item.PropertyName))
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldSelect(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                        else
                        {
                            pathTemplateField = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularFieldInput(tableInfo));
                            textTemplateField = Read.AllText(tableInfo, pathTemplateField, this._defineTemplateFolder);
                        }
                    }

                    itemForm = FormFieldSize(configContext, tableInfo, item, textTemplateField, true);
                    itemForm = itemForm
                        .Replace("<#formType#>", "filter")
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#className#>", tableInfo.ClassName)
                        .Replace("<#isRequired#>", "")
                        .Replace("<#ReletedClass#>", PropertyInstance(tableInfo, item.PropertyName));

                    classBuilderForm += string.Format("{0}{1}", itemForm, System.Environment.NewLine);
                }
            }

            classBuilder = classBuilder.Replace("<#formFields#>", classBuilderForm);
            return classBuilder;
        }
        #endregion

        #region models

        private void ExecuteTemplateAngularModel(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularModel(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularModel(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }
        private void ExecuteTemplateAngularModelFilter(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularModelFilter(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularModelFilter(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }
        #endregion

        #region component

        private void ExecuteTemplateAngularRouting(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularRouting(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularRouting(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularComponent(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularComponent(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularComponent(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularComponentHtml(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularComponentHtml(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularComponentHtml(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        private void ExecuteTemplateAngularModule(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularModule(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.AngularModule(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }

        #endregion

        #region routing generated


        private void ExecuteTemplateAngularRoutingGenerated(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeFront)
                return;

            var pathOutput = PathOutputAngular.PathOutputAngularRoutingGenerated(tableInfo, configContext);

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._defineTemplateFolder.Define(tableInfo), DefineTemplateNameAngular.RoutingGenerated(tableInfo));
            var textTemplate = Read.AllText(tableInfo, pathTemplate, this._defineTemplateFolder);
            var classBuilder = GenericTagsTransformer(tableInfo, configContext, textTemplate);

            var itens = string.Empty;

            foreach (var item in configContext.TableInfo)
            {
                var str = @"    { path: '" + item.ClassName.ToLower() + "', loadChildren: './views/" + item.ClassName.ToLower() + "/" + item.ClassName.ToLower() + ".module#" + item.ClassName + "Module' },";
                itens += string.Format("{0}{1}", str, System.Environment.NewLine);
            }

            classBuilder = classBuilder.Replace("<#pathsRoute#>", itens);

            using (var stream = new StreamWriter(pathOutput)) { stream.Write(classBuilder); }
        }


        #endregion

        #region utils

        private string FormFieldSize(Context configContext, TableInfo tableInfo, Info info, string textTemplateForm, bool twoCols)
        {


            var cols = "col-md-12";
            if (twoCols)
                cols = "col-md-6";
            if (IsStringLengthBig(info, configContext))
                cols = "col-md-12";

            return textTemplateForm.Replace("<#colformField#>", cols);
        }

        private bool FieldInBlackListSave(TableInfo tableInfo, string propertyName)
        {
            if (tableInfo.FieldsConfig.NotIsAny())
                return false;

            return tableInfo.FieldsConfig
                .Where(_ => _.Name.ToUpper() == propertyName.ToUpper())
                .Where(_ => _.Create == false)
                .IsAny();
        }

        private bool FieldInBlackListFilter(TableInfo tableInfo, string propertyName)
        {
            if (tableInfo.FieldsConfig.NotIsAny())
                return false;

            return tableInfo.FieldsConfig
                .Where(_ => _.Name.ToUpper() == propertyName.ToUpper())
                .Where(_ => _.Filter == false)
                .IsAny();
        }
        #endregion

    }
}
