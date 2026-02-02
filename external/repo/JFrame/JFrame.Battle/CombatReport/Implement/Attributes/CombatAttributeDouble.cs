
using System;

namespace JFramework
{
    public class CombatAttributeDouble : CombatAttribute<double>
    {
        public override double CurValue { get { return curValue + GetAllExtraValue(); } set => curValue = value; }

        public CombatAttributeDouble(string uid, double value, double maxValue) : base(uid, value, maxValue)
        {
        }

        public override double Plus(double value)
        {
            curValue += value;
            curValue = Math.Min(curValue, MaxValue);
            return CurValue;
        }

        public override double PlusMax(double value)
        {
            MaxValue += value;
            return MaxValue;
        }

        public override double Minus(double value)
        {
            curValue -= value;
            curValue = Math.Max(curValue, 0);
            return CurValue;
        }

        public override double MinusMax(double value)
        {
            MaxValue -= value;
            MaxValue = Math.Max(MaxValue, 0);
            curValue = Math.Min(curValue, MaxValue);
            return MaxValue;
        }

        public override double Multi(double value)
        {
            curValue *= value;
            curValue = Math.Min(curValue, MaxValue);
            return CurValue;
        }

        public override double MultiMax(double value)
        {
            MaxValue *= value;
            return MaxValue;
        }

        public override double Div(double value)
        {
            if (value == 0)
                throw new ArgumentException("除數不能為0");

            curValue = curValue / value;
            return CurValue;
        }

        public override double DivMax(double value)
        {
            if (value == 0)
                throw new ArgumentException("除數不能為0");

            MaxValue = MaxValue / value;
            curValue = Math.Min(curValue, MaxValue);
            return MaxValue;
        }

        public override bool IsMax()
        {
            return CurValue == MaxValue;
        }

        public override double GetAllExtraValue()
        {
            double result = 0;

            foreach(var extraValue in extraAttributes)
            {
                result += extraValue.Value;
            }

            return result;
        }

        public override void AddExtraValue(string extraUid, double value)
        {
            if (extraAttributes.ContainsKey(extraUid))
            {
                extraAttributes[extraUid] += value;
            }
            else
                extraAttributes.Add(extraUid, value);
        }

        public override bool MinusExtraValue(string extraUid, double value)
        {
            if (extraAttributes.ContainsKey(extraUid))
            {
                extraAttributes[extraUid] -= value;
                return true;
            }
            else
                return false;
        }
    }

}