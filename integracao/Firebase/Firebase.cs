using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp;
using Registro;

namespace Firebase{
    public class FirebaseCommands{
        private static Registro.Dados obj=GetJson.lerJson();
        private static IFirebaseConfig config = new FirebaseConfig(){
            AuthSecret=obj.AuthSecret,
            BasePath=obj.BasePath
        };
        private static IFirebaseClient client = new FirebaseClient(config);
        private static int maxId=0;

        public static void contMaxId(){
            List<Livro> listLivroObject=Read("");
            foreach(Livro livro in listLivroObject){
                if(livro!=null){
                    maxId=livro.Id;
                }
            }
        }

        public static string formarHorario(){
            int dia = DateTime.Now.Day;
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            string hora = DateTime.Now.ToString("HH:mm");
            string Horario=dia+"/"+mes+"/"+ano+" - "+hora;
            return Horario;
        }

        public static List<Livro> Create(string Nome,string Autor,string Assunto,string Preco){
            maxId++;
            string Horario=formarHorario();
            double novoPreco=Convert.ToDouble(Preco);
            Preco=string.Format("{0:C}",novoPreco);
            Livro novoLivro = new Livro{
                Id=maxId,
                Nome=Nome,
                Autor=Autor,
                Assunto=Assunto,
                Horario=Horario,
                Preco=Preco
            };
            client.Set($"Livros/{maxId}", novoLivro);//Create the User/{targetId} path and add the newUser object in this path 
            return Read("");
        }

        public static List<Livro> Delete(int targetId){
            client.Delete($"Livros/{targetId}");//Delete the path targetId
            return Read("");
        }

        public static List<Livro> Read(string Nome){
            FirebaseResponse livroData;
            List<Livro> listLivroObject;
            List<Livro> finalList=new List<Livro>();
            livroData=client.Get("Livros/");
            listLivroObject=livroData.ResultAs<List<Livro>>();
            if(Nome==""){
                return listLivroObject;
            }else{
                foreach(Livro livro in listLivroObject){
                    if(livro!=null){
                        if((livro.Nome).IndexOf(Nome)!=-1){
                            finalList.Add(livro);
                        }
                    }
                }
                return finalList;
            }
        }

        public static List<Livro> Update(int targetId,string Nome,string Autor,string Assunto,string Preco){
            string Horario=formarHorario();
            double novoPreco=Convert.ToDouble(Preco);
            Preco=string.Format("{0:C}", novoPreco);
            Livro novoLivro = new Livro{//Create de object
                Id=targetId,
                Nome=Nome,
                Autor=Autor,
                Assunto=Assunto,
                Horario=Horario,
                Preco=Preco
            };
            client.Update($"Livros/{targetId}", novoLivro);//Update the User/{targetId} path with the updatedUser data 
            return Read("");
        }
    }
    public class Livro{//It creates the object that will be sended to DB
        public int Id;
        public string Nome;
        public string Autor;
        public string Assunto;
        public string Horario;
        public string Preco;
    }
}
