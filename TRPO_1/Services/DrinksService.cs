using System;
using System.Collections.Generic;
using TRPO_1.Models;

namespace TRPO_1.Services
{
    public class DrinksService
    {
        public delegate void BalanceChange(int prevValue, int currentValue);
        public event BalanceChange BalanceChanged;

        public int Balance { get; set; } = 0;
        private Wallet _serviceWallet = new Wallet();

        public DrinksService()
        {
            var _moneyUnitPool = new MoneyUnitPool();
            for (var _ = 0; _ < 100; _++)
            {
                _serviceWallet.TopUp(_moneyUnitPool.Get());
            }
        }

        public void TopUp(MoneyUnit moneyUnit)
        {
            SetBalance(Balance + moneyUnit.Quantity);
            _serviceWallet.TopUp(moneyUnit);
        }
        
        public bool Buy(Product product)
        {
            if (Balance >= product.Price)
            {
                SetBalance(Balance - (int)product.Price);
                return true;
            }
            return false;
        }

        public List<MoneyUnit> GetChange()
        {
            var change = _serviceWallet.Take(Balance);
            if (change != null)
            {
                SetBalance(0);
            }
            return change;
        }

        private void SetBalance(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }
            var prevValue = Balance;
            Balance = value;

            BalanceChanged(prevValue, Balance);
        }
    }
}
