using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace Payment.WalletAPI.Entity
{
    public class CreateAccount
    {
        public string UserId { get; set; }
        public decimal InitialBalance { get; set; }
    }

    // Configuration for Transaction entity
    
}
