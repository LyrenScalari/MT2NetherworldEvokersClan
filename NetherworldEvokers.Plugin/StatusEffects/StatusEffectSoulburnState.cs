using System.Collections;
using ShinyShoe;
namespace NetherworldEvokers.StatusEffects
{
    class StatusEffectSoulburnState : StatusEffectState, IDamageStatusEffect
    {
        public const string StatusId = "netherworldevokers_soulburn";
        
        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams, ICoreGameManagers coreGameManagers)
        {
            return inputTriggerParams.triggerType == Triggers.OnSoulburn;
        }

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams, ICoreGameManagers coreGameManagers)
        {
            CharacterState character = inputTriggerParams.associatedCharacter;
            if (character == null)
            {
                yield break;
            }
            int SoulburnStacks = character.GetStatusEffectStacks(StatusId);
            CoreSignals.DamageAppliedPlaySound.Dispatch(Damage.Type.Corruption);
            yield return coreGameManagers.GetCombatManager().ApplyDamageToTarget(GetDamageAmount(SoulburnStacks*character.GetNumberUniqueStatusEffectsInCategory(StatusEffectData.DisplayCategory.Negative, true)), character, new CombatManager.ApplyDamageToTargetParameters
            {
                damageType = Damage.Type.Corruption,
                affectedVfx = GetSourceStatusEffectData()?.GetOnAffectedVFX(),
                relicState = inputTriggerParams.suppressingRelic
            });
            yield break;
        }
        public override int GetEffectMagnitude(int stacks = 1)
        {
            return GetDamageAmount(stacks);
        }
        private int GetDamageAmount(int stacks)
        {
            return (GetParamInt() + relicManager.GetModifiedStatusMagnitudePerStack("soulburn", GetAssociatedCharacter().GetTeamType())) * stacks;
        }
    }
}