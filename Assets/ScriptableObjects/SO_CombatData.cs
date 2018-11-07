using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodIsLove/Combat/CombatData", fileName = "CombatData")]
public class SO_CombatData : ScriptableObject 
{
	public SO_GenericEvent HeroesCollidedEvent;
	public SO_GenericEvent CombatSequenceStartedEvent;
    public SO_GenericEvent CombatOptionChosenEvent;
    public SO_GenericEvent CombatOptionChosenLocallyEvent;
    public SO_GenericEvent CombatSequenceCompletedEvent;
    public SO_GenericEvent CombatSequenceRestartedEvent;
	public SO_GenericEvent ShowCombatResultsEvent;
    public SO_GenericEvent LocalHeroKilledEvent;
    public SO_GenericEvent CombatTimerStartedEvent;
    public SO_GenericEvent CombatTimerEndedEvent;
}
