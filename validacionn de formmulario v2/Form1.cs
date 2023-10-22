using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace validacionn_de_formmulario_v2
{
    public partial class Form1 : Form

    {
        string conexionSQL = "Server=localhost;Port=3306;Database=programacion_based;Uid=root;Pwd=osorio12;";
        public Form1()
        {
            InitializeComponent();
            
            textBox3.TextChanged += validadedad;
            textBox4.TextChanged += validadestatura;
            textBox5.TextChanged += validartelefono;
            textBox1.TextChanged += validadNombre;
            textBox2.TextChanged += validadapellido;
        }
        private void InsertarRegistro(string nombre, string apellidos, int edad, decimal estatura, string telefono, string genero)
        {
            using (MySqlConnection conection = new MySqlConnection(conexionSQL))
            {
                conection.Open();
                string insertQuery = "INSERT INTO registros (Nombre, Apellidos, Edad, Estatura, Telefono, Genero)" +
                                    "VALUES (@Nombre, @Apellidos, @Edad, @Estatura, @Telefono, @Genero)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Apellidos", apellidos);
                    command.Parameters.AddWithValue("@Edad", edad);
                    command.Parameters.AddWithValue("@Estatura", estatura);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Genero", genero);

                    command.ExecuteNonQuery();




                }
                conection.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombres = textBox1.Text;
            string apellidos = textBox2.Text;
            string edad = textBox3.Text;
            string estatura = textBox4.Text;
            string telefono = textBox5.Text;

            // Obtener el género seleccionado
            string genero = "";
            if (radioButton1.Checked)
            {
                genero = "Hombre";
            }
            else if (radioButton1.Checked)
            {
                genero = "Mujer";
            }

            // Validar que los campos tengan el formato correcto
            if ((EsEnteroValido(edad) && EsDecimalValido(estatura) &&
            EsEnteroValidoDe10Digitos(telefono) && EsTextoValido(nombres) && EsTextoValido(apellidos)))
            {
                // Crear una cadena con los datos
                string datos = $"Nombres: {nombres}\r\nApellidos: {apellidos}\r\nTelefono:{telefono}kg\r\nEstatura: {estatura}cm\r\nEdad:{edad}años\r\nGenero: {genero}\r\n";
                // Guardar los datos en un archivo de texto
                string rutaarchivo = "D://TXT PROGRAMACION.txt";

                bool archivoexiste = File.Exists(rutaarchivo);
                if (archivoexiste == false)
                {
                    File.WriteAllText(rutaarchivo, datos);

                }
                else
                {
                    using (StreamWriter writer = new StreamWriter("datos.txt", true))
                    {
                        if (archivoexiste)
                        {
                            writer.WriteLine();
                            InsertarRegistro(nombres, apellidos, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                            MessageBox.Show("Datos ingresados correctamente: ");

                        }
                        else
                        {
                            writer.WriteLine(datos);
                            InsertarRegistro(nombres, apellidos, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                            MessageBox.Show("Datos ingresados correctamente");
                        }


                        

                    }

                }


                // Mostrar un mensaje con los datos capturados
                MessageBox.Show("Datos guardados con éxito:\n\n" + datos, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los controles después de guardar

            }
            else
            {
                MessageBox.Show("Por favor, ingrese datos válidos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }

        private bool EsEnteroValidoDe10Digitos(string input)
        {
            if (input.Length != 10)
            {
                return false;
            }
            if (!input.All(char.IsDigit))
            {
                return false;
            }
            return true;
        }
       private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$");
        }

        private void validadedad(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsEnteroValido(textBox.Text))
            {
                MessageBox.Show("porfavor , ingresa una edad valida", "erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }

        }


        private void validadestatura(object sender, EventArgs e)

        {
            TextBox textBox = (TextBox)sender;
            if (!EsDecimalValido(textBox.Text))
            {
                MessageBox.Show("porfavor , ingresa una estaura valida", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }

        }



        private void validartelefono(object sender, EventArgs e)

        {
            TextBox textBox = (TextBox)sender;
            string input = textBox.Text;
            if (input.Length < 10)
            {
                if (!EsEnteroValidoDe10Digitos(input))
                {

                    return;
                }

            }
            if (!EsEnteroValidoDe10Digitos(input))
            {

                {

                    MessageBox.Show("porfavor , ingresa una telefono valido", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Clear();

                }


            }
        }
        private void validadNombre(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsTextoValido(textBox.Text))
            {
                MessageBox.Show("porfavor , ingresa un nombre valido", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }

        }
        private void validadapellido(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsTextoValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingresa un apellido válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;


        }
    }
}
