
using System;

namespace JFramework
{
    public class CombatAttributeLong : CombatAttribute<long>
    {
        public override long CurValue { get { return curValue + GetAllExtraValue(); } set => curValue = value; }

        public CombatAttributeLong(string uid, long value, long maxValue) : base(uid, value, maxValue)
        {
        }

        public override long Plus(long value)
        {
            CurValue += value;
            CurValue = Math.Min(CurValue, MaxValue);
            return CurValue;
        }

        public override long PlusMax(long value)
        {
            MaxValue += value;
            return MaxValue;
        }

        public override long Minus(long value)
        {
            CurValue -= value;
            CurValue = Math.Max(CurValue, 0);
            return CurValue;
        }

        public override long MinusMax(long value)
        {
            MaxValue -= value;
            MaxValue = Math.Max(MaxValue, 0);
            CurValue = Math.Min(CurValue, MaxValue);
            return MaxValue;
        }

        public override long Multi(long value)
        {
            CurValue *= value;
            CurValue = Math.Min(CurValue, MaxValue);
            return CurValue;
        }

        public override long MultiMax(long value)
        {
            MaxValue *= value;
            return MaxValue;
        }

        public override long Div(long value)
        {
            if (value == 0)
                throw new ArgumentException("除數不能為0");

            CurValue = CurValue / value;
            return CurValue;
        }

        public override long DivMax(long value)
        {
            if (value == 0)
                throw new ArgumentException("除數不能為0");

            MaxValue = MaxValue / value;
            CurValue = Math.Min(CurValue, MaxValue);
            return MaxValue;
        }

        public override bool IsMax()
        {
            return CurValue == MaxValue;
        }

        public override long GetAllExtraValue()
        {
            throw new NotImplementedException();
        }

        public override void AddExtraValue(string extraUid, long value)
        {
            if (extraAttributes.ContainsKey(extraUid))
            {
                extraAttributes[extraUid] += value;
            }
            else
                extraAttributes.Add(extraUid, value);
        }

        public override bool MinusExtraValue(string extraUid, long value)
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