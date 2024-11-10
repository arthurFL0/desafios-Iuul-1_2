using ConsultorioOdontologico.Controladores;
using ConsultorioOdontologico.Model;
using System.Text.RegularExpressions;
using static System.Console;


namespace ConsultorioOdontologico
{
    internal class InterfaceConsole
    {
        ControladoraPaciente ControladoraP { get; }
        ControladoraConsulta ControladoraConsulta { get; }

        public InterfaceConsole(ControladoraPaciente cp, ControladoraConsulta cc)
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

                if (op == "1")
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
                else if (op == "2")
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
                else if (op == "3")
                {
                    break;
                }

            } while (true);
        }



        private void Cadastrar()
        {
            string cpf, nome, data;

            LerCPF(out cpf, false);

            nome = LerString("Nome: ");
            data = LerString("Data de nascimento: ");

            do
            {
                Dictionary<string, string> listaErros;
                if (ControladoraP.CadastrarPaciente(cpf, nome, data, out listaErros))
                {
                    WriteLine("Paciente cadastrado com sucesso!");
                    break;
                };

                foreach (var erro in listaErros)
                {
                    switch (erro.Key) {

                        case "Nome":
                            WriteLine(erro.Value);
                            nome = LerString("Nome: ");
                            break;
                        case "CPF":
                            WriteLine(erro.Value);
                            LerCPF(out cpf, false);
                            break;
                        case "DataNascimento":
                            WriteLine(erro.Value);
                            data = LerString("Data de nascimento: ");
                            break;
                    }
                }


            } while (true);

        }

        private void ExcluirPaciente()
        {

        }

        private void ListarPacientes(string ordenacao)
        {

        }

        
        private void Agendar()
        {
            string cpf, data, horaInicial, horaFinal;
         

            LerCPF(out cpf, true);
            data = LerString("Data da consulta: ");
            horaInicial = LerString("Hora Inicial: ");
            horaFinal = LerString("Hora Final: ");

            do
            {
                try
                {
                    Dictionary<string, List<string>> listaErros;
                    Paciente p = ControladoraP.PegarPaciente(cpf);
                    bool temErroHoraInicial = false;
                    if(ControladoraConsulta.CadastrarConsulta(p, data, horaInicial, horaFinal, out listaErros))
                    {
                        WriteLine("Agendamento realizado com sucesso!");
                        break;

                    }

                foreach (var erro in listaErros)
                    {
                        switch (erro.Key)
                        {

                            case "data":
                                foreach(var msg in erro.Value)
                                    WriteLine(msg);
                                data = LerString("Data da consulta: ");
                                break;
                            case "horaInicial":
                                temErroHoraInicial = true;
                                foreach (var msg in erro.Value)
                                    WriteLine(msg);
                                horaInicial = LerString("Hora Inicial: ");
                                break;
                            case "horaFinal":
                                foreach (var msg in erro.Value)
                                    WriteLine(msg);
                                horaFinal = LerString("Hora Final: ");
                                break;
                            case "Erro-futuro":
                                foreach (var msg in erro.Value)
                                    WriteLine(msg);
                                data = LerString("Data da consulta: ");
                                if(!temErroHoraInicial)
                                    horaInicial = LerString("Hora Inicial: ");
                            break;
                            }
                    }
                    

                } catch (Exception e)
                {
                    WriteLine(e.Message);
                    LerCPF(out cpf, true);
                    data = LerString("Data da consulta: ");
                    horaInicial = LerString("Hora Inicial: ");
                    horaFinal = LerString("Hora Final: ");
                }

            } while (true);



        }

       

        private void CancelarAgendamento()
        {

        }

        private void ListarAgenda()
        {
            string dataInicio, dataFim, opcao;
            IReadOnlyCollection<Consulta> listaConsultas;
            List<string> listaDatasPercorridas = new List<string>();

            opcao = LerString("Apresentar a agenda T-Toda ou P-Periodo: ");
            if(opcao == "P")
            {
                do
                {
                    dataInicio = LerString("Data inicial: ");
                    Match m = Regex.Match(dataInicio, @"^([0][1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/([0-9]{4})$");
                    if (m.Success)
                    {
                        break;
                    }

                    WriteLine("A data inicial precisa estar no formato dd/MM/yyyy");

                } while (true);

                do
                {
                    dataFim = LerString("Data final: ");
                    Match m = Regex.Match(dataFim, @"^([0][1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/([0-9]{4})$");
                    if (m.Success)
                    {
                        break;
                    }

                    WriteLine("A data final precisa estar no formato dd/MM/yyyy");

                } while (true);

                listaConsultas = ControladoraConsulta.PegarConsultasPorPeriodo(dataInicio, dataFim);

            }
            else
            {
                listaConsultas = ControladoraConsulta.PegarConsultas();
            }

            WriteLine("");
            WriteLine("");
            WriteLine("-------------------------------------------------------------");
            WriteLine("   Data    H.Ini H.Fim Tempo Nome                     Dt.Nasc. ");
            WriteLine("-------------------------------------------------------------");
            foreach (var consulta in listaConsultas)
            {
                string data = consulta.DataConsulta.ToShortDateString();

                if (listaDatasPercorridas.Contains(data))
                {
                    data = "".PadLeft(data.Length);
                }
                else
                {
                    listaDatasPercorridas.Add(data);
                }

                WriteLine($"{data} {consulta.HoraInicial.ToShortTimeString()} " +
                    $"{consulta.HoraFinal.ToShortTimeString()} {(consulta.HoraFinal - consulta.HoraInicial).ToString().Substring(0,5)} " +
                    $"{consulta.Paciente.Nome.PadRight(23)} {consulta.Paciente.DataNascimento.ToShortDateString()}");

            }
            WriteLine("-------------------------------------------------------------");
            WriteLine("");

        }

        private void LerCPF(out string cpf, bool cpfDeveExistir)
        {
            do
            {
                Write("CPF: ");
                cpf = ReadLine() ?? "";
                cpf = cpf.Trim();

                try
                {
                    ControladoraP.VerificaCPF(cpf, cpfDeveExistir);
                }
                catch (Exception ex)
                {

                    WriteLine(ex.Message);
                    continue;
                }

                break;

            } while (true);
        }

        private string LerString(string mensagemConsole)
        {
            Write(mensagemConsole);
            string str = ReadLine() ?? "";
            return str.Trim();
        }


    }



}
