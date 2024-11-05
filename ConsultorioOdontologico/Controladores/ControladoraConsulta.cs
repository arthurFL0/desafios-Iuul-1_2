using ConsultorioOdontologico.Model;

namespace ConsultorioOdontologico.Controladores
{
    internal class ControladoraConsulta
    {
        private Persistencia persistencia;

        public ControladoraConsulta(Persistencia persistencia)
        {
            this.persistencia = persistencia;
        }

        public bool CadastrarConsulta(Consulta c)
        {
            if (persistencia.salvarConsulta(c))
                return true;

            return false;
        }
    }
}
