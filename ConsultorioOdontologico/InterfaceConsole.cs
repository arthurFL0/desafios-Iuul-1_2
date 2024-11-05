using System.Globalization;
using System.Text.RegularExpressions;
using ConsultorioOdontologico.Controladores;
using ConsultorioOdontologico.Extensoes;
using ConsultorioOdontologico.Model;
using static System.Console;


namespace ConsultorioOdontologico
{
    internal class InterfaceConsole
    {
         ControladoraPaciente ControladoraP {  get; }
         ControladoraConsulta ControladoraConsulta { get; }

        public InterfaceConsole (ControladoraPaciente cp,ControladoraConsulta cc)
        {
            ControladoraP = cp;
            ControladoraConsulta = cc;
        }


        public void Iniciar()
        {
            do
            {
                WriteLine("Menu Principal");
                WriteLine("1-Cadastro de Pacientes\n2-Agenda\n3-Fim");
                var op = ReadLine();

                if(op == "1")
                {
                    do
                    {
                        WriteLine("Menu do Cadastro de Pacientes");
                        WriteLine("1-Cadastro de novo Paciente\n2-Excluir Paciente" +
                            "\n3-Listar Pacientes (Ordenado por CPF)\n4-Listar Pacientes (Ordenado por Nome)\n5-Voltar ao menu Principal");

                        var op2 = ReadLine();

                        switch (op2)
                        {
                            case "1": Cadastrar();
                                    break;
                            case "2": ExcluirPaciente();
                                    break;
                            case "3": ListarPacientes("cpf");
                                    break;
                            case "4": ListarPacientes("nome");
                                    break;
                            case "5": break;
                            default: 
                                    WriteLine("Insira uma das opções listadas");
                                    break;
                        }

                        if (op2 == "5")
                            break;

                    } while (true);
                }
                else if(op == "2")
                {
                    do
                    {
                        WriteLine("Agendar");
                        WriteLine("1-Agendar Consulta\n2-Cancelar Agendamento" +
                            "\n3-Listar Agenda \n4-Voltar ao menu Principal");

                        var op3 = ReadLine();

                        switch (op3)
                        {
                            case "1":
                                Agendar();
                                break;
                            case "2":
                                CancelarAgendamento();
                                break;
                            case "3":
                                ListarAgenda();
                                break;
                            case "4":
                                break;
                            default:
                                WriteLine("Insira uma das opções listadas");
                                break;
                        }

                        if (op3 == "4")
                            break;

                    } while (true);       
                }
                else if(op == "3")
                {
                    break;
                }

            } while (true);
        }



        private void Cadastrar()
        {
            string cpf,nome;
            DateTime dataNascimento;

            LerCPF(out cpf,false);

            do
            {
                Write("Nome: ");
                nome = ReadLine() ?? "";
                nome.Trim();

                if (nome.Length >= 5)
                    break;

                WriteLine("Erro: Nome precisa ter pelo menos 5 caracteres.");
            } while (true);

            do
            {
                Write("Data de Nascimento: ");
                string data = ReadLine() ?? "";
                data.Trim();
                DateTime trezeAnos = DateTime.Now.AddYears(-13);

                // A Barra invertida \ que é usada para escapar / no Regex precisa ser escapada em C# com ela mesma \\
                // entao \/ = \\/ em C#

                // Ao usar @ antes da string o compilador entende que é para ignorar isso e o escape torna-se desnecessário

                Match m = Regex.Match(data, @"^([0][1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/([0-9]{4})$");

                if (!m.Success)
                {
                    WriteLine("Erro: Data não corresponde ao formato DD/MM/YYYY");
                    continue;
                }

                DateTime.TryParse(data, out dataNascimento);


                if (dataNascimento > trezeAnos)
                {
                    WriteLine("Erro: Paciente precisa ter mais de treze anos");
                    continue;
                }

                break;

       
            } while (true);

            try
            {
                Paciente p = new Paciente(cpf, nome, dataNascimento);
                ControladoraP.CadastrarPaciente(p);
            }
            catch (Exception ex)
            {

                WriteLine(ex.Message);
                Cadastrar();
            }

            WriteLine("Paciente cadastrado com sucesso!");


        }

        private void ExcluirPaciente()
        {

        }

        private void ListarPacientes(string ordenacao)
        {

        }

        private void Agendar()
        {
            string cpf;
            DateTime dataConsulta, horaInicial, horaFinal;
            DateTime aberturaConsultorio = InformacaoConsultorio.HoraAbertura;
            DateTime encerramentoConsultorio = InformacaoConsultorio.HoraEncerramento;

            LerCPF(out cpf,true);

            do
            {
                Write("Data da consulta: ");
                string data = ReadLine() ?? "";
                data.Trim();

                Match m = Regex.Match(data, @"^([0][1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/([0-9]{4})$");

                if (!m.Success)
                {
                    WriteLine("Erro: Data não corresponde ao formato DD/MM/YYYY");
                    continue;
                }

                DateTime.TryParse(data, out dataConsulta);

                break;

            } while (true);

            do
            {
                Write("Hora inicial: ");
                string horaI = ReadLine() ?? "";
                horaI.Trim();

                Match m = Regex.Match(horaI, @"^(([0-1]?[0-9])|2[0-3])(00|15|30|45)$");

                if (!m.Success)
                {
                    WriteLine("Erro: Hora não corresponde ao formato HHmm ou não respeita o intervalo de 15 minutos");
                    continue;
                }

                horaInicial = DateTime.ParseExact(horaI, "HHmm", CultureInfo.InvariantCulture);


                if(horaInicial < aberturaConsultorio || horaInicial > encerramentoConsultorio.AddMinutes(-15))
                {
                    WriteLine("Erro: Hora inicial fora do horário de atendimento");
                    continue;
                }

                break;
            } while (true);

            do
            {
                Write("Hora final: ");
                string horaF = ReadLine() ?? "";
                horaF.Trim();

                Match m = Regex.Match(horaF, @"^(([0-1]?[0-9])|2[0-3])(00|15|30|45)$");

                if (!m.Success)
                {
                    WriteLine("Erro: Hora não corresponde ao formato HHmm ou não respeita o intervalo de 15 minutos");
                    continue;
                }

                horaFinal = DateTime.ParseExact(horaF, "HHmm", CultureInfo.InvariantCulture);

                if(horaFinal < horaInicial)
                {
                    WriteLine("Erro: Hora Final não pode ser menor do que a hora inicial");
                    continue;
                }

                if (horaFinal > encerramentoConsultorio)
                {
                    WriteLine("Erro: Hora final fora do horário de atendimento");
                    continue;
                }




                break;
            } while (true);

            dataConsulta = dataConsulta.AddHours(horaInicial.Hour).AddMinutes(horaInicial.Minute);

            if(dataConsulta < DateTime.Now)
            {
                WriteLine("A data da consulta precisa estar no futuro.");
                Agendar();
            }

            try
            {
                Paciente p = ControladoraP.PegarPaciente(cpf);
                ControladoraConsulta.CadastrarConsulta(new Consulta(p, dataConsulta, horaInicial, horaFinal));
            }catch(Exception e)
            {
                WriteLine(e.Message);
                Agendar();
            }

        }

        private void CancelarAgendamento()
        {

        }

        private void ListarAgenda()
        {

        }

        private void LerCPF(out string cpf, bool cpfDeveExistir)
        {
            do
            {
                Write("CPF: ");
                cpf = ReadLine() ?? "";
                cpf.Trim();

                if (!cpf.ehValido())
                {
                    WriteLine("Erro: CPF Inválido.");
                    continue;
                }

                try
                {
                    ControladoraP.VerificaCPF(cpf,cpfDeveExistir);
                }
                catch (Exception ex)
                {

                    WriteLine(ex.Message);
                    continue;
                }

                break;

            } while (true);
        }


    }


}
