using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRPO_1.Models;

namespace TRPO_1.Services
{
    public class DrinksService
    {
        private List<MoneyUnit> _moneyUnits = new List<MoneyUnit>();

        public float Balance => _moneyUnits.Select(x => x.Quantity).Sum();

        public void TopUp(MoneyUnit moneyUnit)
        {
            _moneyUnits.Add(moneyUnit);
        }
    }
}
