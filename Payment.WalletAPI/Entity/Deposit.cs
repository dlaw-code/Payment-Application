using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Payment.WalletAPI.Entity
{
    public class Deposit
    {
        public Guid AccountId { get; set; } // Changed to Guid
        public decimal Amount { get; set; }
    }

    // Configuration for ApplicationUser

}
