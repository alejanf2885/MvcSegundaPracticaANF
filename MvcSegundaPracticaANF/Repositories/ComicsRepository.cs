using Microsoft.Data.SqlClient;
using MvcSegundaPracticaANF.Models;
using System.Data;

namespace MvcSegundaPracticaANF.Repositories
{
    public class ComicsRepository
    {
        DataTable tablaComics = new DataTable();
        SqlConnection cn;
        SqlCommand com;

        public ComicsRepository()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=Comics;Persist Security Info=True;User ID=SA;Trust Server Certificate=True";
            string sql = "select * from COMICS";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaComics = new DataTable();

            adapter.Fill(this.tablaComics);

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;


            List<Comic> comics = new List<Comic>();

            foreach(var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }


        public Comic GetComic(int idComic)
        {

            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idComic
                           select datos;


            Comic comic = new Comic();

            foreach (var row in consulta)
            {
                comic.IdComic = row.Field<int>("IDCOMIC");
                comic.Nombre = row.Field<string>("NOMBRE");
                comic.Imagen = row.Field<string>("IMAGEN");
                comic.Descripcion = row.Field<string>("DESCRIPCION");
            }

            return comic;
        }

        public async Task CreateComic(string nombre, string imagen, string descripcion)
        {

            string sql = "insert into COMICS values( @idcomic, @nombre,@imagen, @descripcion) ";


            int idcomic = GetMaxId() + 1;

            this.com.Parameters.AddWithValue("@idcomic", idcomic);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();

            await this.com.ExecuteNonQueryAsync();

            await this.cn.CloseAsync();
            this.com.Parameters.Clear();




        }

        public int GetMaxId()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;

            int maxid  =  consulta.Max(x => x.Field<int>("IDCOMIC"));

            return maxid;

        }
    }
}
