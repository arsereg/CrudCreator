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

            Attr nombre = new Attr();
            Attr ubicacion = new Attr();
            Attr capMax = new Attr();
            Attr cantParqueoBus = new Attr();
            Attr costoParqueo = new Attr();

            nombre.Name = "nombre";
            nombre.Type = "string";
            nombre.Visibility = "private";

            ubicacion.Name = "ubicacion";
            ubicacion.Type = "string";
            ubicacion.Visibility = "private";

            capMax.Name = "capacidadMaxima";
            capMax.Type = "string";
            capMax.Visibility = "private";

            cantParqueoBus.Name = "cantParqueoBus";
            cantParqueoBus.Type = "int";
            cantParqueoBus.Visibility = "private";

            costoParqueo.Name = "costoParqueo";
            costoParqueo.Type = "double";
            costoParqueo.Visibility = "private";


            Entity prueba = new Entity();
            Attr[] arr = { nombre, ubicacion, capMax, cantParqueoBus, costoParqueo };
            prueba.Define("Terminal", arr);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AgregarRow()
        {
            MessageBox.Show("Tonses");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AgregarRow();
        }
    }
}
