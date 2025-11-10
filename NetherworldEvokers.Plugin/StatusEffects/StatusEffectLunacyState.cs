using System.Collections;
namespace NetherworldEvokers.StatusEffects
{
    class StatusEffectLunacyState : StatusEffectState
    {
       public const string StatusId = "netherworldevokers_lunacy";

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams, ICoreGameManagers coreGameManagers)
        {
            return inputTriggerParams.triggerType == CharacterTriggerData.Trigger.OnMoonLit || inputTriggerParams.triggerType == CharacterTriggerData.Trigger.OnMoonShade;
        }

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams, ICoreGameManagers coreGameManagers)
        {
            CharacterState character = inputTriggerParams.associatedCharacter;
            if (character == null)
            {
                yield break;
            }
            int LunacyStacks = character.GetStatusEffectStacks(StatusId);
            if (inputTriggerParams.triggerType == CharacterTriggerData.Trigger.OnMoonLit)
            {
                character.BuffDamage(DamageValue(LunacyStacks), null, fromStatusEffect: true);
                yield break;
            }
            if (inputTriggerParams.triggerType == CharacterTriggerData.Trigger.OnMoonShade)
            {
                character.DebuffDamage(DamageValue(LunacyStacks), null, fromStatusEffect: true);
                yield break;
            }
            yield break;
        }

        private int DamageValue(int stacks)
        {
            return GetMagnitudePerStack() * stacks;
        }

        public override void OnStacksAdded(CharacterState character, int numStacksAdded, CharacterState.AddStatusEffectParams addStatusEffectParams, ICoreGameManagers coreGameManagers)
        {
            if (numStacksAdded > 0 && coreGameManagers.GetPlayerManager().CurrentMoonPhaseMatches(2, PlayerManager.MoonPhaseModifierMode.Ignore, -1, coreGameManagers))
            {
                character.BuffDamage(DamageValue(numStacksAdded), null, fromStatusEffect: true);
            }
        }

        public override void OnStacksRemoved(CharacterState character, int numStacksRemoved, ICoreGameManagers coreGameManagers)
        {
            if (character == null)
            {
                return;
            }
            if (numStacksRemoved > 0 && coreGameManagers.GetPlayerManager().CurrentMoonPhaseMatches(2,PlayerManager.MoonPhaseModifierMode.Ignore,-1,coreGameManagers))
            {
                character.DebuffDamage(DamageValue(numStacksRemoved), null, fromStatusEffect: true);
            }
        }

        public override int GetEffectMagnitude(int stacks = 1)
        {
            return DamageValue(stacks);
        }

        public override int GetMagnitudePerStack()
        {
            return GetParamInt() + relicManager.GetModifiedStatusMagnitudePerStack(StatusId, GetAssociatedCharacter().GetTeamType());
        }
    }
}