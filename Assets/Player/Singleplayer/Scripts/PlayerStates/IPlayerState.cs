using UnityEngine;

public interface IPlayerState
{
    public IPlayerState Tick(PlayerController player, PlayerInputs input);
    public void Enter(PlayerController player);
    public void Exit(PlayerController player);
}
