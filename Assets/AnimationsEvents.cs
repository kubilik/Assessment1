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
        Anim.SetBool("Attack", false);
    }
}
