using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public static class HelperFlow
    {

        public enum Eflows
        {

            AtualizarAplicacao = 1,
            AtualizarRepository = 2,
            GerarCodigo,
            GerarCodigoEspecifico,
            Sair

        }

        public static void Flow(string[] args, Func<IEnumerable<ExternalResource>> GetConfigExternarReources, HelperSysObjectsBase sysObject)
        {
            var executeFlow = FlowOptions(args, GetConfigExternarReources, sysObject);

            if (!executeFlow)
                PrinstScn.WriteLine("Fluxo Não implementado");

            Flow(args, GetConfigExternarReources, sysObject);
        }

        private static bool FlowOptions(string[] args, Func<IEnumerable<ExternalResource>> GetConfigExternarReources, HelperSysObjectsBase sysObject)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Escolha as opções;");
            Console.WriteLine("[{0} = >> Atualizar Resources na aplicacao]", (int)Eflows.AtualizarAplicacao);
            Console.WriteLine("[{0} = >> Atualizar Resources para pull request]", (int)Eflows.AtualizarRepository);
            Console.WriteLine("[{0} = >> Gerar Código ]", (int)Eflows.GerarCodigo);
            Console.WriteLine("[{0} = >> Gerar Código de um Contexto ou Classe Especifica]", (int)Eflows.GerarCodigoEspecifico);
            Console.WriteLine("[{0} = >> Sair", (int)Eflows.Sair);

            var flow = Console.ReadLine();

            if (flow == ((int)Eflows.AtualizarAplicacao).ToString())
            {
                PrinstScn.WriteLine("Atualizando os recursos externos da aplicacao");
                HelperExternalResources.Clone(GetConfigExternarReources());
                return true;
            }


            if (flow == ((int)Eflows.AtualizarRepository).ToString())
            {
                PrinstScn.WriteLine("Atualizando os recursos externos para fazer pull request");
                HelperExternalResources.Update(GetConfigExternarReources());
                return true;
            }

            if (flow == ((int)Eflows.GerarCodigo).ToString())
            {
                PrinstScn.WriteLine("Gerar direto na pasta dos projetos? [S=Sim, N=Não]");
                var usePathProjects = Console.ReadLine();

                MainWithOutConfirmation(args, usePathProjects.ToLower() == "s", sysObject);

                return true;
            }

            if (flow == ((int)Eflows.GerarCodigoEspecifico).ToString())
            {

                PrinstScn.WriteLine("Gerar direto na pasta dos projetos? [S=Sim, N=Não]");
                var usePathProjects = Console.ReadLine();

                MainWithConfirmation(args, usePathProjects.ToLower() == "s", sysObject);

                return true;
            }

            if (flow == ((int)Eflows.Sair).ToString())
            {
                return true;
            }

            return false;
        }

        private static void MainWithConfirmation(string[] args, bool UsePathProjects, HelperSysObjectsBase sysObject)
        {
            PrinstScn.WriteLine("Atualizando / Criando Contextos ou classes especificas!");

            foreach (var item in sysObject.Contexts)
            {
                PrinstScn.WriteLine("Deseja Atualizar/Criar o Contexto? {0} [S=Sim, N=Não]", item.Namespace);
                var accept = Console.ReadLine();
                if (accept == "s")
                {
                    PrinstScn.WriteLine("Deseja Escolher uma classe, Digite o nome dela?");
                    var className = Console.ReadLine();
                    if (!string.IsNullOrEmpty(className))
                        sysObject.MakeClass(item, className, UsePathProjects);
                    else
                        sysObject.MakeClass(item);
                }

            }

        }

        private static void MainWithOutConfirmation(string[] args, bool UsePathProjects, HelperSysObjectsBase sysObject)
        {
            PrinstScn.WriteLine("Atualizando / Criando todos os Contextos");

            foreach (var item in sysObject.Contexts)
            {
                PrinstScn.WriteLine(item.Namespace);
                sysObject.MakeClass(item, UsePathProjects);
            }
        }



    }
}
