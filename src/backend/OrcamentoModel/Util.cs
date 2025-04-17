using MongoDB.Bson;

namespace Orcamento.Models
{
    public class Util
    {
        //TODO: pensar numa forma de pegar do appsettings
        public static string GuidSemCategoria { get { return "617a073c70752ef5b7e108df"; } }

        public static string NewObjectId()
        {
            return new ObjectId(System.Guid.NewGuid().ToString().Replace("-", "")).ToString();
        }

        public static string NewObjectId(string value)
        {
            return new ObjectId(value.Replace("-", "")).ToString();
        }
    }
}
