using ShinyShoe.Logging;
using System.Collections;
namespace NetherworldEvokers.StatusEffects
{
    /// <summary>
    /// Example status effect that makes the damage always be one.
    /// </summary>
    class StatusEffectAetherialState : StatusEffectState
    {
        public const string StatusId = "evokers_aetherial";

        protected override IEnumerator OnTriggered(StatusEffectState.InputTriggerParams inputTriggerParams, StatusEffectState.OutputTriggerParams outputTriggerParams, ICoreGameManagers coreGameManagers)
        {
            CharacterState character = inputTriggerParams.associatedCharacter;
            if (character == null)
            {
                yield return false;
            } else
            {
                int statusEffectStacks = inputTriggerParams.associatedCharacter.GetStatusEffectStacks("captured_soul");
                if (statusEffectStacks >= 1)
                {
                    inputTriggerParams.associatedCharacter.RemoveStatusEffect("captured_soul", 1, false);
                }
                if (inputTriggerParams.associatedCharacter.GetStatusEffectStacks("captured_soul") < 1)
                {
                    CardState spawnerCard = character.GetSpawnerCard();
                    if (spawnerCard != null)
                    {
                        if (!character.PreviewMode)
                        {
                            spawnerCard.SetRemoveFromStandByPileOverride(CardPile.ExhaustedPile);
                        }
                    }
                    character.SetDespawned();
                    inputTriggerParams.associatedCharacter.GetCharacterManager().RemoveCharacter(inputTriggerParams.associatedCharacter, death: false, vanishUnit: true, isLastDeathInSet: true);
                }
                yield break;
            }
        }
    }
}