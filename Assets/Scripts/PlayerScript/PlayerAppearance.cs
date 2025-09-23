using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAppearance : SceneSingletonMonoBehaviour<PlayerAppearance>
{
    [SerializeField] private SpriteRenderer sr;
    public Action<EntityMoves> playerAnimationAction;
    public Action<int> playerFlipAction;

    private void Start()
    {
        playerAnimationAction += gameObject.GetComponent<PlayerAnimator>().SetAnimation;
        playerFlipAction += Flip;
        
    }

    private void Flip(int flipper)
    {
        if (flipper < 0)
        {
            sr.flipX = true;
        }
        else if (flipper > 0)
        {
            sr.flipX = false;
        }
    }
    
    
}
