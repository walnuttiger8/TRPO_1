using System.Collections.Generic;
using System.Linq;
using TRPO_1.Models;

namespace TRPO_1.Services
{
    public class Wallet
    {
        private List<MoneyUnit> _moneyUnits = new List<MoneyUnit>();

        public float Balance => _moneyUnits.Select(x => x.Quantity).Sum();

        public void TopUp(MoneyUnit moneyUnit)
        {
            _moneyUnits.Add(moneyUnit);
        }

        public List<MoneyUnit> Take(int amount)
        {
            var change = Calculate(amount, _moneyUnits);
            if (change != null)
            {
                foreach (var unit in change)
                {
                    _moneyUnits.Remove(unit);
                }
            }
            return change;
        }

        private static List<MoneyUnit> Calculate(int target, List<MoneyUnit> units, List<MoneyUnit> prefix = null)
        {
            prefix = prefix ?? new List<MoneyUnit>();
            if (target == 0)
            {
                return prefix;
            }
            units = units.OrderByDescending(x => x.Quantity).ToList();
            foreach (var unit in units)
            {
                if (unit.Quantity <= target)
                {
                    prefix.Add(unit);
                    units.Remove(unit);
                    target -= unit.Quantity;
                    return Calculate(target, units, prefix);
                }
            }
            return null;
        }
    }
}
