# gerador
Aplicação .net console destinada a gerar código  com base e templates e tags &lt;#nomeDaTag#>  no formato texto.


<h1>Veja a lista das Tags A Seguir</h1>

<h2>Principais:</h2>

<p><b><#namespace#></b>: Namespace usada nos projetos por exemplo empresa.projeto.modulo.camada [SM.Gerador.Front.Console]</p>
<p><b><#domainSource#></b>:Qual o nome da camada de Domínio , por exemplo [SM.Gerador.Core.Domain] nesse caso é Core</p>
<p><b><#className#></b>: Nome da Classe por padrão vais ser um nome de Tabela essa tag poderá ser customizada por um metodo que pode ser sobrescrito assim podemo tirar sufixos e prefixos ou implementar qualquer regra que seja</p>
<p><b><#classNameLower#></b>: ClassName em minusculo</p>
<p><b><#KeyName#></b> : Nome da Chave de identificação da Entidade , obtida do banco</p>
<p><b><#KeyType#></b> : Tipo da Chave de identificação da Entidade , obtida do banco</p>
<p><b><#module#></b> : Nome do Modulo por exemplo [SM.Gerador.Access.API] , nesse caso é Access</p>
<p><b><#IDomain#></b> : Interface a ser Atribuida na classe de dominio</p>
<p><b><#KeyNames#></b> : Usando para quando a chave e composta</p>
<p><b><#tablename#></b> : Nome da Tabela sem tratamento</p>
<p><b><#contextName#></b> : Caso não configurado retorna o Modulo</p>
<p><b><#contextNameLower#></b>: contextName em minusculo</p>
<p><b><#WhereSingle#></b> : lambda linq com as chaves para obter um item</p>
<p><b><#orderByKeys#></b> : lambda linq com as chaves para ordenar</p>
<p><b><#company#></b> : Nome da empresa por exemplo [SM.Gerador.Access.API] , nesse caso é SM</p>

<h2>Obsoletas:</h2>

<p><b><#namespaceRoot#></b> :</p>
<p><b><#namespaceDomainSource#></b> :</p>
<p><b><#classNameFormated#></b> :</p>
<p><b><#inheritClassName#></b> :/p>
<p><b><#boundedContext#></b> :</p>
<p><b><#toolName#></b> :</p>
