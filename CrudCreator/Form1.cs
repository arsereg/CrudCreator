using CrudCreator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Attr uno = new Attr();
            Attr dos = new Attr();
            Attr fechaNacimiento = new Attr();
            Attr tres = new Attr();
            uno.Name = "Nombre";
            uno.Type = "String";
            uno.Visibility = "Public";
            dos.Name = "Apellido";
            dos.Type = "String";
            dos.Visibility = "Private";
            fechaNacimiento.Name = "fechaNacimiento";
            fechaNacimiento.Type = "DateTime";
            fechaNacimiento.Visibility = "public";
            tres.Name = "Color";
            tres.Type = "String";
            tres.Visibility = "private";

            Entity prueba = new Entity();
            Attr[] arr = { uno, dos, fechaNacimiento, tres };
            prueba.Define("Figura", arr);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testobject.cs", prueba.ClassDefinition);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsql.cs", prueba.SqlCreate);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlUpdate.cs", prueba.SqlUpdate);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlSelectAll.cs", prueba.SqlSelectAll);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlSelectById.cs", prueba.SqlSelectById);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlDelete.cs", prueba.SqlDelete);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlInsert.cs", prueba.SqlInsert);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testMapper.cs", prueba.Mapper);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
