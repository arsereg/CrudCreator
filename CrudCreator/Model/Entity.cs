using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudCreator.Model
{
    class Entity
    {
        String classDefinition;
        String sqlCreate;
        String sqlUpdate;
        String sqlSelectAll;

        public string ClassDefinition { get => classDefinition; set => classDefinition = value; }
        public string SqlCreate { get => sqlCreate; set => sqlCreate = value; }
        public string SqlUpdate { get => sqlUpdate; set => sqlUpdate = value; }
        public string SqlSelectAll { get => sqlSelectAll; set => sqlSelectAll = value; }

        public void Define(String pname, Attr[] param)
        {
            DefineClass(pname, param);
            DefineSql(pname, param);
            DefineUpdate(pname, param);
            DefineSelectAll(pname);
            DefineSelectById(pname);
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
public class "+pname+@" : BaseEntity
{"+
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

CREATE PROCEDURE Select_All_" + pname + @"\n 
	
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE " + pname + @"
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

CREATE PROCEDURE Select_All_" + pname + @"\n 
	@P_ID_" + pname + @"
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE " + pname + @"
   select * from " + pname + @"
    where id_"+pname+@" = @P_ID
END
GO
";
            SqlSelectAll = result;
        }

        private void DefineDelete(String pname)
        {

            string result = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Select_All_" + pname + @"\n 
	@P_ID_" + pname + @"
AS
BEGIN
	SET NOCOUNT ON;
	Delete from " + pname+@"
    where id_" + pname + @" = @P_ID_"+pname+@"
END
GO
";
            SqlSelectAll = result;
        }

    }
}
