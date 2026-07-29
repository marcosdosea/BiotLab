<div align="center">
  <img src="./Codigo/BiotLabWeb/wwwroot/assets/BIOTLAB.svg" width="60%" alt="Logo do BiotLab"><br><br>

  <p><strong>Sistema Web de apoio à gestão de biotérios acadêmicos</strong></p>

  <p>
    Centralização de informações, controle operacional e rastreabilidade
    das atividades realizadas no biotério.
  </p>
</div>

<hr>

<h2>Sumário</h2>

<ul id="nav">
  <li><a href="#sobre">1. Sobre o Projeto</a></li>
  <li><a href="#problema">2. O Problema</a></li>
  <li><a href="#solucao">3. A Solução</a></li>
  <li><a href="#funcionalidades">4. Funcionalidades</a></li>
  <li><a href="#perfis">5. Perfis de Acesso</a></li>
  <li><a href="#tecnologias">6. Tecnologias</a></li>
  <li><a href="#arquitetura">7. Arquitetura</a></li>
  <li><a href="#testes">8. Testes e Avaliação</a></li>
  <li><a href="#estado">9. Estado Atual</a></li>
  <li><a href="#linksuteis">10. Links Úteis</a></li>
  <li><a href="#equipe">11. Equipe</a></li>
</ul>

<hr>

<h2 id="sobre">1. Sobre o Projeto :microscope:</h2>

<p>
  O <strong>BiotLab</strong> é um sistema Web de código aberto desenvolvido para apoiar
  a gestão de biotérios acadêmicos. A aplicação busca reunir, em um único ambiente,
  informações administrativas e operacionais que normalmente ficam distribuídas em
  planilhas, registros físicos e documentos separados.
</p>

<p>
  O projeto foi desenvolvido no contexto do Trabalho de Conclusão de Curso:
</p>

<blockquote>
  <strong>BiotLab: um sistema de apoio à gestão de biotérios acadêmicos</strong>
</blockquote>

<p>
  A proposta está voltada principalmente a universidades, institutos de pesquisa e
  centros de ensino que utilizam animais em atividades científicas e educacionais.
</p>

<h2 id="problema">2. O Problema :warning:</h2>

<p>
  Biotérios acadêmicos precisam manter registros confiáveis sobre animais, gaiolas,
  haréns, experimentos, anestésicos, responsáveis e ocorrências.
</p>

<p>
  Quando essas informações são controladas apenas por planilhas, documentos físicos
  ou ferramentas genéricas, podem surgir dificuldades como:
</p>

<p><strong>- Informações dispersas:</strong> registros armazenados em diferentes arquivos e documentos.</p>

<p><strong>- Baixa rastreabilidade:</strong> dificuldade para acompanhar o histórico das atividades e identificar os responsáveis.</p>

<p><strong>- Controle operacional limitado:</strong> problemas no acompanhamento de gaiolas, haréns, experimentos e insumos.</p>

<p><strong>- Retrabalho:</strong> repetição de cadastros e maior esforço para localizar e atualizar informações.</p>

<p><strong>- Risco de inconsistências:</strong> registros incompletos, duplicados ou desatualizados.</p>

<p>
  No levantamento realizado durante o projeto, 13 dos 15 participantes informaram
  utilizar planilhas eletrônicas para organizar as informações do biotério. O controle
  de estoque e a rastreabilidade dos registros estiveram entre as dificuldades mais citadas.
</p>

<h2 id="solucao">3. A Solução :bulb:</h2>

<p>
  O BiotLab centraliza os principais registros do biotério em uma aplicação acessível
  por navegador Web.
</p>

<p>
  A solução foi estruturada para apoiar atividades administrativas e operacionais,
  permitindo que cada usuário acesse as funcionalidades relacionadas ao seu perfil.
  Dessa forma, o sistema contribui para uma organização mais clara dos dados e para
  o acompanhamento das atividades realizadas.
</p>

<p>
  Por ser um projeto de código aberto, o BiotLab também pode ser estudado, adaptado
  e ampliado por outras instituições conforme suas necessidades.
</p>

<h2 id="funcionalidades">4. Funcionalidades :gear:</h2>

<p><strong>- Autenticação e controle de acesso:</strong> entrada segura no sistema e proteção de funcionalidades conforme o perfil do usuário.</p>

<p><strong>- Gerenciamento de estudantes:</strong> cadastro, consulta, edição, ativação, desativação e atribuição de papéis de acesso.</p>

<p><strong>- Instituições e biotérios:</strong> manutenção dos cadastros institucionais utilizados pelo sistema.</p>

<p><strong>- Pesquisadores e fornecedores:</strong> registro das pessoas e organizações relacionadas às atividades do biotério.</p>

<p><strong>- Gaiolas:</strong> controle de código, localização, situação, quantidade de machos e fêmeas e biotério associado.</p>

<p><strong>- Haréns:</strong> registro das informações relacionadas ao manejo reprodutivo dos animais.</p>

<p><strong>- Povoamento de gaiolas:</strong> associação entre gaiolas, haréns e o usuário responsável.</p>

<p><strong>- Experimentos:</strong> cadastro dos projetos, período de realização, cepa e pesquisadores associados.</p>

<p><strong>- Anestésicos:</strong> manutenção do catálogo de produtos, registro de entradas, lotes, quantidades e valores.</p>

<p><strong>- Uso de anestésicos:</strong> registro da quantidade utilizada, procedimento, data, experimento e responsável.</p>

<p><strong>- Obituário:</strong> registro de óbitos e associação do evento à gaiola e ao pesquisador responsável.</p>

<h2 id="perfis">5. Perfis de Acesso :busts_in_silhouette:</h2>

<table>
  <thead>
    <tr>
      <th>Perfil</th>
      <th>Responsabilidades</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><strong>Administrador</strong></td>
      <td>Gerencia usuários, permissões e cadastros institucionais.</td>
    </tr>
    <tr>
      <td><strong>Pesquisador</strong></td>
      <td>Representa o responsável pelo biotério e acompanha operações administrativas da unidade.</td>
    </tr>
    <tr>
      <td><strong>Estudante</strong></td>
      <td>Representa o usuário operacional, podendo ser atribuído a pesquisadores ou técnicos autorizados.</td>
    </tr>
  </tbody>
</table>

<h2 id="tecnologias">6. Tecnologias :computer:</h2>

<h3>Back-end</h3>

<ul>
  <li>C#;</li>
  <li>.NET 8;</li>
  <li>ASP.NET Core MVC;</li>
  <li>Entity Framework Core;</li>
  <li>ASP.NET Core Identity;</li>
  <li>AutoMapper;</li>
  <li>MySQL.</li>
</ul>

<h3>Front-end</h3>

<ul>
  <li>Razor;</li>
  <li>Tailwind CSS;</li>
  <li>CSS;</li>
  <li>JavaScript;</li>
  <li>jQuery.</li>
</ul>

<h3>Desenvolvimento e testes</h3>

<ul>
  <li>Visual Studio 2022;</li>
  <li>GitHub;</li>
  <li>MSTest;</li>
  <li>Entity Framework Core InMemory;</li>
  <li>Moq;</li>
  <li>Selenium WebDriver.</li>
</ul>

<h2 id="arquitetura">7. Arquitetura :building_construction:</h2>

<p>
  O BiotLab utiliza o padrão MVC na camada Web e uma organização inspirada em
  Clean Architecture. A solução separa responsabilidades entre interface, serviços,
  domínio, autenticação e persistência.
</p>

<p><strong>- BiotLabWeb:</strong> controllers, views, view models e mapeamentos utilizados pela interface.</p>

<p><strong>- Service:</strong> serviços responsáveis pelas operações da aplicação e por parte das validações.</p>

<p><strong>- Core:</strong> entidades, DTOs, interfaces e elementos compartilhados pelo sistema.</p>

<p><strong>- Identity:</strong> autenticação, autorização, usuários e papéis de acesso.</p>

<p><strong>- Database:</strong> persistência das informações utilizadas pelos módulos.</p>

<p>
  Essa separação reduz o acoplamento entre os componentes e facilita a manutenção,
  a realização de testes e a evolução da aplicação.
</p>

<h2 id="testes">8. Testes e Avaliação :white_check_mark:</h2>

<p>
  A avaliação do BiotLab foi realizada com dados simulados e testes automatizados.
</p>

<p>
  Foram executados <strong>108 testes</strong>:
</p>

<ul>
  <li><strong>61 testes</strong> na camada de serviços;</li>
  <li><strong>47 testes</strong> na camada Web.</li>
</ul>

<p>
  Todos os casos previstos no conjunto executado foram aprovados.
</p>

<p>
  Também foram realizados testes funcionais automatizados com Selenium WebDriver
  para verificar o carregamento da página de login, o bloqueio de acesso anônimo a
  rotas protegidas e a autenticação de um usuário administrador.
</p>

<p>
  Esses resultados representam evidências iniciais sobre o funcionamento das
  funcionalidades avaliadas e não substituem testes prolongados em ambiente real.
</p>

<h2 id="estado">9. Estado Atual :construction:</h2>

<p>
  O BiotLab possui uma versão funcional com os principais módulos administrativos
  e operacionais implementados.
</p>

<p>
  Como oportunidades de evolução, destacam-se:
</p>

<ul>
  <li>validação com usuários em um biotério real;</li>
  <li>testes de integração, segurança e desempenho;</li>
  <li>avaliações de usabilidade, acessibilidade e aceitação;</li>
  <li>ampliação dos relatórios operacionais e regulatórios;</li>
  <li>melhoria dos alertas automáticos;</li>
  <li>aperfeiçoamento do controle de estoque;</li>
  <li>implementação de mecanismos de auditoria;</li>
  <li>integração com outros serviços institucionais;</li>
  <li>ampliação da documentação de instalação e adaptação.</li>
</ul>

<h2 id="linksuteis">10. Links Úteis :link:</h2>

<p>
  <a href="https://www.youtube.com/watch?v=0JTTKtgrr5s" target="_blank">
    1 - Vídeo de apresentação
  </a>
</p>

<p>
  <a href="https://docs.google.com/document/d/1l29CZnmfKLUCU2HY0SV4n96pnU2cZXW8TyMLCicN3TQ/edit?usp=sharing" target="_blank">
    2 - Manual de uso
  </a>
</p>

<h2 id="equipe">11. Equipe :technologist: :man_technologist: :woman_technologist:</h2>

<table align="center">
  <tr>
    <td align="center">
      <a href="https://github.com/IagoLResende" target="_blank">
        <img style="border-radius:100px;" src="https://avatars.githubusercontent.com/u/143676154?v=4" width="100px;" alt="Foto de Iago Liberato no GitHub"/><br>
        <sub>
          <b>Iago Liberato Resende de Carvalho</b>
          <p>Desenvolvedor</p>
        </sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/Felip3D3V" target="_blank">
        <img style="border-radius:100px;" src="https://avatars.githubusercontent.com/u/143296850?v=4" width="100px;" alt="Foto de Felipe Mendonça no GitHub"/><br>
        <sub>
          <b>Felipe Mendonça do Sacramento</b>
          <p>Desenvolvedor</p>
        </sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/ziulGui-0" target="_blank">
        <img style="border-radius:100px;" src="https://avatars.githubusercontent.com/u/58690311?v=4" width="100px;" alt="Foto de Luiz Guilherme no GitHub"/><br>
        <sub>
          <b>Luiz Guilherme Andrade Ferreira</b>
          <p>Desenvolvedor</p>
        </sub>
      </a>
    </td>
  </tr>
  <tr>
    <td align="center">
      <a href="https://github.com/NadsonTelesMendonca" target="_blank">
        <img style="border-radius:100px;" src="https://avatars.githubusercontent.com/u/148787131?v=4" width="100px;" alt="Foto de Nadson Teles no GitHub"/><br>
        <sub>
          <b>Nadson Teles Mendonça</b>
          <p>Desenvolvedor</p>
        </sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/Reismysterr" target="_blank">
        <img style="border-radius:100px;" src="https://avatars.githubusercontent.com/u/85704215?v=4" width="100px;" alt="Foto de Icaro Gabriel no GitHub"/><br>
        <sub>
          <b>Icaro Gabriel de Almeida Reis</b>
          <p>Desenvolvedor</p>
        </sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/XIanTavares" target="_blank">
        <img style="border-radius:100px;" src="https://avatars.githubusercontent.com/u/63467519?v=4" width="100px;" alt="Foto de Ian Tavares no GitHub"/><br>
        <sub>
          <b>Ian Tavares Silva</b>
          <p>Desenvolvedor</p>
        </sub>
      </a>
    </td>
  </tr>
</table>

<table align="center">
  <tr>
    <td align="center">
      <a href="https://github.com/marcosdosea" target="_blank">
        <img style="border-radius:100px;" src="https://avatars.githubusercontent.com/u/7799935?v=4" width="100px;" alt="Foto de Marcos Dósea no GitHub"/><br>
        <sub>
          <b>Doutor Marcos Dósea</b>
          <p>PO</p>
        </sub>
      </a>
    </td>
  </tr>
</table>
