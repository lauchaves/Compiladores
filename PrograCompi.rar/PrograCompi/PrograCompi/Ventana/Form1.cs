using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Text.RegularExpressions;



namespace PrograCompi
{
    public partial class Form1 : Form
    {
        Parser1 parser;
        string strfilename;
        Stream myStream;
        public static StringBuilder errores = new StringBuilder();



        public Form1()
        {
            InitializeComponent();
            
            richTextBox1.BackColor = Color.White;
            
        }



        public void openFile() { 
        
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if ((myStream = openFileDialog.OpenFile()) != null)
                {
                    strfilename = openFileDialog.FileName;
                    string filetext = File.ReadAllText(strfilename);


                    string fileName = Path.GetFileName(strfilename);

                    string title = fileName + (tabControl1.TabCount + 1).ToString();
                    TabPage myTabPage = new TabPage(strfilename);
                    tabControl1.TabPages.Add(myTabPage);


                    RichTextBox richTextBox = new RichTextBox();
                    
                    richTextBox.Dock = DockStyle.Fill;
                    richTextBox.AcceptsTab=true;
                    richTextBox.SelectionTabs = new int[] { 25, 50, 75, 100 };
                    myTabPage.Controls.Add(richTextBox);
                    
                    richTextBox.Text = filetext;
                    ColorReservadas();
                    ColorComentarios();



                    richTextBox.SelectionChanged += new EventHandler(RichTextBox_SelectionChanged);


                    myStream.Close();



                }
            }
        
        }


        private void RichTextBox_SelectionChanged(Object sender, EventArgs e)
        {

            int pos = ((RichTextBox) sender).SelectionStart;

            int line = ((RichTextBox)sender).GetLineFromCharIndex(pos)+1;

            int col = pos - ((RichTextBox)sender).GetFirstCharIndexOfCurrentLine()+1;

            filascols.Text = ("Linea: " + line + " Columna: " + col);

        }

        public void saveFile() {

            String nombreTab = "Nuevo_" + ((int)tabControl1.SelectedIndex+1);

            TabPage pageTab = tabControl1.SelectedTab;
            if (pageTab == null)
            {
                richTextBox1.Text = "Error";
                return;
            }
            else if (nombreTab == pageTab.Text)
            {
                RichTextBox richTextPage = (RichTextBox)pageTab.Controls[0];
                SaveFileDialog saveFile1 = new SaveFileDialog();
                
                saveFile1.Filter = "Text Files (.txt)|*.txt|All Files (.)|*.*";
                if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                   saveFile1.FileName.Length > 0)
                {
                    richTextPage.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                    pageTab.Text = saveFile1.FileName;
                }
            }
            else
            {
                RichTextBox richTextPage = (RichTextBox)pageTab.Controls[0];
                File.WriteAllText(pageTab.Text, richTextPage.Text.ToString()); 
            }
        }


        private void cerrarArchivo_Click(object sender, EventArgs e)
        {
            myStream.Close();
            //textArchivo.Clear();
        }


        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void guardarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFile();
        }


        public void compilar() 
        {
            richTextBox1.Text = "";
            TabPage pageTab = tabControl1.SelectedTab;
            if (pageTab == null)
            {
                richTextBox1.Text = "Error";
                return;
            }
            RichTextBox richTextPage = (RichTextBox)pageTab.Controls[0];

            ColorReservadas();
            
            AntlrInputStream input = new AntlrInputStream(richTextPage.Text.ToString());

            try
            {
                Lexer1 lexer = new Lexer1(input);
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                parser = new Parser1(tokens);
               


                //inicio de parsing

      

                //parser.program();

                //cambiar la estrategia de impresion de rrrores 
                // propuesta del profesor que puede variar

                parser.RemoveErrorListeners();
                parser.AddErrorListener(MyErrorListener.INSTANCE);

                 IParseTree tree = parser.program();

                AContextual analisis = new AContextual();
                analisis.Visit(tree);


                
                richTextBox1.Text = errores.ToString();
                if (errores.ToString().Equals("")) {
                    richTextBox1.Text = "Compilación Exitosa!!";
                    richTextBox1.ForeColor = Color.Blue;
                    saveFile();
                }
                saveFile();

                /* Buscar como capturar errores, para mostrarlos donde yo quiera :v  
                   Mostrar fila y columna en editor siempre 
                 
                 */
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Compilacion Fallida");
                //Console.Write(ex);
                Form1.errores.Append(ex.Message.ToString());
                richTextBox1.Text = errores.ToString();
                richTextBox1.ForeColor = Color.Red;
                saveFile();
            }
        }




        private void ColorReservadas()
        {
            TabPage pageTab = tabControl1.SelectedTab;
            if (pageTab == null)
            {
                return;
            }

            RichTextBox richTextPage = (RichTextBox)pageTab.Controls[0];

            string tokens = "(class|const|void|if|else|int|char|bool|true|false|foreach|for|while|break|return|new)";

            Regex rex = new Regex(tokens);

            MatchCollection mc = rex.Matches(richTextPage.Text);

            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;

                richTextPage.Select(startIndex, StopIndex);
                richTextPage.SelectionColor = Color.Blue;
            }
        }




        private void ColorComentarios()
        {
            TabPage pageTab = tabControl1.SelectedTab;
            if (pageTab == null)
            {
                return;
            }

            RichTextBox richTextPage = (RichTextBox)pageTab.Controls[0];
            //Recorre el texto y da formato
            for (int ci = 0; ci < richTextPage.Text.Length; ci++)
            {
                //Busca comentarios de una línea y les da formato
                if ((ci < richTextPage.Text.Length - 2) && richTextPage.Text.Substring(ci, 2) == "//")
                {
                    for (int i = ci; i < richTextPage.Text.Length; i++)
                    {
                        if (richTextPage.Text[i] == '\n')
                        {
                            break;
                        }

                        richTextPage.SelectionStart = i;
                        richTextPage.SelectionLength = 1;
                        richTextPage.SelectionColor = Color.Green;
                    }
                }

                //Busca comentarios de bloque y les da formato
                else if ((ci < richTextPage.Text.Length - 2) && richTextPage.Text.Substring(ci, 2) == "/*")
                {
                    int i;

                    int begin = 0;
                    int end = 0;

                    for (i = ci; i < richTextPage.Text.Length - 1; i++)
                    {
                        if (richTextPage.Text.Substring(i, 2) != "*/")
                        {
                            if (richTextPage.Text.Substring(i, 2) == "/*")
                            {
                                begin += 1;
                            }

                            richTextPage.SelectionStart = i;
                            richTextPage.SelectionLength = 1;
                            richTextPage.SelectionColor = Color.Green;
                        }

                        else
                        {
                            end += 1;

                            richTextPage.SelectionStart = i;
                            richTextPage.SelectionLength = 2;
                            richTextPage.SelectionColor = Color.Green;

                            if (end == begin)
                                break;
                        }
                    }

                    if (i == richTextPage.Text.Length - 1)
                    {
                        richTextPage.SelectionStart = i;
                        richTextPage.SelectionLength = 1;
                        richTextPage.SelectionColor = Color.Green;
                    }
                }

                //Da formato a caracteres con este formato 'n'
                else if ((ci < richTextPage.Text.Length - 3) && richTextPage.Text[ci] == '\'' && richTextPage.Text[ci + 2] == '\'')
                {
                    richTextPage.SelectionStart = ci;
                    richTextPage.SelectionLength = 3;

                    if (richTextPage.SelectionColor != Color.Green)
                    {
                        richTextPage.SelectionColor = Color.DarkRed;
                    }
                }

                //Da formato a caracteres con este formato '\n'
                else if ((ci < richTextPage.Text.Length - 4) && richTextPage.Text[ci] == '\'' && richTextPage.Text[ci + 3] == '\'')
                {
                    richTextPage.SelectionStart = ci;
                    richTextPage.SelectionLength = 4;

                    if (richTextPage.SelectionColor != Color.Green)
                    {
                        richTextPage.SelectionColor = Color.DarkRed;
                    }
                }
            }
        }


        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            errores.Clear();
            compilar();
        }

        private void closeTab()
        {
            if (tabControl1.SelectedTab == null)
                return;
            else
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);

        }

        

        private void btnCloseTab_Click(object sender, EventArgs e)
        {
            closeTab();
            richTextBox1.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void aSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errores.Clear();


            TabPage pageTab = tabControl1.SelectedTab;
            if (pageTab == null)
            {
                richTextBox1.Text = "Error";
                return;
            }
            RichTextBox richTextPage = (RichTextBox)pageTab.Controls[0];

            AntlrInputStream input = new AntlrInputStream(richTextPage.Text.ToString());

            Lexer1 lexer = new Lexer1(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            parser = new Parser1(tokens);

            IParseTree arbolito = parser.program();
            PrettyPrint prettyPrint = new PrettyPrint(null);
            prettyPrint.Visit(arbolito);
            richTextBox1.Text = errores.ToString();


      }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void arbolToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string title = "Nuevo_" + (tabControl1.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            tabControl1.TabPages.Add(myTabPage);

            RichTextBox myRichTextBox = new RichTextBox();
            myRichTextBox.Dock = DockStyle.Fill;
            myRichTextBox.AcceptsTab = true;
            myRichTextBox.SelectionChanged += new EventHandler(RichTextBox_SelectionChanged);
            myTabPage.Controls.Add(myRichTextBox);
        }

    }
}
