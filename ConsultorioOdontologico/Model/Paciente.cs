namespace ConsultorioOdontologico.Model
{
    internal class Paciente : IEquatable<Paciente>
    {
        public string CPF { get; private set; }
        public string Nome { get; private set; }

        public DateTime dataNascimento { get; private set; }

        public List<Consulta> Consultas { get; }

        public Paciente(string cpf, string nome, DateTime dataNascimento)
        {
            CPF = cpf;
            Nome = nome;
            this.dataNascimento = dataNascimento;

            Consultas = new List<Consulta>();
        }

        public void AdicionarConsulta(Consulta c)
        {
            Consultas.Add(c);
        }

        public bool Equals(Paciente? other)
        {
            return other is not null && other.CPF == CPF;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Paciente paciente)
            {
                return Equals(paciente);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return CPF.GetHashCode();
        }
    }
}
