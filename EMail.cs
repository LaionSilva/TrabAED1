using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

////////////////////////////////////////////////////////////////////////////////////////////////////////   
// A biblioteca presente abaixo não é de nossa autoria, foi exportada da internet e sofreu algumas 
// modificações para que fosse possivel funcionar perfeitamente no nosso código.
//
// OBS.: Apenas a classe EMail juntamente com seus métodos foram escritos ou modificados por nós.
namespace logistica{
  public interface IEmailService{
    bool SendEmailMessage(EmailMessage message);
  }

  static class EMail {

    public static void EnviarRelatorio(List<string> relatorio, string email) { //  Cria um email personalizado especialmente para relatórios de entrega
      string mensagem =  //  Cabeçalho
      "=== 0 == 1 == 0 ====&nbsp;&nbsp;A N Ô N I M O U S&nbsp;&nbsp;&nbsp;H . L&nbsp;&nbsp;==== 1 == 0 == 1 ===" + "<br/>" +
      "=== 1 == 0 == 1 ====&nbsp;&nbsp;&nbsp;D I S T R I B U I D O R A&nbsp;&nbsp;&nbsp;==== 0 == 1 == 0 ===" + "<br/><br/>";

      foreach(string m in relatorio) { //  Corpo do relatório
        mensagem += m;
      }
      mensagem += "<br/><br/>Laion Fernandes<br/>Higor Parnoff<br/>Engenharia de Computação - UCL Manguinhos"; //  Rodapé

      EnviarEmail(mensagem, "Realatorio de Entrega A.H.L", email); //  Encaminhar email montado
    }

    public static void EnviarEmail(string mensagem, string assunto, string email) { //  Envia qualquer email que for especificado, é método mais genérico pra enviar os emails
      GmailEmailService gmail = new GmailEmailService();
      EmailMessage msg = new EmailMessage();
      msg.Body = mensagem;
      msg.IsHtml = true;
      msg.Subject = assunto;
      msg.ToEmail = email;
      gmail.SendEmailMessage(msg);
    }
  }

  public class SmtpConfiguration{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool Ssl { get; set; }
  }

  public class EmailMessage{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
  }

  public class GmailEmailService : IEmailService{
    private static Save file = new Save(); 
    private readonly SmtpConfiguration _config;
    public GmailEmailService(){
        _config = new SmtpConfiguration();
        var gmailUserName = "tesparpartes@gmail.com";
        var gmailPassword = file.getSenhas();
        var gmailHost = "smtp.gmail.com";
        var gmailPort = 587;
        var gmailSsl = true;
        _config.Username = gmailUserName;
        _config.Password = gmailPassword;
        _config.Host = gmailHost;
        _config.Port = gmailPort;
        _config.Ssl = gmailSsl;
    }

    public bool SendEmailMessage(EmailMessage message)
    {
        var success = false;
        try
        {
            var smtp = new SmtpClient
            {
                Host = _config.Host,
                Port = _config.Port,
                EnableSsl = _config.Ssl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config.Username, _config.Password)
            };
            using (var smtpMessage = new MailMessage(_config.Username, message.ToEmail))
            {
                smtpMessage.Subject = message.Subject;
                smtpMessage.Body = message.Body;
                smtpMessage.IsBodyHtml = message.IsHtml;
                smtp.Send(smtpMessage);
            }
            success = true;
            Console.Write ("\nEmail enviado\n");
        }
        catch(SmtpException) {
          LogisticaException.ExceptionGrave("LE_Save_SenhaNaoEncontrada");
          return false;
        }
        catch (Exception e) {
          LogisticaException.ExceptionGrave("LE_EMail", e, "EMail", "EnviarEmail/SendEmailMessage"); 
          Console.Write ("\nFalha no envio\n");
          return false;
        }
        return success;
    }
  }
}
