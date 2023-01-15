using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.AudioFormat;
using System.Globalization;

namespace Transcriptor
{
    public partial class Form1 : Form
    {
        CultureInfo ci = new CultureInfo("en-US");

        OpenFileDialog openFileDialog = new OpenFileDialog();
        Grammar dictation = new DictationGrammar();
        static bool completed = false;
        static string reconocido;
        SpeechRecognitionEngine recognizer;

        string path;

        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            recognizer = new SpeechRecognitionEngine(ci);
            dictation.Name = "diccionario";
            recognizer.LoadGrammar(dictation);
            recognizer.SpeechRecognized +=
            new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            recognizer.RecognizeCompleted +=
              new EventHandler<RecognizeCompletedEventArgs>(recognizer_RecognizeCompleted);



        }

        private void buttonCargar_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                textBox1.Text = path;
            }
        }

        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null && e.Result.Text != null)
            {

                reconocido = e.Result.Text;
            }
            else
            {
                MessageBox.Show("No se pudo reconocer nada");
            }
        }

        static void recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error en el reconocimiento");
            }
            if (e.Cancelled)
            {
                MessageBox.Show("Operacion cancelada");
            }
            if (e.InputStreamEnded)
            {
                MessageBox.Show("Se llego al final del audio");
            }
            completed = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            recognizer.SetInputToWaveFile(path);
            recognizer.Recognize();
            listBox1.Items.Add(reconocido);

        }
    }
}
