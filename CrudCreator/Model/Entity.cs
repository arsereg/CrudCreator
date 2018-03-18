using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudCreator.Model
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

        public string ClassDefinition { get => classDefinition; set => classDefinition = value; }
        public string SqlCreate { get => sqlCreate; set => sqlCreate = value; }
        public string SqlUpdate { get => sqlUpdate; set => sqlUpdate = value; }
        public string SqlSelectAll { get => sqlSelectAll; set => sqlSelectAll = value; }
        public string SqlInsert { get => sqlInsert; set => sqlInsert = value; }
        public string SqlSelectById { get => sqlSelectById; set => sqlSelectById = value; }
        public string SqlDelete { get => sqlDelete; set => sqlDelete = value; }

        public void Define(String pname, Attr[] param)
        {
            DefineClass(pname, param);
            DefineSql(pname, param);
            DefineUpdate(pname, param);
            DefineSelectAll(pname);
            DefineSelectById(pname);
            DefineInsert(pname, param);
            DefineDelete(pname);
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
                attributes = attributes + "\n" +visibility + " " + type + " " + name + accessors;
            }
            string result = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_POJO
{
public class "+pname+ @" : BaseEntity
{
    int id;
    public string Id { get => id; set => id= value; }

" +
    attributes
    +@"
        public "+pname+@"()
        {
        }
    }
}";
            this.classDefinition = result;
        }

        private void DefineSql(String pname, Attr[] param)
        {
            string parameters = "";
            for(int i = 0; i < param.Length; i++) {
                parameters += ",\n" + param[i].Name + " " + DefineColumn(param[i]);

            }
            string result = @"CREATE TABLE "+pname+ @" (
id_"+pname+@" INT NOT NULL IDENTITY(1,1) PRIMARY KEY" +
parameters+@"
);
";
            SqlCreate = result;
        }

        private string DefineColumn(Attr param)
        {
            String result = "";
            switch (param.Type.ToLower())
            {
                case "string": result = "varchar(250)";
                    break;
                case "int": result = "int";
                    break;
                case "double": result = "float";
                    break;
                case "datetime": result = "datetime";
                    break;
                case "boolean": result = "bit";
                    break;
            }
            return result;
        }

        private void DefineUpdate(String pname, Attr[] param)
        {
            string sets = "";
            string parameters = "@P_ID"+pname+" int,";
            for (int i = 0; i < param.Length; i++)
            {
                string name = param[i].Name;
                string type = DefineColumn(param[i]);
                parameters += "@P_"+name + " " + type;
                sets += param[i].Name + " = @P_" + name;

                if (i < param.Length -1)
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

CREATE PROCEDURE UPD_"+pname+"\n"+parameters+@" 
	
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE "+pname+@"
   SET "+sets+@"
        WHERE id_"+pname+@" = @P_ID"+pname+@"
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
   select * from "+pname+@"
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
    where id_"+pname+@" = @P_ID_"+pname+@"
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
	Delete from " + pname+@"
    where id_" + pname + @" = @P_ID_"+pname+@"
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
                if (i < param.Length -1)
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
Create PROCEDURE INS_"+pname + "\n" +parametersType+@"
	
AS
BEGIN
	INSERT INTO "+pname+@"("+names+@") VALUES("+parametersName+@")
END
";
            
            SqlInsert = result;
        }




        public void DefineMapper(String pname, Attr[] param)
        {
            string columns = "";
            string parametros = "";

            StringBuilder result = new StringBuilder();
            result.Append("using DataAcess.Dao;\n");
            result.Append("using Entities_POJO;\n");
            result.Append("using System.Collections.Generic;\n");
            result.Append("namespace DataAcess.Mapper\n");
            result.Append("{\n");
            result.Append("public class "+pname+ " : EntityMapper, ISqlStaments, IObjectMapper\n");
            result.Append("{\n");
            result.Append(columns);
            result.Append("public SqlOperation GetCreateStatement(BaseEntity entity)\n");
            result.Append("{\n");
            result.Append("var operation = new SqlOperation {ProcedureName = \"INS_" + pname+ "\"};\n");
            result.Append("var c = ("+pname+ ") entity;\n");
            result.Append(parametros + "\n");
            result.Append("return operation;\n");
            result.Append("}\n");
            for (int i = 0; i < 3; i++)
            {
                result.Append("\n");
            }
            /*Finaliza GetCreateStatement*/

            result.Append("public SqlOperation GetRetriveStatement(BaseEntity entity)");
            result.Append("{");
            result.Append("var operation = new SqlOperation {ProcedureName = \"Select_By_ID_" + pname+"\"};");
            result.Append("var c = ("+pname+")entity;");
            result.Append("operation.AddVarcharParam(DB_COL_ID, c.Id);");
            result.Append("return operation;");
            result.Append("}");

            /*Finaliza GetRetriveStatement*/

            result.Append("public SqlOperation GetRetriveAllStatement()");
            result.Append("{");
            result.Append("var operation = new SqlOperation { ProcedureName = Select_All_" + pname+" };");
            result.Append("return operation;");
            result.Append("}");

            /*Finaliza GetRetrieveAllStatement*/

            result.Append("public SqlOperation GetUpdateStatement(BaseEntity entity)");
            result.Append("{");
            result.Append("var operation = new SqlOperation { ProcedureName = UPD_"+pname+" };");
            result.Append("var c = ("+pname+")entity;");
            result.Append(parametros);
            result.Append("return operation;");
            result.Append("}");

            /*Finaliza GetUpdateStatement*/

            /*SEGUIR ACA*/
        }


































    }
}

