using ConsultorioOdontologico.Excecoes;
using ConsultorioOdontologico.Model;

namespace ConsultorioOdontologico
{
    internal class Persistencia
    {
        private List<Paciente> pacientes;
        private List<Consulta> consultas;
        
        public IReadOnlyCollection<Paciente> PacientesReadOnly
        {
            get { return pacientes.AsReadOnly(); }
        }

        public Persistencia()
        {
            pacientes = new List<Paciente>();
            consultas = new List<Consulta>();
        }

        public bool SalvarPaciente(Paciente p)
        {
            if (cpfNaoExiste(p)) { 
                pacientes.Add(p);
                return true;
                
            }

            return false;
        }

        public bool cpfNaoExiste(Paciente p) { 
            
            return !pacientes.Contains(p)? true : throw new PacienteJaExisteException();
        }

        public bool cpfExiste(Paciente p) {
            return pacientes.Contains(p) ? true : throw new PacienteNaoExisteException();

        }

        public bool salvarConsulta(Consulta c) {
            Paciente? pSalvo = pacientes.Find((p) => p.Equals(c.Paciente));

            if (pSalvo == null)
            {
                throw new PacienteNaoExisteException();
            }

            if(consultas.Contains(c))
            {
                throw new ConsultaSobrepostaException();
            }

            pSalvo.Consultas.Add(c);
            consultas.Add(c);

            return true;
        }

        public Paciente pegarPaciente(string cpf)
        {
            Paciente? pSalvo = pacientes.Find((p) => p.CPF == cpf);
            if (pSalvo == null)
                throw new PacienteNaoExisteException();

            return pSalvo;
        }
    }
}
