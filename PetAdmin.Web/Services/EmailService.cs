using System;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Models.Domain;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace PetAdmin.Web.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmailAfterRequestedSchedule(PetLover petLover, Pet pet, bool isUser = true)
        {
            string message = string.Empty;
            if (isUser)
                message = $"<br>Olá {petLover.Name},<br>A solicitação do agendamento do seu pet <b>{pet.Name}</b> foi feita com sucesso." +
                    $"<br><br>Status do Agendamento: <b>Aguardando confirmação do Pet Shop</b>." +
                    $"<br><br>Você reberá um email quando o status for alterado ou se quiser pode acessar o <a href='https://petwise.azurewebsites.net/'>sistema</a> para o acompanhamento :) {GetSignature()}";
            else
                message = $"<br>Olá {petLover.Name},<br>A solicitação do agendamento do seu pet <b>{pet.Name}</b> foi feita com sucesso." +
                    $"<br><br>Status do Agendamento: <b>Aguardando confirmação do Pet Shop</b>." +
                    $"<br><br>Você reberá um email quando o status for alterado." +
                    $"{GetRegisterLink()} {GetSignature()}";
            SendEmail(petLover.Email, petLover.Name, "Solicitação de Agendamento", message);
        }

        public void SendEmailAfterConfirmedSchedule(SchedulePetConfirmEmailDto dto)
        {
            var day = dto.DateWithHour.Day < 10 ? $"0{dto.DateWithHour.Day}" : dto.DateWithHour.Day.ToString();
            var month = dto.DateWithHour.Month < 10 ? $"0{dto.DateWithHour.Month}" : dto.DateWithHour.Month.ToString();
            var hour = dto.DateWithHour.Hour < 10 ? $"0{dto.DateWithHour.Hour}" : dto.DateWithHour.Hour.ToString();
            var minutes = dto.DateWithHour.Minute < 10 ? $"0{dto.DateWithHour.Minute}" : dto.DateWithHour.Minute.ToString();

            string message = string.Empty;

            if (dto.IsUser)
                message = $"<br>Olá {dto.PetLoverName},<br>O agendamento do seu pet <b>{dto.PetName}</b> foi confirmado para o dia " +
                $"{day}/{month}/{dto.DateWithHour.Year} às {hour}h{minutes} :) {GetSignature()}";
            else
                message = $"<br>Olá {dto.PetLoverName},<br>O agendamento do seu pet <b>{dto.PetName}</b> foi confirmado para o dia " +
                $"{day}/{month}/{dto.DateWithHour.Year} às {hour}h{minutes} :) " +
                $"{GetRegisterLink()} {GetSignature()}";

            SendEmail(dto.PetLoverEmail, dto.PetLoverName, "Confirmação de Agendamento", message);
        }

        public void SendEmailAfterCompletedSchedule(SchedulePetCompleteEmailDto dto)
        {
            string message = string.Empty;

            if (dto.IsUser)
                message = $"<br>Olá {dto.PetLoverName},<br>O {dto.ScheduleItemName} do seu pet <b>{dto.PetName}</b> foi concluído!" +
                $"{GetSignature()}";
            else
                message = $"<br>Olá {dto.PetLoverName},<br>O {dto.ScheduleItemName} do seu pet <b>{dto.PetName}</b> foi concluído!" +
                $"{GetRegisterLink()} {GetSignature()}";

            SendEmail(dto.PetLoverEmail, dto.PetLoverName, "Serviço Concluído", message);
        }

        public void SendEmailAfterCanceledSchedule(SchedulePetCancelEmailDto dto)
        {
            var day = dto.DateWithHour.Day < 10 ? $"0{dto.DateWithHour.Day}" : dto.DateWithHour.Day.ToString();
            var month = dto.DateWithHour.Month < 10 ? $"0{dto.DateWithHour.Month}" : dto.DateWithHour.Month.ToString();
            var hour = dto.DateWithHour.Hour < 10 ? $"0{dto.DateWithHour.Hour}" : dto.DateWithHour.Hour.ToString();
            var minutes = dto.DateWithHour.Minute < 10 ? $"0{dto.DateWithHour.Minute}" : dto.DateWithHour.Minute.ToString();

            string message = $"<br>Olá, <br>Infelizmente o agendamento do pet <b>{dto.PetName}</b> marcado para o dia " +
                $"{day}/{month}/{dto.DateWithHour.Year} às {hour}h{minutes} foi <b>cancelado</b>. {GetSignature()}";

            SendEmail(dto.PetLoverEmail, dto.PetLoverName, "Cancelamento de Agendamento", message);
            SendEmail(dto.EmployeeEmail, string.Empty, "Cancelamento de Agendamento", message);
        }

        public void SendEmailWithNewPassword(string email, string password)
        {
            string message = $"<br>Olá, Sua nova senha é {password}<br>" +
                $"Ao entrar no sistema você poderá trocar essa senha gerada por uma outra. Basta entrar na página de configurações e definir uma nova senha. {GetSignature()}";

            //Os outros métodos não estão mandando
            SendEmailAllowed(email, string.Empty, "Nova Senha", message);
        }

        private string GetRegisterLink()
        {
            return $"<br><br>Se cadastre no nosso <a href='https://petwise.azurewebsites.net/'>sistema</a> para fazer agendamentos de maneira rápida e simples e ter acesso a dicas e descontos :)";
        }

        private string GetSignature()
        {
            return $"<br><br><br><br><b>Pipo Petwise</b> <br> <a href='https://petwise.azurewebsites.net/'>https://petwise.azurewebsites.net/</a> <br> contatopetwise@gmail.com <br> Rio de Janeiro - RJ";
        }

        private void SendEmailAllowed(string email, string name, string subject, string messageText)
        {
            try
            {
                var apiKey = _configuration.GetValue<string>("Mailjet:apiKey");
                var secretKey = _configuration.GetValue<string>("Mailjet:secretKey");
                var url = "https://api.mailjet.com/v3.1/send";

                var message = new EmailMessage();
                message.Messages.Add
                (
                    new Message
                    {
                        From = new FromMessage
                        {
                            Email = "contatopetwise@gmail.com",
                            Name = "PetWise"
                        },
                        To = new List<ToMessage>
                        {
                            new ToMessage
                            {
                                Email = email,
                                Name = name
                            }
                        },
                        Subject = subject,
                        HTMLPart = messageText
                    }
                );

                var json = JsonConvert.SerializeObject(message, Formatting.None,
                                      new JsonSerializerSettings
                                      {
                                          NullValueHandling = NullValueHandling.Ignore
                                      });

                using (var httpClient = new HttpClient())
                {
                    Uri baseUri = new Uri(url);
                    var byteArray = Encoding.ASCII.GetBytes($"{apiKey}:{secretKey}");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var reponse = httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SendEmail(string email, string name, string subject, string messageText)
        {
            //try
            //{
            //    var apiKey = _configuration.GetValue<string>("Mailjet:apiKey");
            //    var secretKey = _configuration.GetValue<string>("Mailjet:secretKey");
            //    var url = "https://api.mailjet.com/v3.1/send";

            //    var message = new EmailMessage();
            //    message.Messages.Add
            //    (
            //        new Message
            //        {
            //            From = new FromMessage
            //            {
            //                Email = "contatopetwise@gmail.com",
            //                Name = "PetWise"
            //            },
            //            To = new List<ToMessage>
            //            {
            //                new ToMessage
            //                {
            //                    Email = email,
            //                    Name = name
            //                }
            //            },
            //            Subject = subject,
            //            HTMLPart = messageText
            //        }
            //    );

            //    var json = JsonConvert.SerializeObject(message, Formatting.None,
            //                          new JsonSerializerSettings
            //                          {
            //                              NullValueHandling = NullValueHandling.Ignore
            //                          });

            //    using (var httpClient = new HttpClient())
            //    {
            //        Uri baseUri = new Uri(url);
            //        var byteArray = Encoding.ASCII.GetBytes($"{apiKey}:{secretKey}");
            //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        var reponse = httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;
            //    }
            //}
            //catch(Exception ex)
            //{

            //}

            //-----------------------
            //-----------------------

            //GMAIL PURO

            /*
            using MimeKit;
            using MimeKit.Text;
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("PetWise", "contatopetwise@gmail.com"));
                mimeMessage.To.Add(new MailboxAddress("PetWise", email));
                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = messageText
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(_configuration.GetValue<string>("GmailEmailHost"), _configuration.GetValue<int>("GmailEmailPort"), true);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_configuration.GetValue<string>("GmailUserName"), _configuration.GetValue<string>("GmailEmailPassword"));

                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
            }
            */
        }
    }

    public class EmailMessage
    {
        public List<Message> Messages { get; set; }

        public EmailMessage()
        {
            Messages = new List<Message>();
        }
    }

    public class Message
    {
        public FromMessage From { get; set; }
        public List<ToMessage> To { get; set; }
        public string Subject { get; set; }
        public string TextPart { get; set; }
        public string HTMLPart { get; set; }

        public Message()
        {
            To = new List<ToMessage>();
        }
    }

    public class FromMessage
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ToMessage
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
