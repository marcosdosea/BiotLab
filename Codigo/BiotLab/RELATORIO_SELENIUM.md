# Relatório de testes Selenium — BiotLab

**Data da execução:** 17/07/2026  
**Aplicação:** BiotLab Web  
**Navegador:** Google Chrome em modo headless  
**Framework:** Selenium WebDriver 4.46.0  
**Ambiente:** ASP.NET Core .NET 8 e MySQL local

## Resumo executivo

Foram executados três testes funcionais de interface. Todos os cenários foram
aprovados, incluindo a autenticação administrativa e o acesso ao dashboard.

| Cenário | Resultado | Evidência validada |
| --- | --- | --- |
| Carregamento da página de login | Aprovado | Formulário, campo de e-mail, campo de senha e botão de entrada visíveis |
| Proteção de módulo para usuário anônimo | Aprovado | Acesso a `/Instituicao` redirecionado para o login com `ReturnUrl` |
| Login administrativo | Aprovado | Autenticação aceita, saída da página de login e dashboard “BiotLab” exibido |

## Resultados das execuções

### Execução com evidências visuais

- Total: 3
- Aprovados: 3
- Ignorados: 0
- Falhas: 0
- Duração: aproximadamente 13 segundos
- Resultado estruturado: [`selenium-results.trx`](BiotLabTests/TestResults/selenium-results.trx)

## Evidências visuais

### Página de login

![Página de login](TestResults/SeleniumScreenshots/01-pagina-login.png)

### Redirecionamento de acesso anônimo

![Redirecionamento para login](TestResults/SeleniumScreenshots/02-redirecionamento-login.png)

### Dashboard após login administrativo

![Dashboard administrativo](TestResults/SeleniumScreenshots/03-dashboard-administrador.png)

### Execução pública

- Total: 3
- Aprovados: 2
- Ignorados: 1
- Falhas: 0
- Motivo do teste ignorado: credenciais ainda não haviam sido fornecidas por
  variáveis de ambiente.

### Execução autenticada

- Total: 1
- Aprovados: 1
- Ignorados: 0
- Falhas: 0
- Duração do teste: aproximadamente 6 segundos
- Duração total da execução: aproximadamente 7,4 segundos

## Avaliação

Os fluxos essenciais cobertos estão funcionando:

1. A página de autenticação apresenta os controles necessários.
2. Rotas protegidas impedem acesso anônimo.
3. Um administrador válido consegue entrar no sistema.
4. Após a autenticação, o dashboard administrativo é apresentado.

## Limitações

Esta execução é um teste inicial de fumaça. Ainda não cobre:

- logout e expiração da sessão;
- bloqueio após tentativas inválidas;
- recuperação de senha;
- permissões de estudante;
- cadastro, alteração, consulta e exclusão nos módulos;
- validações de formulários;
- comportamento em outros navegadores e resoluções;
- acessibilidade e desempenho.

## Risco de segurança identificado

Foram observadas credenciais armazenadas diretamente em arquivos
`appsettings`. Elas não são reproduzidas neste relatório. Recomenda-se:

1. revogar ou rotacionar as credenciais expostas;
2. remover senhas dos arquivos versionados;
3. usar variáveis de ambiente ou .NET User Secrets;
4. manter credenciais exclusivas para testes automatizados.

## Próximos testes recomendados

1. Login inválido e bloqueio de conta.
2. Logout e tentativa de reutilizar a sessão.
3. Controle de acesso por perfil.
4. Fluxos CRUD dos módulos administrativos.
5. Capturas de tela automáticas em caso de falha.
6. Geração de arquivo TRX e relatório HTML em cada execução.
