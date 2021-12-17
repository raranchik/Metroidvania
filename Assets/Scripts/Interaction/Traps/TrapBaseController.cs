using System;
using Characters.Herochar;
using UnityEngine;

namespace Interaction.Traps
{
    public abstract class TrapBaseController : InteractionBaseController
    {
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            GameObject obj = other.gameObject;
            string gameObjectTag = obj.tag;
            if (gameObjectTag.Equals("Player"))
            {
                obj.GetComponent<HerocharController>().OnHit();
            }
        }

    }

}