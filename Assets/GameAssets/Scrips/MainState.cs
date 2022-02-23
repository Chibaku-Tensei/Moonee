using System.Collections;
using System.Collections.Generic;
using Base;
using Base.MessageSystem;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class MainState : GameState
    {
        public override void UpdateBehaviour(float dt)
        {
            Messenger.RaiseMessage(SystemMessage.Input, GameStateController.InputAction.Phase, GameStateController.InputAction.Position);
        }
    }
}

