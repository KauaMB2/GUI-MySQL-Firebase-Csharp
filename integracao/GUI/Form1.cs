using MySQL.Queries;
using Registro;
namespace GUI{
    public partial class Forms : Form{
        public Forms(){
            Registro.Dados obj=GetJson.lerJson();
            // Access the properties of the object like this:
            if((obj.MySQL==true)&&(obj.Firebase==false)){
                MySQLCommands.ConnectionDB();
            }
            if((obj.MySQL==true)&&(obj.Firebase==false)||(obj.MySQL==false)&&(obj.Firebase==true)){
                InitializeComponentMain();
            }else{
                InitializeComponentChoice();
            }
        }
    }
}
