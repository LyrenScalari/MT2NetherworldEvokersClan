using System.Collections;
namespace NetherworldEvokers.StatusEffects
{
    class StatusEffectAetherialState : StatusEffectState
    {
        public const string StatusId = "netherworldevokers_aetherial";

        protected override IEnumerator OnTriggered(StatusEffectState.InputTriggerParams inputTriggerParams, StatusEffectState.OutputTriggerParams outputTriggerParams, ICoreGameManagers coreGameManagers)
        {
            CharacterState character = inputTriggerParams.associatedCharacter;
            if (character == null)
            {
                yield break;
            }
            int statusEffectStacks = character.GetStatusEffectStacks("captured_soul");
            if (statusEffectStacks >= 1)
            {
                character.RemoveStatusEffect("captured_soul", 1, false);
            }
            if (character.GetStatusEffectStacks("captured_soul") < 1)
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
                yield return character.Sacrifice(null, true);
                List<CharacterState> list = new List<CharacterState>();
                coreGameManagers.GetHeroManager().AddCharactersInRoomToList(list,character.GetCurrentRoomIndex());
                coreGameManagers.GetMonsterManager().AddCharactersInRoomToList(list, character.GetCurrentRoomIndex());
                foreach (CharacterState item in list)
                {
                    yield return coreGameManagers.GetCombatManager().QueueAndRunTrigger(item, CharacterTriggerData.Trigger.CardExhausted);
                }
            }
            yield break;
        }
    }
}