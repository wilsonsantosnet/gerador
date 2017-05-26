using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public static class TypeConvertCSharp
    {

        public static string Convert(string typeSQl, int isNullable)
        {

            switch (typeSQl)
            {
                case "char":
                case "nchar":
                case "nvarchar":
                case "varchar":
                case "text":
                    return "string";
                case "date":
                case "datetime":
                    return isNullable == 1 ? "DateTime?" : "DateTime";
                case "bigint":
                    return isNullable == 1 ? "Int64?" : "Int64";
                case "int":
                    return isNullable == 1 ? "int?" : "int";
                case "bit":
                    return isNullable == 1 ? "bool?" : "bool";
                case "tinyint":
                    return isNullable == 1 ? "byte?" : "byte";
                case "smallint":
                case "int16":
                    return isNullable == 1 ? "Int16?" : "Int16";
                case "numeric":
                case "decimal":
                case "money":
                    return isNullable == 1 ? "decimal?" : "decimal";
                case "float":
                    return isNullable == 1 ? "float?" : "float";
                case "image":
                    return isNullable == 1 ? "byte[]" : "byte[]";



                default:
                    return typeSQl;
            }

        }


    }
}
