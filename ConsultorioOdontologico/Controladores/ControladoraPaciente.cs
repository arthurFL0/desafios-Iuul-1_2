using ConsultorioOdontologico.Model;

namespace ConsultorioOdontologico.Controladores
{
    internal class ControladoraPaciente
    {
        private Persistencia persistencia;

        public ControladoraPaciente(Persistencia p)
        {
            persistencia = p;
        }

        public bool CadastrarPaciente(Paciente p)
        {
            if (persistencia.SalvarPaciente(p))
                return true;

            return false;
        }

        public bool VerificaCPF(string cpf, bool deveExistir)
        {
            Paciente p = new Paciente(cpf, "", new DateTime(2000, 01, 01));
            return deveExistir ? persistencia.cpfExiste(p) : persistencia.cpfNaoExiste(p);
        }

        public Paciente PegarPaciente(string cpf)
        {
            return persistencia.pegarPaciente(cpf);
        }

    }
}
