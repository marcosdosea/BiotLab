# Configurar envio real de e-mail no BiotLab

O projeto foi ajustado para usar SMTP real em vez de Mailtrap.

## Onde configurar

Edite os dois arquivos abaixo, porque em ambiente local o ASP.NET Core carrega também o `appsettings.Development.json`:

- `Codigo/BiotLab/BiotLabWeb/appsettings.json`
- `Codigo/BiotLab/BiotLabWeb/appsettings.Development.json`

Configure a seção:

```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSsl": true,
  "Username": "seuemail@gmail.com",
  "Password": "senha-de-app-do-gmail",
  "FromEmail": "seuemail@gmail.com",
  "FromName": "BiotLab"
}
```

## Gmail

Para Gmail/Google Workspace, use:

- Host: `smtp.gmail.com`
- Port: `587`
- EnableSsl: `true`
- Username: seu e-mail completo
- Password: senha de app do Google, não a senha normal da conta
- FromEmail: normalmente o mesmo e-mail usado em `Username`

## Importante

Não envie o projeto com a senha real dentro do ZIP ou do GitHub. Para produção, prefira configurar a senha por variável de ambiente, user secrets ou configuração segura do servidor.

Variáveis de ambiente equivalentes:

```text
EmailSettings__Host=smtp.gmail.com
EmailSettings__Port=587
EmailSettings__EnableSsl=true
EmailSettings__Username=seuemail@gmail.com
EmailSettings__Password=senha-de-app
EmailSettings__FromEmail=seuemail@gmail.com
EmailSettings__FromName=BiotLab
```

## Link do convite

A seção abaixo define o endereço usado no botão do e-mail de convite:

```json
"App": {
  "BaseUrl": "https://localhost:44396"
}
```

Se o convite for aberto por outra pessoa em outro computador, esse endereço precisa ser uma URL acessível para ela, por exemplo a URL publicada do sistema. Se deixar `localhost`, o link só funciona no computador onde o projeto está rodando.
