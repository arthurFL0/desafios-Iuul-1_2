namespace ConsultorioOdontologico.Model
{
    internal class Consulta : IEquatable<Consulta>
    {
        public Paciente Paciente { get; }

        public DateTime DataConsulta { get; }

        public DateTime HoraInicial { get; }

        public DateTime HoraFinal { get; }


        public Consulta(Paciente p, DateTime d, DateTime hInicial, DateTime hFinal)
        {
            Paciente = p;
            DataConsulta = d;
            HoraInicial = hInicial;
            HoraFinal = hFinal;
        }

        public bool Equals(Consulta? other)
        {
            return other is not null && DataConsulta.Date == other.DataConsulta.Date
             && other.HoraInicial < HoraFinal && other.HoraFinal > HoraInicial;
            // Impedir datas sobrepostas (interseção ou iguais)
        }

        public override bool Equals(object? obj)
        {
            if (obj is Consulta consulta)
            {
                return Equals(consulta);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return DataConsulta.GetHashCode() ^ HoraInicial.GetHashCode() ^ HoraFinal.GetHashCode();
        }
    }
}
