using System;                       // Funciones básicas de C#.
using System.Collections.Generic;   // Permite trabajar con listas de datos.
using System.IO;                   // Permite crear y escribir archivos.
using System.Net.Http;              // Permite realizar peticiones a la API.
using System.Text;                  // Permite utilizar StringBuilder y codificación UTF-8.
using System.Text.Json;             // Permite procesar y organizar datos JSON.
using System.Windows.Forms;         // Permite utilizar los controles de Windows Forms.

namespace API_TAREA
{
    // Clase principal que representa el formulario de la aplicación.
    public partial class Form1 : Form
    {
        // Cliente HTTP reutilizable para conectarse a la API.
        private static readonly HttpClient cliente = new HttpClient();

        // Variable que almacena la respuesta obtenida de la API.
        private string respuestaJson = "";

        // Constructor del formulario.
        public Form1()
        {
            // Inicializa los controles creados en el diseñador.
            InitializeComponent();
        }

        // Evento del título de la URL, si está conectado en el diseñador.
        private void label1_Click(object sender, EventArgs e)
        {
            // Actualmente no se necesita ninguna acción.
        }

        // =====================================================
        // BOTÓN CONSULTAR
        // =====================================================

        // Este método se ejecuta cuando el usuario pulsa Consultar.
        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener la dirección escrita en el campo de URL.
                string url = txtUrl.Text.Trim();

                // Comprobar que la URL tenga un formato válido.
                if (!Uri.TryCreate(
                        url,
                        UriKind.Absolute,
                        out Uri? uri)
                    || uri == null
                    || (uri.Scheme != "https" && uri.Scheme != "http"))
                {
                    // Mostrar un mensaje si la dirección no es válida.
                    MessageBox.Show(
                        "Introduce una URL válida.",
                        "URL incorrecta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // Detener la ejecución si la URL es incorrecta.
                    return;
                }

                // Desactivar el botón para evitar consultas simultáneas.
                btnConsultar.Enabled = false;

                // Mostrar un mensaje mientras se espera la respuesta.
                rbtResultado.Text = "Consultando la API...";

                // Enviar la petición HTTP.
                HttpResponseMessage respuestaHttp =
                    await cliente.GetAsync(uri);

                // Leer el contenido de la respuesta.
                respuestaJson =
                    await respuestaHttp.Content.ReadAsStringAsync();

                // Comprobar si la API respondió con un error.
                if (!respuestaHttp.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        respuestaJson,
                        "Error de la API",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                // Convertir el texto recibido en un documento JSON.
                using JsonDocument documento =
                    JsonDocument.Parse(respuestaJson);

                // Obtener el elemento principal del JSON.
                JsonElement raiz = documento.RootElement;

                // StringBuilder permitirá construir el texto de varios chistes.
                StringBuilder resultado = new StringBuilder();

                // =====================================================
                // VARIOS CHISTES
                // =====================================================

                if (raiz.TryGetProperty(
                        "jokes",
                        out JsonElement chistes) &&
                    chistes.ValueKind == JsonValueKind.Array)
                {
                    // Contador para mostrar el número de cada chiste.
                    int numero = 1;

                    // Recorrer todos los chistes recibidos.
                    foreach (JsonElement chiste in chistes.EnumerateArray())
                    {
                        // Obtener el tipo de chiste.
                        string? tipo =
                            chiste.GetProperty("type").GetString();

                        // Mostrar el número del chiste.
                        resultado.AppendLine("CHISTE " + numero);
                        resultado.AppendLine();

                        // Si el chiste tiene una sola parte.
                        if (tipo == "single")
                        {
                            string? texto =
                                chiste.GetProperty("joke").GetString();

                            resultado.AppendLine(texto);
                        }

                        // Si el chiste tiene pregunta y respuesta.
                        else if (tipo == "twopart")
                        {
                            string? pregunta =
                                chiste.GetProperty("setup").GetString();

                            string? respuesta =
                                chiste.GetProperty("delivery").GetString();

                            resultado.AppendLine(pregunta);
                            resultado.AppendLine();
                            resultado.AppendLine(respuesta);
                        }

                        // Separar los chistes visualmente.
                        resultado.AppendLine();
                        resultado.AppendLine(
                            "----------------------------------------");
                        resultado.AppendLine();

                        // Aumentar el contador.
                        numero++;
                    }

                    // Mostrar todos los chistes en el RichTextBox.
                    rbtResultado.Text = resultado.ToString();
                }

                // =====================================================
                // UN SOLO CHISTE
                // =====================================================

                else
                {
                    // Obtener el tipo del chiste.
                    string? tipo =
                        raiz.GetProperty("type").GetString();

                    // Si el chiste tiene una sola parte.
                    if (tipo == "single")
                    {
                        string? chiste =
                            raiz.GetProperty("joke").GetString();

                        rbtResultado.Text = chiste;
                    }

                    // Si el chiste tiene pregunta y respuesta.
                    else if (tipo == "twopart")
                    {
                        string? pregunta =
                            raiz.GetProperty("setup").GetString();

                        string? respuesta =
                            raiz.GetProperty("delivery").GetString();

                        rbtResultado.Text =
                            pregunta + "\r\n\r\n" + respuesta;
                    }

                    // Si llega otro formato.
                    else
                    {
                        rbtResultado.Text =
                            "La API devolvió un formato de chiste no reconocido.";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Manejar errores de conexión.
                respuestaJson = "";
                rbtResultado.Clear();

                MessageBox.Show(
                    "No se pudo consultar la API.\n" + ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                // Manejar errores cuando la respuesta no contiene JSON válido.
                respuestaJson = "";
                rbtResultado.Clear();

                MessageBox.Show(
                    "La respuesta recibida no es un JSON válido.\n" + ex.Message,
                    "Error de formato",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro error inesperado.
                respuestaJson = "";
                rbtResultado.Clear();

                MessageBox.Show(
                    "Ocurrió un error al consultar la API.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // Reactivar el botón tanto si la consulta funciona como si falla.
                btnConsultar.Enabled = true;
            }
        }

        // =====================================================
        // BOTÓN GUARDAR EN TXT
        // =====================================================

        // Este método permite guardar la respuesta en un archivo de texto.
        private void btnTxt_Click(object sender, EventArgs e)
        {
            // Comprobar que ya se haya consultado la API.
            if (string.IsNullOrWhiteSpace(respuestaJson))
            {
                MessageBox.Show(
                    "Primero debes consultar la API.",
                    "Sin resultados",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                // Crear un cuadro de diálogo para elegir dónde guardar el archivo.
                using SaveFileDialog guardar = new SaveFileDialog();

                // Limitar los archivos mostrados a la extensión TXT.
                guardar.Filter = "Archivo de texto (*.txt)|*.txt";

                // Establecer el nombre inicial del archivo.
                guardar.FileName = "Resultado_API.txt";

                // Establecer el título de la ventana.
                guardar.Title = "Guardar resultado en TXT";

                // Establecer la extensión predeterminada.
                guardar.DefaultExt = "txt";

                // Mostrar la ventana y comprobar si el usuario acepta.
                if (guardar.ShowDialog() == DialogResult.OK)
                {
                    // Escribir los datos en el archivo utilizando UTF-8.
                    File.WriteAllText(
                        guardar.FileName,
                        respuestaJson,
                        Encoding.UTF8);

                    // Confirmar que el archivo se guardó.
                    MessageBox.Show(
                        "Archivo TXT guardado correctamente.",
                        "Guardado exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Mostrar cualquier error ocurrido al guardar.
                MessageBox.Show(
                    "Error al guardar el archivo TXT.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // BOTÓN GUARDAR EN JSON
        // =====================================================

        // Este método guarda la respuesta de la API en formato JSON.
        private void btnJson_Click(object sender, EventArgs e)
        {
            // Comprobar que existan datos disponibles.
            if (string.IsNullOrWhiteSpace(respuestaJson))
            {
                MessageBox.Show(
                    "Primero debes consultar la API.",
                    "Sin resultados",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                // Crear la ventana para seleccionar la ubicación del archivo.
                using SaveFileDialog guardar = new SaveFileDialog();

                // Permitir guardar archivos con extensión JSON.
                guardar.Filter = "Archivo JSON (*.json)|*.json";

                // Definir el nombre inicial.
                guardar.FileName = "Resultado_API.json";

                // Definir el título de la ventana.
                guardar.Title = "Guardar resultado en JSON";

                // Establecer la extensión predeterminada.
                guardar.DefaultExt = "json";

                // Comprobar si el usuario seleccionó una ubicación.
                if (guardar.ShowDialog() == DialogResult.OK)
                {
                    // Escribir el contenido JSON en el archivo.
                    File.WriteAllText(
                        guardar.FileName,
                        respuestaJson,
                        Encoding.UTF8);

                    // Informar que el archivo se guardó correctamente.
                    MessageBox.Show(
                        "Archivo JSON guardado correctamente.",
                        "Guardado exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje si ocurre un error al guardar.
                MessageBox.Show(
                    "Error al guardar el archivo JSON.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // BOTÓN GUARDAR EN CSV
        // =====================================================

        // Este método convierte los datos recibidos en filas y columnas.
        private void btnCsv_Click(object sender, EventArgs e)
        {
            // Comprobar que la API haya devuelto datos.
            if (string.IsNullOrWhiteSpace(respuestaJson))
            {
                MessageBox.Show(
                    "Primero debes consultar la API.",
                    "Sin resultados",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                // Leer la respuesta almacenada como documento JSON.
                using JsonDocument documento =
                    JsonDocument.Parse(respuestaJson);

                // Obtener el elemento principal de la respuesta.
                JsonElement raiz = documento.RootElement;

                // Crear una lista para almacenar los registros.
                List<JsonElement> resultados = new List<JsonElement>();

                // Comprobar si la respuesta contiene una propiedad llamada "jokes".
                if (raiz.ValueKind == JsonValueKind.Object &&
                    raiz.TryGetProperty(
                        "jokes",
                        out JsonElement chistes) &&
                    chistes.ValueKind == JsonValueKind.Array)
                {
                    // Recorrer cada chiste devuelto por la API.
                    foreach (JsonElement chiste in chistes.EnumerateArray())
                    {
                        // Agregar el chiste a la lista de resultados.
                        resultados.Add(chiste);
                    }
                }
                // Comprobar si la respuesta es directamente un arreglo JSON.
                else if (raiz.ValueKind == JsonValueKind.Array)
                {
                    // Recorrer todos los elementos del arreglo.
                    foreach (JsonElement elemento in raiz.EnumerateArray())
                    {
                        // Agregar cada elemento a la lista.
                        resultados.Add(elemento);
                    }
                }
                // Si no es un arreglo, tratar la respuesta como un solo registro.
                else
                {
                    resultados.Add(raiz);
                }

                // Crear una lista con los nombres de las columnas.
                List<string> columnas = new List<string>();

                // Recorrer los resultados para identificar sus propiedades.
                foreach (JsonElement resultado in resultados)
                {
                    // Las columnas se obtienen de las propiedades de los objetos.
                    if (resultado.ValueKind == JsonValueKind.Object)
                    {
                        foreach (JsonProperty propiedad
                                 in resultado.EnumerateObject())
                        {
                            // Agregar la columna solamente si no existe.
                            if (!columnas.Contains(propiedad.Name))
                            {
                                columnas.Add(propiedad.Name);
                            }
                        }
                    }
                }

                // Verificar que existan columnas para exportar.
                if (columnas.Count == 0)
                {
                    MessageBox.Show(
                        "No hay datos que se puedan exportar a CSV.",
                        "Sin datos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Crear un objeto para construir el contenido del CSV.
                StringBuilder csv = new StringBuilder();

                // Crear la lista de encabezados.
                List<string> encabezados = new List<string>();

                // Preparar cada nombre de columna para el formato CSV.
                foreach (string columna in columnas)
                {
                    encabezados.Add(EscaparCSV(columna));
                }

                // Escribir la primera fila, que contiene los encabezados.
                csv.AppendLine(string.Join(",", encabezados));

                // Recorrer los registros para crear las filas.
                foreach (JsonElement resultado in resultados)
                {
                    // Crear una lista para los valores de la fila actual.
                    List<string> valores = new List<string>();

                    // Recorrer todas las columnas disponibles.
                    foreach (string columna in columnas)
                    {
                        // Inicializar el valor vacío.
                        string valor = "";

                        // Buscar la propiedad correspondiente en el registro.
                        if (resultado.ValueKind == JsonValueKind.Object &&
                            resultado.TryGetProperty(
                                columna,
                                out JsonElement dato))
                        {
                            // Convertir los valores nulos en campos vacíos.
                            if (dato.ValueKind == JsonValueKind.Null)
                            {
                                valor = "";
                            }
                            // Conservar los objetos y arreglos como JSON.
                            else if (dato.ValueKind == JsonValueKind.Object ||
                                     dato.ValueKind == JsonValueKind.Array)
                            {
                                valor = dato.GetRawText();
                            }
                            // Convertir los demás tipos a texto.
                            else
                            {
                                valor = dato.ToString();
                            }
                        }

                        // Preparar el valor para que sea compatible con CSV.
                        valores.Add(EscaparCSV(valor));
                    }

                    // Escribir la fila completa separando los valores por comas.
                    csv.AppendLine(string.Join(",", valores));
                }

                // Crear la ventana para guardar el archivo CSV.
                using SaveFileDialog guardar = new SaveFileDialog();

                // Permitir guardar archivos con extensión CSV.
                guardar.Filter = "Archivo CSV (*.csv)|*.csv";

                // Establecer el nombre inicial del archivo.
                guardar.FileName = "Resultado_API.csv";

                // Establecer el título de la ventana.
                guardar.Title = "Guardar resultado en CSV";

                // Establecer la extensión predeterminada.
                guardar.DefaultExt = "csv";

                // Comprobar si el usuario confirma la ubicación.
                if (guardar.ShowDialog() == DialogResult.OK)
                {
                    // Guardar el contenido CSV con codificación UTF-8.
                    File.WriteAllText(
                        guardar.FileName,
                        csv.ToString(),
                        Encoding.UTF8);

                    // Informar que la exportación terminó.
                    MessageBox.Show(
                        "Archivo CSV guardado correctamente.",
                        "Guardado exitoso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (JsonException ex)
            {
                // Informar si la respuesta no puede procesarse como JSON.
                MessageBox.Show(
                    "No se pudieron procesar los datos JSON.\n" + ex.Message,
                    "Error de formato",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Mostrar cualquier otro error al generar el CSV.
                MessageBox.Show(
                    "Error al guardar el archivo CSV.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // MÉTODO AUXILIAR PARA FORMATEAR VALORES CSV
        // =====================================================

        // Evita que las comas y las comillas rompan las columnas del CSV.
        private string EscaparCSV(string valor)
        {
            // Duplicar las comillas internas y encerrar el campo entre comillas.
            return "\"" + valor.Replace("\"", "\"\"") + "\"";
        }

        private void rbtResultado_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
