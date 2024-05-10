using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStateNetwork
{
    public IPlayerStateNetwork Tick(PlayerControllerNetwork player, PlayerInputs input);
    public void Enter(PlayerControllerNetwork player);
    public void Exit(PlayerControllerNetwork player);
}
