using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GeneralResources.CSharp.Records.DiscriminatedUnions.HttpResponse;

namespace GeneralResources.CSharp.Records.DiscriminatedUnions
{
    public class DiscriminatedUnions
    {

        [Fact]
        public void Case1()
        {
            //await _emailRepository.GetByIdAsync(evt.EmailId) ?? throw new EmailNotFoundException(evt.EmailId);
            //Possivel chamada com a construção do objeto
            var email = new Email();

            //var result = await _validationService.ValidateAsync(email, cancellationToken);

            // Use result to find valid and invalid destinations...
            // Attempt to send email and catch any exceptions...
            var sendingAttempt = BuildASendingAttemptHere();

            email.Send(sendingAttempt);

            //Salva o resultado no banco
            //await _emailRepository.UpdateAsync(email, cancellationToken);
        }

        public SendingAttempt BuildASendingAttemptHere()
        {
            // Simulação de lógica de envio
            var emails = new List<string> { "example1@example.com", "example2@example.com" };
            var success = true;  // Sucesso parcial
            var trackingId = Guid.NewGuid();

            if (success && emails.Count > 0)
            {
                return new SendingAttempt.SentToSome(trackingId, emails);
            }
            else if (!success)
            {
                return new SendingAttempt.FailedToSend("Unable to send emails due to connection issues.");
            }
            else
            {
                return new SendingAttempt.SentToNone();
            }
        }

        [Fact]
        public void Case2()
        {
            HttpResponse response = new Success(200, "OK");

            string result = response switch
            {
                Success success => $"Success: {success.StatusCode} - {success.Content}",
                Redirect redirect => $"Redirect to: {redirect.Url}",
                Error error => $"Error: {error.Message}",
                _ => "Unknown response"
            };
        }
    }

    /// <summary>
    /// Case 1
    /// </summary>
    public class Email
    {
        // Imagine more properties like From, Subject, Body here...
        private readonly IEnumerable<string> _recipients = new List<string>();

        public Email Send(SendingAttempt attempt)
        {
            switch (attempt)
            {
                case SendingAttempt.SentToSome:
                    // Set trackingId and mark as Sent for some recipients
                    // Mark all other recipients as Invalid
                    break;

                case SendingAttempt.SentToNone:
                    // Mark all recipients as Invalid
                    break;

                case SendingAttempt.FailedToSend:
                    // Mark all recipients as Failed
                    break;
            }
            return this;
        }
    }

    /// <summary>
    /// Case 1 - discriminated unions (ou tagged unions)
    /// </summary>
    public abstract record SendingAttempt
    {
        private SendingAttempt() { }

        public record SentToSome(Guid TrackingId, IEnumerable<string> Emails) : SendingAttempt;
        public record SentToNone() : SendingAttempt;
        public record FailedToSend(string Message) : SendingAttempt;
    }

   
    /// <summary>
    /// Case 2
    /// </summary>
    public abstract record HttpResponse
    {
        private HttpResponse() { }

        public record Success(int StatusCode, string Content): HttpResponse;
        public record Redirect(string Url): HttpResponse;
        public record Error(string Message): HttpResponse;
    }
}
