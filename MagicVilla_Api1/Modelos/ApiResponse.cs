using System.Net;

namespace MagicVilla_Api1.Modelos
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsExitoso { get; set; } = true;

        public List<string> ErrorMessages { get; set; }    


        public object Resultado { get; set; }


    }
}
 