using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Payment.WalletAPI.Entity
{
    public class Deposit
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }

    // Configuration for ApplicationUser
    
}
