using CrudCreatorConsole.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudCreatorConsole
{
    class Program
    {
        private static Entity unaEntidad = new Entity();
        private static List<Attr> atributos = new List<Attr>();
        private static String fullName = "";
        private static String name = "";
        private static String sqlName = "";
        private static bool attAdded = false;
        private static bool nameAdded = false;
        static void Main(string[] args)
        {
            Init();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("8===D");
            Console.WriteLine("Llevese esta");
            Console.ForegroundColor = ConsoleColor.Red;
            System.Threading.Thread.Sleep(5000);
        }

        public static void Init()
        {
            int accion = -1;
            do
            {
                MostrarMenu();
                accion = int.Parse(Console.ReadLine());
                RealizarAccion(accion);
            } while (accion != 0);
        }

        public static void MostrarMenu()
        {
            string[] opciones = { "Asignar Nombre de Clase", "Agregar Atributo", "Procesar" ,"Reiniciar" };
            for (int i = 0; i < opciones.Length; i++)
            {
                Console.WriteLine((i + 1) +"- "+ opciones[i]);
            }
            Console.WriteLine("0- Salir");
        }

        public static void RealizarAccion(int paccion)
        {
            switch (paccion)
            {
                case 1: AsignarNombre();
                    break;
                case 2: AgregarAtributo();
                    break;
                case 3:Procesar();
                    break;
                case 4:Reiniciar();
                    break;
            }
        }

        public static void AsignarNombre()
        {
            Console.WriteLine("Digite el nombre de la clase.");
            string userInput = Console.ReadLine();
            fullName = userInput;
            name = Entity.PascalCase(userInput);
            sqlName = userInput.ToUpper().Replace(" ", "_");
            nameAdded = true;
            Console.WriteLine();
        }

        public static void AgregarAtributo()
        {
            Attr unAtributo = new Attr();
            Console.WriteLine("Digite el nombre del atributo:");
            String userInput = Console.ReadLine();
            Console.WriteLine();
            unAtributo.SqlName = userInput.ToUpper().Replace(" ", "_");
            unAtributo.Name = Entity.PascalCase(userInput);
            Console.WriteLine("Seleccione la privacidad del atributo:");
            Console.WriteLine("1- public");
            Console.WriteLine("2- private");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1: unAtributo.Visibility = "public";
                    break;
                case 2: unAtributo.Visibility = "private";
                    break;
            }
            Console.WriteLine("");
            Console.WriteLine("Digite el tipo:");
            string[] types = { "String", "int", "double", "bool", "DateTime"};
            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine((i + 1) +"- "+ types[i]);
            }
            unAtributo.Type = types[int.Parse(Console.ReadLine()) -1];
            atributos.Add(unAtributo);
            attAdded = true;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("----------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Parametro agregado: ");
            ImprimirAtributos(unAtributo);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("----------");
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static void Reiniciar()
        {
            Console.WriteLine("Desea reiniciar el estado? S/N");
            if (Console.ReadLine().ToLower().Equals("s"))
            {
                atributos = new List<Attr>();
                name = "";
                attAdded = false;
                nameAdded = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("---------------");
                Console.WriteLine("Todos los campos se reiniciaron");
                Console.WriteLine("---------------");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("---------------");
                Console.WriteLine("Se mantiene estado");
                Console.WriteLine("---------------");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void Procesar()
        {
            if (attAdded && nameAdded)
            {
                Console.WriteLine("Está seguro que desea procesar la entidad? S/N");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(name);
                Console.ForegroundColor = ConsoleColor.White;
                foreach (Attr at in atributos)
                {
                    ImprimirAtributos(at);
                }
                if (Console.ReadLine().ToLower().Equals("s"))
                {
                    unaEntidad.Define(name, atributos.ToArray(), sqlName, fullName);
                    EscribirArchivos();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("Entidad " + name + " creada");
                    Console.WriteLine("-----------------------------");
                    Console.ForegroundColor = ConsoleColor.White;
                    Reiniciar();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("Entidad no escrita. Estado se mantiene intacto.");
                    Console.WriteLine("-----------------------------");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("---------------");
                Console.WriteLine("Asegurese de haber agregado un nombre y al menos un atributo para la entidad");
                Console.WriteLine("---------------");
                Console.ForegroundColor = ConsoleColor.White;
                
            }
        }

        public static void EscribirArchivos()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Objetos\" + @"\" + name.ToLowerInvariant() + @"\";
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName + Entity.PascalCase(name) + ".cs", unaEntidad.ClassDefinition);
            System.IO.File.WriteAllText(file.FullName + name + "_SQL_Create.sql", unaEntidad.SqlCreate);
            System.IO.File.WriteAllText(file.FullName + name + "_SQL_Update.sql", unaEntidad.SqlUpdate);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Select_All.sql", unaEntidad.SqlSelectAll);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Select_By_Id.sql", unaEntidad.SqlSelectById);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Delete.sql", unaEntidad.SqlDelete);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Insert.sql", unaEntidad.SqlInsert);
            System.IO.File.WriteAllText(file.FullName + name + "Mapper.cs", unaEntidad.Mapper);
            System.IO.File.WriteAllText(file.FullName + name + "Manager.cs", unaEntidad.Manager);
            Process.Start(path);
        }

        public static void ImprimirAtributos(Attr pAtributo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(pAtributo.Visibility + " ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(pAtributo.Type + " ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(pAtributo.Name);
            Console.ForegroundColor = ConsoleColor.White;
        }

       
    }
}
