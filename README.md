# gerador
Aplicação .net console destinada a gerar código  com base e templates e tags &lt;#nomeDaTag#>  no formato texto.


<h1>Veja a lista das Tags A Seguir</h1>

<h2>Principais:</h2>

<p><#namespace#>: Namespace usada nos projetos por exemplo empresa.projeto.modulo.camada [SM.Gerador.Front.Console]</p>
<p><#domainSource#>:Qual o nome da camada de Domínio , por exemplo [SM.Gerador.Core.Domain] nesse caso é Core</p>
<p><#className#>: Nome da Classe por padrão vais ser um nome de Tabela essa tag poderá ser customizada por um metodo que pode ser sobrescrito assim podemo tirar sufixos e prefixos ou implementar qualquer regra que seja</p>
<p><#classNameLower#>: ClassName em minusculo</p>
<p><#KeyName#> : Nome da Chave de identificação da Entidade , obtida do banco</p>
<p><#KeyType#> : Tipo da Chave de identificação da Entidade , obtida do banco</p>
<p><#module#> : Nome do Modulo por exemplo [SM.Gerador.Access.API] , nesse caso é Access</p>
<p><#IDomain#> : Interface a ser Atribuida na classe de dominio</p>
<p><#KeyNames#> : Usando para quando a chave e composta</p>
<p><#tablename#> : Nome da Tabela sem tratamento</p>
<p><#contextName#> : Caso não configurado retorna o Modulo</p>
<p><#contextNameLower#> : contextName em minusculo</p>
<p><#WhereSingle#> : lambda linq com as chaves para obter um item</p>
<p><#orderByKeys#> : lambda linq com as chaves para ordenar</p>
<p><#company#> : Nome da empresa por exemplo [SM.Gerador.Access.API] , nesse caso é SM</p>

<h2>Obsoletas:</h2>

<p><#namespaceRoot#> :</p>
<p><#namespaceDomainSource#> :</p>
<p><#classNameFormated#> :</p>
<p><#inheritClassName#> :/p>
<p><#boundedContext#> :</p>
<p><#toolName#> :</p>
