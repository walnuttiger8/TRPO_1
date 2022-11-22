using System;
using System.Collections.Generic;
using TRPO_1.Models;

namespace TRPO_1.Services
{
    public class AfricanException : Exception { }
    public class RussianException : Exception { }

    public class DrinksService
    {
        private const int _minAvailableVolume = 10;
        private const int _maxAvailableVolume = 50;

        public delegate void BalanceChange(int prevValue, int currentValue);
        public event BalanceChange BalanceChanged;

        public delegate void VolumeChange(float prevValue, float currentValue);
        public event VolumeChange VolumeChanged;

        public int Balance { get; set; } = 0;
        public float ServiceBalance
        {
            get { return _serviceWallet.Balance; }
        }
        public float MaxVolume
        {
            get
            {
                return _maxAvailableVolume;
            }
        }
        public float MinVolume
        {
            get
            {
                return _minAvailableVolume;
            }
        }
        public float Volume { get; set; } = 0;
        private Wallet _serviceWallet = new Wallet();

        public DrinksService()
        {
            var _moneyUnitPool = new MoneyUnitPool();
            for (var _ = 0; _ < 100; _++)
            {
                _serviceWallet.TopUp(_moneyUnitPool.Get());
            }
            InitializeVolume();
        }

        public void TopUp(MoneyUnit moneyUnit)
        {
            SetBalance(Balance + moneyUnit.Quantity);
            _serviceWallet.TopUp(moneyUnit);
        }
        
        public bool Buy(Product product)
        {
            if (Volume < product.Volume)
            {
                throw new AfricanException();
            }
            if (Balance < product.Price)
            {
                throw new RussianException();
            }
            SetBalance(Balance - (int)product.Price);
            SetVolume(Volume - product.Volume);
            return true;
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

            if (BalanceChanged != null)
            {
                BalanceChanged(prevValue, Balance);
            }
        }

        private void SetVolume(float value)
        {
            if (value < _minAvailableVolume)
            {
                throw new ArgumentException();
            }
            var prevValue = Volume;
            Volume = value;

            if (VolumeChanged != null)
            {
                VolumeChanged(prevValue, Volume);
            }
        }

        private void InitializeVolume()
        {
            var random = new Random();
            SetVolume(random.Next(_minAvailableVolume, _maxAvailableVolume));
        }
    }
}
