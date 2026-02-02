using JFramework;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class TiktokAttributesBuilder : IJCombatAttrBuilder
    {
        IAttributeService attributeService;
        public TiktokAttributesBuilder(IAttributeService attributeService)
        {
            this.attributeService = attributeService;
        }
        public List<IUnique> Create()
        {
            var result = new List<IUnique>();

            //一级属性
            var formationUnitAttack = attributeService.GetAttack(); //soldider + formation
            var formationUnitDefence = attributeService.GetDefence();//soldider + formation
            var formationUnitSpeed = attributeService.GetSpeed(); //samurai + soldier
            var formationUnitHp = attributeService.GetHp(); //formula + formation
            var formationUnitMaxHp = attributeService.GetMaxHp(); //formula + formation
            var hp = new GameAttributeInt(TiktokAttributesType.Hp.ToString(), formationUnitHp, formationUnitHp);
            var maxHp = new GameAttributeInt(TiktokAttributesType.MaxHp.ToString(), formationUnitMaxHp, formationUnitMaxHp);
            var attack = new GameAttributeInt(TiktokAttributesType.Attack.ToString(), formationUnitAttack, formationUnitAttack);
            var defence = new GameAttributeInt(TiktokAttributesType.Defence.ToString(), formationUnitDefence, formationUnitDefence);
            var speed = new GameAttributeInt(TiktokAttributesType.Speed.ToString(), formationUnitSpeed, formationUnitSpeed);

            //二级属性
            var formationLevel = attributeService.GetLevel(); //formation
            var formationPower = attributeService.GetPower(); //samurai
            var formationDef = attributeService.GetDef();//samurai
            var formationInt = attributeService.GetIntel();//samurai
            var level = new GameAttributeInt(TiktokAttributesType.Level.ToString(), formationLevel, formationLevel);
            var power = new GameAttributeInt(TiktokAttributesType.Power.ToString(), formationPower, formationPower);
            var def = new GameAttributeInt(TiktokAttributesType.Def.ToString(), formationDef, formationDef);
            var intel = new GameAttributeInt(TiktokAttributesType.Intel.ToString(), formationInt, formationInt);

            //一级属性
            result.Add(hp);
            result.Add(maxHp);
            result.Add(speed);
            result.Add(attack);
            result.Add(defence);

            //二级属性
            result.Add(level);
            result.Add(power);
            result.Add(def);
            result.Add(intel);


            return result;
        }
    }
}

