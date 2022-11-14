using System;
using System.Collections.Generic;
using System.Linq;
using TRPO_1.Models;

namespace TRPO_1.Services
{
    public class MoneyUnitPool
    {
        private readonly List<MoneyUnit> _availableUnits;
        private Random _selector;


        public MoneyUnitPool()
        {
            _selector = new Random();
            _availableUnits = new List<MoneyUnit>()
            {
                new MoneyUnit() {Quantity=500, Image=Resource.r500},
                new MoneyUnit() {Quantity=100, Image=Resource.r100},
                new MoneyUnit() {Quantity=50, Image=Resource.r50},
                new MoneyUnit() {Quantity=10, Image=Resource.r10},
                new MoneyUnit() {Quantity=5, Image=Resource.r5},
                new MoneyUnit() {Quantity=2, Image=Resource.r2},
                new MoneyUnit() {Quantity=1, Image=Resource.r1},
            };
        }

        public MoneyUnit Get()
        {
            var unit = _availableUnits[_selector.Next(_availableUnits.Count)];
            return unit.Clone();
        }

        public MoneyUnit Get(int quantity)
        {
            var unit = _availableUnits.Where(x => x.Quantity == quantity).FirstOrDefault();
            if (unit != null)
            {
                return unit.Clone();
            }
            return null;
        }
    }
}
