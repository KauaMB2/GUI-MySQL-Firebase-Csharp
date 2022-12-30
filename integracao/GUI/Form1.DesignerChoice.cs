using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using Registro;
using MySQL.Queries;
using Firebase;
namespace GUI{
    public partial class Forms{
        private Button buttonFirebase;//Define global object
        private Button buttonMySQL;//Define global object
        private Label labelFirebase;//Define global object
        private Label labelMySQL;//Define global object
        private Label labelAuthSecret;//Define global object
        private Label labelBasePath;//Define global object
        private Label labelPrincipal;//Define global object
        private Label labelSenha;//Define global object
        private Label labelNomeDB;//Define global object
        private TextBox inputAuthSecret;//Define global object
        private TextBox inputBasePath;//Define global object
        private TextBox inputNomeDB;//Define global object
        private TextBox inputSenha;//Define global object
        public void escolherMySQL(object sender, EventArgs e){
            Registro.Dados novoRegistro=new Registro.Dados{
                MySQL=false,
                Firebase=false,
                SenhaDB=inputSenha.Text,
                NomeDB=inputNomeDB.Text,
                AuthSecret="",
                BasePath=""
            };
            var dadosJson_Serializado=JsonConvert.SerializeObject(novoRegistro);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+@"\registro\Registro.json",dadosJson_Serializado);
            try{
                MySQLCommands.ConnectionDB();
                MySQLCommands.OpenConnectionDB();
                novoRegistro.MySQL=true;
            }catch{
                MessageBox.Show("Preencha todos as informações corretamente","ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);// Display error message in message box
                return;
            }
            dadosJson_Serializado=JsonConvert.SerializeObject(novoRegistro);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+@"\registro\Registro.json",dadosJson_Serializado);
            inputSenha.Text="";
            inputNomeDB.Text="";
            MessageBox.Show("Nome e senha corretos!\nReinicie a aplicação","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void escolherFirebase(object sender, EventArgs e){
            Registro.Dados novoRegistro=new Registro.Dados{
                MySQL=false,
                Firebase=false,
                SenhaDB="",
                NomeDB="",
                AuthSecret=inputAuthSecret.Text,
                BasePath=inputBasePath.Text
            };
            var dadosJson_Serializado=JsonConvert.SerializeObject(novoRegistro);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+@"\registro\Registro.json",dadosJson_Serializado);
            try{
                FirebaseCommands.Read("");
                novoRegistro.Firebase=true;
            }
            catch{
                MessageBox.Show("Preencha todos as informações corretamente","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dadosJson_Serializado=JsonConvert.SerializeObject(novoRegistro);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+@"\registro\Registro.json",dadosJson_Serializado);
            
            inputAuthSecret.Text="";
            inputBasePath.Text="";
            MessageBox.Show("AuthSecret e BasePath corretos!\nReinicie a aplicação","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void InitializeComponentChoice(){
            this.buttonFirebase=new Button();
            this.buttonMySQL=new Button();
            this.labelFirebase=new Label();
            this.labelMySQL=new Label();
            this.labelAuthSecret=new Label();
            this.labelBasePath=new Label();
            this.labelPrincipal=new Label();
            this.labelSenha=new Label();
            this.labelNomeDB=new Label();
            this.inputAuthSecret=new TextBox();
            this.inputBasePath=new TextBox();
            this.inputNomeDB=new TextBox();
            this.inputSenha=new TextBox();
            // labelPrincipal
            this.labelPrincipal.Location = new Point(10, 10);//Position
            this.labelPrincipal.Size = new Size(450, 50);//Size
            this.labelPrincipal.ForeColor = Color.Black;//text's color
            this.labelPrincipal.Font = new Font("Arial", 15, FontStyle.Bold);//label's font
            this.labelPrincipal.Text="Selecione um Banco de Dados e preencha as \ninformações abaixo";//labels's text
            this.labelPrincipal.BackColor=Color.LightGreen;//labels's background color
            // labelFirebase
            this.labelFirebase.Location = new Point(80,80);//Position
            this.labelFirebase.Size = new Size(140, 30);//Size
            this.labelFirebase.ForeColor = Color.Red;//text's color
            this.labelFirebase.Font = new Font("Arial", 20, FontStyle.Bold);//label's font
            this.labelFirebase.Text="Firebase";//labels's text
            this.labelFirebase.BackColor=Color.LightGreen;//labels's background color
            // labelMySQL
            this.labelMySQL.Location = new Point(360,80);//Position
            this.labelMySQL.Size = new Size(120, 30);//Size
            this.labelMySQL.ForeColor = Color.Red;//text's color
            this.labelMySQL.Font = new Font("Arial", 20, FontStyle.Bold);//label's font
            this.labelMySQL.Text="MySQL";//labels's text
            this.labelMySQL.BackColor=Color.LightGreen;//labels's background color
            // labelAuthSecret
            this.labelAuthSecret.Location = new Point(20,120);//Position
            this.labelAuthSecret.Size = new Size(90,20);//Size
            this.labelAuthSecret.ForeColor = Color.Black;//text's color
            this.labelAuthSecret.Font = new Font("Arial", 12, FontStyle.Regular);//label's font
            this.labelAuthSecret.Text="AuthSecret:";//labels's text
            this.labelAuthSecret.BackColor=Color.LightGreen;//labels's background color
            // labelBasePath
            this.labelBasePath.Location = new Point(20,160);//Position
            this.labelBasePath.Size = new Size(90,20);//Size
            this.labelBasePath.ForeColor = Color.Black;//text's color
            this.labelBasePath.Font = new Font("Arial", 12, FontStyle.Regular);//label's font
            this.labelBasePath.Text="BasePath:";//labels's text
            this.labelBasePath.BackColor=Color.LightGreen;//labels's background color
            // inputAuthSecret
            this.inputAuthSecret.Location = new Point(110, 120);//Position
            this.inputAuthSecret.Size = new Size(140, 20);//Size
            // inputBasePath
            this.inputBasePath.Location = new Point(100, 157);//Position
            this.inputBasePath.Size = new Size(140, 20);//Size
            // labelSenha
            this.labelSenha.Location = new Point(270,120);//Position
            this.labelSenha.Size = new Size(90,20);//Size
            this.labelSenha.ForeColor = Color.Black;//text's color
            this.labelSenha.Font = new Font("Arial", 12, FontStyle.Regular);//label's font
            this.labelSenha.Text="SenhaDB:";//labels's text
            this.labelSenha.BackColor=Color.LightGreen;//labels's background color
            // labelNomeDB
            this.labelNomeDB.Location = new Point(270,157);//Position
            this.labelNomeDB.Size = new Size(90,20);//Size
            this.labelNomeDB.ForeColor = Color.Black;//text's color
            this.labelNomeDB.Font = new Font("Arial", 12, FontStyle.Regular);//label's font
            this.labelNomeDB.Text="NomeDB:";//labels's text
            this.labelNomeDB.BackColor=Color.LightGreen;//labels's background color
            // inputSenha
            this.inputSenha.Location = new Point(350, 120);//Position
            this.inputSenha.Size = new Size(140, 20);//Size
            // inputNomeDB
            this.inputNomeDB.Location = new Point(350, 157);//Position
            this.inputNomeDB.Size = new Size(140, 20);//Size
            // buttonFirebase
            this.buttonFirebase.Location = new Point(20, 200);//Position
            this.buttonFirebase.Size = new Size(140, 23);//Size
            this.buttonFirebase.ForeColor=Color.White;
            this.buttonFirebase.TabIndex = 0;//Allow you select this component using the 'tab' in your keyboard
            this.buttonFirebase.Text = "Confirmar Firebase";//Button's text
            this.buttonFirebase.Click += new System.EventHandler(this.escolherFirebase);//Call the method if the button be clicked
            this.buttonFirebase.BackColor=Color.DarkRed;//Button's color
            // buttonMySQL
            this.buttonMySQL.Location=new Point(270, 200);//Position
            this.buttonMySQL.Size=new Size(140, 23);//Size
            this.buttonMySQL.ForeColor=Color.White;
            this.buttonMySQL.TabIndex=0;//Allow you select this component using the 'tab' in your keyboard
            this.buttonMySQL.Text="Confirmar MySQL";//Button's text
            this.buttonMySQL.Click += new System.EventHandler(this.escolherMySQL);//Call the method if the button be clicked
            this.buttonMySQL.BackColor=Color.DarkRed;//Button's color

            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle=FormBorderStyle.FixedSingle;//Desable the Resizable of the GUI
            this.MaximizeBox=false;// Prevent the GUI be maximized
            this.ClientSize=new Size(500, 300);
            this.BackColor=Color.LightGreen;
            this.Controls.Add(this.labelPrincipal);
            this.Controls.Add(this.labelFirebase);
            this.Controls.Add(this.labelMySQL);
            this.Controls.Add(this.inputAuthSecret);
            this.Controls.Add(this.inputBasePath);
            this.Controls.Add(this.inputNomeDB);
            this.Controls.Add(this.inputSenha);
            this.Controls.Add(this.labelAuthSecret);
            this.Controls.Add(this.labelBasePath);
            this.Controls.Add(this.labelNomeDB);
            this.Controls.Add(this.labelSenha);
            this.Controls.Add(this.buttonFirebase);
            this.Controls.Add(this.buttonMySQL);
            this.Text = "Form1";
        }
    }
}
