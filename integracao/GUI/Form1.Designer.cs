using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;
using MySQL.Queries;
using Firebase;
using Newtonsoft.Json;
using Registro;

namespace GUI{ //Define o namespace da aplicação 
    public partial class Forms{ //Define the Forms as a subclass of Form
        private Button buttonInsert;//Define global object
        private Button buttonSearch;//Define global object
        private Button buttonDelete;//Define global object
        private Button buttonUpdate;//Define global object
        private Button buttonReset;//Define global object
        private Label labelSearch;//Define global object
        private Label labelValueTrackBar;//Define global object
        private Label labelName;//Define global object
        private Label labelAuthor;//Define global object
        private Label labelSubject;//Define global object
        private Label labelPrice;//Define global object
        private TextBox inputSearch;//Define global object
        private TextBox inputName;//Define global object
        private TextBox inputAuthor;//Define global object
        private TrackBar trackBarPrice;//Define global object
        private Panel panel1;//Define global object
        private ComboBox comboboxSubject;//Define global object
        private DataGridView dataGridView1;//Define global object

        private static MySqlDataReader reader;
        private static Registro.Dados obj=GetJson.lerJson();//Get the deserialized json data  
        private static List<Firebase.Livro> listLivroObject;

        private void buttonReset_Click(object sender, EventArgs e){
            Registro.Dados novoRegistro=new Registro.Dados{
                MySQL=false,
                Firebase=false,
                SenhaDB="",
                NomeDB="",
                AuthSecret="",
                BasePath=""
            };
            var dadosJson_Serializado=JsonConvert.SerializeObject(novoRegistro);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+@"\registro\Registro.json",dadosJson_Serializado);
            MessageBox.Show("Reinicie a aplicação","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void buttonSearch_Click(object sender, EventArgs e){//Event handler for the button click
            if((obj.MySQL==true)&&(obj.Firebase==false)){
                MySQLCommands.OpenConnectionDB();
                reader=MySQLCommands.Select(inputSearch.Text);
                construirTabela();
                MySQLCommands.CloseConnectionDB();
            }
            if((obj.MySQL==false)&&(obj.Firebase==true)){
                listLivroObject=FirebaseCommands.Read(inputSearch.Text);
                construirTabela();
            }
            inputSearch.Text="";
        }
        private void buttonInsert_Click(object sender, EventArgs e){
            string price=Convert.ToString(trackBarPrice.Value);
            if((inputName.Text=="")||(inputAuthor.Text=="")||(comboboxSubject.Text=="")){
                MessageBox.Show("Preencha todos as informações","ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);// Display error message in message box
                return;
            }
            if((obj.MySQL==true)&&(obj.Firebase==false)){
                MySQLCommands.OpenConnectionDB();
                MySQLCommands.Insert(inputName.Text,inputAuthor.Text,comboboxSubject.Text,price);
                reader=MySQLCommands.Select("");
                construirTabela();
                MySQLCommands.CloseConnectionDB();
            }
            if((obj.MySQL==false)&&(obj.Firebase==true)){
                listLivroObject=FirebaseCommands.Create(inputName.Text,inputAuthor.Text,comboboxSubject.Text,price);
                construirTabela();
            }
            inputName.Text="";
            inputAuthor.Text="";
            comboboxSubject.Text="";
            comboboxSubject.Text="";
        }
        private void buttonDelete_Click(object sender, EventArgs e){
            try{
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];//Select the row
                int id=Convert.ToInt32(selectedRow.Cells[0].Value.ToString());//Get the first column of the row(Id)
                if((obj.MySQL==true)&&(obj.Firebase==false)){
                    MySQLCommands.OpenConnectionDB();
                    MySQLCommands.Delete(id);
                    reader=MySQLCommands.Select("");
                    construirTabela();
                    MySQLCommands.CloseConnectionDB();
                }
                if((obj.MySQL==false)&&(obj.Firebase==true)){
                    listLivroObject=FirebaseCommands.Delete(id);
                    construirTabela();
                }
            }catch{
                // Show message error
                MessageBox.Show("Por favor, selecione uma linha corretamente","ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonUpdate_Click(object sender, EventArgs e){
            string price=Convert.ToString(trackBarPrice.Value);
            if((inputName.Text=="")||(inputAuthor.Text=="")||(comboboxSubject.Text=="")){
                MessageBox.Show("Preencha todos as informações","ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);// Display error message in message box
                return;
            }
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];// Get the selected line
            string id = selectedRow.Cells[0].Value.ToString();//Get the first column(Id) of this row
            if((obj.MySQL==true)&&(obj.Firebase==false)){
                MySQLCommands.OpenConnectionDB();
                MySQLCommands.Update(id,inputName.Text,inputAuthor.Text,comboboxSubject.Text,price);
                reader=MySQLCommands.Select("");
                construirTabela();
                MySQLCommands.CloseConnectionDB();
            }
            if((obj.MySQL==false)&&(obj.Firebase==true)){
                int idInt=Convert.ToInt32(id);
                listLivroObject=FirebaseCommands.Update(idInt,inputName.Text,inputAuthor.Text,comboboxSubject.Text,price);
                construirTabela();
            }
            inputName.Text="";
            inputAuthor.Text="";
            comboboxSubject.Text="";
        }
        private void construirTabela(){
            dataGridView1.Rows.Clear();//Clear the table
            if((obj.MySQL==true)&&(obj.Firebase==false)){
                while (reader.Read()){
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = reader.GetInt32(0) });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = reader.GetString(1) });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = reader.GetString(2) });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = reader.GetString(3) });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = reader.GetString(4) });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = reader.GetString(5) });
                    dataGridView1.Rows.Add(row);
                }
            }
            if((obj.MySQL==false)&&(obj.Firebase==true)){
                foreach(Livro livro in listLivroObject){
                    if(livro!=null){
                        DataGridViewRow row = new DataGridViewRow();
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = livro.Id });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = livro.Nome });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = livro.Autor });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = livro.Assunto });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = livro.Horario });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = livro.Preco });
                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }
        private void trackBar_ValueChanged(object sender, EventArgs e){
            labelValueTrackBar.Text=trackBarPrice.Value.ToString();
        }

        private void InitializeComponentMain(){
            this.trackBarPrice = new TrackBar();//Define component
            this.buttonInsert = new Button();//Define component
            this.buttonSearch = new Button();//Define component
            this.buttonDelete = new Button();//Define component
            this.buttonUpdate = new Button();//Define component
            this.buttonReset=new Button();//Define component
            this.labelValueTrackBar=new Label();//Define component
            this.labelSearch = new Label();//Define component
            this.labelName=new Label();//Define component
            this.labelAuthor=new Label();//Define component
            this.labelSubject=new Label();//Define component
            this.labelPrice=new Label();//Define component
            this.inputSearch = new TextBox();//Define component
            this.inputAuthor=new TextBox();//Define component
            this.inputName=new TextBox();//Define component
            this.panel1=new Panel();//Define component
            this.comboboxSubject=new ComboBox();//Define component
            this.dataGridView1 = new DataGridView();//Define component

            // panel1
            this.panel1.Size = new Size(600, 170);
            this.panel1.Location = new Point(0, 0);
            this.panel1.BackColor = Color.DarkBlue;

            // buttonSearch
            this.buttonSearch.Location = new Point(15, 75);//Position
            this.buttonSearch.Name = "button1";//Button's name(Unique Identifier)
            this.buttonSearch.Size = new Size(75, 23);//Size
            this.buttonSearch.TabIndex = 0;//Allow you select this component using the 'tab' in your keyboard
            this.buttonSearch.Text = "Pesquisar";//Button's text
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);//Call the method if the button be clicked
            this.buttonSearch.BackColor=Color.White;//Button's color
            // buttonInsert
            this.buttonInsert.Location = new Point(440, 40);//Position
            this.buttonInsert.Name = "button2";//Button's name(Unique Identifier)
            this.buttonInsert.Size = new Size(75, 23);//Size
            this.buttonInsert.TabIndex = 0;//Allow you select this component using the 'tab' in your keyboard
            this.buttonInsert.Text = "Inserir";//Button's text
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);//Call the method if the button be clicked
            this.buttonInsert.BackColor=Color.White;//Button's color
            // buttonDelete
            this.buttonDelete.Location = new Point(440, 80);//Position
            this.buttonDelete.Name = "button3";//Button's name(Unique Identifier)
            this.buttonDelete.Size = new Size(75, 23);//Size
            this.buttonDelete.TabIndex = 0;//Allow you select this component using the 'tab' in your keyboard
            this.buttonDelete.Text = "Deletar";//Button's text
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);//Call the method if the button be clicked
            this.buttonDelete.BackColor=Color.White;//Button's color
            // buttonUpdate
            this.buttonUpdate.Location = new Point(440, 120);//Position
            this.buttonUpdate.Name = "button4";//Button's name(Unique Identifier)
            this.buttonUpdate.Size = new Size(75, 23);//Size
            this.buttonUpdate.TabIndex = 0;//Allow you select this component using the 'tab' in your keyboard
            this.buttonUpdate.Text = "Atualizar";//Button's text
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);//Call the method if the button be clicked
            this.buttonUpdate.BackColor=Color.White;//Button's color
            // buttonReset
            this.buttonReset.Location = new Point(500, 540);//Position
            this.buttonReset.Name = "Reset";//Button's name(Unique Identifier)
            this.buttonReset.Size = new Size(75, 23);//Size
            this.buttonReset.TabIndex = 0;//Allow you select this component using the 'tab' in your keyboard
            this.buttonReset.Text = "Resetar";//Button's text
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);//Call the method if the button be clicked
            this.buttonReset.BackColor=Color.DarkRed;//Button's color
            // labelSearch
            this.labelSearch.Location = new Point(15, 15);//Position
            this.labelSearch.Name = "Search";//label's name
            this.labelSearch.ForeColor = Color.White;//text's color
            this.labelSearch.Font = new Font("Arial", 12, FontStyle.Bold);//label's font
            this.labelSearch.Text="Pesquisar";//labels's text
            this.labelSearch.BackColor=Color.DarkBlue;//labels's background color
            // labelName
            this.labelName.Location = new Point(165, 15);//Position
            this.labelName.Name = "Name";//label's name
            this.labelName.ForeColor = Color.White;//text's color
            this.labelName.Font = new Font("Arial", 12, FontStyle.Bold);//label's font
            this.labelName.Text="Nome";//labels's text
            this.labelName.BackColor=Color.DarkBlue;//labels's background color
            // labelAuthor
            this.labelAuthor.Location = new Point(315, 15);//Position
            this.labelAuthor.Name = "Author";//label's name
            this.labelAuthor.ForeColor = Color.White;//text's color
            this.labelAuthor.Font = new Font("Arial", 12, FontStyle.Bold);//label's font
            this.labelAuthor.Text="Autor";//labels's text
            this.labelAuthor.BackColor=Color.DarkBlue;//labels's background color
            // labelSubject
            this.labelSubject.Location = new Point(165, 80);//Position
            this.labelSubject.Name = "Subject";//label's name
            this.labelSubject.ForeColor = Color.White;//text's color
            this.labelSubject.Font = new Font("Arial", 12, FontStyle.Bold);//label's font
            this.labelSubject.Text="Assunto";//labels's text
            this.labelSubject.BackColor=Color.DarkBlue;//labels's background color
            // labelPrice
            this.labelPrice.Location = new Point(315, 80);//Position
            this.labelPrice.Name = "Price";//label's name
            this.labelPrice.ForeColor = Color.White;//text's color
            this.labelPrice.Font = new Font("Arial", 12, FontStyle.Bold);//label's font
            this.labelPrice.Text="Preço(R$)";//labels's text
            this.labelPrice.BackColor=Color.DarkBlue;//labels's background color
            // labelValueTrackBar
            this.labelValueTrackBar.Location = new Point(410, 110);//Position
            this.labelValueTrackBar.Name = "ValueTrackBar";//label's name
            this.labelValueTrackBar.ForeColor = Color.White;//text's color
            this.labelValueTrackBar.Font = new Font("Arial", 12, FontStyle.Bold);//label's font
            this.labelValueTrackBar.Text="0";//labels's text
            this.labelValueTrackBar.BackColor=Color.DarkBlue;//labels's background color

            // inputSearch
            this.inputSearch.Location = new Point(15, 45);//Position
            this.inputSearch.Size = new Size(100, 20);//Size
            // inputName
            this.inputName.Location = new Point(165, 45);//Position
            this.inputName.Size = new Size(100, 20);//Size
            // inputAuthor
            this.inputAuthor.Location = new Point(315, 45);//Position
            this.inputAuthor.Size = new Size(100, 20);//Size
            // comboboxSubject
            this.comboboxSubject.Location = new Point(165, 110);//Position
            this.comboboxSubject.Size = new Size(100, 20);//Size
            this.comboboxSubject.Items.Add("Ficção");
            this.comboboxSubject.Items.Add("Romance");
            this.comboboxSubject.Items.Add("Terror");
            this.comboboxSubject.Items.Add("Aventura");
            this.comboboxSubject.Items.Add("Comédia");
            this.comboboxSubject.Items.Add("Ensaio");
            this.comboboxSubject.Items.Add("Poesia");
            this.comboboxSubject.Items.Add("Drama");
            this.comboboxSubject.Items.Add("Biografia");
            this.comboboxSubject.Items.Add("Mitologia");
            comboboxSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            // trackBarPrice
            this.trackBarPrice.Location = new Point(315, 110);//Position
            this.trackBarPrice.Size = new Size(100, 20);//Size
            this.trackBarPrice.TabIndex = 1;//Allow you select this component using the 'tab' in your keyboard
            this.trackBarPrice.BackColor=Color.DarkBlue;//Trackbar's background color
            this.trackBarPrice.Minimum = 0;//min
            this.trackBarPrice.Maximum = 100;//max
            this.trackBarPrice.Orientation = Orientation.Horizontal;//Change the orientation of trackBar
            this.trackBarPrice.ValueChanged+=new System.EventHandler(this.trackBar_ValueChanged);//Add event
            this.trackBarPrice.Value=0;//Initial value

            dataGridView1.Location=new Point(25,230);//Position
            dataGridView1.Size=new Size(550,300);//Size
            dataGridView1.Columns.Add("ID", "ID");//Add a column
            dataGridView1.Columns.Add("Name", "Name");//Add a column
            dataGridView1.Columns.Add("Autor", "Autor");//Add a column
            dataGridView1.Columns.Add("Assunto", "Assunto");//Add a column
            dataGridView1.Columns.Add("Horario", "Horario");//Add a column
            dataGridView1.Columns.Add("Preço", "Preço");//Add a column
            dataGridView1.Columns[0].Width = 30;//Define column width
            dataGridView1.Columns[1].Width = 110;//Define column width
            dataGridView1.Columns[4].Width = 110;//Define column width
            dataGridView1.Columns[5].Width = 60;//Define column width
            dataGridView1.ReadOnly=true;//Desable the editable of the table
            if((obj.MySQL==true)&&(obj.Firebase==false)){
                MySQLCommands.OpenConnectionDB();
                reader=MySQLCommands.Select("");
                dataGridView1.Rows.Clear();
                construirTabela();
                MySQLCommands.CloseConnectionDB();
            }
            if((obj.MySQL==false)&&(obj.Firebase==true)){
                FirebaseCommands.contMaxId();
                listLivroObject=FirebaseCommands.Read("");
                construirTabela();
            }
            this.Name = "GUI1";//GUI's name
            this.Text = "Interface Gráfica Básica";//GUI's title
            this.FormBorderStyle = FormBorderStyle.FixedSingle;//Desable the Resizable of the GUI
            this.MaximizeBox=false;// Prevent the GUI be maximized
            this.ClientSize = new Size(600, 600);//(Width,Height)
            this.BackColor=Color.White;//Gui's background color
            this.Controls.Add(this.labelSearch);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonInsert);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.inputSearch);
            this.Controls.Add(this.inputName);
            this.Controls.Add(this.inputAuthor);
            this.Controls.Add(this.comboboxSubject);
            this.Controls.Add(this.trackBarPrice);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.labelSubject);
            this.Controls.Add(this.labelValueTrackBar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.PerformLayout();//Force GUI to update content
        }
    }
}