using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    private Player Player;
    private Animator Anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Player = GetComponentInParent<Player>();
        Anim = GetComponent<Animator>();
    }

    public void IsDashingFalse()
    {
        Anim.SetBool("isDashing", false);
        Player.controller.ChangeDashingState(false);
    }
    public void AnimationEnableHitbox()
    {
        Player.controller.EnableHitbox();
    }

    public void AnimationDisableHitbox()
    {
        Player.controller.DisableHitbox();
    }

    public void AnimationAttackingFalse()
    {
        Player.controller.NextComboStep();
        Anim.SetBool("Attack", false);
    }

    public void AnimationCanAttackingTrue()
    {
        Player.controller.SetCanAttack(true);
    }
    public void AnimationCanAttackingFalse()
    {
        Player.controller.SetCanAttack(false);
    }
}
