using MySql.Data.MySqlClient;
using Registro;
namespace MySQL.Queries{ //Define o namespace da aplicação 
    public partial class MySQLCommands{ //Define the Form1 as a subclass of Form
        private static MySqlConnection connection;
        public static void ConnectionDB(){
            Registro.Dados obj=GetJson.lerJson();
            string connectionString=$"server=localhost;database={obj.NomeDB};user id=root;password={obj.SenhaDB}";
            connection = new MySqlConnection(connectionString);
        }
        public static void Update(string id,string nomeLivro,string nomeAutor,string assunto,string preco){
            int dia = DateTime.Now.Day;
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            string hora = DateTime.Now.ToString("HH:mm");
            string horario=dia+"/"+mes+"/"+ano+" - "+hora;
            MySqlCommand command=new MySqlCommand("UPDATE livros SET Nome='"+nomeLivro+"',Autor='"+nomeAutor+"',Assunto='"+assunto+"',Horario='"+horario+"',Preço='"+preco+"' where Id='"+id+"';", connection);
            command.ExecuteNonQuery();
        }
        public static void Delete(int Id){
            MySqlCommand command=new MySqlCommand("DELETE FROM livros WHERE Id = "+Id, connection);
            command.ExecuteNonQuery();
        }
        public static void Insert(string nomeLivro,string nomeAutor,string assunto,string preco){
            MySqlCommand command;
            double novoPreco=Convert.ToDouble(preco);
            preco=string.Format("{0:C}", novoPreco);
            int idMax;
            try{
                command=new MySqlCommand("SELECT MAX(Id) FROM livros;",connection);
                idMax=(int)command.ExecuteScalar();
            }
            catch{
                idMax=0;
            }
            connection.Close();
            connection.Open();
            int dia = DateTime.Now.Day;
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            string hora = DateTime.Now.ToString("HH:mm");
            string horario=dia+"/"+mes+"/"+ano+" - "+hora;
            command=new MySqlCommand("INSERT INTO livros (Id,Nome, Autor, Horario, Assunto, Preço)VALUES ('"+(idMax+1)+"','"+nomeLivro+"','"+nomeAutor+"','"+horario+"','"+assunto+"','"+preco+"');", connection);
            command.ExecuteNonQuery();
        }
        public static MySqlDataReader Select(string nomeLivro){
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM livros", connection);
            long RowNumbers=(long)command.ExecuteScalar();
            if(nomeLivro==""){
                command=new MySqlCommand("select * from livros;",connection);
            }else{
                command=new MySqlCommand("select * from livros where Nome LIKE '%"+nomeLivro+"%';",connection);
            }
            MySqlDataReader reader=command.ExecuteReader();
            return reader;
        }
        public static void OpenConnectionDB(){
            connection.Open();
        }
        public static void CloseConnectionDB(){
            connection.Close();
        }
    }
}