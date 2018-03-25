using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudCreatorConsole.Model
{
    class Entity
    {
        String classDefinition;
        String sqlCreate;
        String sqlUpdate;
        String sqlSelectAll;
        String sqlSelectById;
        String sqlInsert;
        String sqlDelete;
        String mapper;

        public string ClassDefinition { get => classDefinition; set => classDefinition = value; }
        public string SqlCreate { get => sqlCreate; set => sqlCreate = value; }
        public string SqlUpdate { get => sqlUpdate; set => sqlUpdate = value; }
        public string SqlSelectAll { get => sqlSelectAll; set => sqlSelectAll = value; }
        public string SqlInsert { get => sqlInsert; set => sqlInsert = value; }
        public string SqlSelectById { get => sqlSelectById; set => sqlSelectById = value; }
        public string SqlDelete { get => sqlDelete; set => sqlDelete = value; }
        public string Mapper { get => mapper; set => mapper = value; }

        public void Define(String pname, Attr[] param)
        {
            DefineClass(pname, param);
            DefineSql(pname, param);
            DefineUpdate(pname, param);
            DefineSelectAll(pname);
            DefineSelectById(pname);
            DefineInsert(pname, param);
            DefineDelete(pname);
            DefineMapper(pname, param);
        }
        private void DefineClass(String pname, Attr[] param)
        {

            string attributes = "";
            for (int i = 0; i < param.Length; i++)
            {

                string visibility = param[i].Visibility.ToLower();
                string type = param[i].Type;
                string name = param[i].Name;
                string accessors = " { get; set; }\n\r";
                attributes = attributes + "\n" + visibility + " " + type + " " + name + accessors;
            }
            string result = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_POJO
{
public class " + PascalCase(pname) + @" : BaseEntity
{
<<<<<<< Updated upstream
    int id"+ pname.First().ToString().ToUpper() + pname.Substring(1) + @";
    public int Id"+ pname.First().ToString().ToUpper() + pname.Substring(1) + @" { get => id"+ pname.First().ToString().ToUpper() + pname.Substring(1) + @"; set => id"+ pname.First().ToString().ToUpper() + pname.Substring(1) + @"= value; }
=======
    int id;
    public int Id { get => id; set => id= value; }
>>>>>>> Stashed changes

" +
    attributes
    + @"
        public " + pname + @"()
        {
        }
    }
}";
            this.classDefinition = result;
        }

        private void DefineSql(String pname, Attr[] param)
        {
            string parameters = "";
            for (int i = 0; i < param.Length; i++)
            {
                parameters += ",\n" + param[i].SqlName + " " + DefineColumn(param[i]);

            }
            string result = @"CREATE TABLE " + pname + @" (
id_" + pname + @" INT NOT NULL IDENTITY(1,1) PRIMARY KEY" +
parameters + @"
);
";
            SqlCreate = result;
        }

        private string DefineColumn(Attr param)
        {
            String result = "";
            switch (param.Type.ToLower())
            {
                case "string":
                    result = "varchar(250)";
                    break;
                case "int":
                    result = "int";
                    break;
                case "double":
                    result = "float";
                    break;
                case "datetime":
                    result = "datetime";
                    break;
                case "bool":
                    result = "bit";
                    break;
            }
            return result;
        }

        private void DefineUpdate(String pname, Attr[] param)
        {
            string sets = "";
            string parameters = "@P_ID" + pname + " int,";
            for (int i = 0; i < param.Length; i++)
            {
                string name = param[i].Name;
                string type = DefineColumn(param[i]);
                parameters += "@P_" + name + " " + type;
                sets += param[i].Name + " = @P_" + name;

                if (i < param.Length - 1)
                {
                    parameters += ",\n";
                    sets += ",\n";
                }
                else
                {
                    parameters += "\n";
                    sets += "\n";
                }

            }


            string result = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE UPD_" + pname + "\n" + parameters + @" 
	
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE " + pname + @"
   SET " + sets + @"
        WHERE id_" + pname + @" = @P_ID" + pname + @"
END
GO
";
            SqlUpdate = result;
        }



        private void DefineSelectAll(String pname)
        {

            string result = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Select_All_" + pname + @"
	
AS
BEGIN
	SET NOCOUNT ON;
   select * from " + pname + @"
END
GO
";
            SqlSelectAll = result;
        }

        private void DefineSelectById(String pname)
        {

            string result = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Select_By_ID_" + pname + @"
	@P_ID_" + pname + @" int
AS
BEGIN
	SET NOCOUNT ON;
   select * from " + pname + @"
    where id_" + pname + @" = @P_ID_" + pname + @"
END
GO
";
            SqlSelectById = result;
        }

        private void DefineDelete(String pname)
        {

            string result = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Del_" + pname + @"
	@P_ID_" + pname + @" int
AS
BEGIN
	SET NOCOUNT ON;
	Delete from " + pname + @"
    where id_" + pname + @" = @P_ID_" + pname + @"
END
GO
";
            sqlDelete = result;
        }

        public void DefineInsert(String pname, Attr[] param)
        {

            string names = "";
            for (int i = 0; i < param.Length; i++)
            {
                names += param[i].Name;
                if (i < param.Length - 1)
                {
                    names += ",\n";
                }
            }
            string parametersType = "";
            for (int i = 0; i < param.Length; i++)
            {
                string name = param[i].Name;
                string type = DefineColumn(param[i]);
                parametersType += "@P_" + name + " " + type;

                if (i < param.Length - 1)
                {
                    parametersType += ",\n";
                }
                else
                {
                    parametersType += "\n";
                }
            }


            string parametersName = "";
            for (int i = 0; i < param.Length; i++)
            {
                string name = param[i].Name;
                string type = DefineColumn(param[i]);
                parametersName += "@P_" + name;

                if (i < param.Length - 1)
                {
                    parametersName += ",\n";
                }
                else
                {
                    parametersName += "\n";
                }
            }
            string result = @"
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE INS_" + pname + "\n" + parametersType + @"
	
AS
BEGIN
	INSERT INTO " + pname + @"(" + names + @") VALUES(" + parametersName + @")
END
";

            SqlInsert = result;
        }




        public void DefineMapper(String pname, Attr[] param)
        {
            string columns = "";
            string parametros = "";
            string parametrosParaConstruir = "";

            for (int i = 0; i < param.Length; i++)
            {
                string unParam = "private const string DB_COL_" + param[i].Name + " = \"" + param[i].SqlName + "\";";
                string prefix = param[i].Name + " = ";
                string suffix = "(DB_COL_" + param[i].Name + ", c." + param[i].Name + ");\n";
                columns += unParam + "\n";
                switch (param[i].Type.ToLower())
                {
                    case "string":
                        parametros += "operation.AddVarcharParam" + suffix;
                        parametrosParaConstruir += prefix + "GetStringValue(row, DB_COL_" + param[i].Name + ")";
                        break;
                    case "int":
                        parametros += "operation.AddIntParam" + suffix;
                        parametrosParaConstruir += prefix + "GetIntValue(row, DB_COL_" + param[i].Name + ")";
                        break;
                    case "double":
                        parametros += "operation.AddDoubleParam" + suffix;
                        parametrosParaConstruir += prefix + "GetDoubleValue(row, DB_COL_" + param[i].Name + ")";
                        break;
                    case "datetime":
                        parametros += "operation.AddVarcharParam" + suffix;
                        parametrosParaConstruir += prefix + "GetDateValue(row, DB_COL_" + param[i].Name + ")";
                        break;
                    case "bool":
                        parametros += "operation.AddVarcharParam" + suffix;
                        parametrosParaConstruir += prefix + "GetBooleanValue(row, DB_COL_" + param[i].Name + ")";
                        break;
                }
                if (i < param.Length - 1)
                {
                    parametrosParaConstruir += ",\n";
                }
            }

            StringBuilder result = new StringBuilder();
            result.Append("using DataAccess.Dao;\n");
            result.Append("using Entities_POJO;\n");
            result.Append("using System.Collections.Generic;\n");
            result.Append("namespace DataAccess.Mapper\n");
            result.Append("{\n");
            result.Append("public class " + pname + " : EntityMapper, ISqlStaments, IObjectMapper\n");
            result.Append("{\n");
            result.Append(columns);

            /*Comienzan definiciones de procedimientos*/
            result.Append("public SqlOperation GetCreateStatement(BaseEntity entity)\n");
            result.Append("{\n");
            result.Append("var operation = new SqlOperation {ProcedureName = \"INS_" + pname + "\"};\n");
            result.Append("var c = (" + pname + ") entity;\n");
            result.Append(parametros + "\n");
            result.Append("return operation;\n");
            result.Append("}\n");
            for (int i = 0; i < 3; i++)
            {
                result.Append("\n");
            }
            /*Finaliza GetCreateStatement*/

            result.Append("public SqlOperation GetRetriveStatement(BaseEntity entity)\n");
            result.Append("{\n");
            result.Append("var operation = new SqlOperation {ProcedureName = \"Select_By_ID_" + pname + "\"};\n");
            result.Append("var c = (" + pname + ")entity;\n");
            result.Append("operation.AddVarcharParam(DB_COL_ID, c.Id);\n");
            result.Append("return operation;");
            result.Append("}\n");
            for (int i = 0; i < 3; i++)
            {
                result.Append("\n");
            }
            /*Finaliza GetRetriveStatement*/

            result.Append("public SqlOperation GetRetriveAllStatement()\n");
            result.Append("{\n");
            result.Append("var operation = new SqlOperation { ProcedureName = Select_All_" + pname + " };\n");
            result.Append("return operation;\n");
            result.Append("}\n");
            for (int i = 0; i < 3; i++)
            {
                result.Append("\n");
            }
            /*Finaliza GetRetrieveAllStatement*/

            result.Append("public SqlOperation GetUpdateStatement(BaseEntity entity)\n");
            result.Append("{\n");
            result.Append("var operation = new SqlOperation { ProcedureName = UPD_" + pname + " };\n");
            result.Append("var c = (" + pname + ")entity;\n");
            result.Append(parametros + "\n");
            result.Append("return operation;\n");
            result.Append("}\n");
            for (int i = 0; i < 3; i++)
            {
                result.Append("\n");
            }
            /*Finaliza GetUpdateStatement*/

            result.Append("public SqlOperation GetDeleteStatement(BaseEntity entity)\n");
            result.Append("{\n");
            result.Append("var operation = new SqlOperation { ProcedureName = Del_" + pname + " };\n");
            result.Append("var c = (" + pname + ")entity;\n");
            result.Append("operation.AddVarcharParam(DB_COL_ID, c.Id);\n");
            result.Append("return operation;\n");
            result.Append("}\n");
            for (int i = 0; i < 3; i++)
            {
                result.Append("\n");
            }
            /*Finaliza GetDeleteStatement*/

            result.Append("public List<BaseEntity> BuildObjects(List<Dictionary<string, object>> lstRows)\n");
            result.Append("{\n");
            result.Append("var lstResults = new List<BaseEntity>();\n");
            result.Append("foreach (var row in lstRows)\n");
            result.Append("{\n");
            result.Append("var " + pname + " = BuildObject(row);\n");
            result.Append("lstResults.Add(" + pname + ");\n");
            result.Append("}\n");
            for (int i = 0; i < 3; i++)
            {
                result.Append("\n");
            }
            /*Finaliza BuildObjects*/

            result.Append("public BaseEntity BuildObject(Dictionary<string, object> row)\n");
            result.Append("{\n");
            result.Append("var " + pname.ToUpper() + " = new " + pname + "\n");
            result.Append("{\n");
            result.Append(parametrosParaConstruir + "\n");
            result.Append("};\n");
            result.Append("return customer;\n");
            result.Append("}\n");
            result.Append("}\n");
            result.Append("}\n");
            Mapper = result.ToString();
        }



        public static string PascalCase(string textToChange)
        {
            System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();

            foreach (char c in textToChange)
            {

                if (!Char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString();


            result = result.ToLower();

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            return myTI.ToTitleCase(result).Replace(" ", String.Empty);

        }

    }
}

