﻿using CrudCreator.Model;
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
            uno.Name = "Nombre";
            uno.Type = "String";
            uno.Visibility = "Public";
            dos.Name = "Apellido";
            dos.Type = "String";
            dos.Visibility = "Private";
            fechaNacimiento.Name = "fechaNacimiento";
            fechaNacimiento.Type = "DateTime";
            fechaNacimiento.Visibility = "public";

            Entity prueba = new Entity();
            Attr[] arr = { uno, dos, fechaNacimiento };
            prueba.Define("Persona", arr);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testobject.cs", prueba.ClassDefinition);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsql.cs", prueba.SqlCreate);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlUpdate.cs", prueba.SqlUpdate);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlSelectAll.cs", prueba.SqlSelectAll);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlSelectById.cs", prueba.SqlSelectById);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlDelete.cs", prueba.SqlDelete);
            System.IO.File.WriteAllText(@"C:\Users\arser\Desktop\sqltests\testsqlInsert.cs", prueba.SqlInsert);
        }
    }
}
