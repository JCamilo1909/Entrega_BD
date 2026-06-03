using SistemaRegistros.Models;

namespace SistemaRegistros.Services
{
    public class PersonaGestor
    {
        private List<Compra> listaCompras = new List<Compra>();

        public List<Compra> ObtenerTodos()
        {
            return listaCompras;
        }

        public void Agregar(Compra compra)
        {
            compra.Id = listaCompras.Count + 1;
            listaCompras.Add(compra);
        }
    }
}