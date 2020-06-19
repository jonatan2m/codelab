using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DomainEvents.MediatRExample1
{
    public  interface IEntity
    {

    }

    public class Payment
    {
        private decimal _fee = 0;
        public readonly decimal GrossAmount;
        public decimal NetAmount => GrossAmount - _fee;

        public Payment(decimal amount)
        {
            GrossAmount = amount;
        }


        public void ApplyFee(decimal fee)
        {
            _fee = fee;
        }
    }

    public class PaymentRecorded : INotification, IRequest<decimal>
    {
        public readonly decimal Amount;

        public PaymentRecorded(Payment payment)
        {
            Amount = payment.NetAmount;
        }
    }

    public class Bank
    {
        public ICollection<Payment> Payments { get; }

        public Bank()
        {
            Payments = new List<Payment>();
        }

        public void RecordPayment(Payment payment)
        {
            //Check if Payment is Valid
            /* some code */

            Payments.Add(payment);

            //Notify who wants to know about payments coming
            /*  */
        }
    }

    public class CalculatePaymentFee : IRequestHandler<PaymentRecorded, decimal>
    {
        private const decimal BaseAmount = 1000;
        
        public Task<decimal> Handle(PaymentRecorded request, CancellationToken cancellationToken)
        {
            if (request.Amount > BaseAmount)
                return Task.FromResult(request.Amount * 0.01M);

            return Task.FromResult(request.Amount * 0.02M);
        }
    }

    /// <summary>
    /// Create report about payments. It seems an ETL process but not exactly this.
    /// Just for instance...
    /// Notification doesn't need to return information.
    /// We need to wait saving Payment at the DataBase before processing
    /// </summary>
    public class PaymentReportHandler : INotificationHandler<PaymentRecorded>
    {
        public Task Handle(PaymentRecorded notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
