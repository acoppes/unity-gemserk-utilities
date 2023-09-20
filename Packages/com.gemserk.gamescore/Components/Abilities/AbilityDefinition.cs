using System;
using Gemserk.Utilities;
using UnityEngine.Serialization;

namespace Game.Components.Abilities
{
    [Serializable]
    public class AbilityDefinition
    {
        public string name;
        
        public float cooldown;
        
        [FormerlySerializedAs("chargeTime")] 
        public float startTime;

        [FormerlySerializedAs("attackType")] 
        public Ability.ExecutionType executionType = Ability.ExecutionType.None;

        public Ability.ReloadCooldownType cooldownType = Ability.ReloadCooldownType.IfNoExecuting;
        public Ability.ResetCooldownType resetCooldownType = Ability.ResetCooldownType.None;

        public bool startsLoaded;

        public bool autoTarget;
        
        [FormerlySerializedAs("autoTargeting")] 
        public Targeting targeting;

        public int charges = 1;

        public Ability Create()
        {
            var ability = new Ability
            {
                name = name,
                cooldown = new Cooldown(cooldown),
                startTime = new Cooldown(startTime),
                executionType = executionType,
                cooldownType = cooldownType,
                resetCooldownType = resetCooldownType,
                autoTarget = autoTarget,
                targeting = targeting,
                currentCharges = charges,
                totalCharges = charges,
            };
            
            if (startsLoaded)
            {
                ability.cooldown.Fill();
            }
            
            return ability;
        }
    }
}