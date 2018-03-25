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
        private static String name = "";
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
            name = Console.ReadLine();
            nameAdded = true;
        }

        public static void AgregarAtributo()
        {
            Attr unAtributo = new Attr();
            Console.WriteLine("Digite el nombre del atributo:");
            String userInput = Console.ReadLine();
            unAtributo.SqlName = userInput.ToUpper().Replace(" ", "_");
            unAtributo.Name = PascalCase(userInput);
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
            Console.WriteLine("Digite el tipo:");
            string[] types = { "String", "int", "double", "bool", "DateTime"};
            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine((i + 1) +"- "+ types[i]);
            }
            unAtributo.Type = types[int.Parse(Console.ReadLine()) -1];
            atributos.Add(unAtributo);
            attAdded = true;
        }

        public static void Reiniciar()
        {
            atributos = new List<Attr>();
            name = "";
            attAdded = false;
            nameAdded = false;
        }

        public static void Procesar()
        {
            if (attAdded && nameAdded)
            {
                unaEntidad.Define(name, atributos.ToArray());
                EscribirArchivos();
                Reiniciar();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Entidad " + name + " agregada");
                Console.WriteLine("-----------------------------");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("Asegurese de haber agregado un nombre y al menos un atributo para la entidad");
            }
        }

        public static void EscribirArchivos()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Objetos\" + @"\" + name.ToLowerInvariant() + @"\";
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName + name + ".cs", unaEntidad.ClassDefinition);
            System.IO.File.WriteAllText(file.FullName + name + "_SQL_Create.sql", unaEntidad.SqlCreate);
            System.IO.File.WriteAllText(file.FullName + name + "_SQL_Update.sql", unaEntidad.SqlUpdate);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Select_All.sql", unaEntidad.SqlSelectAll);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Select_By_Id.sql", unaEntidad.SqlSelectById);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Delete.sql", unaEntidad.SqlDelete);
            System.IO.File.WriteAllText(file.FullName + name + "_Sql_Insert.sql", unaEntidad.SqlInsert);
            System.IO.File.WriteAllText(file.FullName + name + "_Mapper.cs", unaEntidad.Mapper);
            Process.Start(path);
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
