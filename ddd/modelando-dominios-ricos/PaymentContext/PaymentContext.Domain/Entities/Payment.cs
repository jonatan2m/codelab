using System;

namespace PaymentContext.Domain.Entities
{
    public abstract class Payment
    {
        public string Number { get; private set; }
        public DateTime PaidDate { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public decimal Total { get; private set; }
        public decimal TotalPaid { get; private set; }
        public string Payer { get; private set; }
        public string Document { get; private set; }
        public string Address { get; private set; }
        public string Email { get; private set; }

    }

    public class BoletoPayment
    {

    }

    //Não se salva número completo do cartão, Código de segurança, Data de expiração
    //Melhor salvar somente os 4 últimos digítos do cartão.
    //Regras do PCI
    //https://www.ecommercebrasil.com.br/artigos/o-que-e-pci-e-quais-sao-as-normas-dos-cartoes-de-credito-na-internet/
    public class CreditCardPayment
    {

    }

    public class PayPalPayment
    {

    }
}