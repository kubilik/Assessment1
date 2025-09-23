using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    private Player Player;
    private Animator Anim;
    private EnemyMelee EnemyMelee;


    void Start()
    {
        Player = GetComponentInParent<Player>();
        Anim = GetComponent<Animator>();
        EnemyMelee = GetComponentInParent<EnemyMelee>();
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

    public void EnemyAnimationChangeAlertToFalse()
    {
        EnemyMelee.ChangeAlertToFalse();
    }
    public void EnemyAnimationChangeAttackToFalse()
    {
        EnemyMelee.ChangeAttackToFalse();
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

    public void SpawnAxe()
    {
        Player.controller.SpawnAxe();
    }

    public void SpawnAxeFalse()
    {
        Anim.SetBool("ThrowAxe", false);
    }
}
