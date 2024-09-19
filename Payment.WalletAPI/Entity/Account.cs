using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Payment.WalletAPI.Entity
{
    public class Account
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal DailyLimit { get; set; } = 100000; // Default daily limit
        public decimal DailySpent { get; set; } = 0; // Track daily spending

    }
}