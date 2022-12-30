using Newtonsoft.Json;
using System.IO;
namespace Registro{
    public class Dados{
        public bool MySQL {get;set;}
        public bool Firebase {get;set;}
        public string SenhaDB {get;set;}
        public string NomeDB {get;set;}
        public string AuthSecret {get;set;}
        public string BasePath {get;set;}
    }
    public class GetJson{
        public static Dados lerJson(){
            string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+@"registro/Registro.json");
            // Deserialize the JSON string into a Registro.Dados object
            Dados obj = JsonConvert.DeserializeObject<Dados>(json);
            return obj;
        }
    }
}